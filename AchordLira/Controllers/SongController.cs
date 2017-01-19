using AchordLira.Models.Neo4J;
using AchordLira.Models.Neo4J.Models;
using AchordLira.Models.Redis;
using AchordLira.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AchordLira.Controllers
{
    public class SongController : Controller
    {
        //GET /Song/Index/
        public ActionResult Index(string artist, string song,string genre)
        {
            ViewBag.showNav = true;
            SongPageViewModel pageModel = new SongPageViewModel();
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

            //Getting artist songs
            pageModel.artistSongs = dbNeo4j.SongReadArtistSongs(artist);

            //Getting song data
            pageModel.song = dbNeo4j.SongRead(artist,song);

            //Getting song comment
            pageModel.comments = dbNeo4j.CommentRead(artist, song);

            //Check is song favorite
            if (pageModel.user != null)
                pageModel.favorite = dbNeo4j.SongCheckIsFavorite(song, artist, pageModel.user.name);

            pageModel.artist = artist;

            dbRedis.IncrementSongVisitCount(artist + " - " + song);

            return View(pageModel);
        }

        //GET /Song/CreateDraft/
        public ActionResult CreateDraft(string artist, string song, string content)
        {
            ViewBag.showNav = false;
            PageViewModel pageModel = new PageViewModel();

            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                pageModel.user = (ViewUser)(Session["user"]);
            else
                return Redirect("/");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            if (artist == null && song == null && content == null)
                return View(pageModel);

            if (artist == null || song == null || content == null)
                ViewBag.error = "Data is missing from required fields.";
            else if (artist.Length < 1 || song.Length < 1 || content.Length < 1)
                ViewBag.error = "Data is missing from required fields.";

            if (ViewBag.error != null)
                return View(pageModel);

            SongDraft draft = new SongDraft();
            draft.name = song;
            draft.artist = artist;
            draft.link = "/" + artist + "/" + song;
            draft.date = DateTime.Now.ToString("dd-MM-yyyy");
            draft.content = content;

            dbNeo4j.SongDraftCreate(draft, pageModel.user.name);

            dbRedis.AddAdminNotification();

            return Redirect("/User/");
        }

        //GET /Song/Approve/
        public ActionResult Approve(string user,string artist,string name,string content,string approve)
        {
            ViewBag.showNav = false;
            //Samo admin moze da appruvuje
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");

            PageViewModel pageModel = new PageViewModel();
            pageModel.user = (ViewUser)(Session["user"]);

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            if (approve != null)
            {
                //Provera da li postoji Artist
                List<string> allArtists = dbNeo4j.ArtistRead();
                if (!allArtists.Contains(artist))
                {
                    ViewBag.noArtist = true;
                    ViewBag.draft = dbNeo4j.SongDraftRead(user, artist, name);
                    return View(pageModel);
                }

                //Promenjeni podatci
                ViewSong draft = new ViewSong();
                draft.creator = user;
                draft.name = name;
                draft.content = content;
                draft.artist = artist;
                draft.approved = false;
                draft.date= DateTime.Now.ToString("dd-MM-yyyy");

                ViewBag.draft = draft;
                if (artist == null && name == null && content == null)
                    return View(pageModel);
                if (artist == null || name == null || content == null)
                    ViewBag.error = "Data is missing from required fields.";
                else if (artist.Length < 1 || name.Length < 1 || content.Length < 1)
                    ViewBag.error = "Data is missing from required fields.";

                if (ViewBag.error != null)
                    return View(pageModel);

                //Kriranje nove pesme
                Song song = new Song();
                song.name = name;
                song.content = content;
                song.date= DateTime.Now.ToString("dd-MM-yyyy");
                song.link = "/" + artist + "/" + user + "/";

                dbNeo4j.SongCreate(song, user, artist);
                SongDraft songDraft = new SongDraft();
                songDraft.name = name;
                songDraft.artist = artist;
                dbNeo4j.SongDraftDelete(songDraft, user);
                dbNeo4j.SongRequestDelete(artist, name);

                return Redirect("/User/");
            }
            ViewBag.draft = dbNeo4j.SongDraftRead(user, artist, name);

            return View(pageModel);
        }

        //GET /Song/DeleteDraft/
        public ActionResult DeleteDraft(string user, string artist,string name)
        {
            //Samo admin brise draft
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();
            SongDraft draft = new SongDraft();
            draft.artist = artist;
            draft.name = name;

            dbNeo4j.SongDraftDelete(draft,user);
            dbRedis.ClearAdminNotifications();

            return Redirect("/User/");
        }

        //GET /Song/AddFavorite/
        public ActionResult AddFavorite(string artist, string name)
        {
            ViewUser user;
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                user = ((ViewUser)Session["user"]);
            else
                return Redirect("/");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            dbNeo4j.SongAddToFavorites(name, artist, user.name);
            string uri = "";
            if (Request.UrlReferrer != null)
                uri = Request.UrlReferrer.ToString();
            return Redirect(uri);
        }

        //GET /Song/DeleteFavorite/
        public ActionResult DeleteFavorite(string artist, string name)
        {
            ViewUser user;
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                user = ((ViewUser)Session["user"]);
            else
                return Redirect("/");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            dbNeo4j.SongRemoveFromFavorites(name, artist, user.name);

            string uri = "";
            if (Request.UrlReferrer != null)
                uri = Request.UrlReferrer.ToString();
            return Redirect(uri);
        }

        //GET /Song/CreateComment/
        public ActionResult CreateComment(string artist, string song,string title,string content)
        {
            string uri = "";
            if (Request.UrlReferrer != null)
                uri = Request.UrlReferrer.ToString();

            ViewUser user;
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                user = ((ViewUser)Session["user"]);
            else
                return Redirect("/");

            if(content==null || content=="")
                return Redirect(uri);

            Comment comment = new Comment();
            comment.title = title;
            comment.content = content;
            comment.date=DateTime.Now.ToString("dd-MM-yyyy");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            dbNeo4j.CommentCreate(comment, user.name, artist, song);

            return Redirect(uri);
        }
    }
}