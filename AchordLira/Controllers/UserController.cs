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
            return View();
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }
    }
}