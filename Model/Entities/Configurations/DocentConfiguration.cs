using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.Entities.Configurations;

class DocentConfiguration : IEntityTypeConfiguration<Docent>
{
    public void Configure(EntityTypeBuilder<Docent> builder)
    {
        //throw new NotImplementedException();

        builder.ToTable("Docenten");

        builder.HasIndex(b => new { b.Voornaam, b.Familienaam })
            .HasDatabaseName("Idx_DocentNaam");

        //builder
        //	.HasIndex(b => b.CampusId)
        //	.HasDatabaseName("Idx_DocentCampus");

        //builder
        //	.HasIndex(b => b.LandCode)
        //	.HasDatabaseName("Idx_DocentLand");

        builder.HasKey(c => c.DocentId);

        builder.Property(b => b.DocentId)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Voornaam)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Familienaam)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(b => b.Wedde)
            .HasColumnName("Maandwedde")
            .HasColumnType("decimal(18, 4)");

        builder.Property(b => b.InDienst)
            .HasColumnType("date");

        //builder
        //	.HasOne(b => b.Campus)
        //	.WithMany(c => c.Docenten)
        //	.HasForeignKey(b => b.CampusId);

        //builder
        //	.HasOne(b => b.Land)
        //	.WithMany(c => c.Docenten)
        //	.HasForeignKey(b => b.LandCode);
    }
}