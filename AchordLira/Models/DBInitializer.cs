using AchordLira.Models.Neo4J;
using AchordLira.Models.Neo4J.Models;
using AchordLira.Models.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AchordLira.Models.Neo4J;
using System.IO;
using System.Web.Hosting;
using System.Text.RegularExpressions;

namespace AchordLira.Models
{
    public class DBInitializer
    {
        string userFilePath = @"~/Initialization_Data/users.txt";
        string genresFilePath = @"~/Initialization_Data/genres.txt";
        string artistFilePath = @"~/Initialization_Data/artists.txt";
        string songFolderPath = @"~/Initialization_Data/Song/";
        string favoritesFilePath = @"~/Initialization_Data/favorites.txt";
        string commentsFilePath = @"~/Initialization_Data/comments.txt";
        string songRequestFilePath = @"~/Initialization_Data/requests.txt";
        string songDraftFolderPath = @"~/Initialization_Data/SongDrafts/";

        public DBInitializer()
        {
        }

        public void Initialize()
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            StreamReader stream;
            //Check is database initialized
            if (dbNeo4j.UserExists("admin", "admin"))
                return;

            DeleteAllData();
            dbRedis.ResetHashCounter();

            #region Create users

            User admin = new User();
            admin.name = "admin";
            admin.email = "admin";
            admin.password = "admin";
            admin.admin = true;
            admin.date = date;
            dbNeo4j.UserCreate(admin);
            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(userFilePath));
                while (!stream.EndOfStream)
                {
                    User user = new User();
                    user.name = stream.ReadLine();
                    user.email = stream.ReadLine();
                    user.password = stream.ReadLine();
                    user.admin = false;
                    user.date = date;
                    stream.ReadLine();
                    dbNeo4j.UserCreate(user);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }

            #endregion

            #region Create genres

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(genresFilePath));
                while (!stream.EndOfStream)
                {
                    Genre genre = new Genre();
                    genre.name = stream.ReadLine();
                    stream.ReadLine();
                    dbNeo4j.GenreCreate(genre);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }

            #endregion

            #region Create artists

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(artistFilePath));
                while (!stream.EndOfStream)
                {
                    Artist artist = new Artist();
                    artist.name = stream.ReadLine();
                    artist.biography = stream.ReadLine();
                    artist.website = stream.ReadLine();

                    string genresClean = Regex.Replace(stream.ReadLine(), " *, *", ",");
                    List<string> genreNames = genresClean.Split(',').ToList();
                    List<string> checkGenres = dbNeo4j.GenreRead();
                    List<string> validGenreNames = genreNames.Intersect(checkGenres).ToList();
                    List<Genre> validGenres = new List<Genre>();
                    foreach (string genreName in validGenreNames)
                    {
                        Genre tmp = new Genre();
                        tmp.name = genreName;
                        validGenres.Add(tmp);
                    }
                    dbNeo4j.ArtistCreate(artist, validGenres);
                    stream.ReadLine();
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }
            #endregion

            #region Create songs

            string[] songPaths= Directory.GetFiles(HostingEnvironment.MapPath(songFolderPath));
            foreach(string songPath in songPaths)
            {
                try
                {
                    stream = new StreamReader(songPath);
                    Song song = new Song();
                    song.name = stream.ReadLine();
                    string artist= stream.ReadLine();
                    string user = stream.ReadLine();
                    stream.ReadLine();
                    song.content = stream.ReadToEnd();
                    song.date = date;
                    dbNeo4j.SongCreate(song,user,artist);
                    stream.Close();
                }
                catch (Exception e)
                {

                }
            }

            #endregion

            #region Create favorites

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(favoritesFilePath));
                while (!stream.EndOfStream)
                {
                    string song = stream.ReadLine();
                    string artist= stream.ReadLine();
                    string user = stream.ReadLine();
                    stream.ReadLine();
                    dbNeo4j.SongAddToFavorites(song,artist,user);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }

            #endregion

            #region Create Comments

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(commentsFilePath));
                while (!stream.EndOfStream)
                {
                    Comment comment = new Comment();
                    comment.title= stream.ReadLine();
                    comment.content = stream.ReadLine();
                    comment.date = date;
                    string song = stream.ReadLine();
                    string artist = stream.ReadLine();
                    string user = stream.ReadLine();
                    stream.ReadLine();
                    dbNeo4j.CommentCreate(comment,user,artist,song);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }

            #endregion

            #region Create requests

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(songRequestFilePath));
                while (!stream.EndOfStream)
                {
                    SongRequest songRequest = new SongRequest();
                    songRequest.author = stream.ReadLine();
                    songRequest.artist = stream.ReadLine();
                    songRequest.song = stream.ReadLine();
                    songRequest.date = date;
                    stream.ReadLine();
                    dbNeo4j.SongRequestCreate(songRequest);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }

            #endregion

            #region Create songdrafts

            string[] songDraftPaths = Directory.GetFiles(HostingEnvironment.MapPath(songDraftFolderPath));
            foreach (string songDraftPath in songDraftPaths)
            {
                try
                {
                    stream = new StreamReader(songDraftPath);
                    SongDraft songDraft = new SongDraft();
                    songDraft.name = stream.ReadLine();
                    songDraft.artist = stream.ReadLine();
                    string user= stream.ReadLine();
                    songDraft.content = stream.ReadToEnd();
                    songDraft.date = date;
                    dbNeo4j.SongDraftCreate(songDraft, user);
                    stream.Close();
                    dbRedis.AddAdminNotification();
                }
                catch (Exception e)
                {

                }
            }

            #endregion

        }

        public void DeleteAllData()
        {
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();
            dbNeo4j.DeleteBase();
            dbRedis.DeleteAll();        
        }
    }
}