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

        #endregion

        #region Gener

        public void GenreCreate(Genre genre)
        {
            //TODO: Proveri da li postiji zanar
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

        #region Song

        public void SongCreate(Song song,String user,String artist)
        {
            //TODO: Proveri da li postoji song i user i artist
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", song.name);
            dictionary.Add("link", song.link);
            dictionary.Add("content", song.content);
            dictionary.Add("date", song.date);
            dictionary.Add("approved", song.approved);

            CypherQuery query = new CypherQuery("CREATE (song:Song { name: {name}, link: {link}, content: {content}, date: {date}, approved: {approved}})",
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
        }

        public void SongDelete(String name)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("name", name);

            CypherQuery query = new CypherQuery("MATCH (song:Song) WHERE song.name = {name} DETACH DELETE song",
                       dictionary, CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public void SongUpdate()
        {

        }

        public List<ViewSong> SongRead(List<string> songs)
        {
            List<ViewSong> result=new List<ViewSong>();
            Dictionary<string, object> dictionary;
            foreach(string song in songs)
            {
                dictionary = new Dictionary<string, object>();
                dictionary.Add("name", song);

                CypherQuery query = new CypherQuery("MATCH (song:Song { name: {name}) RETURN song",
                           dictionary, CypherResultMode.Set);
                List<Song> qresSong= ((IRawGraphClient)client).ExecuteGetCypherResults<Song>(query).ToList();

                query = new CypherQuery("MATCH (user:User)-[relation:CREATED]->(song:Song) WHERE song.name = {name} RETURN user",
                           dictionary, CypherResultMode.Set);
                List<User> qresUser = ((IRawGraphClient)client).ExecuteGetCypherResults<User>(query).ToList();
                Song resSong = null;
                if (qresSong.Count > 0)
                    resSong = qresSong.First();
                User resUser = null;
                if (qresUser.Count > 0)
                    resUser = qresUser.First();
                ViewSong temp = new ViewSong(resSong, resUser);
                result.Add(temp);
            }
            return result;
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

    }
}