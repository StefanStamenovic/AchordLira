using AchordLira.Models.Neo4J;
using AchordLira.Models.Neo4J.Models;
using AchordLira.Models.Redis;
using AchordLira.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            pageModel.userSongs = dbNeo4j.GetUserSongsAndDrafts(pageModel.user.name);

            //Geting user favorite songs
            pageModel.favoritSongs = dbNeo4j.UserGetFavoriteSongs(pageModel.user.name);

            //Getting user admin notification count
            pageModel.adminNotifications = dbRedis.GetAdminNotificationsCount();

            //Geting user admin request songs
            pageModel.requestedSongs = dbNeo4j.SongDraftRead();

            //User List
            pageModel.userList = dbNeo4j.UserRead();

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

        public ActionResult Delete(string name)
        {
            //Samo admin moze da obrise korisnika
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            dbNeo4j.UserDelete(name);

            return Redirect("/User/");
        }


        #region Artist and Genre

        public ActionResult CreateGenre(string name)
        {

            //Samo admin dodaje zanr
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");


            if (name == null || name == "")
            {
                return Redirect("/User/");
            }

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            List<string> check = dbNeo4j.GenreRead();

            if (check.Contains(name))
            {
                return Redirect("/User/#addGenreModal");
            }
            else
            {
                Genre genre = new Genre();
                genre.name = name;
                dbNeo4j.GenreCreate(genre);
                return Redirect("/User/");
            }

        }

        public ActionResult DeleteGenre(string name)
        {

            //Samo admin brise zanr
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");

            if (name == null || name == "")
            {
                return Redirect("/User/");
            }

            
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            List<string> check = dbNeo4j.GenreRead();

            if (!check.Contains(name))
            {
                return Redirect("/User/#deleteGenreModal");
            }
            else
            {
                dbNeo4j.GenreDelete(name);
                return Redirect("/User/");
            }

        }

        public ActionResult CreateArtist(string name, string bio, string website, string genres)
        {
            //Samo admin dodaje izvodjaca
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");

            if (name == null || name == "" || bio == null || bio == "" || website == null || website == "" || genres == null || genres == "")
            {
                return Redirect("/User/");
            }

            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            List<string> checkArtists = dbNeo4j.ArtistRead();

            if (checkArtists.Contains(name))
            {
                return Redirect("/User/#addArtistModal");
            }
            else
            {
                string genresClean = Regex.Replace(genres, " *, *", ",");
                List<string> genreNames = genresClean.Split(',').ToList();
                List<string> checkGenres = dbNeo4j.GenreRead();
                List<string> validGenreNames = genreNames.Intersect(checkGenres).ToList();
                List<Genre> validGenres = new List<Genre>();
                foreach (string genreName in validGenreNames)
                {
                    Genre tmp = new Genre();
                    tmp.name = genreName;
                    validGenres.Add(tmp);
                }

                if(validGenres == null)
                    return Redirect("/User/");

                Artist artist = new Artist();
                artist.name = name;
                artist.biography = bio;
                artist.website = website;
                artist.link = "/" + artist;
                dbNeo4j.ArtistCreate(artist, validGenres);
                return Redirect("/User/");
            }

        }

        public ActionResult DeleteArtist(string name)
        {


            //Samo admin brise zanr
            if (Session["user"] == null || ((ViewUser)Session["user"]).admin == false)
                return Redirect("/");


            if (name == null || name == "")
            {
                return Redirect("/User/");
            }


            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();

            List<string> check = dbNeo4j.ArtistRead();

            if (!check.Contains(name))
            {
                return Redirect("/User/#deleteArtistModal");
            }
            else
            {
                dbNeo4j.ArtistDelete(name);
                return Redirect("/User/");
            }
        }

        #endregion


        [HttpPost]
        public ActionResult RefreshSubmissions()
        {
            RedisDataProvider dp = new RedisDataProvider();
            dp.ClearAdminNotifications();
            return new HttpStatusCodeResult(200);
        }

    }
}