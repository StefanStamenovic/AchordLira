using AchordLira.Models.Neo4J;
using AchordLira.Models.Redis;
using AchordLira.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AchordLira.Controllers
{
    public class ArtistController : Controller
    {
        // GET: /Artist/Index
        public ActionResult Index(string artist,string genre)
        {
            ArtistPageViewModel pageModel = new ArtistPageViewModel();
            //Is loged 
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                pageModel.user = (ViewUser)(Session["user"]);
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            #region NavBarData

            if (genre == "All")
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

            //Getting artist songs
            pageModel.artistSongs = dbNeo4j.SongReadArtistSongs(artist);

            //Geting artist info
            pageModel.artist = dbNeo4j.ArtistReadOne(artist);

            ViewBag.showNav = true;
            return View(pageModel);
        }
    }
}