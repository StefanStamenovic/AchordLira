using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.ViewModels
{
    public class ViewSong
    {
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string content { get; set; }
        public string creator { get; set; }
        public string date { get; set; }
    }
}