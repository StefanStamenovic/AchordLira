using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.ViewModels
{
    public class ViewMessageRequest
    {
        public String author { get; set; }
        public String artist { get; set; }
        public String song { get; set; }
        public String date { get; set; }
    }
}