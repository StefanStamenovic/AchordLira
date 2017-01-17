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
        public string password { get; set; }
        public string link { get; set; }
        public bool admin { get; set; }
    }
}