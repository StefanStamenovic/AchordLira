using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.Neo4J.Models
{
    public class Artist
    {
        public string name { get; set; }
        public string link { get; set; }
        public string biography { get; set; }
        public string website { get; set; }
    }
}