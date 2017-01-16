using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;
using AchordLira.Models.Neo4J.Models;
using Neo4jClient.Cypher;

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
            dictionary.Add("link", user.link);
            dictionary.Add("admin", user.admin);

            CypherQuery query = new CypherQuery("CREATE (user:User { name: {name}, email: {email}, link: {link}, admin: {admin}})",
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

        public void UserRead()
        {

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

        public void GenreRead()
        {

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

        public void ArtistRead()
        {

        }

        #endregion

        #region SongSubmission

        public void SongSubmissionCreate(SongSubmission songSubmission)
        {
            
        }

        public void SongSubmissionDelete()
        {

        }

        public void SongSubmissionUpdate()
        {

        }

        public void SongSubmissionRead()
        {

        }

        #endregion

        #region Song

        public void SongCreate()
        {

        }

        public void SongDelete()
        {

        }

        public void SongUpdate()
        {

        }

        public void SongRead()
        {

        }

        #endregion

        #region Comment

        public void CommentCreate()
        {

        }

        public void CommentDelete()
        {

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

        public void SongRequestRead()
        {

        }

        #endregion

    }
}