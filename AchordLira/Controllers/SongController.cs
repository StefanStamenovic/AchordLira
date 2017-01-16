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
            SongPageViewModel pageModel = new SongPageViewModel();
            pageModel.artist = "Smak";
            pageModel.song = new ViewSong
            {
                id = 0,
                name = "Daire",
                content = @"Em       G   D
Negde iz daleka
Em      G  D
čuju se daire
Em        G D
mesečevom rekom
Em     G   D
dolaze Cigani
Em       A     G F# H   (prelaz)
negde iz dalekaaaaa

Ref.
Em       D   C     (prelaz)  
Izvire u oku sjaj
Em       D        C
da li je java il' san
Em    D        C
daire dajte mi vi
Em     D       C G F# Em
navire pesma u meeeeeeni

Fenjeri se pale
vatra je u ljudima
orošene lale
leže na grudima
fenjeri se pale

Ref.

Ako umrem sada
kupite daire
svirajte mi tiho
ali me nje pazite
ako umrem sada",
                creator = "Stefan"
            };
            pageModel.songs= new List<ViewSong>
            {
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
            pageModel.messages = new List<ViewMessage>
            {
                new ViewMessage {
                    id=0,
                    content="Tab je super.",
                    creator="Stefan"
                },
                new ViewMessage {
                    id=1,
                    content="Pesma je odlicna.",
                    creator="Anonimus"
                },
            };

            var model = pageModel;
            return View(model);
        }
    }
}