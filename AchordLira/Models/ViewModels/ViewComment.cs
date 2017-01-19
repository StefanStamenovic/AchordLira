using AchordLira.Models.Neo4J.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.ViewModels
{
    public class ViewComment
    {
        public string content { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string date { get; set; }

        public ViewComment(Comment comment,String author)
        {
            title = comment.title;
            content = comment.content;
            this.author = author;
            date = comment.date;
        }
    }
}