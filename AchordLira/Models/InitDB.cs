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

            //inicijalizacija USER-2
            User user_2 = new User();
            user_2.name = "User_2";
            user_2.email = "user_2";
            user_2.password = "user_2";
            user_2.link = "user_2";
            user_2.admin = false;
            user_2.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.UserCreate(user_2);

            //inicijalizacija USER-3
            User user_3 = new User();
            user_3.name = "User_3";
            user_3.email = "user_3";
            user_3.password = "user_3";
            user_3.link = "user_3";
            user_3.admin = false;
            user_3.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.UserCreate(user_3);

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

            //inicijalizacija zanrova (9)
            Genre genre_9 = new Genre();
            genre_9.name = "punk";
            dbNeo4j.GenreCreate(genre_9);

            ////////////inicijalizacija izvodjaca/////////////////////////////////////////////////

            //A
            Artist artist_1 = new Artist();
            artist_1.name = "Aerodrom";
            artist_1.link = "";  //TODO: treba da se napravi kasnije
            artist_1.biography = ""; //TODO: biografija kasnije
            artist_1.website = "https://en.wikipedia.org/wiki/Aerodrom_(band)";
            List<Genre> l1 = new List<Genre>();
            l1.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l1);

            Artist artist_2 = new Artist();
            artist_2.name = "Atomsko skloniste";
            artist_2.link = "";  //TODO: treba da se napravi kasnije
            artist_2.biography = ""; //TODO: biografija kasnije
            artist_2.website = "https://en.wikipedia.org/wiki/Atomsko_skloni%C5%A1te";
            List<Genre> l2 = new List<Genre>();
            l2.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l2);

            //B
            Artist artist_3 = new Artist();
            artist_3.name = "Babe";
            artist_3.link = "";  //TODO: treba da se napravi kasnije
            artist_3.biography = ""; //TODO: biografija kasnije
            artist_3.website = "https://en.wikipedia.org/wiki/Babe_(Serbian_band)";
            List<Genre> l3 = new List<Genre>();
            l3.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l3);

            Artist artist_4 = new Artist();
            artist_4.name = "Bajaga";
            artist_4.link = "";  //TODO: treba da se napravi kasnije
            artist_4.biography = ""; //TODO: biografija kasnije
            artist_4.website = "https://en.wikipedia.org/wiki/Bajaga_i_Instruktori";
            List<Genre> l4 = new List<Genre>();
            l4.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l4);

            //C
            Artist artist_5 = new Artist();
            artist_5.name = "Crvena Jabuka";
            artist_5.link = "";  //TODO: treba da se napravi kasnije
            artist_5.biography = ""; //TODO: biografija kasnije
            artist_5.website = "https://en.wikipedia.org/wiki/Crvena_jabuka";
            List<Genre> l5 = new List<Genre>();
            l5.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l5);

            //D
            Artist artist_6 = new Artist();
            artist_6.name = "Divlje Jagode";
            artist_6.link = "";  //TODO: treba da se napravi kasnije
            artist_6.biography = ""; //TODO: biografija kasnije
            artist_6.website = "https://en.wikipedia.org/wiki/Divlje_jagode";
            List<Genre> l6 = new List<Genre>();
            l6.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l6);

            Artist artist_7 = new Artist();
            artist_7.name = "Dejan Cukic";
            artist_7.link = "";  //TODO: treba da se napravi kasnije
            artist_7.biography = ""; //TODO: biografija kasnije
            artist_7.website = "https://en.wikipedia.org/wiki/Dejan_Cuki%C4%87";
            List<Genre> l7 = new List<Genre>();
            l7.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l7);

            //E
            Artist artist_8 = new Artist();
            artist_8.name = "EKV";
            artist_8.link = "";  //TODO: treba da se napravi kasnije
            artist_8.biography = ""; //TODO: biografija kasnije
            artist_8.website = "https://en.wikipedia.org/wiki/Ekatarina_Velika";
            List<Genre> l8 = new List<Genre>();
            l8.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l8);

            Artist artist_9 = new Artist();
            artist_9.name = "Elektricni orgazam";
            artist_9.link = "";  //TODO: treba da se napravi kasnije
            artist_9.biography = ""; //TODO: biografija kasnije
            artist_9.website = "http://www.elektricniorgazam.com/";
            List<Genre> l9 = new List<Genre>();
            l9.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l9);

            //F
            Artist artist_10 = new Artist();
            artist_10.name = "Flamingosi";
            artist_10.link = "";  //TODO: treba da se napravi kasnije
            artist_10.biography = ""; //TODO: biografija kasnije
            artist_10.website = "https://sh.wikipedia.org/wiki/Flamingosi";
            List<Genre> l10 = new List<Genre>();
            l10.Add(genre_1);
            l10.Add(genre_2);
            dbNeo4j.ArtistCreate(artist_1, l10);

            //G
            Artist artist_11 = new Artist();
            artist_11.name = "Galija";
            artist_11.link = "";  //TODO: treba da se napravi kasnije
            artist_11.biography = ""; //TODO: biografija kasnije
            artist_11.website = "https://en.wikipedia.org/wiki/Galija";
            List<Genre> l11 = new List<Genre>();
            l11.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l11);

            Artist artist_12 = new Artist();
            artist_12.name = "Goblini";
            artist_12.link = "";  //TODO: treba da se napravi kasnije
            artist_12.biography = ""; //TODO: biografija kasnije
            artist_12.website = "http://goblini.com/";
            List<Genre> l12 = new List<Genre>();
            l12.Add(genre_9);
            dbNeo4j.ArtistCreate(artist_1, l12);

            //H
            Artist artist_13 = new Artist();
            artist_13.name = "Hladno pivo";
            artist_13.link = "";  //TODO: treba da se napravi kasnije
            artist_13.biography = ""; //TODO: biografija kasnije
            artist_13.website = "https://en.wikipedia.org/wiki/Hladno_pivo";
            List<Genre> l13 = new List<Genre>();
            l13.Add(genre_9);
            dbNeo4j.ArtistCreate(artist_1, l13);

            //I
            Artist artist_14 = new Artist();
            artist_14.name = "Idoli";
            artist_14.link = "";  //TODO: treba da se napravi kasnije
            artist_14.biography = ""; //TODO: biografija kasnije
            artist_14.website = "https://bs.wikipedia.org/wiki/Idoli";
            List<Genre> l14 = new List<Genre>();
            l14.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l14);

            //K
            Artist artist_15 = new Artist();
            artist_15.name = "Kerber";
            artist_15.link = "";  //TODO: treba da se napravi kasnije
            artist_15.biography = ""; //TODO: biografija kasnije
            artist_15.website = "https://en.wikipedia.org/wiki/Kerber";
            List<Genre> l15 = new List<Genre>();
            l15.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l15);

            Artist artist_16 = new Artist();
            artist_16.name = "Kraljevski apartman";
            artist_16.link = "";  //TODO: treba da se napravi kasnije
            artist_16.biography = ""; //TODO: biografija kasnije
            artist_16.website = "https://en.wikipedia.org/wiki/Kraljevski_Apartman";
            List<Genre> l16 = new List<Genre>();
            l16.Add(genre_9);
            dbNeo4j.ArtistCreate(artist_1, l16);

            //L
            Artist artist_17 = new Artist();
            artist_17.name = "Leb i Sol";
            artist_17.link = "";  //TODO: treba da se napravi kasnije
            artist_17.biography = ""; //TODO: biografija kasnije
            artist_17.website = "https://en.wikipedia.org/wiki/Leb_i_sol";
            List<Genre> l17 = new List<Genre>();
            l17.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l17);

            //M
            Artist artist_18 = new Artist();
            artist_18.name = "Metallica";
            artist_18.link = "";  //TODO: treba da se napravi kasnije
            artist_18.biography = ""; //TODO: biografija kasnije
            artist_18.website = "https://metallica.com/";
            List<Genre> l18 = new List<Genre>();
            l18.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l18);

            //N
            Artist artist_19 = new Artist();
            artist_19.name = "Negativ";
            artist_19.link = "";  //TODO: treba da se napravi kasnije
            artist_19.biography = ""; //TODO: biografija kasnije
            artist_19.website = "https://en.wikipedia.org/wiki/Negative_(Serbian_band)";
            List<Genre> l19 = new List<Genre>();
            l19.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l19);

            Artist artist_20 = new Artist();
            artist_20.name = "Neverne bebe";
            artist_20.link = "";  //TODO: treba da se napravi kasnije
            artist_20.biography = ""; //TODO: biografija kasnije
            artist_20.website = "http://www.neverne-bebe.com/";
            List<Genre> l20 = new List<Genre>();
            l20.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l20);

            //O
            Artist artist_21 = new Artist();
            artist_21.name = "Osvajaci";
            artist_21.link = "";  //TODO: treba da se napravi kasnije
            artist_21.biography = ""; //TODO: biografija kasnije
            artist_21.website = "https://en.wikipedia.org/wiki/Osvaja%C4%8Di";
            List<Genre> l21 = new List<Genre>();
            l21.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l21);

            //P
            Artist artist_22 = new Artist();
            artist_22.name = "Parni Valjak";
            artist_22.link = "";  //TODO: treba da se napravi kasnije
            artist_22.biography = ""; //TODO: biografija kasnije
            artist_22.website = "https://en.wikipedia.org/wiki/Parni_Valjak";
            List<Genre> l22 = new List<Genre>();
            l22.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l22);

            Artist artist_23 = new Artist();
            artist_23.name = "Plavi Orkestar";
            artist_23.link = "";  //TODO: treba da se napravi kasnije
            artist_23.biography = ""; //TODO: biografija kasnije
            artist_23.website = "https://en.wikipedia.org/wiki/Plavi_orkestar";
            List<Genre> l23 = new List<Genre>();
            l23.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l23);

            //R
            Artist artist_24 = new Artist();
            artist_24.name = "Riblja Corba";
            artist_24.link = "";  //TODO: treba da se napravi kasnije
            artist_24.biography = ""; //TODO: biografija kasnije
            artist_24.website = "http://riblja-corba.com/";
            List<Genre> l24 = new List<Genre>();
            l24.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l24);

            Artist artist_25 = new Artist();
            artist_25.name = "Ritam Nereda";
            artist_25.link = "";  //TODO: treba da se napravi kasnije
            artist_25.biography = ""; //TODO: biografija kasnije
            artist_25.website = "https://en.wikipedia.org/wiki/Ritam_Nereda";
            List<Genre> l25 = new List<Genre>();
            l25.Add(genre_9);
            l25.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l25);

            //S
            Artist artist_26 = new Artist();
            artist_26.name = "Smak";
            artist_26.link = "";  //TODO: treba da se napravi kasnije
            artist_26.biography = ""; //TODO: biografija kasnije
            artist_26.website = "https://en.wikipedia.org/wiki/Smak";
            List<Genre> l26 = new List<Genre>();
            l26.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_1, l26);

            Artist artist_27 = new Artist();
            artist_27.name = "SARS";
            artist_27.link = "";  //TODO: treba da se napravi kasnije
            artist_27.biography = ""; //TODO: biografija kasnije
            artist_27.website = "https://en.wikipedia.org/wiki/S.A.R.S.";
            List<Genre> l27 = new List<Genre>();
            l27.Add(genre_3);
            l27.Add(genre_2);
            dbNeo4j.ArtistCreate(artist_1, l27);

            //T
            Artist artist_28 = new Artist();
            artist_28.name = "Tap 011";
            artist_28.link = "";  //TODO: treba da se napravi kasnije
            artist_28.biography = ""; //TODO: biografija kasnije
            artist_28.website = "https://en.wikipedia.org/wiki/Tap_011";
            List<Genre> l28 = new List<Genre>();
            l28.Add(genre_3);
            l28.Add(genre_2);
            dbNeo4j.ArtistCreate(artist_1, l28);

            //V
            Artist artist_29 = new Artist();
            artist_29.name = "Van Gogh";
            artist_29.link = "";  //TODO: treba da se napravi kasnije
            artist_29.biography = ""; //TODO: biografija kasnije
            artist_29.website = "https://en.wikipedia.org/wiki/Van_Gogh_(band)";
            List<Genre> l29 = new List<Genre>();
            l29.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_29, l29);

            Artist artist_30 = new Artist();
            artist_30.name = "Vlada Divljan";
            artist_30.link = "";  //TODO: treba da se napravi kasnije
            artist_30.biography = ""; //TODO: biografija kasnije
            artist_30.website = "https://en.wikipedia.org/wiki/Vlada_Divljan";
            List<Genre> l30 = new List<Genre>();
            l30.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_30, l30);

            //Y
            Artist artist_31 = new Artist();
            artist_31.name = "Yu grupa";
            artist_31.link = "";  //TODO: treba da se napravi kasnije
            artist_31.biography = ""; //TODO: biografija kasnije
            artist_31.website = "https://en.wikipedia.org/wiki/YU_Grupa";
            List<Genre> l31 = new List<Genre>();
            l31.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_31, l31);

            //Z
            Artist artist_32 = new Artist();
            artist_32.name = "Zana";
            artist_32.link = "";  //TODO: treba da se napravi kasnije
            artist_32.biography = ""; //TODO: biografija kasnije
            artist_32.website = "https://sh.wikipedia.org/wiki/Zana_(grupa)";
            List<Genre> l32 = new List<Genre>();
            l32.Add(genre_3);
            dbNeo4j.ArtistCreate(artist_32, l32);

            //inicijalizacija pesama 
            Song song_1 = new Song();
            song_1.name = "Digni me visoko";
            song_1.link = "/Aerodrom/Digni me visoko";
            song_1.content = @"C
Uzmi me sa sobom i povedi me
  G            C G
I povedi me, i povedi me
C
Digni me visoko da ti ispričam
      G C           G
Svoje snove sve, svoje snove sve.


Odvedi me do mora gdje su djevojke
Gdje su djevojke, gdje su djevojke
Koje nikad nikom nisu rekle ne
Nisu rekle ne, nisu rekle ne.


Am C
Al' ja ipak nisam luda
Am C
Koja vjeruje u čuda
Am C
Što se kriju iza mora
  F        G
I gora dalekih.


Spusti me na zamlju, ali oprezno
Ali oprezno, ali oprezno
Hrabar sam, ali neću izazivat zlo
Izazivat zlo, izazivat zlo.

Možda jednog dana ipak uspijem
Ipak uspijem ipak uspijem
Samo ako prije se ne napijem
Se ne napijem, se ne napijem.

Jer ja ipak nisam luda
Koja vjeruje u čuda
Što se kriju iza mora
I gora dalekih.";
            song_1.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_1.approved = true;
            dbNeo4j.SongCreate(song_1, "User_1", "Aerodrom");

            //inicijalizacija pesama 
            Song song_2 = new Song();
            song_2.name = "Bez Kaputa";
            song_2.link = "/Atomsko skloniste/Bez kaputa";
            song_2.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_2.approved = true;
            song_2.content = @"Uvod: G A# Am  
      F C G
      G A# Am E

C             Am
Zbog tebe sam navlaio odijela
Jer si me takvog htjela
I zbog jedne smiješne kravate
Uspjeli su da te odvrate

Duša mi ulicom luta
Bez kaputa, bez kaputa
Srce mi kuća jače
Spale su mi hlače

Duša mi ulicom luta
Bez kaputa, bez kaputa
A glava neda mira
Bez šešira bez šešira

Uvod.

Zbog tebe sam stavljao manžetne
Za uzdahe kad nas netko sretne
A onda je došlo do skandala
Kad su mi dva dugmeta otpala.";
            dbNeo4j.SongCreate(song_2, "User_1", "Atomsko skloniste");

            Song song_3 = new Song();
            song_3.name = "Ko Me Ter'o";
            song_3.link = "/Babe/Ko Me Ter'o";
            song_3.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_3.approved = true;
            song_3.content = @"Am         G           C   E 
Sticajem čudnih okolnosti
dok sam tonuo u dosadu
ruku spasa si mi pružila
Am         G        Am  G 
time si me ti zadužila.

Znao sam da si prevrtljiva
i na onu stvar nasrtljiva
o tebi sam čuo zao glas
kako posrneš za tili čas.


            Am          G
Ref:  Ko me ter'o da te volim?      
            C E                      
      Ko me ter'o?                   
      Am          G                     
      Ko me ter'o da te volim?       
            AmG (a može i E7)        
      Ko me ter'o?                  


Majka mi je lepo branila
da iz kuće izlazim tad ja.
Ali ja je nisam slušao,
tebe sam te noći probao.

Morao sam tebe probati,
morao sam jer sam bio lud.
Morao sam sebe kazniti,
morao sam nisam imao kud.


Ref:  Ko me ter'o da... 2x";
            dbNeo4j.SongCreate(song_3, "User_1", "Babe");

            Song song_4 = new Song();
            song_4.name = "220 u voltima";
            song_4.link = "/Bajaga/220 u voltima";
            song_4.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_4.approved = true;
            song_4.content = @"F 
C                   F      G 
Pale se svetla zuji elektrika 
C                       F     G 
Kroz vazduh kruži čista energija 
C                     F   G 
Gledam u pod a pod se pomera 
        C   F   G 
Pod nogama 
 
E-Dur mi izlazi iz svih ćelija 
Doboš me udara samo na dva 
Ja ne znam koliko imam napona 
U voltima 
 
ref: 
      C                  F   G 
To je taj   zvuk koji me pomera 
      C                  F  G 
To je taj   zvuk koji me obara 
      C                  F   G 
To je taj   zvuk koji me pomera 
       C               F    G 
Sa dvesta   dvadeset u voltima 
       C   F  G 
U voltima";
            dbNeo4j.SongCreate(song_4, "User_1", "Bajaga");

            Song song_5 = new Song();
            song_5.name = "Godine Prolaze";
            song_5.link = "/Bajaga/Godine Prolaze";
            song_5.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_5.approved = true;
            song_5.content = @"Intro: D A Hm G D A D G A    x2

G A      D      A                             
Sećam se slike ja 
          Hm    G
Od pre par godina
         D     A  
Na slici proleće 
         D   G
i sunčan dan    
  A     D    A
U bašti mirisa
         Hm   G
cvetovi irisa
       D     A
Šarena hartija
       D    G A
I ti i i ja
   
Sećam se slike ja
pre jedno mesec dva 
na slici decembarski 
vreo dan 

Na reku uzdaha
sunce se ne vraća
zelena obala 
i ti i ja
   
Ref:
   
       D     A 
Godine prolaze
         Hm    G
Nervoznim korakom
       D     A
Godine prolaze
        D   G 
mi stojimo
   A     D     A
Jesmo li sretniji
       Hm    G
jesmo pametniji
       D    A  
godine prolaze
         D  G A
Mi gledamo";
            dbNeo4j.SongCreate(song_5, "User_1", "Bajaga");

            Song song_6 = new Song();
            song_6.name = "Gore Dole";
            song_6.link = "/Bajaga/Gore Dole";
            song_6.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_6.approved = true;
            song_6.content = @"G
Eeeeeeee Eeeeeeeeeaaa
D         C
Eeeeeeeee  Eeeeeeeeeaa

G
Nemoj nikad biti dole
Mada neki i to vole
Nemoj nikad biti dole
Jer se dole noge bole

     D           C
Gore dole   dole gore
         G   
Sve je gore  dole

Nemoj nikad biti gore
Za to uvek ima fore
Nemoj nikad biti gore
Jer si gore i pregore
Nemoj nikad biti dole
Amda neki i to vole
Nemoj nikad biti dole
Jer se dole noge bole

Gore.....


C     D    G
Idemo malo dole
C     D    G
Idemo malo gore


Nemoj nikad biti dole
Mada neki i to vole
Nemoj nikad biti dole
Jer se dole noge bole

Gore dole...

Nemoj biti u sredini 
Jer ćeš biti u manjini
Nemoj biti u sredini
Ili prvi bar otkini

Gore ...

Idemo malo.... ";
            dbNeo4j.SongCreate(song_6, "User_1", "Bajaga");

            Song song_7 = new Song();
            song_7.name = "Moji su drugovi";
            song_7.link = "/Bajaga/Moji su drugovi";
            song_7.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_7.approved = true;
            song_7.content = @"         Am             E7              Am    
Moji su drugovi biseri rasuti po celom svetu
          C            G                C       
I ja sam selica pa ih ponekad sretnem u letu
         F                         C
Dal’ je to sudbina il ko zna šta li je
            F                  C
Kad god se sretnemo uvek se zalije
       E7                       Am
Uvek se završi s nekom od naših pesama

Moji su drugovi, žestoki momci velikog srca
I kad se pije i kad se ljubi i kad se puca
Gore od Aljaske do australije
Kad god se sretnemo uvek se zalije
uvek se završi s nekom od naših pesama

       G                    C
Da smo živi i zdravi još godina sto
       G                    C
Da je pesme i vina i da nas čuva Bog
       G                    C
Da su najbolje žene uvek pored nas
              E7                Am
Jer ovaj život je kratak i prozuji za čas

Za moje drugove ja molim vetrove za pune jedra
Puteve sigurne a noći zvezdane i jutra vedra
Dal’ je to sudbina il ko zna šta li je
Kad god se sretnemo uvek se zalije
Uvek se završi s nekom od naših pesama

Da smo živi i zdravi još godina što
Da je pesme i vina i da nas čuva bog
Da su najbolje žene uvek pored nas
Jer ovaj život je kratak i prozuji za čas";
            dbNeo4j.SongCreate(song_7, "User_1", "Bajaga");

            Song song_8 = new Song();
            song_8.name = "Vesela pesma";
            song_8.link = "/Bajaga/Vesela pesma";
            song_8.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_8.approved = true;
            song_8.content = @"C                     G              C               
Kako sam tužan kad se setim moja jedina 
                  G              C 
Koliko noći pored mene nisi spavala 
                        G               C 
Tužna detinjstva svoga odmah sam se setio 
                   G               C 
Tužan sam bio a da nisam ni primetio 
 
Neću da pitam dušo šta si noćas radila 
U mome srcu gorku travu si posadila 
Ne slušam zato šta mi ljudi govore 
Ja noćas pijem samo zbog ljubomore 
 
 
bridge: 
 
C                 Fmaj7         C 
Sijaju zvezde kao oči konobarice 
                     G             C 
Da si bar ovde pa da dođu i drugarice 
                     Fmaj7          C   
niz moje grlo klizi ljuta prepečenica 
                   G             C 
neka me složi kao složena rečenica 
 
 
Pomozi Bože ne znam kako ću sa ženam 
Ne znaš jel' piće sada teče tvojim venam 
Zbog jedne žene koja dušu mi je ukrala 
Ti noćas piješ,nek poteče rakija 
 
ref: 
C                  G              
Ja moram da pijem da zaboravim 
C                  G 
Ja ne smem trezan tu da boravim";
            dbNeo4j.SongCreate(song_8, "User_1", "Bajaga");

            Song song_9 = new Song();
            song_9.name = "Plavi safir";
            song_9.link = "/Bajaga/Vesela pesma";
            song_9.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_9.approved = true;
            song_9.content = @"A               
Kuda si posla sa tim tamnim očima
A
I čije ime ti na usni počiva
        D
Kaži mi ko ti gužva postelju
G        D             A
I ko ima zlatne ključeve od tvojih tajnih odaja

Daj da te pratim gde god da si krenula
Srećan je onaj s kim si noći delila
Daj da te pratim i da budem sena
Da ne budeš usamljena kad po vodi budeš hodala
 
Kuda si pošla sa tim tamnim očima
I čije ime ti na usni počiva
Kaži mi ko ti gužva postelju
I ko ima zlatne ključeve od tvojih tajnih odaja

Daj da te pratim gde god da si krenula
Srećan je onaj s kim si noći delila
Daj da te pratim i da budem sena
Da ne budeš usamljena kad po vodi budeš hodala

A
Hajde  plavi moj safiru
A
Hajde čežnjo i nemiru
D      G      D      A
Hajde dođi i ostani tu

Duvaju promaje kroz vrata naroda
Tuda smo prošli mi još davno nekada
Kaži mi ko ti gužva postelju 
I ko ima ključeve od tvojih tajnih odaja";
            dbNeo4j.SongCreate(song_9, "User_1", "Bajaga");

            Song song_10 = new Song();
            song_10.name = "Lepa Janja Ribareva Kci";
            song_10.link = "/Bajaga/Lepa Janja Ribareva Kci";
            song_10.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_10.approved = true;
            song_10.content = @"Uvod: D A Hm F#m D A G 
 
D                 A
Maslina je oliva, mene sunce poliva
Hm                  F#m
prozvale su kajsije rumene,
G                   D
pa se vidi jasnije, pa se čuje glasnije
A
na celom svetu od sto milja
 
Mene sunce ne dira, zrake loše servira
pa te dobro ne vidim ovaj put,
onda vidim jebo te, nigde takve lepote
na celom svetu od sto milja
 
Refren.
G A   Hm        F#m  
Niko, lepši nego ti,
G           D        A
lepa Janja, ribareva kći
         Hm F#m  Hm        F#m   
na celom svetu, lepote takve mi, 
G         D        A
samo lepa ribareva kći
 
Maslina je oliva, mene sunce poliva
prozvale su kajsije rumene,
pa se vidi jasnije, pa se cue glasnije
na celom svetu od sto milja
 
Mene sunce ne dira, zrake loshe servira
istopiti maškaru, svaki put
pa se ljutiš na mene, pa mi tražiš zamene
na celom svetu od sto milja
 
ref.
 
(A D A Hm F#m G D A)
 
Onda pesme drugi deo, da promenim ja bi hteo
jer ne volim depresivne krajeve,
zato želim ribarima, puno sreće sviovima
i punim mreže ribe sveže

ref.

(D A Hm F#m G D A A)";
            dbNeo4j.SongCreate(song_10, "User_1", "Bajaga");

            Song song_11 = new Song();
            song_11.name = "Dok Sanjas";
            song_11.link = "/Crvena Jabuka/Dok Sanjas";
            song_11.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_11.approved = true;
            song_11.content = @"  D 
Ispisacu poneku rijec i za tebe 
        G             D 
Vjekova mir vracas mi ti 
D 
Smirujes vode nabujale 
    G                  D 
Moj sapat i krik ces pomiriti 
A                            G 
Pijem ti dah kao rijec svaku noc 
               D 
Prije nego sto usnijes 
       A                     G 
I taj tren i zauvijek i samo ti 
           D 
Ti si mi u mislima. 
 
 
           G 
I kad nisi tu kraj mene, da znas da me bolis 
  D 
I nocas ja cu da te cuvam dok spavas 
   A 
I ljubim tamo gdje najvise volis 
    D 
Dok sanjas. 
 
 
Hodao sam ja bos po ostrici 
Kamen samoce zivot mi gradio 
I stalno cekas lopova da karte otvori 
Hiljadu puta dosad me uvalio 
Ljubim ti oci ko u bunilu 
Svaku noc tu boju usnim ja 
Ti si izvjesnost u ovom ludilu 
I jedino sunce sto mi sja. 
 
I kad nisi tu kraj mene, da znas da me bolis 
I nocas ja cu da te cuvam dok spavas 
I ljubim tamo gdje najvise volis 
Dok sanjas.";
            dbNeo4j.SongCreate(song_11, "User_1", "Crvena Jabuka");

            Song song_12 = new Song();
            song_12.name = "Jedina Moja";
            song_12.link = "/Divlje Jagode/Jedina moja";
            song_12.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_12.approved = true;
            song_12.content = @"Em              Am 
Pogledaj u moje oci 
C     D              Em 
Zasto skrivas pogled taj 
Em              Am 
Pogledaj u moje lice 
C   D            Em 
I pokazi osmijeh svoj. 
 
 
Am    C           Em 
Djevojko u sutonu tihom 
D                   G 
Pokraj rijeke sto koracas 
Am       C         Em 
S tuznom sjenom na licu svom 
C              D                   Em 
I kosom njeznom i kosom plavom kao lan. 
 
 
G                Am 
Jedina moja tebi sviram ja 
D                Em 
Jedina moja tebi pjevam ja 
G                Am 
Jedina moja tebi sviram ja 
D                Em 
Jedina moja tebi pjevam ja. 
    Am  D Dsus Em 
Hej hej.";
            dbNeo4j.SongCreate(song_12, "User_1", "Divlje Jagode");

            Song song_13 = new Song();
            song_13.name = "Mokre Ulice";
            song_13.link = "/Dejan Cukic/Mokre Ulice";
            song_13.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_13.approved = true;
            song_13.content = @"F---am--------G  
Sanjam kabanice
G             C            G
da li znas da moja ljubav nema granice
G          C         D
ipak nije nuzno biti uvek kraj tebe
em     D        G
jer ja te volim Milice

Da li znas za razlog 
zasto nocu spavam sam
mozda bolje bude ako pogledas u dlan
a dlan mi zeno oderan

D                    C        G
Ja stojim ovde visok do tavanice
D                   C         G
i moja ljubav za te nema granice
em     D        G 
jer ja te volim Milice

G            C          G
prolazim niz beogradske mokre ulice
G            C           D
u mislima ja nosim tvoje poljupce 
em       D        G
i sanjam bez kabanice

D                    C        G
Ja stojim ovde visok do tavanice
D                   C         G
i moja ljubav za te nema granice
em     D        G
jer ja te volim Milice
em       D           F--am--G
i sanjam bez kabanice
          F---am---G     
a sve niz mokre ulice

Ja stojim ovde visok do tavanice
i moja ljubav za te nema granice
jer ja te volim Milice
jer ja te volim Milice";
            dbNeo4j.SongCreate(song_13, "User_1", "Dejan Cukic");

            Song song_14 = new Song();
            song_14.name = "Budi Sam";
            song_14.link = "/EKV/Budi Sam";
            song_14.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_14.approved = true;
            song_14.content = @"Intro: H   G   Esus2

 	 Abm
Treba mi svet
            Db
Otvoren za poglede
              Dbm
Otvoren za trcanje

I treba mi soba
Da primi pet hiljada ljudi
Sa dignutim casama
Sa dignutim casama

I lomi se kristal
Svetlucaju staklene iskre
Pod nasim nogama
Kao potpuni stranci
Sa staklom u ocima
Sa staklom u grudima

   E
Na licima

H                       G     Esus2
Budi sam, na ulici budi sam
Budi sam, na ulici budi sam";
            dbNeo4j.SongCreate(song_14, "User_1", "EKV");

            Song song_15 = new Song();
            song_15.name = "Bejbe ti nisi tu";
            song_15.link = "/Elektricni orgazam/Bejbe ti nisi tu";
            song_15.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_15.approved = true;
            song_15.content = @"C
Jutro tek je svanulo, 
G
nezno me dodirnulo
F	  C
Budim se, shvatam, vise nisi tu.
C	     F
Otvori oci bejbe,
     G		  C
ovaj dan je tvoj bejbe    
F	    	  G	        C
Al' bejbe, bejbe, bejbe ti nisi tu.


Ref.
C			    G
Bejbe, bejbe, bejbe ti nisi tu	
Am			   F
Kazem, bejbe, bejbe, babe ti nisi tu	   
C
Kada dodje san, tada znam
F
Sanjam te zadnji put    
C		  G		C
Jer bejbe, bejbe, bejbe ti nisi tu.

Mislis da si pametna
U stvari, bas si blesava
Nema te, to je kraj, to je kraj
A gde ces sada bejbe, 
ti si sasvim luda bejbe
Jer bejbe, bejbe, bejbe ti nisi tu.";
            dbNeo4j.SongCreate(song_15, "User_1", "Elektricni Orgazam");

            Song song_16 = new Song();
            song_16.name = "Obala";
            song_16.link = "/Flamingosi/Obala";
            song_16.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_16.approved = true;
            song_16.content = @"SOLO:
Fm                               G#          Fm
9-9/(7)-7-7/(5)-5-5----------------------------)
---------------------9/(10)/(9)----------------)
-------------------------------6-9-------------)2X
------------------------------------7/(6)------)
-------------------------------------------7-9-)
-----------------------------------------------)
1STROFA
Fm       G#
Falis mi ti
            D#
da produzis sa mnom ka obali
           A#m
i pomognes mi
   C#
da zavolim bas sve
D#             Fm
godine sto dolaze
2STROFA
Fm       G#
Falis mi ti
          D#
shvatices ako se okrenes
        A#m
i zaboravis sve
    C#
sto srce steze ti
    D#          Fm
sve zakone dosadne
REFREN:
Fm       G#     D#         A#m
Jer ti i ja moramo da probamo

da spavamo pod nebom
C#         D#  G#
i po vatri hodamo
G#    D#          A#m
u kolima bez tablica

uz pesmu svud po svetu
C#       D#  G#
putevima lutamo

G#  D#           A#m
Jedemo gde stignemo

i bas pred punom plazom
C#         D#    G#
goli da se skinemo
G#   D#          A#m
granice ne postoje

i sve sto treba
      C#        D#   Fm
jeste ljubav da potraje
SOLO:......
1STROFA....
2STROFA....
REFREN:....
1STROFA....
      C#            D#            Fm
Jedan zivot malo je krenimo do obale";
            dbNeo4j.SongCreate(song_16, "User_1", "Flamingosi");

            Song song_17 = new Song();
            song_17.name = "Da Me Nisi";
            song_17.link = "/Galija/Da Me Nisi";
            song_17.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_17.approved = true;
            song_17.content = @"G                F          C
Kad me neka zena podseti na tebe,
G               F           C
dugo ne mogu da pobegnem od sebeâ€¦
G                F     C       G       F  C
Najbolje bi bilo da me nisi ni volela!

G               F        C
Brodovi su moji duboko u moru,
G                F           C
cujem te u skoro svakom razgovoruâ€¦
G                F     C       G       F  C
Najbolje bi bilo da me nisi ni volela!


   G         D     C      D   G          D   C
Da me nisi toliko shvatila, otisla bi i ne bi patila,
   D G         D     C           D
da me nisi toliko shvatila, bio bih srecnijiâ€¦
  G         D    C       D   G          D   C
Da mi nisi toliko znacila, ova noc bi bila drugacija,
  D G         D    C           D
da mi nisi toliko znacila, bio bih srecniji 



G                F          C
Nocas je na nebu samo jedna zvezda,
G                 F          C
najlepsa je ptica ostala bez gnezdaâ€¦
G                F     C       G       F  C
Najbolje bi bilo da me nisi ni volela!

G                F         C
Nije lako, duso, pregaziti reku 
G                 F        C
kad se skupi neka suza u covekuâ€¦
G                F     C       G       F  C
Najbolje bi bilo da me nisi ni volela!


 G         D     C      D   G          D   C
 Da me nisi toliko shvatila, otisla bi i ne bi patila,
  D G         D     C           D
 da me nisi toliko shvatila, bio bih srecnijiâ€¦
  G         D    C       D   G          D   C
Da mi nisi toliko znacila, ova noc bi bila drugacija,
  D G         D    C           D
da mi nisi toliko znacila, bio bih srecniji";
            dbNeo4j.SongCreate(song_17, "User_2", "Galija");

            Song song_18 = new Song();
            song_18.name = "U Magnovenju";
            song_18.link = "/Goblini/U Magnovenju";
            song_18.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_18.approved = true;
            song_18.content = @"C                 G
Distorzija misli, u centru glave
C             G          C   G
čučim u uglu i buljim u akvarijum
C              G
moj pas glasno duva u trubu
Am   G   F
U Magnovenju

Nameštam usta za A izgovaram U
patuljci me zovu zaboravljenim imenom
U Magnovenju

Ref:

Am G F C             G
O o o, Uzalud tražim pogodan citat
O o o, psiholog brani doktorat na temu
O o o, uzalud tražim pogodan citat
Am G  F
U Magnovenju

Tražim razlog da se utopim u masu
duša mi leti dok razmišljam o svemu
moj pas glasno duva u trubu
Radimo non - stop.";
            dbNeo4j.SongCreate(song_18, "User_2", "Goblini");

            Song song_19 = new Song();
            song_19.name = "Pitala Si Me";
            song_19.link = "/Hladno pivo/Pitala Si Me";
            song_19.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_19.approved = true;
            song_19.content = @"Fmaj7
Pitala si me zasto tvoje ime
Am
Nikad ne provlacim kroz rime
         Fmaj7
Zasto ga nikad ne ukrasim metaforama krasnim
      C              Am            G
Pa da se time hvalis kolegicama s posla

Ma nema sanse nema tih para
Da mi ikad budes dio repertoara
Pa da tvoje ime pjevusi ko stigne
Da si svako tek tako na brzinu puni tisinu

Mislim da bih razbio gitaru
Da cujem da te fickaju na pisoaru
Da me jednog dana gledas ponizeno
Iz kuta ducana gdje je sve u pola snizeno


     Am                 F              C   G
Ova pjesma o tebi koja si mi sve na svijetu
      Am                F        C  C/H
Ovaj pokusaj da kazem ne izrecivo
           Am          F      C G
Nikad se nece naci na nekom cdju
       Am     F            G
Imati naslov cjenu i bar kod


Zar da radio voditelj krestava glasa
Preko nase himne trazi vlasnike pasa
Da se cereka, kao dobra je zeka
U svaki jebeni singl ubaciti jingle


Am
Nista nije sveto
        F
Sve je bruto i neto,
       C    G
Sve je zabava          x2


Bitno da nesto svira za zedna uha
Da nikog ne dira dok pere il' kuha
Da narodu skrati onih osam sati
I da lijepo stane izmedju dvije reklame

- Refren - 

Pitala si me zasto tvoje ime
Nikad ne provlacim kroz rime
Zasto ga ne nikad ne ukrasim metaforama krasnim
Pa da se time hvalis kolegicama s posla";
            dbNeo4j.SongCreate(song_19, "User_2", "Hladno pivo");

            Song song_20 = new Song();
            song_20.name = "Malena";
            song_20.link = "/Idoli/Malena";
            song_20.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_20.approved = true;
            song_20.content = @"C          Em7       Am          G
Jedina, malena, volim te, sakam te
jedina, malena, zelim da
jedina, malena, volim te, sakam te
jedina, malena, zelim da fukam te

...

Ref.

C             Em7      Am                G 
Bez tebe ne mogu da spavam
bez tebe vise ja ne ucim ko pre
i samo mogu jos da lutam
i tako probam da zaboravim sve";
            dbNeo4j.SongCreate(song_20, "User_1", "Idoli");

            Song song_21 = new Song();
            song_21.name = "Medena";
            song_21.link = "/Kerber/Medena";
            song_21.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_21.approved = true;
            song_21.content = @"Intro-Am C D C-D X3
      Am G D

PRELAZ-

A-------0-3-0---0
E-0-0-3-------3--


Am     C
Sasvim slucajno smo
D        E
na ovom svetu
Am      c       D       pr.
sretnemo se kao ptice u letu.
Am     C             D       E       
Svako ljubav trazi retko ko nalazi. Am G D

Sasvim slucajno se susretnu oci
pomislimo tada sve cemo moci
jednom kad smo stali
stranci smo postali.

ref:
      Am    c    G          D
Hajde digni me, digni me iz snova
    Am  C   G         D
promeni se budi malo nova,
     Am  C  G            D   Am  G D
moja medena sto si tako ledena.

intro(pratnja uz kratki solo)x1

Padamo polako kao kapi kise
osecamo kako ne disemo vise.
Uzalud su reci sve je tako slucajno.

Hajde digni me, digni me iz snova
promeni se budi malo nova,
moja medena sto si tako ledena

intro(pratnja uz duzi solo) x2

ref:x puta";
            dbNeo4j.SongCreate(song_21, "User_1", "Kerber");

            Song song_22 = new Song();
            song_22.name = "Jesen";
            song_22.link = "/Kraljevski apartman/Jesen";
            song_22.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_22.approved = true;
            song_22.content = @"Em                   D
Jesen je duga hladna, padaju kise
Am           C          D
U mojoj kosi jedna seda vise
Em            D
Prolaze godine sve dok trepnes okom
Am                   C            D
Kako vreme brzo leti nekim svojim tokom
C     Hm     Em
A ja, ja sam umoran o o o
C     Hm     Am
A ja, ja sam umoran

Ref:
       G        D           Am       Em      Em D
Kad bi mogao da vratim svoj zivot iz pocetka
C      D        Em
Bio bi isti kao sad
       G        D         Am          Em    Em D
Da sam mogao da svatim da ljubav nije vecna
C      D        Em
Ne bi danas bio sam

solo/skidan na akusticnoj gitari g=dizi zicu na gore
E                       D                       G      D         E
e-----------------------|-----------------------|----------------------|
h-----------------------|-----------------------|----------------------|
g---0--4g-4g-4g-2---2-4-|---0--4g-4g-4g-2-------|----2-----------------|
d-2---------------5-----|-2---------------5-4-2-|-5----5-4----2-5--4-2-|
a-----------------------|-----------------------|-----------5----------|
e-----------------------|-----------------------|----------------------|
E                       D                       C          Hm
e-----------------------|-----------------------|----------------------|
h-----------------------|------7-8-7---7-8-10---|-7-8-7------7-8-7-----|
g---7--9g-9g-9g-7---7-9-|---7--------9--------9-|-------9----------9---|
d-9---------------9-----|-9---------------------|---------10---------9-|
a-----------------------|-----------------------|----------------------|
e-----------------------|-----------------------|----------------------|
  E
e------------------||
h-12g-8-7-8--7-----||
g--------------7-9-||
d------------------||
a------------------||
e------------------||

Em                 D              
Pevam iz sveg srca, peva cu i dalje
Am                   C       D
Samo tako bol u dusi moze da prestane
Em                       D
Moj zivot je obicna prica kao mnoge druge
Am              C         D
Usponi i padovi kao vozne pruge
C    Hm     Em
A ja ja sam umoran
C    Hm     Am
A ja ja sam umoran

Ref 2X
Kad bi mogao ...";
            dbNeo4j.SongCreate(song_22, "User_2", "Kraljevski Apartman");

            Song song_23 = new Song();
            song_23.name = "Tako Blizu";
            song_23.link = "/Leb I Sol/Tako Blizu";
            song_23.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_23.approved = true;
            song_23.content = @"Intro: E7/9

        Em              C/G
Iskopaj bunar, duboko u meni
      D          Em
Želje svoje pronađi
Dm6/C#                G
Kiša prestaje, vlažne dlanove
       D/F#      C
Ostavi mi na košulji

Moje misli, tako nevažne,
Soba puna ćutanja
Svaki pokret, sada suvišan,
      D/F#    C  D
Preti nam istina


ref:
Am    G          D/F#       Em
  Tako blizu, tako blizu do Sunca,
Em    D          Ć          C  C/H
Ptice zbunjeno odleteše bez krila
Am    G          D/F#       Em
  Tako blizu, tako blizu do suza,
Em    D             Ć     C  C/H
      Ne znam da si ikada bila
C C/H D
O-o---ooo...";
            dbNeo4j.SongCreate(song_23, "User_1", "Leb I Sol");

            Song song_24 = new Song();
            song_24.name = "Nothing Else Matters";
            song_24.link = "/Metallica/Nothing Else Matters";
            song_24.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_24.approved = true;
            song_24.content = @"Em                 D      C
So close no matter how far
Em                    D             C 
Couldn't be much more from the heart
Em               D         C
Forever trusting who we are
G   B            Em
And nothing else matters

Em             D            C
Never opened myself this way
Em                       D      C
Life is ours, we live it our way
Em                      D       C
All these words I don't just say
G   B            Em
And nothing else matters

Em               D          C
Trust I seek and find in you
Em               D            C
Every day for us something new
Em              D             C
Open mind for a different view
G   B            Em
And nothing else matters 

{refren}
C A D                          C
     Never cared for what they do
  A D                          C
     Never cared for what they know
  A D      Em
     But I know

{ponavljanje prve strofe}
{refren}
{ponavljanje druge i trece strofe}

C A D                          C
     Never cared for what they say
  A D                          C
     Never cared for what they play
  A D                          C
     Never cared for what they do
  A D                          C
     Never cared for what they know
  A D      Em
     But I know

{ponavljanje prve strofe}";
            dbNeo4j.SongCreate(song_24, "User_2", "Metallica");

            Song song_25 = new Song();
            song_25.name = "Oblaci";
            song_25.link = "/Negativ/Oblaci";
            song_25.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_25.approved = true;
            song_25.content = @"INTRO: C#5 H5 A#5 A5

C#      H          A#  A
Kisa ce zemlju oprati
C#        H             A#  A
A gore na nebu sunce ce sijati
    C#5         H5         A#5   A5
Ponovo ja se tu nista ne pitam i pevam 
C#5        H5        A#5   A
Tu tu turu tutu tutu aaaaaaaaa

E5     F#5     A5 
Uuuuuuuuuuuuuu jeeee
H5            E5(VIIp)    F#5       A5
Volela bih da znam sta se krije iza oblaka
E5         F#5        A5    H5         E5(VIIp)   F#5       A5
Uuuuuuu ja nemam vremena da bolje pogledam sta se krije iza oblaka

Krije se iza oblaka
Negde daleko na hiljade svetlosnih godina
Ja tu nista ne shvatam
I samo pevam tu tu turu tutu tutu aaaaaaaaa

Uuuuuuuuuuuuuu jeeee
Volela bih da znam sta se krije iza oblaka
Uuuuuuuuuuuu 
Ja nemam vremena da bolje pogledam sta se krije iza oblaka  X2";
            dbNeo4j.SongCreate(song_25, "User_1", "Negativ");

            Song song_26 = new Song();
            song_26.name = "Balkan";
            song_26.link = "/Neverne bebe/Balkan";
            song_26.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_26.approved = true;
            song_26.content = @"F          Am        Dm
Svetlost u zoru širi krila
F       Am         Dm
posvuda tvoje živa mi bila
F       Am          Dm
greh ko staklo dušu slama
F    Am       Dm
bole oči puca glava.

Upali sveće neka dođu
sve što mislim nemam kome
budu sreće onda prođu
i kom ću Bogu kad me slome.

A#      F             Dm
Gde god krenem put me vodi
A#      F               Dm
na ovaj Balkan južno od sreće
A#       F          Dm
da nađem te i da te rodim
F        Am            Dm
zar tako malo ,malo me neće.

Upali sveće neka dođu
sve što mislim nemam kome
za sve dane koji prođu
za čiju sreću kad me slome.

Gde god krenem put me vodi
na ovaj Balkan južno od sreće
da nađem te i da te rodim
zar tako malo ,malo me neće.";
            dbNeo4j.SongCreate(song_26, "User_1", "Neverne Bebe");

            Song song_27 = new Song();
            song_27.name = "Krv I Led";
            song_27.link = "/Osvajaci/Krv I Led";
            song_27.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_27.approved = true;
            song_27.content = @"Am    G         F
 Mozes poci bilo gde
 C     G             F
 Mozes sve sto zelis ti
 Da l' ce rec il' duse glas
 Bilo sta da promeni

 A kad odes sacuvaj
 Nase tajne najvece
 Srce tesko boluje
 Ali znam prebolece

 G              G#m Am A|-7-5-3-|
 Neka me bog ne ostavi
 G
 Kada dan sakrije zvezde

 Am    F       C      G
 Krv i led, na usnama tvojim
 Krv i led, a na meni topli tragovi
 Bojim se, sve vise se bojim
 Krv i led, jer to nisi ti

 Uvod

 A kad odes sacuvaj
 Srce tesko boluje
 Ali znam prebolece

 Neka me bog ne ostavi
 Kada dan sakrije zvezde

 Krv i led, na usnama tvojim
 Krv i led, a na meni topli tragovi
 Bojim se, sve vise se bojim
 Krv i led, jer to nisi ti

 Solo: Am F C G x3 F G Am

 Krv i led, na usnama tvojim
 Krv i led, a na meni topli tragovi
 Bojim se, sve vise se bojim
 Krv i led, jer tooo-ooo             x4";
            dbNeo4j.SongCreate(song_27, "User_1", "Osvajaci");

            Song song_28 = new Song();
            song_28.name = "Dodji";
            song_28.link = "/Parni Valjak/Dodji";
            song_28.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_28.approved = true;
            song_28.content = @"Uvod: F#m D C#m /2X

F#m     E      C#m       D    C#      F#m
Dođi, zaboravi, nudim ti noći čarobne
    E       D        C#        F#m
i buđenja u postelji punoj šecera
Ispred mog prozora drvo divljeg kestenja
Puno plodova koje nitko ne treba
Anđeli nek' te čuvaju kada vrijeme oboli
Da li čovjek sve baš sve na kraju preboli

Prijelaz: F#m E

     D          E     C#         F#m
Zaspao bih sada ja na tvojim rukama
D        E          C#m
Budio se ne bih nikada ...
     D            E     C#        F#m
neka vrijeme sada broji svoje godine
D           Hm        C#sus4 C#
Meni je vec dosta čekanja

Dođi, zaboravi, nudim ti noći čarobne
i buđenja u postelji punoj šećera
Dođi i ostani, nudim ti suze k'o bisere
Moje namjere još su uvijek skrivene";
            dbNeo4j.SongCreate(song_28, "User_1", "Parni Valjak");

            Song song_29 = new Song();
            song_29.name = "Kaja";
            song_29.link = "/Plavi orkestar/Kaja";
            song_29.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_29.approved = true;
            song_29.content = @"F#m        C#	     F#m    E
Ne mogu ti pomoc ove noci 
D           E           A 
jer ne mogu protiv srca svog
D          E            A 
jer nemogu skriti svoju bol
   Hm        C#              F#m
uzalud, ovaj put, sve je gotovo 

Ne pitaj me s kim cu da se budim 
jer, to vise necu biti ja 
umrijecu, na tudim rukama 
sanjajuc, miris tvog, plavog uvojka 
F#m   Hm
Kaja, ove noci
F#m   Hm 
kaja, sklopi oci
F#m   C#         F#m  
kaja, ne cekaj me ti 

Kaja, ove noci 
kaja, sklopi oci 
necu, ti se vratiti 

Ne vjeruj da spominjem te cesto 
da se tajno molim za tebe 
strazarim kraj tvoje ulice 
da uz rum, gubim um i tiho nestajem 

Jezdim nad pucinama hvara 
na njeznom vjetru zaborava 
ispod mog balkona miris oguljelih narandi 

Orkestar izgubljenih snova i djeca 
talasi odnose vjetrove 
i na kraju puta samo ti i samo bol";
            dbNeo4j.SongCreate(song_29, "User_1", "Plavi orkestar");

            Song song_30 = new Song();
            song_30.name = "Amsterdam";
            song_30.link = "/Riblja Corba/Amsterdam";
            song_30.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_30.approved = true;
            song_30.content = @"Hm         A       Hm   A
Neko mi je ukrao biciklo
Hm           A          Hm    A
jer ga nisam dobro vezo lancom
Hm         A          Hm   A
na biciklo ovde sam naviko
Hm          A        Hm     A
da se ne bi osecao strancom.

Kucici po ulicama kaku
cipelu sam umazo govancom
hoce da me grizaju u mraku
da se ne bi osecao strancom

Ref:

D                   A
U kafanu pusu marihuanu
D                      A
ili neku drogu piju s' nogu
D                      A
te sam noci spavao pod sankom
D                   E
da se ne bi osecao strancom.


Hm A    Hm  A Hm
Am_ster_dam...";
            dbNeo4j.SongCreate(song_30, "User_1", "Riblja Corba");

            Song song_31 = new Song();
            song_31.name = "10 godina";
            song_31.link = "/Ritam Nereda/10 godina";
            song_31.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_31.approved = true;
            song_31.content = @"Napomena: šesta žica je preštimovana

  Tema:
E|----------------|----------------|
H|----------------|----------------|
G|----7-----------|5-----------5h7-|
D|----------7-----|------5---------|
A|----------------|----------------|
D|0-0---0-0---0-0-|--0-0---0-0-----|

  Strofa:
E|----------------|----------------|----------------|----------------|
H|----------------|----------------|----------------|----------------|
G|----------------|----------------|----------------|----------------|
D|----7-----7---7-|----7-----7---7-|----7-----7---7-|----7-----7---7-|
A|----5-----5---5-|----5-----5---5-|----5-----5---5-|----5-----5---5-|
D|0-0---0-0---0---|0-0---0-0---0---|0-0---0-0---0---|0-0---0-0---0---|

E|----------------|----------------|----------------|----------------|
H|----------------|----------------|----------------|----------------|
G|----------------|----------------|----------------|----------------|
D|----7-----7---7-|----7-----7---7-|----7-----7---7-|----7-----------|
A|----5-----5---5-|----5-----5---5-|----5-----5---5-|----5-----------|
D|0-0---0-0---0---|0-0---0-0---0---|0-0---0-0---0---|0-0---0-3b4r3b4-|

  Refren:
           D5
Ne nije me strah,
        B5
Nije me strah,
       C5    A5      D5
Suviše znam, nije me strah
          B5        C5  A5  D5
Da glasno kažem ono što osećam
          B5    C5   A5   D5
I kada me boli, boli ostajem
         B5             F5
Duboko u meni, duboko u tebi
       D5 (u drugom refrenu D#5)
10 godina

  Prelaz:
E|----------------|----------------|----------------|----------------|
H|----------------|----------------|----------------|----------------|
G|----------------|----------------|----------------|----------------|
D|----------------|----------------|----------------|----------------|
A|0-0---0-0-0-0-0-|0-0---0-0-0-0-0-|1-1---1-1-1-1-1-|1-1---1-1-1-1-1-|
D|0-0---0-0-0-0-0-|0-0---0-0-0-0-0-|1-1---1-1-1-1-1-|1-1---1-1-1-1-1-|

  Kraj prelaza: F5

  Korišćeni akordi:
D5  -X75XXX
B5  -88XXXX
C5  -10 10 X X X X
A5  -77XXXX
F5  -33XXXX
D#5 -11XXXX";
            dbNeo4j.SongCreate(song_31, "User_2", "Ritam Nereda");

            Song song_32 = new Song();
            song_32.name = "Profesor";
            song_32.link = "/Smak/Profesor";
            song_32.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_32.approved = true;
            song_32.content = @"uvod: A, (A) D, Cadd9 C/Hadd9 A

A      (A)    D
U nasem gradu zivi 
Cadd9    C/Hadd9 A
Taj covek vecito mlad
A      (A)  D
Znaju stari znaju mladi
Cadd9   C/Hadd9 A
O njemu pricacu sad

Taj profesor dane gubi
Vreme trosi na nas
On muziku strasno ljubi
Svira klavir i bas

ref:
A           D
Hej, profesore
         C           C/H   A
Mi znamo sta si ludo voleo pre
A            D
Dzez, profesore
       C       C/H     A
Pokazi kako se sviralo pre

Em  G        Am    F
Taj profesor ljudi
G           Am
Nema nikoga svog
Em   G      Am    F
Samo muziku ljubi
G               Am
Ona je za njega bog

Taj profesor ljudi
Cesto napusta cas
Em  G        Am   F
Tad direktor ludi
G     D  G  C  F     E
Psuje njega klavir i bas";
            dbNeo4j.SongCreate(song_32, "User_1", "Smak");


            Song song_33 = new Song();
            song_33.name = "Lutka";
            song_33.link = "/S.A.R.S/Lutka";
            song_33.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_33.approved = true;
            song_33.content = @"D          Dmaj7
Ko mi tebe uze
Hm           A
ko mi tebe posla
Em        F#m     G
ko provocira nase suze
      A
svaki put kad bi dosla

Meni mozak brani
da se tebi predam
tvoja pojava me hrani
al  se ipak ne dam

I samo te gledam
osecaj je izvanredan
i toj drogi bicu predan
makar ost o cedan

Ref  2x
D      A         Hm
Lutko, ja sam resen
   A             Em  F#m  G-A   2X
da vecno s tobom plesem

Pogled tvoj me srami
alc mi strasno godi
miris tvoj me mami
dodir tvoj me vodi

Ti si cilj mog lutanja
i predmet pobude
razlog mog drhtanja
boja moje pozude

I dok tonem u san
u mraku cujem tvoj glas
na jastuku stiskam tvoj dlan
orkestar svira za nas";
            dbNeo4j.SongCreate(song_33, "User_1", "S.A.R.S");

            Song song_34 = new Song();
            song_34.name = "Reka";
            song_34.link = "/Tap 011/Reka";
            song_34.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_34.approved = true;
            song_34.content = @"C          G                    Am

Negde u daljini jedna reka protice

              F     G           C

sklopim oci i vidim nas kraj nje

ko ta reka moja ljubav prema tebi je

vecnija od reci najvece

Usne tad u zanosu se vrelom stapaju

a mi smo jedini na svetu

putujemo kao zvezde dve po svemiru

                        G

bol i tuga tada nestaju

Refren:

          F           C

Zagrli me sad, tu ostani

               G    Am

i deo moga srca postani

jer nema sile te da razdvoji

dvoje u pravoj ljubavi

Zagrli me sad, tu ostani

i deo moga srca postani

jer nema sile te da razdvoji

           G         FG   C

dvoje u pravoj ljubavi

Negde u tisini dragi dodir budi me

pustam da me radost opije

ko taj dodir moja ljubav prema tebi je

vecnija od pesme najbolje";
            dbNeo4j.SongCreate(song_34, "User_1", "Tap 011");

            Song song_35 = new Song();
            song_35.name = "Put";
            song_35.link = "/Van Gogh/Put";
            song_35.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_35.approved = true;
            song_35.content = @"D                   G       E  F
Ja sam kralj,ja sam lopov i gad,
lancima okovan mrzim tvoj svet.
Umno poremećen,novcem opterećen
besnilom zaražen,ludilom pregažen
lažem,kradem i opijam se.

C          F
To smo mi, za nas.

ref.
E              
Ne postoji put 
D        A    C   D    G A
kojim se lako ide u raj,
sve kad se lomi u tebi srećan si ako mu ugledaš kraj.";
            dbNeo4j.SongCreate(song_30, "User_35", "Van Gogh");

            Song song_36 = new Song();
            song_36.name = "Ona To Zna";
            song_36.link = "/Vlada Divljan/Ona To Zna";
            song_36.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_36.approved = true;
            song_36.content = @"D G Em A

D               Em
Ona to zna, ona zna,
       A         D
da je dugo, dugo,
D      G        Em
ona to zna, ona zna
      A
da je dugo, dugo
D          G        EmA
volim vec, e e e, e e e

Molim je da, molim je,
da mi bar jedanput
molim je, molim je 
da mi bar jedanput
veruje, e-e-e

G        A           D
I kad je vidim ja se pitam
      Em
da l' postoji
G        A           D
i kad je vidim ja se pitam
Em
da l' ona zna ?

D   G        Em
Ona zna, ona zna,
       A               D
da je ludo, ludo volim ja
D   G        Em
ona zna, ona zna
      A      D  GEmA
da je ludo volim

I svake noci kada sanjam
ona me voli
i svake noci ja se pitam
da l' ona zna";
            dbNeo4j.SongCreate(song_36, "User_2", "Vlada Divljan");

            Song song_37 = new Song();
            song_37.name = "Crni leptir";
            song_37.link = "/Yu Grupa/Crni leptir";
            song_37.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_37.approved = true;
            song_37.content = @"Key of C


Em     C         Em            Am
Ulicna svetiljka baca svetlost u krug
Em       C       G    Am       Em
dok crni leptiri lete pravo na nju
Em  C        Em       Am
ali ne znaju da tu je kraj
Em    C     G     Am        Em
da ih ubija njene svetlosti sjaj

Em        C     Em         Am
Nekad sam leteo leptir bio i ja
Em          C       G     Am Em
svetlost me mamila, krila mi sprzila
Em   C       Em     Am
crni leptiru bezi u noc
Em    C       G   Am      Em
jutro sacekaj svetlost ce doc'";
            dbNeo4j.SongCreate(song_37, "User_1", "Yu Grupa");

            Song song_38 = new Song();
            song_38.name = "Balada o Pisonji i Zugi";
            song_38.link = "/Zabranjeno pusenje/Balada o Pisonji i Zugi";
            song_38.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_38.approved = true;
            song_38.content = @"G         G    D          Em
Pisonja i zuga su pamtili dobro
C            C      G       D
Sta im je te veceri govorio lepi
G       G      C       Em
More je provod more su koke
C       D            D          G
More je izvor zivota je li tako Moke

Moke je jos dodao i to da se strankinje praskaju pravo
i da je u zaustrogu kampu svaku noc drugu jebav'o
Pisonja i zuga mogli su ih slusati i noc cijelu
Pisonja i zuga imali su krv sedamnaest godina vrelu

C   D     G
pisonja i zuga

[G  Em  G  Em  G  Em  Dsus4  D]

jos istu noc su pisonja i zuga
maznuli kasetas iz doma invalida
a malo zatim i autobus autoprevoza sa hrida
pisonja ubaci u brzinu
to vrelo ljetnje vece oko 22 casa
a sad pravac more viknu zuga iz sveg glasa

pisonja je vozio i pusio duvan
zuga voli crne a pisonja plave
a posle mora dalje u svijet
samo se hrabri dokopaju slave
a onda zbogom barake na breci
viknu zuga i kasetas odvrnu jace
dobicete razglednicu iz africke zemlje safari
- zbogom zohari

ref.
C   D     G    G
pisonja i zuga 
C   D      Em    Em D C
dva vjerna druga
Em    Em D C            Em  Em D C
krote        opasne krivine
         G                   D
molim te cuvaj ih kraljice brzine

murija je blokirala cestu
negde kod bradine oko 23 i 5
u autobusu je svirao bugi-vugi
vidjevsi drotove pri brzini od osamdeset na sat
vezite se polecemo pisonja rece zugi

strahovit tresak zapara zrak
cak i iskre poletese u mrak
pricali su ujutro i kleli se u majku
da su auspuh i retrovizor nasli cak
pedest metara dalje u jarku

ref.

dok su pisonju nosili u hitnu
on ugleda mesec na nebu i rece
boze kako neki mogu gore a ja i zuga ni na more
posle toga je pao u nesvjest i vise nije mogao cuti
kako se jos dugo nan nebu smijao mjesec zuti
pisonja i zuga...


SOLO: (U A, za original sve spustiti za cijeli stepen, u G)

  D   E     A
e|---------------------------------------------------|
H|3---5-----17b19--17b19--17b19r17-14----------------|
G|2---4-------------------------------16b18r16-14----|
D|4---6-------------------------------------------16-|
A|5---7----------------------------------------------|
E|---------------------------------------------------|

  Pisonja i Zuga

  A
e|-------------------------------------------------------|
H|-------------------------------14----------------------|
G|------14-14----14-16-14--16b18----18r16-14-16-14-------|
D|14-16-14-14-16-14---------------------------------1614-|
A|-------------------------------------------------------|
E|-------------------------------------------------------|

  D                          F#m
e|------------------------------------------------------------|
H|------------------------------17----------------------------|
G|---14-16-14-16b18r16-14-16b18----16b18r16-14cccccc----------|
D|16----------------------------------------------------16-14-|
A|------------------------------------------------------------|
E|------------------------------------------------------------|

  D
e|-------------------------------------------------|
H|-------------------------------------------------|
G|------14-14----14-16----14-14-14----14-16-14-16--|b
D|14-16-------16----------14-------16--------------|
A|-------------------------------------------------|
E|-------------------------------------------------|

  A                         E
e|----------------------------------------------------|
H|---14--1514---14-15-14------------------------------|
G|18---------16----------16b18-r16-14-16-16brbrbrbrbr-|
D|----------------------------------------------------|
A|----------------------------------------------------|
E|----------------------------------------------------|

  A
e|----14-----------------------------------------|
H|-14----17-14-----------------------------------|
G|-------------16b18r16-14----14-16-14-----------|
D|-------------------------16--------------16-14-|
A|-----------------------------------------------|
E|-----------------------------------------------|

  D                       F#m
e|-----------------------------------------------|
H|-----------------------------------------------|
G|---14-14----14-16-14-16b18---ccccc-------18r16-|
D|16-------16------------------------------------|
A|-----------------------------------------------|
E|-----------------------------------------------|

  D                      E
e|---------------------------------------------------------------------------|
H|---------------------------17-------------------17-------17-------17-------|
G|14----------14-16-14-16b18------18r16-14--16b18----16b18----16b18----18r16-|
D|---16-14-16----------------------------------------------------------------|
A|---------------------------------------------------------------------------|
E|---------------------------------------------------------------------------|

  A
e|-------------14------14------14------14------14------14------14------14-|
H|-----14-17b19---17b19---17b19---17b19---17b19---17b19---17b19---17b19---|
G|14----------------------------------------------------------------------|
D|14----------------------------------------------------------------------|
A|------------------------------------------------------------------------|
E|------------------------------------------------------------------------|

  D                            F#m
e|-----14-17----14-17-14----14--------------------------------|
H|17b19------17----------17----17b19-r17----------------------|
G|---------------------------------------16b18r16-14----------|
D|------------------------------------------------14----16-14-|
A|------------------------------------------------------------|
E|------------------------------------------------------------|

  D
e|---------------------------------------------------|
H|---------------------------------------------------|
G|---14-14----14-16-14-16b18-14----14-----14-16-14---|
D|16-------16-------------------16-------------------|
A|---------------------------------------------------|
E|---------------------------------------------------|

  A                    E
e|---------------------------------------------|
H|---------------------------------------------|
G|16b18r16-14----14-16-14----14-16----14-16-14-|
D|------------16-------14----------------------|
A|---------------------------------------------|
E|---------------------------------------------|

  A
  tr.
e|-------------------
H|141714171417....14-
G|-------------------|
D|-------------------|
A|-------------------|
E|-------------------|

  D                  F#m
e|------------------------------------------------|
H|17b19-cccc--14--14------------------------------|
G|--------------16---16b18--r16-14----14----------|
D|---------------------------------16-14----16-14-|
A|------------------------------------------------|
E|------------------------------------------------|

  D                       E
e|------------------------------------------------------------------------|
H|-----------------------------17----------------17------17------17-------|
G|---14-16-14-16b18r16-14-16b18----18r16-14-16b18---16b18---16b18---18r16-|
D|16----------------------------------------------------------------------|
A|------------------------------------------------------------------------|
E|------------------------------------------------------------------------|

  A           D                       E
e|--------------------------------------------------------|
H|--------------------------------------------------------|
G|14-------------14-14----14-------------14-14----14------|
D|14----16-14-16-14-14-16-14----16-14-16-14-14-16-14------|
A|--------------------------------------------------------|
E|--------------------------------------------------------|

  A
e|17b19r17-14-17-14----14------------------------------------------|
H|------------------17----17-14------------------------------------|
G|------------------------------17-16-14-16-14----14---------------|
D|---------------------------------------------16----16-14---------|
A|--------------------------------------------------------161514---|
E|--------------------------------------------------------------17-|

e|------------------------------5-----|
H|------------------------------5-----|
G|-------------16b18------r16---6-----|
D|------------------------------7-----|
A|14b16--r14--------------------7-----|
E|------------------------------5-----|";
            dbNeo4j.SongCreate(song_38, "User_1", "Zabranjeno pusenje");

            Song song_39 = new Song();
            song_39.name = "Dodirni Mi Kolena";
            song_39.link = "/Zana/Dodirni Mi Kolena";
            song_39.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            song_39.approved = true;
            song_39.content = @"C#m
Hej moja dusice izbaci bubice
                         B
iz svoje lepe lude plave glavice

ne budi dete
C#m
Obuci papuce, dodaj mi jastuce
                             B
nezno me zagrli i ponasaj se prirodno

D#m
Skuvaj mi kafu,napravi sendvic
G#
lepo ugosti i zadovolji me

Hej moja dusice, ostavi lutkice
koje te jure i stalno ti dosadjuju
ne budi dete
Kupi mi haljine, srebrene lancice
crvene maline i kartu do Amerike

Znamo se skoro vec deset dana
daj mi svoj auto i kljuc od stana

C#m           F#m          C#m
Hej, na sveze mleko mirise dan
      F#m           C#m
ptice pevaju na sav glas
      F#m
jutro njise vetar
  G#
dodirni mi kolena, to bih bas volela

Hej, plavo nebo zuri u stan
zuti leptir mazi moj vrat
jutro njise vetar
dodirni mi kolena, to bih bas volela

SOLO:
|C#m   |B     |D#m   |G#    | X4
|C#m   |F#m   |C#m   |F#m   |
|C#m   |F#m   |G#    |      | X2
";
            dbNeo4j.SongCreate(song_39, "User_1", "Zana");

            //komentar
            Comment comment_1 = new Comment();
            comment_1.id = 1;
            comment_1.content = "Super pesma!";
            comment_1.date = DateTime.Now.ToString("mm:hh dd-MM-yyyy");
            dbNeo4j.CommentCreate(comment_1, "User_3", "Vesela pesma");
        }
    }
}