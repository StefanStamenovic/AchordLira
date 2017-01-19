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
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string text, string genre)
        {

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            ViewBag.showNav = true;
            SearchPageViewModel pageModel = new SearchPageViewModel();


            //Is loged 
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
            {
                pageModel.user = (ViewUser)(Session["user"]);
                dbRedis.AddSearchPhraseFromUser(pageModel.user.name, text);
            }

            

            List<string> redisResults = dbRedis.AutoComplete(pageModel.user != null ? pageModel.user.name : null, text, false);

            if (genre == "All")
                pageModel.genre = null;
            else
                pageModel.genre = genre;

            //Getting artists
            pageModel.artists = dbNeo4j.ArtistRead(genre);

            //Getting genres
            pageModel.genres = dbNeo4j.GenreRead();

            //Getting artist songs
            pageModel.matched = dbNeo4j.SearchResults(redisResults);

            pageModel.text = text;

            //TODO: Napravi da se prikazuju podatci o artistu

            return View(pageModel);
        }
    }
}