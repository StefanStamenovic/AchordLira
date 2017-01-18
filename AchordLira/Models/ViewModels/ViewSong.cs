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
        public string artist { get; set; }
        public string link { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public bool approved { get; set; }

        public String creator { get; set; }

        public ViewSong(Song song,String user,String artist)
        {
            name = song.name;
            link = song.link;
            content = song.content;
            date = song.date;
            approved = true;
            creator = user;
            this.artist = artist;
        }
        public ViewSong(SongDraft draft, String user)
        {
            name = draft.name;
            link = draft.link;
            content = draft.content;
            date = draft.date;
            approved = false;
            creator = user;
            this.artist = draft.artist;
        }

        public ViewSong()
        {
        }
    }
}