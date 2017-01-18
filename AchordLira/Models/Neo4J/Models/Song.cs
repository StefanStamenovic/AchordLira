using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.Neo4J.Models
{
    public class Song
    {
        public string name { get; set; }
        public string link { get; set; }
        public string content { get; set; }
        public string date { get; set; }
    }
}