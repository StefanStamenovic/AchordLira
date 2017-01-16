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

        public void GenreCreate()
        {

        }

        public void GenreDelete()
        {

        }
        
        public void GenreUpdate()
        {

        }

        public void GenreRead()
        {

        }

        #endregion

        #region Artist

        public void ArtistCreate()
        {

        }

        public void ArtistrDelete()
        {

        }

        public void ArtistUpdate()
        {

        }

        public void ArtistRead()
        {

        }

        #endregion

        #region SongSubmission

        public void SongSubmissionCreate()
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

        public void SongRequestCreate()
        {

        }

        public void SongRequestDelete()
        {

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