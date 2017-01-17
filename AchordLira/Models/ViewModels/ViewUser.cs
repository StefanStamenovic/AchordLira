using AchordLira.Models.Neo4J.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.ViewModels
{
    public class ViewUser
    {
        public string name { get; set; }
        public string email { get; set; }
        public string link { get; set; }
        public bool admin { get; set; }
        public String date { get; set; }
        public ViewUser(User user)
        {
            name = user.name;
            email = user.email;
            link = user.email;
            admin = user.admin;
            date = user.date;
        }
    }
}