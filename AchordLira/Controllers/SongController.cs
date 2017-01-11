using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AchordLira.Controllers
{
    public class SongController : Controller
    {
        public ActionResult Index(string name,string artist)
        {
            ViewBag.songName = name + artist;
            return View();
        }
    }
}