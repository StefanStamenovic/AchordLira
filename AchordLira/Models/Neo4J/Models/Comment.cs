using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.Neo4J.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string date { get; set; }
    }
}