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
        public ActionResult Index(string artist, string song,string genre)
        {
            ViewBag.showNav = true;
            SongPageViewModel pageModel = new SongPageViewModel();
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

            //Getting song data
            pageModel.song = dbNeo4j.SongRead(artist,song);

            //Getting song comment
            pageModel.comments = dbNeo4j.CommentRead(artist, song);

            //Check is song favorite
            if (pageModel.user != null)
                pageModel.favorite = dbNeo4j.SongCheckIsFavorite(song, artist, pageModel.user.name);

            pageModel.artist = artist;

            //TODO: Dodaj redis posetu
            return View(pageModel);
        }
        
        
        public ActionResult Create(string artist, string song, string content)
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
            draft.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            draft.content = content;

            dbNeo4j.SongDraftCreate(draft, pageModel.user.name);

            dbRedis.AddAdminNotification();

            return Redirect("/User/");
        }

        //Prikazuje pesmu adminu i daje opciju izmeni i potvrde 
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
                draft.date= DateTime.Now.ToString("mm:hh dd-MM-yyyy");

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
                song.date= DateTime.Now.ToString("mm:hh dd-MM-yyyy");
                song.link = "/" + artist + "/" + user + "/";

                dbNeo4j.SongCreate(song, user, artist);
                SongDraft songDraft = new SongDraft();
                songDraft.name = name;
                songDraft.artist = artist;
                dbNeo4j.SongDraftDelete(songDraft, user);

                return Redirect("/User/");
            }
            ViewBag.draft = dbNeo4j.SongDraftRead(user, artist, name);

            return View(pageModel);
        }

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

        public ActionResult Delete(string artist, string name)
        {
            ViewUser user;
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                user = ((ViewUser)Session["user"]);
            else
                return Redirect("/");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            dbNeo4j.SongRemoveFromFavorites(name, artist, user.name);

            return Redirect("/User/");
        }

        public ActionResult CreateMessage(string artist, string song,string title,string content)
        {
            ViewUser user;
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                user = ((ViewUser)Session["user"]);
            else
                return Redirect("/");
            return Redirect("/");
        }
    }
}