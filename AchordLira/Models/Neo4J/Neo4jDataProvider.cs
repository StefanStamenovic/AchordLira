using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;
using AchordLira.Models.Neo4J.Models;
using Neo4jClient.Cypher;
using AchordLira.Models.ViewModels;
using AchordLira.Models.Redis;

namespace AchordLira.Models.Neo4J
{
    public class Neo4jDataProvider
    {
        private GraphClient client;
        private Uri db_adres = new Uri("http://localhost:7474/db/data");
        private string user_name = "neo4j";
        private string password = "Stefan@1994";
        public string erorr = null;
        private RedisDataProvider dbRedis = new RedisDataProvider();

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
            dictionary.Add("admin_name", "admin");
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:CREATED]->(song:Song) WHERE user.name = {name} WITH song MATCH (admin:User) WHERE admin.name = {admin_name} CREATE (admin)-[relation:CREATED]->(song) ",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            query = new CypherQuery("MATCH (user:User)-[relation:REQUESTED]->(draft:SongDraft) WHERE user.name = {name} RETURN draft",
                       dictionary, CypherResultMode.Set);
            int count = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList().Count;

            for (int i = 0; i < count; i++)
            {
                dbRedis.RemoveAdminNotification();
            }

            query = new CypherQuery("MATCH (user:User)-[relation:REQUESTED]->(draft:SongDraft) WHERE user.name = {name} DETACH DELETE draft",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            query = new CypherQuery("MATCH (user:User) WHERE user.name = {name} DETACH DELETE user ",
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

        public List<ViewUser> UserRead()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("true", true);

            CypherQuery query = new CypherQuery("MATCH (user:User) WHERE user.admin <> {true} RETURN user",
                           dictionary, CypherResultMode.Set);
            List<User> result = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
            List<ViewUser> users = new List<ViewUser>();
            foreach (User user in result)
            {
                ViewUser tmp = new ViewUser();
                tmp.date = user.date;
                tmp.name = user.name;
                tmp.email = user.email;
                tmp.admin = user.admin;
                tmp.link = user.link;
                users.Add(tmp);
            }
            return users;
        }

        public ViewUser UserRead(String name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);
            CypherQuery query = new CypherQuery("MATCH (user:User) WHERE user.name = {name} RETURN user",
                       dictionary, CypherResultMode.Set);
            User result = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList().FirstOrDefault();
            if (result==null)
                return null;
            ViewUser user = new ViewUser(result);
            return user;
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

            dbRedis.AddGenreToRedis();
        }

        public void GenreDelete(String name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);

            CypherQuery query = new CypherQuery("MATCH (genre:Genre) WHERE genre.name = {name} DETACH DELETE genre",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);

            dbRedis.RemoveGenreFromRedis();
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

            dbRedis.AddArtistToRedis();
        }

        public void ArtistDelete(String name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);

            CypherQuery query = new CypherQuery("MATCH (artist:Artist) WHERE artist.name = {name} DETACH DELETE artist",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            dbRedis.RemoveArtistFromRedis();
        }

        public void ArtistUpdate()
        {

        }

        //Vraca sve artiste po zanru
        public Dictionary<String, List<ViewArtist>> ArtistRead(String genre)
        {
            Dictionary<String, List<ViewArtist>> result = new Dictionary<string, List<ViewArtist>>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("character", c);
                CypherQuery query;
                if (genre != null)
                {
                    dictionary.Add("genre_name", genre);
                    query = new CypherQuery("MATCH (artist:Artist)-[relation:BELONG]->(genre:Genre) WHERE artist.name STARTS WITH {character} AND genre.name = {genre_name} RETURN artist",
                       dictionary, CypherResultMode.Set);
                }
                else
                {
                    query = new CypherQuery("MATCH (artist:Artist) WHERE artist.name STARTS WITH {character} RETURN artist",
                       dictionary, CypherResultMode.Set);
                }
                
                
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

        public List<string> ArtistRead()
        {
            List<string> result = new List<string>();
            CypherQuery query = new CypherQuery("MATCH (artist:Artist) RETURN artist",
                       null, CypherResultMode.Set);
            List<Artist> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Artist>(query).ToList();
            foreach (Artist artist in qres)
                result.Add(artist.name);
            return result;
        }

        public ViewArtist ArtistReadOne(string name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);
            CypherQuery query = new CypherQuery("MATCH (artist:Artist) WHERE artist.name = {name} RETURN artist",
                   dictionary, CypherResultMode.Set);
            Artist qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Artist>(query).ToList().FirstOrDefault();
            if (qres == null)
                return null;
            ViewArtist artist = new ViewArtist(qres);
            return artist;
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

            dbRedis.AddSongToRedis(artist + " - " + song.name);
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

            dbRedis.AddSongToRedis(artis + " - " + name);
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

        public ViewSong SongRead(String artist,String song)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", song);
            CypherQuery query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name} RETURN song",
                   dictionary, CypherResultMode.Set);
            List<Song> qresSongs = ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList();
            Song rsong = null;
            ViewSong result = null;
            if (qresSongs.Count > 0)
            {
                dictionary = new Dictionary<string, object>();
                dictionary.Add("artist_name", artist);
                dictionary.Add("song_name", song);
                query = new CypherQuery("MATCH (user:User)-[relation:CREATED]->(song:Song)-[relation1:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name} RETURN user",
                   dictionary, CypherResultMode.Set);
                User qresUser = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList().FirstOrDefault();
                rsong = qresSongs.First();
                string user = null;
                if (qresUser != null)
                    user = qresUser.name;
                result = new ViewSong(rsong, user, artist);
            }
            return result;
        }

        public List<ViewSong> SongRead(List<string> songs)
        {
            List<ViewSong> result=new List<ViewSong>();

            foreach (string item in songs)
            {
                string song = item.Substring(item.IndexOf(" - ") + 3);
                string artist = item.Substring(0, item.IndexOf(" - "));

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("artist_name", artist);
                dictionary.Add("song_name", song);
                CypherQuery query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name} RETURN song",
                       dictionary, CypherResultMode.Set);
                Song qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList().FirstOrDefault();
                if (qres != null)
                    result.Add(new ViewSong(qres, null, artist));
            }
            
            return result;
        }

        public List<ViewSong> SongReadArtistSongs(String artist)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            CypherQuery query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE artist.name = {artist_name} RETURN song",
                       dictionary, CypherResultMode.Set);
            List<Song> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList();
            List<ViewSong> songs = new List<ViewSong>();
            foreach(Song song in qres)
            {
                ViewSong temp = new ViewSong(song,null,artist);
                songs.Add(temp);
            }
            return songs;
        }

        public void SongAddToFavorites(string name, string artist, string user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", name);
            dictionary.Add("user_name", user);
            CypherQuery query = new CypherQuery("MATCH (song:Song)-[relation1:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name} WITH song MATCH (user:User) WHERE user.name = {user_name} CREATE (user)-[relation:FAVORITE]->(song)",
                   dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongRemoveFromFavorites(string name, string artist, string user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", name);
            dictionary.Add("user_name", user);
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:FAVORITE]->(song:Song)-[relation1:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name} and user.name = {user_name}  DELETE relation",
                   dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public bool SongCheckIsFavorite(string song, string artist, string user)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", song);
            dictionary.Add("user_name", user);
            CypherQuery query = new CypherQuery("MATCH (user:User)-[relation:FAVORITE]->(song:Song)-[relation1:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name} and user.name = {user_name}  RETURN song",
                   dictionary, CypherResultMode.Set);
            Song qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList().FirstOrDefault();
            if (qres == null)
                return false;
            return true;
        }

        #endregion

        #region Comment

        public void CommentCreate(Comment comment,String user, String artist,String song)
        {
            int id = Convert.ToInt32(GetCommentId()) + 1;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("id", id);
            dictionary.Add("title", comment.title);
            dictionary.Add("content", comment.content);
            dictionary.Add("date", comment.date);

            CypherQuery query = new CypherQuery("CREATE (comment:Comment { id: {id}, title: {title}, content: {content}, date: {date}})",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            dictionary = new Dictionary<string, object>();
            dictionary.Add("user_name", user);
            dictionary.Add("comment_id", id);
            query = new CypherQuery("MATCH (user:User{name: {user_name}}),(comment:Comment{id: {comment_id}}) CREATE (user)-[relation:COMMENTED_BY]->(comment)",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);

            dictionary = new Dictionary<string, object>();
            dictionary.Add("comment_id", id);
            dictionary.Add("song_name", song);
            dictionary.Add("artist_name", artist);
            query = new CypherQuery("MATCH (comment:Comment{id: {comment_id}}) WITH comment MATCH (song:Song)-[relation1:PERFORMED_BY]->(artist:Artist) WHERE song.name = {song_name} AND artist.name = {artist_name} CREATE (comment)-[relation:COMMENT_TO]->(song)",
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

        public List<ViewComment> CommentRead(String artist, String song)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist_name", artist);
            dictionary.Add("song_name", song);
            CypherQuery query = new CypherQuery("MATCH (comment:Comment)-[relation: COMMENT_TO]->(song: Song)-[relation1:PERFORMED_BY]->(artist:Artist) WHERE song.name = { song_name } AND artist.name = { artist_name} RETURN comment",
                   dictionary, CypherResultMode.Set);
            List<Comment> qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Comment>(query).ToList();
            List<ViewComment> comments = new List<ViewComment>();
            foreach (Comment comment in qres)
            {
                dictionary = new Dictionary<string, object>();
                dictionary.Add("id", comment.id);

                query = new CypherQuery("MATCH (user:User)-[relation:COMMENTED_BY]->(comment:Comment) WHERE comment.id = {id}  RETURN user",
                   dictionary, CypherResultMode.Set);
                User qresUser = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList().FirstOrDefault();
                if (qresUser!=null)
                {
                    ViewComment temp = new ViewComment(comment, qresUser.name);
                    comments.Add(temp);
                }
            }
            return comments;
        }

        private string GetCommentId()
        {
            CypherQuery query = new CypherQuery("MATCH (comment:Comment) WHERE exists(comment.id) RETURN max(comment.id)",
                   null, CypherResultMode.Set);
            string maxID = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList().FirstOrDefault();
            if (maxID == null)
                return "-1";
            return (string)maxID;
        }

        #endregion

        #region SongRequest

        public void SongRequestCreate(SongRequest songRequest)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("author", songRequest.author);
            dictionary.Add("artist", songRequest.artist);
            dictionary.Add("song", songRequest.song);
            dictionary.Add("date", songRequest.date);

            CypherQuery query = new CypherQuery("CREATE (songrequest:SongRequest { author: {author}, artist: {artist}, song: {song}, date: {date}})",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongRequestDelete(String artist,String song, String author)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("artist", artist);
            dictionary.Add("song", song);
            dictionary.Add("author", author);

            CypherQuery query = new CypherQuery("MATCH (songrequest:SongRequest{artist: {artist}, song: {song}, author: {author}}) DELETE songrequest",
                       dictionary, CypherResultMode.Set);

            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongRequestDelete(String artist, String song)
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
            //uzmi najskorijih 7(ili manje ako ih nema toliko)
            List<ViewSongRequest> oredered = result.OrderBy(o => o.date).ToList().GetRange(0, 7 <= result.Count ? 7 : result.Count);
            oredered.Reverse();
            return oredered;
        }

        #endregion

        #region Utility

        public List<ViewSong> GetUserSongsAndDrafts(string user)
        {
            List<ViewSong> allSongs = SongRead(user);
            allSongs.AddRange(SongDraftRead(user));
            return allSongs;
        }

        public List<ViewSong> SearchResults(List<string> text)
        {
            List<ViewSong> matchedSongs = new List<ViewSong>();

            foreach (string word in text)
            {
                string song = word.Substring(word.IndexOf(" - ") + 3);
                string artist = word.Substring(0, word.IndexOf(" - "));

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("artist", artist);
                dictionary.Add("song", song);

                CypherQuery query = new CypherQuery("MATCH (song:Song)-[relation:PERFORMED_BY]->(artist:Artist) WHERE song.name={song} AND artist.name={artist} RETURN song",
                       dictionary, CypherResultMode.Set);
                Song qres = ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList().FirstOrDefault();

                if (qres != null)
                {
                    query = new CypherQuery("MATCH (user:User)-[relation:CREATED]->(song:Song)-[relation2:PERFORMED_BY]->(artist:Artist) WHERE song.name={song} AND artist.name={artist} RETURN user",
                            dictionary, CypherResultMode.Set);
                    User user = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList().FirstOrDefault();

                    ViewSong tmp = new ViewSong();
                    tmp.name = qres.name;
                    tmp.artist = artist;
                    tmp.date = qres.date;
                    tmp.content = qres.content;
                    tmp.link = qres.link;
                    tmp.creator = user.name;
                    tmp.approved = true;

                    matchedSongs.Add(tmp);
                }
            }

            return matchedSongs;
        }

        public void DeleteBase()
        {
            CypherQuery query = new CypherQuery("MATCH (node) DETACH DELETE node",
                       null, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        #endregion
    }
}