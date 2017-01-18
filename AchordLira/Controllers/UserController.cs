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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            UserPageViewModel pageModel = new UserPageViewModel();
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                pageModel.user = (ViewUser)(Session["user"]);
            else
                return Redirect("/");

            pageModel.user = (ViewUser)(Session["user"]);

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();
        

            pageModel.genre = null;

            //Getting artists
            pageModel.artists = dbNeo4j.ArtistRead(pageModel.genre);

            //Getting genres
            pageModel.genres = dbNeo4j.GenreRead();

            //Geting user created songs
            pageModel.userSongs = dbNeo4j.SongRead(pageModel.user.name);

            //Geting user favorite songs
            pageModel.favoritSongs = dbNeo4j.UserGetFavoriteSongs(pageModel.user.name);

            //Getting user admin notification count
            pageModel.adminNotifications = dbRedis.GetAdminNotificationsCount();

            //Geting user admin request songs
            pageModel.requestedSongs = dbNeo4j.SongDraftRead();

            ViewBag.showNav = true;
            return View(pageModel);
        }
        public ActionResult Register(string name, string email, string password, string confirm)
        {
            ViewBag.showNav = false;
            if (Session["user"] != null && Session["user"].GetType() == (typeof(ViewUser)))
                return Redirect("/");
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            dbNeo4j.UserExists(email, name);

            PageViewModel pageModel = new PageViewModel();
            pageModel.genre = null;

            //Getting artists
            pageModel.artists = dbNeo4j.ArtistRead(pageModel.genre);

            //Getting genres
            pageModel.genres = dbNeo4j.GenreRead();
            if (name == null && email == null && password == null && confirm == null)
                return View(pageModel);

            if (password != confirm)
                ViewBag.error = "Passwords don't match.";
            if (dbNeo4j.UserExists(email, name))
                ViewBag.error = "User already exists.Try different email or username.";
            if (name == null || email == null || password == null || confirm == null)
                ViewBag.error = "Data is missing from required fields.";
            else if (name.Length < 1 || email.Length < 5 || password.Length < 5 || confirm.Length < 5)
                ViewBag.error = "Some fields don't have required length.";

            if (ViewBag.error != null)
                return View(pageModel);

            User user = new User();
            user.name = name;
            user.email = email;
            user.password = password;
            user.admin = false;
            user.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");

            dbNeo4j.UserCreate(user);

            ViewUser vuser = new ViewUser(user);
            Session["user"] = vuser;
            return Redirect("/");
        }

        // GET: User/Login
        public ActionResult Login(string email,string password)
        {
            ViewBag.showNav = false;
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