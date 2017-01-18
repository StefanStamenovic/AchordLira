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
        // GET: Artist
        public ActionResult Index(string artist,string genre)
        {
            ViewBag.showNav = true;
            ArtistPageViewModel pageModel = new ArtistPageViewModel();
            //Is loged 
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                pageModel.user = (ViewUser)(Session["user"]);
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            if (genre == "All")
                pageModel.genre = null;
            else
                pageModel.genre = genre;

            //Getting artists
            pageModel.artists = dbNeo4j.ArtistRead(genre);

            //Getting genres
            pageModel.genres = dbNeo4j.GenreRead();

            //Getting artist songs
            pageModel.artistSongs = dbNeo4j.SongReadArtistSongs(artist);

            pageModel.artist = artist;

            //TODO: Napravi da se prikazuju podatci o artistu

            return View(pageModel);
        }
    }
}