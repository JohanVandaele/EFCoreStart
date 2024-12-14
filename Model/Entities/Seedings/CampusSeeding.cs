using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.Entities.Seedings;

class CampusSeeding : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> builder)
    {
        // --------------------------
        // Campus objecten definiëren
        // --------------------------
        var andros = new Campus         // nieuwe Campus (Strongly typed)
        {
            Naam = "Andros",
            Straat = "Somersstraat",
            Huisnummer = "22",
            Postcode = "2018",
            Gemeente = "Antwerpen",
            Docenten = null!    // required property
        };

        var delos = new Campus
        {
            Naam = "Delos",
            Straat = "Oude Vest",
            Huisnummer = "17",
            Postcode = "9200",
            Gemeente = "Dendermonde",
            Docenten = null!
        };

        var gavdos = new Campus
        {
            Naam = "Gavdos",
            Straat = "Europalaan",
            Huisnummer = "37",
            Postcode = "3600",
            Gemeente = "Genk",
            Docenten = null!
        };

        var hydra = new Campus
        {
            Naam = "Hydra",
            Straat = "Interleuvenlaan",
            Huisnummer = "2",
            Postcode = "3001",
            Gemeente = "Heverlee",
            Docenten = null!
        };

        var ikaria = new Campus
        {
            Naam = "Ikaria",
            Straat = "Vlamingstraat",
            Huisnummer = "10",
            Postcode = "8560",
            Gemeente = "Wevelgem",
            Docenten = null!
        };

        var oinouses = new Campus
        {
            Naam = "Oinouses",
            Straat = "Archimedesstraat",
            Huisnummer = "4",
            Postcode = "8400",
            Gemeente = "Oostende",
            Docenten = null!
        };

        // --------------
        // Seeding Campus
        // --------------
        builder.HasData
        (
            andros, delos, gavdos, hydra, ikaria, oinouses
        );
    }
}