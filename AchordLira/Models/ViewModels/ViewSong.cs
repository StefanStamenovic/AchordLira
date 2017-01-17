using AchordLira.Models.Neo4J.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.ViewModels
{
    public class ViewSong
    {
        public string name { get; set; }
        public string link { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public bool approved { get; set; }

        public User creator { get; set; }

        public ViewSong(Song song,User user)
        {
            name = song.name;
            link = song.link;
            content = song.content;
            date = song.date;
            approved = song.approved;
            creator = user;
        }
    }
}