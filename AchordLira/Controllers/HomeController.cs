using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AchordLira.Models.ViewModels;
using AchordLira.Models.Neo4J;
using AchordLira.Models.Neo4J.Models;
using AchordLira.Models.Redis;
using AchordLira.Models;

namespace AchordLira.Controllers
{
    public class HomeController : Controller
    {
        //GET /Home/Index
        public ActionResult Index(string genre)
        {
            HomePageViewModel pageModel = new HomePageViewModel();

            //Is loged 
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                pageModel.user = (ViewUser)(Session["user"]);

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            #region NavBarData

            if (genre == "All" || genre == "")
                pageModel.genre = null;
            else
                pageModel.genre = genre;

            //Getting artists
            pageModel.artists = dbNeo4j.ArtistRead(genre);
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (pageModel.artists.ContainsKey(c.ToString()))
                {
                    pageModel.artists[c.ToString()].Sort();
                }
            }


            //Getting genres
            pageModel.genres = dbNeo4j.GenreRead();
            pageModel.genres.Sort();

            #endregion

            //Getting popular songs
            List<string> popularSongNames = dbRedis.GetMostPopularSongs(10);
            pageModel.popularSongs = dbNeo4j.SongRead(popularSongNames);

            //Getting latest songs
            List<string> latestSongNames = dbRedis.GetLatestSongs();
            pageModel.latestSongs = dbNeo4j.SongRead(latestSongNames);

            //Getting songs requests
            pageModel.songRequests = dbNeo4j.SongRequestRead();

            
            ViewBag.showNav = true;

            return View(pageModel);
        }

        //GET /SongRequest/Create/
        public ActionResult Create(string genre,string author,string artist,string song)
        {
            SongRequest songRequest = new SongRequest();
            songRequest.author = author;
            if (author.Equals(""))
                songRequest.author = "Anonymous";
            songRequest.artist = artist;
            songRequest.song = song;
            songRequest.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            if (!artist.Equals("") || !song.Equals(""))
            {
                Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
                dbNeo4j.SongRequestCreate(songRequest);
            }

            string redirectUri = "/";
            if (genre != null && genre != "")
                redirectUri += "?genre=" + genre;
            return Redirect(redirectUri);
        }

        //GET /SongRequest/Delete/
        public ActionResult Delete(string artist, string song, string author, string genre)
        {
            //Samo admin moze da obrise request
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            dbNeo4j.SongRequestDelete(artist, song, author);
            string redirectUri = "/";
            if (genre != null && genre != "")
                redirectUri += "?genre=" + genre;
            return Redirect(redirectUri);
        }

        [HttpPost]
        public JsonResult Search (string text)
        {
            RedisDataProvider dp = new RedisDataProvider();
            string user = null;
            if (Session["user"] != null)
                user = ((ViewUser)Session["user"]).name;
            List<string> results = dp.AutoComplete(user, text);
            JsonResult inter = Json(results);
            return inter;
        }

    }
}