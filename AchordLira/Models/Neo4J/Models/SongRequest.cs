using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.Neo4J.Models
{
    public class SongRequest
    {
        public String author { get; set; }
        public String artist { get; set; }
        public String song { get; set; }
        public String date { get; set; }
    }
}