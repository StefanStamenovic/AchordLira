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
            pageModel.user = new ViewUser
            {
                name = "Stefan",
                email = "stefan.stamenovic@gmail.com",
                link = "/User/Stefan",
                admin = true
            };
            var model = pageModel;
            return View(model);
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