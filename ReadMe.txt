Potrebno je da Redis i Neo4J budu pokrenuti na default portovima, i po potrebi uneti svoje podatke za login u statickim atributima Neo4JDataProvider klase. U folderu rezerva su slike obe baze za svaki slucaj, a Na pocetku Index metode Home kontrolera se poziva metoda klase DBInitializer koja ucitava sve podatke iz prilozenih tekstualnih fajlova, u slucaju da baza ne postoji(proverom postajanja admin korisnika). Za svaki slucaj su date kompletne slike obe baze u folderu Backup. 

Koriscene tehnologije: 
ASP.NET MVC5, Bootstrap, JS, jQuery, Redis, Neo4J

kao i sledeci prilagodjeni moduli:
https://github.com/singerjs/singerjs.github.io
https://github.com/autocompletejs/autocomplete.js

GitHub: 
https://github.com/StefanStamenovic/AchordLira

~Nemanja, Stefan i Stefan