// *********************************************** //
//       SingerJS | Simple chord highlighter.      //     
//       Copyright © 2015 Sergei Kriger            //
// *********************************************** //

function Singer(song) {
    "use strict";

    var notChordsFilter = (/[I-Z]|[А-Я]|[а-я]|c|[e-h]|k|l|[n-r]|t|[v-z]|'|\.|\?|!|:|;|\-/g);
    var coloredSong = "";

    this.init = function () {
        addClass(song);
        displayColoredSong(coloredSong);
    };

    function addClass(song) {
        var text = song.innerHTML,
            lines = text.split(/\n/g);

        //findTitle(lines);
       // findAuthor(lines);
        findChords(lines);

        coloredSong = lines.join("\n");
    }

    function findTitle(lines) {
        if (lines[0].match(notChordsFilter) !== null) {
            lines[0] = "<span class='singer_song_title'>" + lines[0] + "</span>";
        }
    }

    function findAuthor(lines) {
        if (lines[1] !== "" && lines[0].search(/singer_song_title/) !== -1) {
            lines[1] = "<span class='singer_song_author'>" + lines[1] + "</span>";
        }
    }

    function findChords(lines) {
        for (var i = 0; i < lines.length; i++) {
            if (lines[i].match(notChordsFilter) === null && lines[i] !== "") {
                lines[i] = "<span class='singer_song_chords'>" + lines[i] + "</span>";
            }
        }
    }

    function displayColoredSong(coloredSong) {
        song.innerHTML = coloredSong;
    }
}

(function () {
    var song = document.querySelector("#singer_song");
    var singer = new Singer(song);
    singer.init();
})();