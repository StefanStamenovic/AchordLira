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
        public ActionResult Index(string genre)
        {
            HomePageViewModel pageModel = new HomePageViewModel();

            //Is loged 
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
            {
                pageModel.user = (ViewUser)(Session["user"]);
            }
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            pageModel.genre = genre;

            //Getting artists
            pageModel.artists = dbNeo4j.ArtistRead(genre);

            //Getting genres
            pageModel.genres = dbNeo4j.GenreRead();

            //Getting popular songs
            List<string> popularSongNames = dbRedis.GetMostPopularSongs(10);
            pageModel.popularSongs = dbNeo4j.SongRead(popularSongNames);

            //Getting latest songs
            List<string> latestSongNames = dbRedis.GetLatestSongs();
            pageModel.latestSongs = dbNeo4j.SongRead(latestSongNames);

            //Getting songs requests
            pageModel.songRequests = dbNeo4j.SongRequestRead();

            return View(pageModel);
        }

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
    }
}