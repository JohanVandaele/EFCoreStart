using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Services;

namespace Tests;

[TestClass]
public class UnitTestsSqLite
{
    private SqliteConnection connection = null!;
    private DbContextOptions<EFOpleidingenContext> options = null!;

    [TestInitialize]
    public void Initializer()
    {
        var connectionStringBuilder =
            new SqliteConnectionStringBuilder { DataSource = ":memory:" };

        connection = new SqliteConnection(connectionStringBuilder.ToString());
        connection.Open();

        options = new DbContextOptionsBuilder<EFOpleidingenContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new EFOpleidingenContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [TestMethod]
    public void GetDocentenVoorCampus_Docenten_AantalIsZesDocenten()
    {
        //// Arrange
        //var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
        //var connection = new SqliteConnection(connectionStringBuilder.ToString());

        //connection.Open();

        //var options = new DbContextOptionsBuilder<EFOpleidingenContext>()
        //                .UseSqlite(connection)
        //                .Options;

        using var context = new EFOpleidingenContext(options);

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        // Toevoegen campussen
        var andros = new Campus
        {
            CampusId = 1,
            Naam = "Andros",
            Straat = "Somersstraat",
            Huisnummer = "22",
            Postcode = "2018",
            Gemeente = "Antwerpen",
            Docenten = null!
        };

        var delos = new Campus
        {
            CampusId = 2,
            Naam = "Delos",
            Straat = "Oude Vest",
            Huisnummer = "17",
            Postcode = "9200",
            Gemeente = "Dendermonde",
            Docenten = null!
        };

        context.Campussen.AddRange(andros, delos);

        // Toevoegen Landen – LandCode=PK
        var belgie = new Land { LandCode = "BE", Naam = "België", Docenten = null! };
        var nederland = new Land { LandCode = "NL", Naam = "Nederland", Docenten = null! };
        var duitsland = new Land { LandCode = "DE", Naam = "Duitsland", Docenten = null! };
        var frankrijk = new Land { LandCode = "FR", Naam = "Frankrijk", Docenten = null! };
        var italie = new Land { LandCode = "IT", Naam = "Italië", Docenten = null! };
        var luxemburg = new Land { LandCode = "LU", Naam = "Luxemburg", Docenten = null! };

        context.Landen.AddRange(belgie, nederland, duitsland, frankrijk, italie, luxemburg);

        // Toevoegen docenten
        context.Docenten.Add(new Docent()
        {
            DocentId = 001,
            Voornaam = "Willy",
            Familienaam = "Abbeloos",
            Wedde = 1500m,
            HeeftRijbewijs = new bool?(),
            InDienst = new DateOnly(2024, 1, 1),
            Campus = andros,
            Land = belgie
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 002,
            Voornaam = "Joseph",
            Familienaam = "Abelshausen",
            Wedde = 1800m,
            HeeftRijbewijs = true,
            InDienst = new DateOnly(2024, 1, 2),
            Campus = delos,
            Land = nederland
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 003,
            Voornaam = "Joseph",
            Familienaam = "Achten",
            Wedde = 1300m,
            HeeftRijbewijs = false,
            InDienst = new DateOnly(2024, 1, 3),
            Campus = andros,
            Land = frankrijk
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 004,
            Voornaam = "François",
            Familienaam = "Adam",
            Wedde = 1700m,
            HeeftRijbewijs = new bool?(),
            InDienst = new DateOnly(2024, 1, 4),
            Campus = delos,
            Land = duitsland
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 005,
            Voornaam = "Jan",
            Familienaam = "Adriaensens",
            Wedde = 2100m,
            HeeftRijbewijs = true,
            InDienst = new DateOnly(2024, 1, 5),
            Campus = andros,
            Land = italie
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 006,
            Voornaam = "René",
            Familienaam = "Adriaensens",
            Wedde = 1600m,
            HeeftRijbewijs = false,
            InDienst = new DateOnly(2024, 1, 6),
            Campus = delos,
            Land = belgie
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 007,
            Voornaam = "Frans",
            Familienaam = "Aerenhouts",
            Wedde = 1300m,
            HeeftRijbewijs = new bool?(),
            InDienst = new DateOnly(2024, 1, 7),
            Campus = andros,
            Land = belgie
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 008,
            Voornaam = "Emile",
            Familienaam = "Aerts",
            Wedde = 1700m,
            HeeftRijbewijs = true,
            InDienst = new DateOnly(2024, 1, 8),
            Campus = andros,
            Land = belgie
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 009,
            Voornaam = "Jean",
            Familienaam = "Aerts",
            Wedde = 1200m,
            HeeftRijbewijs = false,
            InDienst = new DateOnly(2024, 1, 9),
            Campus = delos,
            Land = belgie
        });

        context.Docenten.Add(new Docent()
        {
            DocentId = 010,
            Voornaam = "Mario",
            Familienaam = "Aerts",
            Wedde = 1600m,
            HeeftRijbewijs = new bool?(),
            InDienst = new DateOnly(2024, 1, 10),
            Campus = andros,
            Land = belgie
        });

        // Save to database
        context.SaveChanges();

        var docentService = new DocentService(context);

        // Act
        var docenten = docentService.GetDocentenVoorCampus(andros);

        // Assert
        Assert.AreEqual(6, docenten.Count());
        //Assert.AreEqual(5, docenten.Count());
    }

    [TestMethod, ExpectedException(typeof(ArgumentException))]
    public void GetDocent_Docent0_ThrowArgumentException()
    {
        // Arrange
        //var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
        //var connection = new SqliteConnection(connectionStringBuilder.ToString());

        //connection.Open();

        //var options = new DbContextOptionsBuilder<EFOpleidingenContext>()
        //    .UseSqlite(connection)
        //    .Options;

        using var context = new EFOpleidingenContext(options);

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        context.Landen.Add(new Land()
        {
            LandCode = "UA",
            Naam = "Oekraïne",
            Docenten = null!
        });

        context.SaveChanges();

        // Act
        var docentService = new DocentService(context);
        docentService.GetDocent(0); // Onbestaande docent 

        // Assert
    }

    [TestMethod]
    public void ToevoegenDocent_DocentZonderLand_DocentHeeftLandUA()
    {
        // Arrange
        //var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
        //var connection = new SqliteConnection(connectionStringBuilder.ToString());

        //connection.Open();

        //var options = new DbContextOptionsBuilder<EFOpleidingenContext>()
        //    .UseSqlite(connection)
        //    .Options;

        using var context = new EFOpleidingenContext(options);

        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();

        var docent = new Docent()
        {
            DocentId = 20,
            Voornaam = "Fanny",
            Familienaam = "Kiekeboe",
            Wedde = 10100,
            InDienst = new DateOnly(2024, 1, 1),
            Campus = new Campus
            {
                CampusId = 10,
                Naam = "Andros",
                Straat = "Somersstraat",
                Huisnummer = "22",
                Postcode = "2018",
                Gemeente = "Antwerpen",
                Docenten = null!
            },
            Land = null!
        };

        // Act
        var docentService = new DocentService(context);
        docentService.ToevoegenDocent(docent);
        context.SaveChanges();

        // Assert
        var docent1 = docentService.GetDocent(20);
        Assert.AreEqual("UA", docent1!.Land.LandCode);

        // Act

        // Assert
    }
}