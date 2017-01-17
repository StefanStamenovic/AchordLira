using AchordLira.Models.Neo4J;
using AchordLira.Models.Neo4J.Models;
using AchordLira.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AchordLira.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            //TODO: Conect data base and get data
            UserPageViewModel pageModel = new UserPageViewModel();
            var model = pageModel;
            return View(model);
        }

        // GET: User/Register
        public ActionResult Register(string name, string email, string password)
        {
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                return Redirect("/");
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            return View();
        }

        // GET: User/Login
        public ActionResult Login(string email,string password)
        {
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                return Redirect("/");
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            User user = dbNeo4j.UserRead(email, password);
            if (user==null)
            {
                PageViewModel pageModel = new PageViewModel();
                pageModel.genre = null;

                //Getting artists
                pageModel.artists = dbNeo4j.ArtistRead(pageModel.genre);

                //Getting genres
                pageModel.genres = dbNeo4j.GenreRead();

                ViewBag.error = "Wrong email or password.";
                if (email == null && password == null)
                    ViewBag.error = null;
                return View(pageModel);
            }
            ViewUser vuser = new ViewUser(user);
            Session["user"] = vuser;
            return Redirect("/");
        }

        public ActionResult Logout()
        {
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                Session["user"] = null;
            return Redirect("/");
        }
    }
}