using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AchordLira.Models.ViewModels;

namespace AchordLira.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //TODO: Conect data base and get data
            HomePageViewModel pageModel = new HomePageViewModel();
            pageModel.user = new ViewUser
            {
                name = "Stefan",
                email = "stefan.stamenovic@gmail.com",
                link = "/User/Stefan",
                admin = true
            };
            pageModel.popularSongs = new List<ViewSong>{
                new ViewSong {
                    id=0,
                    name="Daire",
                    content="Negde iz daleka....",
                    creator="Stefan"
                },
                new ViewSong {
                    id=1,
                    name="Profesor",
                    content="U nasem gradu zivi....",
                    creator="Stefan"
                },
                 new ViewSong {
                    id=0,
                    name="Daire",
                    content="Negde iz daleka....",
                    creator="Stefan"
                },
                new ViewSong {
                    id=1,
                    name="Profesor",
                    content="U nasem gradu zivi....",
                    creator="Stefan"
                },
                 new ViewSong {
                    id=0,
                    name="Daire",
                    content="Negde iz daleka....",
                    creator="Stefan"
                },
                new ViewSong {
                    id=1,
                    name="Profesor",
                    content="U nasem gradu zivi....",
                    creator="Stefan"
                },
            };
            pageModel.songRequests = new List<ViewMessageRequest>
            {
                new ViewMessageRequest {
                    author="Stefan",
                    artist="S.A.R.S",
                    song="Lutka",
                    date=DateTime.Now.ToString("h:mm:ss tt")
                },
                new ViewMessageRequest {
                    author="Stefan",
                    artist="S.A.R.S",
                    song="Perspektiva",
                    date=DateTime.Now.ToString("h:mm:ss tt")
                }
            };
            pageModel.newSongs = new List<ViewSong>{
                new ViewSong {
                    id=0,
                    name="Daire",
                    content="Negde iz daleka....",
                    creator="Stefan"
                },
                new ViewSong {
                    id=1,
                    name="Profesor",
                    content="U nasem gradu zivi....",
                    creator="Stefan"
                }
            };
            pageModel.artists = new Dictionary<string, List<ViewArtist>>();
            pageModel.artists.Add("A", new List<ViewArtist>{ new ViewArtist { name = "Azra", link = "/Artist/Azra" }});
            pageModel.genres = new List<string> { "Zabavno", "Narodno" };
            var model = pageModel;
            return View(model);
        }
    }
}