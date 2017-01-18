using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;
using AchordLira.Models.Neo4J.Models;
using Neo4jClient.Cypher;
using AchordLira.Models.ViewModels;

namespace AchordLira.Models.Neo4J
{
    public class Neo4jDataProvider
    {
        private GraphClient client;
        private Uri db_adres = new Uri("http://localhost:7474/db/data");
        private string user_name = "neo4j";
        private string password = "Stefan@1994";
        public string erorr = null;

        public Neo4jDataProvider()
        {
            client = new GraphClient(db_adres, user_name, password);
            try
            {
                client.Connect();
            }
            catch (Exception e)
            {
                erorr = "Database failed to connect with folowing message : "+e.Message;
            }
        }

        #region User

        public void UserCreate(User user)
        {
            //TODO: Proveri da li postiji user
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", user.name);
            dictionary.Add("email", user.email);
            dictionary.Add("password", user.password);
            dictionary.Add("link", user.link);
            dictionary.Add("admin", user.admin);
            dictionary.Add("date", user.date);

            CypherQuery query = new CypherQuery("CREATE (user:User { name: {name}, email: {email}, password: {password}, link: {link}, admin: {admin}, date: {date}})",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void UserDelete(String name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);

            CypherQuery query = new CypherQuery("MATCH (user:User) WHERE user.name = {name} DELETE user",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void UserUpdate()
        {

        }

        public User UserRead(string email,string password)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("email", email);
            dictionary.Add("password", password);
            CypherQuery query = new CypherQuery("MATCH (user:User{ email: {email}, password: {password}}) RETURN user",
                           dictionary, CypherResultMode.Set);
            List<User> result = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            if (result.Count <= 0)
                return null;
            return result.First();
        }

        //Search to see if user exists whit given email or password
        public bool UserExists(string email, string name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("email", email);
            dictionary.Add("name", name);
            CypherQuery query = new CypherQuery("MATCH (user:User) WHERE user.email = {email} OR user.name = {name} RETURN user",
                           dictionary, CypherResultMode.Set);
            List<User> result = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            if (result.Count <= 0)
                return false;
            return true;
        }


        public void UserAddFavoriteSong(string user,string artist,string song)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", song);
            CypherQuery query = new CypherQuery("MATCH (user:User{name: {user_name}}),(song:Song{name: {song_name}})-[relation:PERFORMED_BY]->(artist:Artist{name: {artist_name}}) CREATE (user)-[relation:FAVORITE]->(song)",
                           dictionary, CypherResultMode.Set);
            List<User> result = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            ((IRawGraphClient)client).ExecuteCypher(query);
        }


        public List<ViewSong> UserGetFavoriteSongs(string user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", user);
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:FAVORITE]->(song:Song) WHERE user.name = {name} RETURN song",
                       dictionary, CypherResultMode.Set);
            List<Song> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList();
            List<ViewSong> songs = new List<ViewSong>();
            foreach (Song song in qres)
            {
                dictionary = new Dictionary<string, object>();
                dictionary.Add("song_name", song.name);
                query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} RETURN artist",
                       dictionary, CypherResultMode.Set);
                List<Artist> qresArtist = ((IRawGraphClient)client).ExecuteGetCypherResults<Artist>(query).ToList();
                Artist artist = null;
                if (qresArtist.Count > 0)
                    artist = qresArtist.First();
                ViewSong temp = new ViewSong(song, user, artist.name);
                songs.Add(temp);
            }
            return songs;
        }


        #endregion

        #region Genre

        public void GenreCreate(Genre genre)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", genre.name);

            CypherQuery query = new CypherQuery("CREATE (genre:Genre { name: {name}})",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void GenreDelete(String name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);

            CypherQuery query = new CypherQuery("MATCH (genre:Genre) WHERE genre.name = {name} DELETE genre",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }
        
        public void GenreUpdate()
        {

        }

        public List<string> GenreRead()
        {
            List<string> result = new List<string>();
            CypherQuery query = new CypherQuery("MATCH (genre:Genre) RETURN genre",
                       null, CypherResultMode.Set);
            List<Genre> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Genre>(query).ToList();
            foreach (Genre genre in qres)
                result.Add(genre.name);

            return result;
        }

        #endregion

        #region Artist

        public void ArtistCreate(Artist artist, List<Genre> genres)
        {
            //TODO: Proveri da li postoji izvodjaci i zanar
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", artist.name);
            dictionary.Add("link", artist.link);
            dictionary.Add("biography", artist.biography);
            dictionary.Add("website", artist.website);

            CypherQuery query = new CypherQuery("CREATE (artist:Artist { name: {name}, link: {link}, biography: {biography}, website: {website}})",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            foreach(Genre genre in genres)
            {
                dictionary = new Dictionary<string, object>();
                dictionary.Add("artist_name", artist.name);
                dictionary.Add("genre_name", genre.name);
                query = new CypherQuery("MATCH (artist:Artist{name: {artist_name}}),(genre:Genre{name: {genre_name}}) CREATE (artist)-[relation:BELONG]->(genre)",
                           dictionary, CypherResultMode.Set);
                ((IRawGraphClient)client).ExecuteCypher(query);
            }
        }

        public void ArtistrDelete(String name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);

            CypherQuery query = new CypherQuery("MATCH (artist:Artist) WHERE artist.name = {name} DETACH DELETE artist",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void ArtistUpdate()
        {

        }

        public Dictionary<String, List<ViewArtist>> ArtistRead(String genre)
        {
            Dictionary<String, List<ViewArtist>> result = new Dictionary<string, List<ViewArtist>>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("genre_name", genre);
                dictionary.Add("character", c);
                CypherQuery query = new CypherQuery("MATCH (artist:Artist)-[relation:BELONG]->(genre:Genre) WHERE substring( lower( artist.name), 0, 1) = {character} AND genre.name = {genre_name} RETURN artist",
                       dictionary, CypherResultMode.Set);
                List<Artist> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Artist>(query).ToList();
                List<ViewArtist> artists = new List<ViewArtist>();
                foreach (Artist artist in qres)
                {
                    ViewArtist temp = new ViewArtist(artist);
                    artists.Add(temp);
                }
                result.Add(c.ToString(), artists);
            }
            return result;
        }

        #endregion

        #region SongDraft

        //Kreira song draft
        public void SongDraftCreate(SongDraft draft, String user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", draft.name);
            dictionary.Add("link", draft.link);
            dictionary.Add("content", draft.content);
            dictionary.Add("date", draft.date);
            dictionary.Add("artist", draft.artist);

            //Kreira draft u bazi
            CypherQuery query = new CypherQuery("CREATE (song:SongDraft { name: {name}, link: {link}, content: {content}, date: {date}, artist: {artist}})",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            //Povezuje draft sa korisnikom
            dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            dictionary.Add("song_name", draft.name);
            query = new CypherQuery("MATCH (user:User{name: {user_name}}),(song:SongDraft{name: {song_name}}) CREATE (user)-[relation:REQUESTED]->(song)",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        //Brise song draft
        public void SongDraftDelete(SongDraft draft, String user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            dictionary.Add("draft_name", draft.name);
            dictionary.Add("artist_name", draft.artist);

            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:REQUESTED]->(draft:SongDraft) WHERE user.name = {user_name} AND draft.name = {draft_name} AND draft.artist = {artist_name} DETACH DELETE draft",
                   dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        //Vraca sve song draftove kao listu viewsonga za prikaz adminu
        public List<ViewSong> SongDraftRead()
        {
            CypherQuery query = new CypherQuery("MATCH (draft:SongDraft) RETURN draft",
                       null, CypherResultMode.Set);

            List<SongDraft> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<SongDraft>(query).ToList();
            List<ViewSong> songs = new List<ViewSong>();
            
            foreach (SongDraft draft in qres)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("draft_name", draft.name);
                dictionary.Add("artist_name", draft.artist);
       
                query = new CypherQuery("MATCH (user:User)-[relation:REQUESTED]->(draft:SongDraft) WHERE draft.name = {draft_name} AND draft.artist = {artist_name} RETURN user",
                       dictionary, CypherResultMode.Set);
                List<User> qresUser = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
                User user = null;
                if (qresUser.Count > 0)
                    user = qresUser.First();
                ViewSong temp = new ViewSong(draft, user.name);
                songs.Add(temp);
            }
            return songs;
        }

        //Vraca sve draftove koje je kreirao zadati korisnik
        public List<ViewSong> SongDraftRead(String user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:REQUESTED]->(draft:SongDraft) WHERE user.name = {user_name} RETURN draft",
                   dictionary, CypherResultMode.Set);
            List<SongDraft> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<SongDraft>(query).ToList();
            List<ViewSong> songs = new List<ViewSong>();
            foreach (SongDraft draft in qres)
            {
                ViewSong temp = new ViewSong(draft, user);
                songs.Add(temp);
            }
            return songs;
        }

        //Vraca draft za potrebe edita i pravljenja pesme
        public ViewSong SongDraftRead(String user,String artist,String song)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            dictionary.Add("artist", artist);
            dictionary.Add("song", song);
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:REQUESTED]->(draft:SongDraft) WHERE user.name = {user_name} AND draft.artist = {artist} AND draft.name = {song} RETURN draft",
                   dictionary, CypherResultMode.Set);
            List<SongDraft> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<SongDraft>(query).ToList();
            if (qres.Count > 0)
            {
                SongDraft draft = qres.First();
                ViewSong result = new ViewSong(draft, user);
                return result;
            }
            return null;
        }

        #endregion

        #region Song

        public void SongCreate(Song song,String user,String artist)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", song.name);
            dictionary.Add("link", song.link);
            dictionary.Add("content", song.content);
            dictionary.Add("date", song.date);

            CypherQuery query = new CypherQuery("CREATE (song:Song { name: {name}, link: {link}, content: {content}, date: {date}})",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            dictionary.Add("song_name", song.name);
            query = new CypherQuery("MATCH (user:User{name: {user_name}}),(song:Song{name: {song_name}}) CREATE (user)-[relation:CREATED]->(song)",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            dictionary = new Dictionary<string, object>();
            dictionary.Add("song_name", song.name);
            dictionary.Add("artist_name", artist);
            query = new CypherQuery("MATCH (song:Song{name: {song_name}}),(artist:Artist{name: {artist_name}}) CREATE (song)-[relation:PERFORMED_BY]->(artist)",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            SongRequestDelete(artist, song.name);
        }

        public void SongDelete(String artis, String name, String user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artis);
            dictionary.Add("user_name", user);
            dictionary.Add("song_name", name);

            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:CREATED]->(song:Song)-[relation1:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND user.name = {user_name} AND artist.name = {artist_name} DETACH DELETE song",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongUpdate()
        {

        }
        public List<ViewSong> SongRead(String user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", user);
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:CREATED]->(song:Song) WHERE user.name = {name} RETURN song",
                       dictionary, CypherResultMode.Set);
            List<Song> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList();
            List<ViewSong> songs = new List<ViewSong>();
            foreach (Song song in qres)
            {
                dictionary = new Dictionary<string, object>();
                dictionary.Add("song_name", song.name);
                query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} RETURN artist",
                       dictionary, CypherResultMode.Set);
                List<Artist> qresArtist = ((IRawGraphClient)client).ExecuteGetCypherResults<Artist>(query).ToList();
                Artist artist = null;
                if (qresArtist.Count > 0)
                    artist = qresArtist.First();
                ViewSong temp = new ViewSong(song, user, artist.name);
                songs.Add(temp);
            }
            return songs;
        }
        public List<ViewSong> SongRead(List<string> songs)
        {
            List<ViewSong> result=new List<ViewSong>();
            //Dictionary<string, object> dictionary;
            //foreach(string song in songs)
            //{
            //    dictionary = new Dictionary<string, object>();
            //    dictionary.Add("name", song);

            //    CypherQuery query = new CypherQuery("MATCH (song:Song { name: {name}) RETURN song",
            //               dictionary, CypherResultMode.Set);
            //    List<Song> qresSong= ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList();

            //    query = new CypherQuery("MATCH (user:User)-[relation:CREATED]->(song:Song) WHERE song.name = {name} RETURN user",
            //               dictionary, CypherResultMode.Set);
            //    List<User> qresUser = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();

            //    Song resSong = null;
            //    if (qresSong.Count > 0)
            //        resSong = qresSong.First();
            //    User resUser = null;
            //    if (qresUser.Count > 0)
            //        resUser = qresUser.First();

            //    dictionary = new Dictionary<string, object>();
            //    dictionary.Add("song_name", resSong.name);
            //    query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} RETURN artist",
            //           dictionary, CypherResultMode.Set);
            //    List<Artist> qresArtist = ((IRawGraphClient)client).ExecuteGetCypherResults<Artist>(query).ToList();

            //    Artist artist = null;
            //    if (qresArtist.Count > 0)
            //        artist = qresArtist.First();
            //    ViewSong temp = new ViewSong(song, user, artist.name);
            //    result.Add(temp);
            //}
            return result;
        }

        public void SongAddToFavorites(string name, string artist, string user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", name);
            dictionary.Add("user_name", user);
            CypherQuery query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name}, (user:User) WHERE user.name = {user_name} CREATE (user)-[relation:FAVORITE]->(song)",
                   dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongRemoveFromFavorites(string name, string artist, string user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", name);
            dictionary.Add("user_name", user);
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:FAVORITE]->(song:Song) WHERE song.name = {song_name} AND artist.name = {artist_name} and user.name = {user_name}  DELETE (user:User)-[relation:FAVORITE]->(song)",
                   dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        #endregion

        #region Comment

        public void CommentCreate(Comment comment,String user,String song)
        {
            //TODO: Treba uzeti novi id i proveriti da li postoje user i song
            int id = 0;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("id", id);
            dictionary.Add("content", comment.content);
            dictionary.Add("date", comment.date);

            CypherQuery query = new CypherQuery("CREATE (comment:Comment { id: {id}, content: {content}, date: {date}})",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            dictionary.Add("comment_id", id);
            query = new CypherQuery("MATCH (user:User{id: {user_name}}),(comment:Comment{id: {comment_id}}) CREATE (user)-[relation:COMMENTED_BY]->(comment)",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            dictionary = new Dictionary<string, object>();
            dictionary.Add("comment_id", id);
            dictionary.Add("song_name", song);
            query = new CypherQuery("MATCH (comment:Comment{id: {comment_id}}),(song:Song{name: {song_name}}) CREATE (comment)-[relation:COMMENT_TO]->(song)",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void CommentDelete(int id)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("id", id);

            CypherQuery query = new CypherQuery("MATCH (comment:Comment) WHERE comment.id = {id} DETACH DELETE comment",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void CommentUpdate()
        {

        }

        public void CommentRead()
        {

        }

        #endregion

        #region SongRequest

        public void SongRequestCreate(SongRequest songRequest)
        {
            //TODO: Proveri da li postiji submission
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("author", songRequest.author);
            dictionary.Add("artist", songRequest.artist);
            dictionary.Add("song", songRequest.song);
            dictionary.Add("date", songRequest.date);

            CypherQuery query = new CypherQuery("CREATE (songrequest:SongRequest { author: {author}, artist: {artist}, song: {song}, date: {date}})",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongRequestDelete(String artist,String song)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist", artist);
            dictionary.Add("song", song);

            CypherQuery query = new CypherQuery("MATCH (songrequest:SongRequest{artist: {artist}, song: {song}}) DELETE songrequest",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongRequestUpdate()
        {

        }

        public List<ViewSongRequest> SongRequestRead()
        {
            CypherQuery query = new CypherQuery("MATCH (songrequest:SongRequest) RETURN songrequest",
                       null, CypherResultMode.Set);
            List<ViewSongRequest> result = ((IRawGraphClient)client).ExecuteGetCypherResults<ViewSongRequest>(query).ToList();

            return result;
        }

        #endregion

        #region Utility

        public List<ViewSong> GetUserSongsAndDrafts(string user)
        {
            List<ViewSong> allSongs = SongRead(user);
            allSongs.AddRange(SongDraftRead(user));
            return allSongs;
        }

        #endregion
    }
}