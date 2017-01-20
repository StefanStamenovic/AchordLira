using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace AchordLira.Models.Redis
{
    public class RedisDataProvider
    {

        #region AutoComplete

        /** Dodaje tekst u kes pretrage za odredjenog (registrovanoh) korisnika */
        public void AddSearchPhraseFromUser(string userID, string phrase)
        {
            var redisClient = RedisDataLayer.GetClient();
            if(phrase != redisClient.GetItemFromList("user.search." + userID, 0))
            {
                redisClient.TrimList("user.search." + userID, 0, 9);
                redisClient.PushItemToList("user.search." + userID, phrase);
            }
            
        }

        /** Metoda za dodavanje pesme u bazu*/
        public void InsertSearchPhrase(string phrase)
        {
            var redisClient = RedisDataLayer.GetClient();

            //prebacuje u lowercase i razdvaja u reci
            string[] words = Regex.
                Replace(phrase.ToLower(), " *- *", " ")
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.Length > 1)
                .ToArray();
            List<string> partialStrings = new List<string>();

            //razdvaja svaku rec u parcijalne reci pocevsi
            //od dva slova, npr "Mouse" ce da postane:
            // [mo, mou, mous, mouse]
            foreach (var word in words)
            {
                List<string> tmp = DecomposeWord(word);
                partialStrings.AddRange(tmp);
            }

            //U Hash se upise vrednost countera kao key
            //a neizmenjeni naziv pesme(bez lowercase) kao value
            string counter = redisClient.GetValue("counter");
            if (counter == null)
            {
                redisClient.SetValue("counter", "1");
                counter = "1";
            }
            redisClient.SetEntryInHash("hash.phrases", counter, phrase);

            //Ako ne postoji, kreira se sortirani skup za svaki parcijalni string
            //u sortirane skupove se upisuje hashkey, tj vrednost countera
            foreach (var partialString in partialStrings)
            {
                redisClient.AddItemToSortedSet("search." + partialString, counter, 1);
            }
           
            //counter++
            redisClient.IncrementValue("counter");
        }

        /**Brisanje pesme iz baze za pretragu */
        public void DeleteSearchPhrase(string phrase)
        {
            var redisClient = RedisDataLayer.GetClient();

            string keyToDelete = "";
            
            //pribavi sve hash kljuceve
            List<string> hashKeys = redisClient.GetHashKeys("hash.phrases");

            //Ako je hash value jednak frazi koja se brise, zapamti njen kljuc
            foreach (var key in hashKeys)
            {
                string value = redisClient.GetValueFromHash("hash.phrases", key);
                if (string.Compare(phrase, value) == 0)
                {
                    keyToDelete = key;
                    break;
                }
            }

            //Brisanje iz hasha
            redisClient.RemoveEntryFromHash("hash.phrases", keyToDelete);
            //Pribave se svi kljucevi(strukture podataka u bazi)
            var setKeys = redisClient.GetAllKeys();
            foreach (var set in setKeys)
            {
                //ako je u pitanju sortirani set za parcijalnu rec(pocinje sa search)
                if(set.StartsWith("search."))
                    //obrisi mu element vezan za frazu koja se brise
                    redisClient.RemoveItemFromSortedSet(set, keyToDelete);
            }
        }

        /** Osnovna metoda, za uneti string vraca sva poklapanja za autocomplete */
        public List<string> AutoComplete(string userID, string text, bool autocomplete)
        {
            var redisClient = RedisDataLayer.GetClient();

            string[] words = Regex.
                Replace(text.ToLower(), " *- *", " ")
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => s.Length > 1)
                .ToArray();

            string[] hashKeys = null;
            List<string> phrases;

            //Ako su uneti samo blanko znaci, obustavi
            if (words.Length == 0)
                return redisClient.GetAllItemsFromList("user.search." + userID);
            //Ako je u pitanju jedna rec, prosto vrati poklapanja
            else if (words.Length == 1)
            {
                //Buduci da autocomplete radi samo od dva ukucana slova, ako korisnik ukuca jedno slovo dobice do 10 poslednjih pretrazenih fraza
                if (words[0].Length == 1 && userID != null && autocomplete)
                    return redisClient.GetAllItemsFromList("user.search." + userID);
                else
                    hashKeys = redisClient.GetRangeFromSortedSet("search." + words[0], 0, -1).ToArray();
            }
            //
            else
            {
                //kljuc za vise reci
                string cacheKey = "cache." + string.Join("|", words);

                //Ako ne postoji kljuc u kesu, napravi ga
                if (!redisClient.ContainsKey(cacheKey))
                {
                    //vrsi se presek skupova i rezultat se smesta u cachekey
                    //podatak u kesu postoji 30 minuta
                    redisClient.StoreIntersectFromSortedSets(cacheKey, words.Select(x => "search." + x).ToArray());
                    redisClient.ExpireEntryIn(cacheKey, new TimeSpan(0, 30, 0));
                }

                hashKeys = redisClient.GetRangeFromSortedSet(cacheKey, 0, -1).ToArray();
            }

            //Uzimaju se neizmenjene fraze iz hesa
            phrases = redisClient.GetValuesFromHash("hash.phrases", hashKeys);
            return phrases;
        }

        //Resetuje counter za inicijalizaciju
        public void ResetHashCounter()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.SetValue("counter", "1");
        }

        /** Pomocna funkcija za dekompoziciju reci u podstringove potrebne za indeksiranje */
        public List<string> DecomposeWord(string word)
        {
            List<string> list = new List<string>();

            for (int i = 2; i <= word.Length; i++)
            {
                list.Add(word.Substring(0, i));
            }

            return list;
        }

        #endregion


        #region Data & Statictics
        
        //Reset baze
        public void DeleteAll()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.FlushAll();
        }

        //++ kada se poseti stranica pesme
        public void IncrementSongVisitCount(string id)
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.IncrementItemInSortedSet("songs.popular", id, 1);
        }

        //<number> najpopularnijih pesama iz sortiranog skupa
        public List<string> GetMostPopularSongs(int number)
        {
            var redisClient = RedisDataLayer.GetClient();
            IDictionary<string, double> tmp = redisClient.GetRangeWithScoresFromSortedSetDesc("songs.popular", 0, number - 1);
            List<string> popular = new List<string>();

            foreach (var item in tmp)
            {
                popular.Add(item.Key);
            }

            return popular;
        }

        //5 poslednjih pesama(lista radi kao red velicine 5)
        public List<string> GetLatestSongs()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetAllItemsFromList("songs.latest");
        }


        /*
         * Dodavanje potrebnih informacija za pesme, izvodjace i zanrove
         */ 

        public void AddSongToRedis(string name)
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.AddItemToSortedSet("songs.popular", name, 0);
            redisClient.IncrementValue("songs.count");
            redisClient.PushItemToList("songs.latest", name);
            redisClient.TrimList("songs.latest", 0, 4);
            InsertSearchPhrase(name);
        }

        public string GetSongCount()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetValue("songs.count");
        }

        public void RemoveSongFromRedis(string name)
        {
            var redisClient = RedisDataLayer.GetClient();
            string listItemIn;
            List<string> listItemsOut = new List<string>();

            for (int i = 0; i < redisClient.GetListCount("songs.latest"); i++)
            {
                listItemIn = redisClient.PopItemFromList("songs.latest");
                if (listItemIn != name)
                    listItemsOut.Add(listItemIn);
            }

            listItemsOut.Reverse();

            for (int i = 0; i < listItemsOut.Count; i++)
            {
                redisClient.PushItemToList("songs.latest", listItemsOut[i]);
            }

            redisClient.RemoveItemFromSortedSet("songs.popular", name);
            redisClient.DecrementValue("songs.count");
            DeleteSearchPhrase(name);
        }

        public void AddArtistToRedis()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.IncrementValue("artists.count");
        }

        public string GetArtistCount()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetValue("artists.count");
        }

        public void RemoveArtistFromRedis()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.DecrementValue("artists.count");
        }

        public void AddGenreToRedis()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.IncrementValue("genres.count");
        }

        public string GetGenreCount()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetValue("genres.count");
        }

        public void RemoveGenreFromRedis()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.DecrementValue("genres.count");
        }

        //Reset obavestenja za admina o novim pesmama kada klikne na tu karticu
        public void ClearAdminNotifications()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.SetValue("admin.notification.count", "0");
        }

        public void AddAdminNotification()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.IncrementValue("admin.notification.count");
        }

        //Brisanje ako se obrisu draftovi dok jos nije clearovano
        public void RemoveAdminNotification()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.DecrementValue("admin.notification.count");
            if(int.Parse(redisClient.GetValue("admin.notification.count")) < 0)
                redisClient.SetValue("admin.notification.count", "0");

        }

        public string GetAdminNotificationsCount()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetValue("admin.notification.count");
        }


        #endregion

    }
}
