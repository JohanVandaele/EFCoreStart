using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Campussen",
                columns: new[] { "CampusId", "Gemeente", "Huisnummer", "CampusNaam", "Postcode", "Straat" },
                values: new object[,]
                {
                    { 1, "Antwerpen", "22", "Andros", "2018", "Somersstraat" },
                    { 2, "Dendermonde", "17", "Delos", "9200", "Oude Vest" },
                    { 3, "Genk", "37", "Gavdos", "3600", "Europalaan" },
                    { 4, "Heverlee", "2", "Hydra", "3001", "Interleuvenlaan" },
                    { 5, "Wevelgem", "10", "Ikaria", "8560", "Vlamingstraat" },
                    { 6, "Oostende", "4", "Oinouses", "8400", "Archimedesstraat" }
                });

            migrationBuilder.InsertData(
                table: "Landen",
                columns: new[] { "LandCode", "Naam" },
                values: new object[,]
                {
                    { "BE", "België" },
                    { "DE", "Duitsland" },
                    { "FR", "Frankrijk" },
                    { "IT", "Italië" },
                    { "LU", "Luxemburg" },
                    { "NL", "Nederland" }
                });

            migrationBuilder.InsertData(
                table: "Docenten",
                columns: new[] { "DocentId", "CampusId", "Familienaam", "HeeftRijbewijs", "InDienst", "LandCode", "Voornaam", "Maandwedde" },
                values: new object[,]
                {
                    { 1, 4, "Abbeloos", null, new DateOnly(2024, 1, 1), "BE", "Willy", 1400m },
                    { 2, 2, "Abelshausen", true, new DateOnly(2024, 1, 2), "NL", "Joseph", 1800m },
                    { 3, 3, "Achten", false, new DateOnly(2024, 1, 3), "DE", "Joseph", 1300m },
                    { 4, 4, "Adam", null, new DateOnly(2024, 1, 4), "FR", "François", 1700m },
                    { 5, 1, "Adriaensens", true, new DateOnly(2024, 1, 5), "IT", "Jan", 2100m },
                    { 6, 6, "Adriaensens", false, new DateOnly(2024, 1, 6), "LU", "René", 1600m },
                    { 7, 3, "Aerenhouts", null, new DateOnly(2024, 1, 7), "BE", "Frans", 1300m },
                    { 8, 1, "Aerts", true, new DateOnly(2024, 1, 8), "NL", "Emile", 1700m },
                    { 9, 2, "Aerts", false, new DateOnly(2024, 1, 9), "DE", "Jean", 1200m },
                    { 10, 6, "Aerts", null, new DateOnly(2024, 1, 10), "FR", "Mario", 1600m },
                    { 11, 5, "Aerts", true, new DateOnly(2024, 1, 11), "IT", "Paul", 2000m },
                    { 12, 5, "Aerts", false, new DateOnly(2024, 1, 12), "LU", "Stefan", 1500m },
                    { 13, 3, "Alexander", null, new DateOnly(2024, 1, 13), "BE", "François", 1900m },
                    { 14, 6, "Allard", true, new DateOnly(2024, 1, 14), "NL", "Henri", 1600m },
                    { 15, 1, "Anciaux", false, new DateOnly(2024, 1, 15), "DE", "Albert", 1100m },
                    { 16, 5, "Anseeuw", null, new DateOnly(2024, 1, 16), "FR", "Urbain", 1500m },
                    { 17, 3, "Antheunis", true, new DateOnly(2024, 1, 17), "IT", "Etienne", 1900m },
                    { 18, 4, "Arlet", false, new DateOnly(2024, 1, 18), "LU", "Jacques", 1400m },
                    { 19, 2, "Arras", null, new DateOnly(2024, 1, 19), "BE", "Wim", 1800m },
                    { 20, 2, "Baens", true, new DateOnly(2024, 1, 20), "NL", "Roger", 2200m },
                    { 21, 5, "Baert", false, new DateOnly(2024, 1, 21), "DE", "Dirk", 1000m },
                    { 22, 4, "Baert", null, new DateOnly(2024, 1, 22), "FR", "Hubert", 1400m },
                    { 23, 2, "Baert", true, new DateOnly(2024, 1, 23), "IT", "Jean-Pierre", 1800m },
                    { 24, 3, "Baeyens", false, new DateOnly(2024, 1, 24), "LU", "Armand", 1300m },
                    { 25, 1, "Baeyens", null, new DateOnly(2024, 1, 25), "BE", "Jan", 1700m },
                    { 26, 1, "Baguet", true, new DateOnly(2024, 1, 26), "NL", "Roger", 2100m },
                    { 27, 6, "Baguet", false, new DateOnly(2024, 1, 27), "DE", "Serge", 1600m },
                    { 28, 3, "Balducq", null, new DateOnly(2024, 1, 28), "FR", "Gérard", 1300m },
                    { 29, 1, "Barbé", true, new DateOnly(2024, 1, 29), "IT", "Koen", 1700m },
                    { 30, 2, "Barras", false, new DateOnly(2024, 1, 30), "LU", "Georges", 1200m },
                    { 31, 6, "Baumans", null, new DateOnly(2024, 1, 31), "BE", "Auguste", 1600m },
                    { 32, 5, "Bauwens", true, new DateOnly(2024, 2, 1), "NL", "Arsène", 2000m },
                    { 33, 5, "Bauwens", false, new DateOnly(2024, 2, 2), "DE", "Henri", 1500m },
                    { 34, 3, "Bayens", null, new DateOnly(2024, 2, 3), "FR", "Jules", 1900m },
                    { 35, 6, "Beckaert", true, new DateOnly(2024, 2, 4), "IT", "Albert", 1600m },
                    { 36, 1, "Beckaert", false, new DateOnly(2024, 2, 5), "LU", "Marcel", 1100m },
                    { 37, 5, "Beeckman", null, new DateOnly(2024, 2, 6), "BE", "Koen", 1500m },
                    { 38, 3, "Beeckman", true, new DateOnly(2024, 2, 7), "NL", "Kamiel", 1900m },
                    { 39, 4, "Beeckman", false, new DateOnly(2024, 2, 8), "DE", "Theophile", 1400m },
                    { 40, 2, "Beheyt", null, new DateOnly(2024, 2, 9), "FR", "Benoni", 1800m },
                    { 41, 2, "Beirnaert", true, new DateOnly(2024, 2, 10), "IT", "Albert", 2200m },
                    { 42, 5, "Belvaux", false, new DateOnly(2024, 2, 11), "LU", "Jean", 1000m },
                    { 43, 4, "Benoit", null, new DateOnly(2024, 2, 12), "BE", "Adelin", 1400m },
                    { 44, 2, "Benoit", true, new DateOnly(2024, 2, 13), "NL", "Auguste", 1800m },
                    { 45, 3, "Berben", false, new DateOnly(2024, 2, 14), "DE", "Jef", 1300m },
                    { 46, 1, "Berckmans", null, new DateOnly(2024, 2, 15), "FR", "Jean-Pierre", 1700m },
                    { 47, 1, "Berton", true, new DateOnly(2024, 2, 16), "IT", "Albert", 2100m },
                    { 48, 6, "Beths", false, new DateOnly(2024, 2, 17), "LU", "Frans", 1600m },
                    { 49, 3, "Beyens", null, new DateOnly(2024, 2, 18), "BE", "René", 1300m },
                    { 50, 1, "Beyssens", true, new DateOnly(2024, 2, 19), "NL", "Herman", 1700m },
                    { 51, 2, "Billiet", false, new DateOnly(2024, 2, 20), "DE", "Albert", 1200m },
                    { 52, 6, "Billiet", null, new DateOnly(2024, 2, 21), "FR", "Hector", 1600m },
                    { 53, 5, "Blavier", true, new DateOnly(2024, 2, 22), "IT", "Marcel", 2000m },
                    { 54, 5, "Blockx", false, new DateOnly(2024, 2, 23), "LU", "Roger", 1500m },
                    { 55, 3, "Blomme", null, new DateOnly(2024, 2, 24), "BE", "Maurice", 1900m },
                    { 56, 6, "Bocklandt", true, new DateOnly(2024, 2, 25), "NL", "Willy", 1600m },
                    { 57, 1, "Bodart", false, new DateOnly(2024, 2, 26), "DE", "Emile", 1100m },
                    { 58, 5, "Boekaerts", null, new DateOnly(2024, 2, 27), "FR", "Alfons", 1500m },
                    { 59, 3, "Bogaert", true, new DateOnly(2024, 2, 28), "IT", "Cesar", 1900m },
                    { 60, 4, "Bogaert", false, new DateOnly(2024, 3, 1), "LU", "Jan", 1400m },
                    { 61, 2, "Bogaerts", null, new DateOnly(2024, 3, 2), "BE", "Jean", 1800m },
                    { 62, 2, "Bonduel", true, new DateOnly(2024, 3, 3), "NL", "Frans", 2200m },
                    { 63, 5, "Boonen", false, new DateOnly(2024, 3, 4), "DE", "Tom", 1000m },
                    { 64, 4, "Boons", null, new DateOnly(2024, 3, 5), "FR", "Jozef", 1400m },
                    { 65, 2, "Borra", true, new DateOnly(2024, 3, 6), "IT", "Gabriel", 1800m },
                    { 66, 3, "Bosmans", false, new DateOnly(2024, 3, 7), "LU", "Joseph", 1300m },
                    { 67, 1, "Boucquet", null, new DateOnly(2024, 3, 8), "BE", "Walter", 1700m },
                    { 68, 1, "Boumon", true, new DateOnly(2024, 3, 9), "NL", "Marcel", 2100m },
                    { 69, 6, "Bracke", false, new DateOnly(2024, 3, 10), "DE", "Ferdinand", 1600m },
                    { 70, 3, "Braeckeveldt", null, new DateOnly(2024, 3, 11), "FR", "Adolphe", 1300m },
                    { 71, 1, "Braekevelt", true, new DateOnly(2024, 3, 12), "IT", "Omer", 1700m },
                    { 72, 2, "Brands", false, new DateOnly(2024, 3, 13), "LU", "Frans", 1200m },
                    { 73, 6, "Brankart", null, new DateOnly(2024, 3, 14), "BE", "Jean", 1600m },
                    { 74, 5, "Brichard", true, new DateOnly(2024, 3, 15), "NL", "Emile", 2000m },
                    { 75, 5, "Brosteaux", false, new DateOnly(2024, 3, 16), "DE", "Georges", 1500m },
                    { 76, 3, "Bruneau", null, new DateOnly(2024, 3, 17), "FR", "Emile", 1900m },
                    { 77, 6, "Bruyère", true, new DateOnly(2024, 3, 18), "IT", "Jean-Marie", 1600m },
                    { 78, 1, "Bruyere", false, new DateOnly(2024, 3, 19), "LU", "Joseph", 1100m },
                    { 79, 5, "Bruylandts", null, new DateOnly(2024, 3, 20), "BE", "Dave", 1500m },
                    { 80, 3, "Bruyneel", true, new DateOnly(2024, 3, 21), "NL", "Johan", 1900m },
                    { 81, 4, "Buysse", false, new DateOnly(2024, 3, 22), "DE", "Lucien", 1400m },
                    { 82, 2, "Brandt", null, new DateOnly(2024, 3, 23), "FR", "Christophe", 1800m },
                    { 83, 2, "Callens", true, new DateOnly(2024, 3, 24), "IT", "Norbert", 2200m },
                    { 84, 5, "Capiot", false, new DateOnly(2024, 3, 25), "LU", "Johan", 1000m },
                    { 85, 4, "Cerami", null, new DateOnly(2024, 3, 26), "BE", "Pino", 1400m },
                    { 86, 2, "Christiaens", true, new DateOnly(2024, 3, 27), "NL", "Georges", 1800m },
                    { 87, 3, "Claes", false, new DateOnly(2024, 3, 28), "DE", "Georges", 1300m },
                    { 88, 1, "Clerckx", null, new DateOnly(2024, 3, 29), "FR", "Karel", 1700m },
                    { 89, 1, "Close", true, new DateOnly(2024, 3, 30), "IT", "Alex", 2100m },
                    { 90, 6, "Corbusier", false, new DateOnly(2024, 3, 31), "LU", "Yvan", 1600m },
                    { 91, 3, "Couvreur", null, new DateOnly(2024, 4, 1), "BE", "Hilaire", 1300m },
                    { 92, 1, "Cretskens", true, new DateOnly(2024, 4, 2), "NL", "Wilfried", 1700m },
                    { 93, 2, "Criquielion", false, new DateOnly(2024, 4, 3), "DE", "Claude", 1200m },
                    { 94, 6, "Daems", null, new DateOnly(2024, 4, 4), "FR", "Emile", 1600m },
                    { 95, 5, "Danneels", true, new DateOnly(2024, 4, 5), "IT", "Gustave", 2000m },
                    { 96, 5, "De Bruyne", false, new DateOnly(2024, 4, 6), "LU", "Fred", 1500m },
                    { 97, 3, "Decabooter", null, new DateOnly(2024, 4, 7), "BE", "Arthur", 1900m },
                    { 98, 6, "De Clerq", true, new DateOnly(2024, 4, 8), "NL", "Hans", 1600m },
                    { 99, 1, "Decock", false, new DateOnly(2024, 4, 9), "DE", "Roger", 1100m },
                    { 100, 5, "Decraeye", null, new DateOnly(2024, 4, 10), "FR", "Georges", 1500m },
                    { 101, 3, "Defraeye", true, new DateOnly(2024, 4, 11), "IT", "Odiel", 1900m },
                    { 102, 4, "De Jonghe", false, new DateOnly(2024, 4, 12), "LU", "Albert", 1400m },
                    { 103, 2, "Delbecque", null, new DateOnly(2024, 4, 13), "BE", "Julien", 1800m },
                    { 104, 2, "Deloor", true, new DateOnly(2024, 4, 14), "NL", "Alfons", 2200m },
                    { 105, 5, "Deloor", false, new DateOnly(2024, 4, 15), "DE", "Gustaaf", 1000m },
                    { 106, 4, "Deltour", null, new DateOnly(2024, 4, 16), "FR", "Hubert", 1400m },
                    { 107, 2, "Deman", true, new DateOnly(2024, 4, 17), "IT", "Paul", 1800m },
                    { 108, 3, "Demeyer", false, new DateOnly(2024, 4, 18), "LU", "Marc", 1300m },
                    { 109, 1, "De Mulder", null, new DateOnly(2024, 4, 19), "BE", "Frans", 1700m },
                    { 110, 1, "De Muynck", true, new DateOnly(2024, 4, 20), "NL", "Johan", 2100m },
                    { 111, 6, "Demuysere", false, new DateOnly(2024, 4, 21), "DE", "Jef", 1600m },
                    { 112, 3, "Depoorter", null, new DateOnly(2024, 4, 22), "FR", "Jules", 1300m },
                    { 113, 1, "Depoorter", true, new DateOnly(2024, 4, 23), "IT", "Richard", 1700m },
                    { 114, 2, "Depredomme", false, new DateOnly(2024, 4, 24), "LU", "Prosper", 1200m },
                    { 115, 6, "Derboven", null, new DateOnly(2024, 4, 25), "BE", "Willy", 1600m },
                    { 116, 5, "Derijcke", true, new DateOnly(2024, 4, 26), "NL", "Germain", 2000m },
                    { 117, 5, "Dernies", false, new DateOnly(2024, 4, 27), "DE", "Michel", 1500m },
                    { 118, 3, "Deruyter", null, new DateOnly(2024, 4, 28), "FR", "Charles", 1900m },
                    { 119, 6, "Desimpelaere", true, new DateOnly(2024, 4, 29), "IT", "Maurice", 1600m },
                    { 120, 1, "Desmet", false, new DateOnly(2024, 4, 30), "LU", "Gilbert", 1100m },
                    { 121, 5, "Desplenter", null, new DateOnly(2024, 5, 1), "BE", "Georges", 1500m },
                    { 122, 3, "Despontin", true, new DateOnly(2024, 5, 2), "NL", "Léon", 1900m },
                    { 123, 4, "De Vlaeminck", false, new DateOnly(2024, 5, 3), "DE", "Eric", 1400m },
                    { 124, 2, "De Vlaeminck", null, new DateOnly(2024, 5, 4), "FR", "Roger", 1800m },
                    { 125, 2, "Devolder", true, new DateOnly(2024, 5, 5), "IT", "Stijn", 2200m },
                    { 126, 5, "Dewaele", false, new DateOnly(2024, 5, 6), "LU", "Maurice", 1000m },
                    { 127, 4, "De Wolf", null, new DateOnly(2024, 5, 7), "BE", "Alfons", 1400m },
                    { 128, 2, "Dhaenens", true, new DateOnly(2024, 5, 8), "NL", "Rudy", 1800m },
                    { 129, 3, "D''Hooghe", false, new DateOnly(2024, 5, 9), "DE", "Michel", 1300m },
                    { 130, 1, "Dierckxsens", null, new DateOnly(2024, 5, 10), "FR", "Ludo", 1700m },
                    { 131, 1, "Dictus", true, new DateOnly(2024, 5, 11), "IT", "Frans", 2100m },
                    { 132, 6, "Driessens", false, new DateOnly(2024, 5, 12), "LU", "Lomme", 1600m },
                    { 133, 3, "Drioul", null, new DateOnly(2024, 5, 13), "BE", "Gustave", 1300m },
                    { 134, 1, "Dupont", true, new DateOnly(2024, 5, 14), "NL", "Marcel", 1700m },
                    { 135, 2, "Eeckhout", false, new DateOnly(2024, 5, 15), "DE", "Niko", 1200m },
                    { 136, 6, "Emonds", null, new DateOnly(2024, 5, 16), "FR", "Nico", 1600m },
                    { 137, 5, "Farazijn", true, new DateOnly(2024, 5, 17), "IT", "Peter", 2000m },
                    { 138, 5, "Frison", false, new DateOnly(2024, 5, 18), "LU", "Herman", 1500m },
                    { 139, 3, "Garnier", null, new DateOnly(2024, 5, 19), "BE", "Henri", 1900m },
                    { 140, 6, "Gielen", true, new DateOnly(2024, 5, 20), "NL", "Frans", 1600m },
                    { 141, 1, "Gijssels", false, new DateOnly(2024, 5, 21), "DE", "Romain", 1100m },
                    { 142, 5, "Godefroot", null, new DateOnly(2024, 5, 22), "FR", "Walter", 1500m },
                    { 143, 3, "Govaerts", true, new DateOnly(2024, 5, 23), "IT", "Dries", 1900m },
                    { 144, 4, "Grysolle", false, new DateOnly(2024, 5, 24), "LU", "Sylvain", 1400m },
                    { 145, 2, "Gyselinck", true, new DateOnly(2024, 5, 25), "BE", "Roger", 1800m },
                    { 146, 2, "Haghedooren", null, new DateOnly(2024, 5, 26), "NL", "Paul", 2200m },
                    { 147, 5, "Hamerlinck", true, new DateOnly(2024, 5, 27), "DE", "Alfred", 1000m },
                    { 148, 4, "Hardiquest", false, new DateOnly(2024, 5, 28), "FR", "Louis", 1400m },
                    { 149, 2, "Hardy", null, new DateOnly(2024, 5, 29), "IT", "Emile", 1800m },
                    { 150, 3, "Hendrikx", true, new DateOnly(2024, 5, 30), "LU", "Marcel", 1300m },
                    { 151, 1, "Hoevenaers", false, new DateOnly(2024, 5, 31), "BE", "Joseph", 1700m },
                    { 152, 1, "Hulsmans", null, new DateOnly(2024, 6, 1), "NL", "Kevin", 2100m },
                    { 153, 6, "Impanis", true, new DateOnly(2024, 6, 2), "DE", "Raymond", 1600m },
                    { 154, 3, "In''t", false, new DateOnly(2024, 6, 3), "FR", "Paul", 1300m },
                    { 155, 1, "In''t", null, new DateOnly(2024, 6, 4), "IT", "Willy", 1700m },
                    { 156, 2, "Janssens", true, new DateOnly(2024, 6, 5), "LU", "Marcel", 1200m },
                    { 157, 6, "Javaux", false, new DateOnly(2024, 6, 6), "BE", "Benjamin", 1600m },
                    { 158, 5, "Kaers", null, new DateOnly(2024, 6, 7), "NL", "Karel", 2000m },
                    { 159, 5, "Kemplaire", true, new DateOnly(2024, 6, 8), "DE", "Francis", 1500m },
                    { 160, 3, "Kerckhove", false, new DateOnly(2024, 6, 9), "FR", "Norbert", 1900m },
                    { 161, 6, "Keteleer", null, new DateOnly(2024, 6, 10), "IT", "Désiré", 1600m },
                    { 162, 1, "Kint", true, new DateOnly(2024, 6, 11), "LU", "Marcel", 1100m },
                    { 163, 5, "Lambot", false, new DateOnly(2024, 6, 12), "BE", "Firmin", 1500m },
                    { 164, 3, "Lambrecht", null, new DateOnly(2024, 6, 13), "NL", "Roger", 1900m },
                    { 165, 4, "Leman", true, new DateOnly(2024, 6, 14), "DE", "Eric", 1400m },
                    { 166, 2, "Leroy", false, new DateOnly(2024, 6, 15), "FR", "Camille", 1800m },
                    { 167, 2, "Liboton", null, new DateOnly(2024, 6, 16), "IT", "Roland", 2200m },
                    { 168, 5, "Lowie", true, new DateOnly(2024, 6, 17), "LU", "Jules", 1000m },
                    { 169, 4, "Lurquin", false, new DateOnly(2024, 6, 18), "BE", "André", 1400m },
                    { 170, 2, "Rik", null, new DateOnly(2024, 6, 19), "NL", "Henri", 1800m },
                    { 171, 3, "Machiels", true, new DateOnly(2024, 6, 20), "DE", "Pierrot", 1300m },
                    { 172, 1, "Maelbrancke", false, new DateOnly(2024, 6, 21), "FR", "André", 1700m },
                    { 173, 1, "Maertens", null, new DateOnly(2024, 6, 22), "IT", "Freddy", 2100m },
                    { 174, 6, "Maes", true, new DateOnly(2024, 6, 23), "LU", "Romain", 1600m },
                    { 175, 3, "Maes", false, new DateOnly(2024, 6, 24), "BE", "Sylvère", 1300m },
                    { 176, 1, "Marchand", null, new DateOnly(2024, 6, 25), "NL", "Joseph", 1700m },
                    { 177, 2, "Martens", true, new DateOnly(2024, 6, 26), "DE", "René", 1200m },
                    { 178, 6, "Martin", false, new DateOnly(2024, 6, 27), "FR", "Jacques", 1600m },
                    { 179, 5, "père", null, new DateOnly(2024, 6, 28), "IT", "Emile", 2000m },
                    { 180, 5, "Mathieu", true, new DateOnly(2024, 6, 29), "LU", "Florent", 1500m },
                    { 181, 3, "Mattan", false, new DateOnly(2024, 6, 30), "BE", "Nico", 1900m },
                    { 182, 6, "Meirhaeghe", null, new DateOnly(2024, 7, 1), "NL", "Filip", 1600m },
                    { 183, 1, "Merckx", true, new DateOnly(2024, 7, 2), "DE", "Axel", 1100m },
                    { 184, 5, "Merckx", false, new DateOnly(2024, 7, 3), "FR", "Eddy", 1500m },
                    { 185, 3, "Messelis", null, new DateOnly(2024, 7, 4), "IT", "André", 1900m },
                    { 186, 4, "Meuleman", true, new DateOnly(2024, 7, 5), "LU", "Maurice", 1400m },
                    { 187, 2, "Meulenberg", false, new DateOnly(2024, 7, 6), "BE", "Eloi", 1800m },
                    { 188, 2, "Mintjens", null, new DateOnly(2024, 7, 7), "NL", "Frans", 2200m },
                    { 189, 5, "Molenaers", true, new DateOnly(2024, 7, 8), "DE", "Yvo", 1000m },
                    { 190, 4, "Mollin", false, new DateOnly(2024, 7, 9), "FR", "Maurice", 1400m },
                    { 191, 2, "Mommerency", null, new DateOnly(2024, 7, 10), "IT", "Arthur", 1800m },
                    { 192, 3, "Monséré", true, new DateOnly(2024, 7, 11), "LU", "Jean-Pierre", 1300m },
                    { 193, 1, "Monty", false, new DateOnly(2024, 7, 12), "BE", "Willy", 1700m },
                    { 194, 1, "Moreels", null, new DateOnly(2024, 7, 13), "NL", "Sammie", 2100m },
                    { 195, 6, "Mottard", true, new DateOnly(2024, 7, 14), "DE", "Alfred", 1600m },
                    { 196, 3, "Mottart", false, new DateOnly(2024, 7, 15), "FR", "Ernest", 1300m },
                    { 197, 1, "Mottiat", null, new DateOnly(2024, 7, 16), "IT", "Louis", 1700m },
                    { 198, 2, "Museeuw", true, new DateOnly(2024, 7, 17), "LU", "Johan", 1200m },
                    { 199, 6, "Nelissen", false, new DateOnly(2024, 7, 18), "BE", "Wilfried", 1600m },
                    { 200, 5, "Neuville", null, new DateOnly(2024, 7, 19), "NL", "François", 2000m },
                    { 201, 5, "Noyelle", true, new DateOnly(2024, 7, 20), "DE", "André", 1500m },
                    { 202, 3, "Nulens", false, new DateOnly(2024, 7, 21), "FR", "Guy", 1900m },
                    { 203, 6, "Nuyens", null, new DateOnly(2024, 7, 22), "IT", "Nick", 1600m },
                    { 204, 1, "Nys", true, new DateOnly(2024, 7, 23), "LU", "Sven", 1100m },
                    { 205, 5, "Ockers", false, new DateOnly(2024, 7, 24), "BE", "Stan", 1500m },
                    { 206, 3, "Oellibrandt", null, new DateOnly(2024, 7, 25), "NL", "Petrus", 1900m },
                    { 207, 4, "Ollivier", true, new DateOnly(2024, 7, 26), "DE", "Valère", 1400m },
                    { 208, 2, "Peelman", false, new DateOnly(2024, 7, 27), "FR", "Eddy", 1800m },
                    { 209, 2, "Peeters", null, new DateOnly(2024, 7, 28), "IT", "Edward", 2200m },
                    { 210, 5, "Petitjean", true, new DateOnly(2024, 7, 29), "LU", "Luc", 1000m },
                    { 211, 4, "Louis", false, new DateOnly(2024, 7, 30), "BE", "Victor", 1400m },
                    { 212, 2, "Pintens", null, new DateOnly(2024, 7, 31), "NL", "Georges", 1800m },
                    { 213, 3, "Pirmez", true, new DateOnly(2024, 8, 1), "DE", "Théodore", 1300m },
                    { 214, 1, "Planckaert", false, new DateOnly(2024, 8, 2), "FR", "Eddy", 1700m },
                    { 215, 1, "Planckaert", null, new DateOnly(2024, 8, 3), "IT", "Jo", 2100m },
                    { 216, 6, "Planckaert", true, new DateOnly(2024, 8, 4), "LU", "Walter", 1600m },
                    { 217, 3, "Planckaert", false, new DateOnly(2024, 8, 5), "BE", "Willy", 1300m },
                    { 218, 1, "Pollentier", null, new DateOnly(2024, 8, 6), "NL", "Michel", 1700m },
                    { 219, 2, "Poncelet", true, new DateOnly(2024, 8, 7), "DE", "Léon", 1200m },
                    { 220, 6, "Proost", false, new DateOnly(2024, 8, 8), "FR", "Louis", 1600m },
                    { 221, 5, "Protin", null, new DateOnly(2024, 8, 9), "IT", "Robert", 2000m },
                    { 222, 5, "Ramon", true, new DateOnly(2024, 8, 10), "LU", "Albert", 1500m },
                    { 223, 3, "Rebry", false, new DateOnly(2024, 8, 11), "BE", "Gaston", 1900m },
                    { 224, 6, "Renders", null, new DateOnly(2024, 8, 12), "NL", "Jens", 1600m },
                    { 225, 1, "Reybrouck", true, new DateOnly(2024, 8, 13), "DE", "Guido", 1100m },
                    { 226, 5, "Rijckaert", false, new DateOnly(2024, 8, 14), "FR", "Marcel", 1500m },
                    { 227, 3, "Ritserveldt", null, new DateOnly(2024, 8, 15), "IT", "Albert", 1900m },
                    { 228, 4, "Roesems", true, new DateOnly(2024, 8, 16), "LU", "Bert", 1400m },
                    { 229, 2, "Rolus", false, new DateOnly(2024, 8, 17), "BE", "Louis", 1800m },
                    { 230, 2, "Ronsse", null, new DateOnly(2024, 8, 18), "NL", "Georges", 2200m },
                    { 231, 5, "Rosseel", true, new DateOnly(2024, 8, 19), "DE", "André", 1000m },
                    { 232, 4, "Salmon", false, new DateOnly(2024, 8, 20), "FR", "Félicien", 1400m },
                    { 233, 2, "Schaeken", null, new DateOnly(2024, 8, 21), "IT", "Léopold", 1800m },
                    { 234, 3, "Scheers", true, new DateOnly(2024, 8, 22), "LU", "Willy", 1300m },
                    { 235, 1, "Schepers", false, new DateOnly(2024, 8, 23), "BE", "Alfons", 1700m },
                    { 236, 1, "Scherens", null, new DateOnly(2024, 8, 24), "NL", "Joseph", 2100m },
                    { 237, 6, "Scherens", true, new DateOnly(2024, 8, 25), "DE", "Jef", 1600m },
                    { 238, 3, "Schotte", false, new DateOnly(2024, 8, 26), "FR", "Briek", 1300m },
                    { 239, 1, "Schoubben", null, new DateOnly(2024, 8, 27), "IT", "Frans", 1700m },
                    { 240, 2, "Scieur", true, new DateOnly(2024, 8, 28), "LU", "Léon", 1200m },
                    { 241, 6, "Sellier", false, new DateOnly(2024, 8, 29), "BE", "Félix", 1600m },
                    { 242, 5, "Sels", null, new DateOnly(2024, 8, 30), "NL", "Edward", 2000m },
                    { 243, 5, "Sercu", true, new DateOnly(2024, 8, 31), "DE", "Albert", 1500m },
                    { 244, 3, "Sercu", false, new DateOnly(2024, 9, 1), "FR", "Patrick", 1900m },
                    { 245, 6, "de Smet", null, new DateOnly(2024, 9, 2), "IT", "Andy", 1600m },
                    { 246, 1, "Somers", true, new DateOnly(2024, 9, 3), "LU", "Joseph", 1100m },
                    { 247, 5, "Steels", false, new DateOnly(2024, 9, 4), "BE", "Tom", 1500m },
                    { 248, 3, "Sterckx", null, new DateOnly(2024, 9, 5), "NL", "Ernest", 1900m },
                    { 249, 4, "Storme", true, new DateOnly(2024, 9, 6), "DE", "Lucien", 1400m },
                    { 250, 2, "Stubbe", false, new DateOnly(2024, 9, 7), "FR", "Tom", 1800m },
                    { 251, 2, "Swerts", null, new DateOnly(2024, 9, 8), "IT", "Roger", 2200m },
                    { 252, 5, "Targez", true, new DateOnly(2024, 9, 10), "LU", "Arthur", 1000m },
                    { 253, 4, "Tchmil", false, new DateOnly(2024, 9, 11), "BE", "Andrei", 1400m },
                    { 254, 2, "Thoma", null, new DateOnly(2024, 9, 12), "NL", "Emmanuel", 1800m },
                    { 255, 3, "Thys", true, new DateOnly(2024, 9, 13), "DE", "Philippe", 1300m },
                    { 256, 1, "Tiberghien", false, new DateOnly(2024, 9, 14), "FR", "Hector", 1700m },
                    { 257, 1, "Tommies", null, new DateOnly(2024, 9, 15), "IT", "Léon", 2100m },
                    { 258, 6, "Troonbeeckx", true, new DateOnly(2024, 9, 16), "LU", "Lode", 1600m },
                    { 259, 3, "Van Avermaet", false, new DateOnly(2024, 9, 17), "BE", "Greg", 1300m },
                    { 260, 1, "Van Bruaene", null, new DateOnly(2024, 9, 18), "NL", "Armand", 1700m },
                    { 261, 2, "Vanconingsloo", true, new DateOnly(2024, 9, 19), "DE", "Georges", 1200m },
                    { 262, 6, "Van Daele", false, new DateOnly(2024, 9, 20), "FR", "Léon", 1600m },
                    { 263, 5, "Van Den Born", null, new DateOnly(2024, 9, 21), "IT", "Charles", 2000m },
                    { 264, 5, "Vandenbroucke", true, new DateOnly(2024, 9, 22), "LU", "Frank", 1500m },
                    { 265, 3, "Vanden Meerschaut", false, new DateOnly(2024, 9, 23), "BE", "Odiel", 1900m },
                    { 266, 6, "Vanderaerden", null, new DateOnly(2024, 9, 24), "NL", "Eric", 1600m },
                    { 267, 1, "Van de Wouwer", true, new DateOnly(2024, 9, 25), "DE", "Kurt", 1100m },
                    { 268, 5, "Van Genechten", false, new DateOnly(2024, 9, 26), "FR", "Richard", 1500m },
                    { 269, 3, "Van Geneugden", null, new DateOnly(2024, 9, 27), "IT", "Martin", 1900m },
                    { 270, 4, "Van Hauwaert", true, new DateOnly(2024, 9, 28), "LU", "Cyrille", 1400m },
                    { 271, 2, "Van Herzele", false, new DateOnly(2024, 9, 30), "BE", "Maurice", 1800m },
                    { 272, 2, "Van Hevel", null, new DateOnly(2024, 10, 1), "NL", "Jules", 2200m },
                    { 273, 5, "Van Hooydonck", true, new DateOnly(2024, 10, 2), "DE", "Edwig", 1000m },
                    { 274, 4, "Van Impe", false, new DateOnly(2024, 10, 3), "FR", "Lucien", 1400m },
                    { 275, 2, "Van Kerkhove", null, new DateOnly(2024, 10, 4), "IT", "Henri", 1800m },
                    { 276, 3, "Van Linden", true, new DateOnly(2024, 10, 5), "LU", "Rik", 1300m },
                    { 277, 1, "Van Looy", false, new DateOnly(2024, 10, 6), "BE", "Rik", 1700m },
                    { 278, 1, "Vannitsen", null, new DateOnly(2024, 10, 7), "NL", "Willy", 2100m },
                    { 279, 6, "Van Petegem", true, new DateOnly(2024, 10, 8), "DE", "Peter", 1600m },
                    { 280, 3, "Van Santvliet", false, new DateOnly(2024, 10, 9), "FR", "Peter", 1300m },
                    { 281, 1, "Van Schil", null, new DateOnly(2024, 10, 10), "IT", "Victor", 1700m },
                    { 282, 2, "van Springel", true, new DateOnly(2024, 10, 11), "LU", "Herman", 1200m },
                    { 283, 6, "Van Steenbergen", false, new DateOnly(2024, 10, 12), "BE", "Rik", 1600m },
                    { 284, 5, "Van Tongerloo", null, new DateOnly(2024, 10, 13), "NL", "Guillaume", 2000m },
                    { 285, 5, "Vantyghem", true, new DateOnly(2024, 10, 14), "DE", "Noël", 1500m },
                    { 286, 3, "Verbrugghe", false, new DateOnly(2024, 10, 15), "FR", "Rik", 1900m },
                    { 287, 6, "Verdyck", null, new DateOnly(2024, 10, 16), "IT", "Auguste", 1600m },
                    { 288, 1, "Verhaert", true, new DateOnly(2024, 10, 17), "LU", "Jozef", 1100m },
                    { 289, 5, "Vermandel", false, new DateOnly(2024, 10, 18), "BE", "René", 1500m },
                    { 290, 3, "Vermaut", null, new DateOnly(2024, 10, 19), "NL", "Stive", 1900m },
                    { 291, 4, "Verschueren", true, new DateOnly(2024, 10, 20), "DE", "Adolf", 1400m },
                    { 292, 2, "Verschueren", false, new DateOnly(2024, 10, 21), "FR", "Constant", 1800m },
                    { 293, 2, "Verstrepen", null, new DateOnly(2024, 10, 22), "IT", "Johan", 2200m },
                    { 294, 5, "Vervaecke", true, new DateOnly(2024, 10, 23), "LU", "Félicien", 1000m },
                    { 295, 4, "Vervaecke", false, new DateOnly(2024, 10, 24), "BE", "Julien", 1400m },
                    { 296, 2, "Vissers", null, new DateOnly(2024, 10, 25), "NL", "Edward", 1800m },
                    { 297, 3, "Vlaemynck", true, new DateOnly(2024, 10, 26), "DE", "Lucien", 1300m },
                    { 298, 1, "Vlaeyen", false, new DateOnly(2024, 10, 27), "FR", "André", 1700m },
                    { 299, 1, "Vliegen", null, new DateOnly(2024, 10, 28), "IT", "Jean", 2100m },
                    { 300, 6, "Wallays", true, new DateOnly(2024, 10, 29), "LU", "Luc", 1600m },
                    { 301, 3, "Walschot", false, new DateOnly(2024, 10, 30), "BE", "René", 1300m },
                    { 302, 1, "Wampers", null, new DateOnly(2024, 10, 31), "NL", "Jean-Marie", 1700m },
                    { 303, 2, "Wancour", true, new DateOnly(2024, 11, 1), "DE", "Robert", 1200m },
                    { 304, 6, "Wellens", false, new DateOnly(2024, 11, 2), "FR", "Bart", 1600m },
                    { 305, 5, "Wesemael", null, new DateOnly(2024, 11, 3), "IT", "Wilfried", 2000m },
                    { 306, 5, "Weylandt", true, new DateOnly(2024, 11, 4), "LU", "Wouter", 1500m },
                    { 307, 3, "Wauters", false, new DateOnly(2024, 11, 5), "BE", "Marc", 1900m },
                    { 308, 6, "Willems", null, new DateOnly(2024, 11, 6), "NL", "Daniel", 1600m },
                    { 309, 1, "Wouters", true, new DateOnly(2024, 11, 7), "DE", "Jozef", 1100m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 294);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 296);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 297);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 298);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 299);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Docenten",
                keyColumn: "DocentId",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "Campussen",
                keyColumn: "CampusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Campussen",
                keyColumn: "CampusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Campussen",
                keyColumn: "CampusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Campussen",
                keyColumn: "CampusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Campussen",
                keyColumn: "CampusId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Campussen",
                keyColumn: "CampusId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Landen",
                keyColumn: "LandCode",
                keyValue: "BE");

            migrationBuilder.DeleteData(
                table: "Landen",
                keyColumn: "LandCode",
                keyValue: "DE");

            migrationBuilder.DeleteData(
                table: "Landen",
                keyColumn: "LandCode",
                keyValue: "FR");

            migrationBuilder.DeleteData(
                table: "Landen",
                keyColumn: "LandCode",
                keyValue: "IT");

            migrationBuilder.DeleteData(
                table: "Landen",
                keyColumn: "LandCode",
                keyValue: "LU");

            migrationBuilder.DeleteData(
                table: "Landen",
                keyColumn: "LandCode",
                keyValue: "NL");
        }
    }
}
