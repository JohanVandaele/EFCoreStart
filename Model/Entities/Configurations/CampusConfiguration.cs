using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.Entities.Configurations;

class CampusConfiguration : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> builder)
    {
        //throw new NotImplementedException();

        builder.ToTable("Campussen");

        builder.HasIndex(b => b.Naam)
            .HasDatabaseName("Idx_CampusNaam")
            .IsUnique();

        builder.HasKey(c => c.CampusId);

        builder.Property(b => b.CampusId)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Naam)
            .HasColumnName("CampusNaam")
            .IsRequired();

        builder.Property(b => b.Straat)
            .HasMaxLength(50);

        builder.Property(b => b.Huisnummer)
            .HasMaxLength(5);

        builder.Property(b => b.Postcode)
            .HasMaxLength(4);

        builder.Property(b => b.Gemeente)
            .HasMaxLength(50);

        builder.Ignore(c => c.Commentaar);
    }
}