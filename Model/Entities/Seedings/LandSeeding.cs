using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.Entities.Seedings;

class LandSeeding : IEntityTypeConfiguration<Land>
{
    public void Configure(EntityTypeBuilder<Land> builder)
    {
        builder.HasData
        (
            new             // Toevoegen nieuw Land (Anonymous type)
            {
                LandCode = "BE",        // Primary Key
                Naam = "België"
            },
            new
            {
                LandCode = "NL",
                Naam = "Nederland"
            },
            new
            {
                LandCode = "DE",
                Naam = "Duitsland"
            },
            new
            {
                LandCode = "FR",
                Naam = "Frankrijk"
            },
            new
            {
                LandCode = "IT",
                Naam = "Italië"
            },
            new
            {
                LandCode = "LU",
                Naam = "Luxemburg"
            }
        );
    }
}