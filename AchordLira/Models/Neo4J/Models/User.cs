using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.Neo4J.Models
{
    public class User
    {
        public String name { get; set; }
        public String email { get; set; }
        public String password { get; set; }
        public String link { get; set; }
        public bool admin { get; set; }
    }
}