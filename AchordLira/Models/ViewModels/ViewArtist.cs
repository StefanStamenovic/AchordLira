using AchordLira.Models.Neo4J.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.ViewModels
{
    public class ViewArtist
    {
        public string name { get; set; }
        public string link { get; set; }
        public string biography { get; set; }
        public string website { get; set; }

        public ViewArtist(Artist artist)
        {
            name = artist.name;
            link = artist.link;
            biography = artist.biography;
            website = artist.website;
        }
    }
}