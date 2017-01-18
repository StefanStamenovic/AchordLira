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
        public ActionResult Index(string artist, string name)
        {
            PageViewModel pageModel = new PageViewModel();
            return View(pageModel);
        }
        
        
        public ActionResult Create(string artist, string song, string content)
        {
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
        public ActionResult Approve(string artist,string name)
        {
            //Samo admin moze da appruvuje
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");
            PageViewModel pageModel = new PageViewModel();
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
            dbRedis.RemoveAdminNotification();

            return Redirect("/User/");
        }

        public ActionResult Delete(string artist, string name)
        {

            return Redirect("/User/");
        }


    }
}