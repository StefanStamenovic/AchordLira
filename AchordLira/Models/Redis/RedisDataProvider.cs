using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AchordLira.Models.Redis
{
    public class RedisDataProvider
    {
        private static string backUpFileName = "db_backup.txt";
        private static string tmpBackUpFilename = "tmpbackup.txt";
        private static bool useFileBackUp = true;

        /** Metoda za testiranje, obrisi kad zavrsis */
        public void Test()
        {
        }

        /** Brisanje sadrzaja baze, reset counter na 1, ucitavanje iz backup fajla */
        //pitn
        public void ResetDatabase()
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.FlushAll();
            redisClient.SetValue("counter", "1");

            //ako je slucajno ostao stari tmp, obrisi ga
            if (File.Exists(tmpBackUpFilename))
                File.Delete(tmpBackUpFilename);

            //Upis iz fajla u bazu
            string line = null;
            using (StreamReader reader = new StreamReader(backUpFileName))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    InsertSearchPhrase(line, true);
                }
            }

            //brisanje starog i rename tmp-a u default ime
            File.Delete(backUpFileName);
            File.Move(tmpBackUpFilename, backUpFileName);
        }

        #region AutoComplete

        /** Dodaje tekst u kes pretrage za korisnika */
        public void AddSearchPhraseFromUser(string userID, string phrase)
        {
            var redisClient = RedisDataLayer.GetClient();
            if(phrase != redisClient.GetItemFromList("user.search." + userID, 0))
            {
                redisClient.TrimList("user.search." + userID, 0, 9);
                redisClient.PushItemToList("user.search." + userID, phrase);
            }
            
        }

        /** Metoda za dodavanje pesme u bazu, i u backup file */
        public void InsertSearchPhrase(string phrase, bool fromBackup)
        {
            var redisClient = RedisDataLayer.GetClient();
    
            //prebacuje u lowercase i razdvaja u reci
            string[] words = phrase.ToLower().Split(' ');
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
            redisClient.SetEntryInHash("hash.phrases", counter, phrase);

            //Ako ne postoji, kreira se sortirani skup za svaki parcijalni string
            //u sortirane skupove se upisuje hashkey, tj vrednost countera
            foreach (var partialString in partialStrings)
            {
                redisClient.AddItemToSortedSet("search." + partialString, counter, 1);
            }

            //Snimanje u file
            if(useFileBackUp)
            {
                string fileName = fromBackup ? tmpBackUpFilename : backUpFileName;

                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine(phrase);
                }
            }
           
            //counter++
            redisClient.IncrementValue("counter");
        }

        /**Brisanje pesme iz baze za pretragu i backup fajla */
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

            //Brisanje iz fajla, prepisuje se ceo fajl u tmp
            //uz izostavljanje linije sa datom frazom
            if (useFileBackUp)
            {
                string line = null;
                using (StreamReader reader = new StreamReader(backUpFileName))
                {
                    using (StreamWriter writer = new StreamWriter(tmpBackUpFilename))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (String.Compare(line, phrase) == 0)
                                continue;

                            writer.WriteLine(line);
                        }
                    }
                }

                //brisanje starog i move u novi, hack za rename
                File.Delete(backUpFileName);
                File.Move(tmpBackUpFilename, backUpFileName);
            }
            
        }

        /** Osnovna metoda, za uneti string vraca sva poklapanja za autocomplete */
        public List<string> AutoComplete(string userID, string text)
        {
            var redisClient = RedisDataLayer.GetClient();

            
            string[] words = text.ToLower().Split(' ');

            string[] hashKeys = null;
            List<string> phrases;

            //Ako su uneti samo blanko znaci, obustavi
            if (words.Length == 0)
                return null;
            //Ako je u pitanju jedna rec, prosto vrati poklapanja
            else if (words.Length == 1)
            {
                if (words[0].Length == 1)
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
        public void IncrementSongVisitCount(string id)
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.IncrementItemInSortedSet("songs.popular", id, 1);
        }

        public List<string> GetMostPopularSongs(int number)
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetRangeFromSortedSetByHighestScore("songs.popular", 0, number);
        }

        public List<string> GetLatestSongs()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetAllItemsFromList("songs.latest");
        }

        public void AddSongToRedis(string id, string name)
        {
            var redisClient = RedisDataLayer.GetClient();
            redisClient.AddItemToSortedSet("songs.popular", id, 0);
            redisClient.IncrementValue("songs.count");
            redisClient.TrimList("songs.latest", 0, 4);
            redisClient.PushItemToList("songs.latest", id);
            InsertSearchPhrase(name, false);
        }

        public string GetSongCount()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetValue("songs.count");
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
        
        public string GetAdminNotificationsCount()
        {
            var redisClient = RedisDataLayer.GetClient();
            return redisClient.GetValue("admin.notification.count");
        }



        #endregion
    }
}
