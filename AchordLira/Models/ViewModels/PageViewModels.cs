﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models.ViewModels
{
    //---------------------------------------------------------------------------------------------//
    //Main page model class
    public class PageViewModel
    {
        //Connected user
        public ViewUser user = null;
        //List of authors
        public Dictionary<String, List<ViewArtist>> artists { get; set; }
        public List<String> genres;
        //Selected genre
        public String genre;
    }
    //---------------------------------------------------------------------------------------------//
    // Home/Index page model
    public class HomePageViewModel:PageViewModel
    {
        public List<ViewSong> popularSongs { get; set; }
        public List<ViewSongRequest> songRequests;
        public List<ViewSong> latestSongs;
    }
    //---------------------------------------------------------------------------------------------//
    //Song page model
    public class SongPageViewModel : PageViewModel
    {
        public string artist { get; set; }
        public ViewSong song { get; set; }
        public bool favorite = false;
        public List<ViewComment> comments { get; set; }
        public List<ViewSong> artistSongs { get; set; }
    }
    //---------------------------------------------------------------------------------------------//
    //User page model
    public class UserPageViewModel : PageViewModel
    {
        public List<ViewSong> userSongs { get; set; }
        public List<ViewSong> favoritSongs { get; set; }
        public List<ViewSong> requestedSongs { get; set; }
        public string adminNotifications { get; set; }

        public List<ViewUser> userList { get; set; }

        public string genreCount { get; set; }
        public string artistCount { get; set; }
        public string songCount { get; set; }

    }
    //---------------------------------------------------------------------------------------------//
    //Artist page model
    public class ArtistPageViewModel : PageViewModel
    {
        public ViewArtist artist { get; set; }
        public List<ViewSong> artistSongs { get; set; }
    }
    //---------------------------------------------------------------------------------------------//
    //Artist page model
    public class SearchPageViewModel : PageViewModel
    {
        public String text { get; set; }
        public List<ViewSong> matched { get; set; }
    }
    //---------------------------------------------------------------------------------------------//
    //User info model
    public class UserInfoPageViewModel : PageViewModel
    {
        public List<ViewSong> userSongs { get; set; }
        public ViewUser profile { get; set; }
    }
}