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
        public string author { get; set; }
        public string date { get; set; }

        public ViewComment(Comment comment,String autor)
        {
            content = comment.content;
            author = author;
            date = comment.date;
        }
    }
}