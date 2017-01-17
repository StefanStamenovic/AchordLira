using AchordLira.Models.Neo4J;
using AchordLira.Models.Neo4J.Models;
using AchordLira.Models.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AchordLira.Models
{
    public class InitDB
    {
        public InitDB()
        {
            Neo4jDataProvider dbNeo4j = new Neo4jDataProvider();
            RedisDataProvider dbRedis = new RedisDataProvider();

            //inicijalizacija ADMIN-a
            User admin = new User();
            admin.name = "Admin";
            admin.email = "admin"; 
            admin.password = "admin";
            admin.link = "admin";
            admin.admin = true;
            admin.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.UserCreate(admin);

            //inicijalizacija USER-1
            User user_1 = new User();
            user_1.name = "User_1";
            user_1.email = "user_1";
            user_1.password = "user_1";
            user_1.link = "user_1";
            user_1.admin = false;
            user_1.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.UserCreate(user_1);

            //inicijalizacija USER-1
            User user_2 = new User();
            user_2.name = "User_2";
            user_2.email = "user_2";
            user_2.password = "user_2";
            user_2.link = "user_2";
            user_2.admin = false;
            user_2.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.UserCreate(user_2);

            //inicijalizacija SongRequest_1
            SongRequest songRequest_1 = new SongRequest();
            songRequest_1.author = "Nemanja";
            songRequest_1.artist = "Pantera";
            songRequest_1.song = "This love";
            songRequest_1.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.SongRequestCreate(songRequest_1);

            //inicijalizacija SongRequest_2
            SongRequest songRequest_2 = new SongRequest();
            songRequest_2.author = "Vuk";
            songRequest_2.artist = "Galija";
            songRequest_2.song = "Mozda sam lud";
            songRequest_2.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.SongRequestCreate(songRequest_2);

            //inicijalizacija SongRequest_3
            SongRequest songRequest_3 = new SongRequest();
            songRequest_3.author = "Marko";
            songRequest_3.artist = "Kerber";
            songRequest_3.song = "Ratne igre";
            songRequest_3.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.SongRequestCreate(songRequest_3);

            //inicijalizacija SongRequest_4
            SongRequest songRequest_4 = new SongRequest();
            songRequest_4.author = "Milica";
            songRequest_4.artist = "Van Gogh";
            songRequest_4.song = "Ludo luda";
            songRequest_4.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.SongRequestCreate(songRequest_4);

            //inicijalizacija SongRequest_5
            SongRequest songRequest_5 = new SongRequest();
            songRequest_5.author = "Ljuba";
            songRequest_5.artist = "Ceca";
            songRequest_5.song = "Kukavica";
            songRequest_5.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.SongRequestCreate(songRequest_5);

            //inicijalizacija SongRequest_6
            SongRequest songRequest_6 = new SongRequest();
            songRequest_6.author = "Ljuba";
            songRequest_6.artist = "Ceca";
            songRequest_6.song = "Kukavica";
            songRequest_6.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.SongRequestCreate(songRequest_6);

            //inicijalizacija zanrova (1)
            Genre genre_1 = new Genre();
            genre_1.name = "folk";
            dbNeo4j.GenreCreate(genre_1);

            //inicijalizacija zanrova (2)
            Genre genre_2 = new Genre();
            genre_2.name = "pop";
            dbNeo4j.GenreCreate(genre_2);

            //inicijalizacija zanrova (3)
            Genre genre_3 = new Genre();
            genre_3.name = "rock";
            dbNeo4j.GenreCreate(genre_3);

            //inicijalizacija zanrova (4)
            Genre genre_4 = new Genre();
            genre_4.name = "funk";
            dbNeo4j.GenreCreate(genre_4);

            //inicijalizacija zanrova (5)
            Genre genre_5 = new Genre();
            genre_5.name = "jazz";
            dbNeo4j.GenreCreate(genre_5);

            //inicijalizacija zanrova (6)
            Genre genre_6 = new Genre();
            genre_6.name = "blues";
            dbNeo4j.GenreCreate(genre_6);

            //inicijalizacija zanrova (7)
            Genre genre_7 = new Genre();
            genre_7.name = "hip hop";
            dbNeo4j.GenreCreate(genre_7);

            //inicijalizacija zanrova (8)
            Genre genre_8 = new Genre();
            genre_8.name = "metal";
            dbNeo4j.GenreCreate(genre_8);





        }
    }
}