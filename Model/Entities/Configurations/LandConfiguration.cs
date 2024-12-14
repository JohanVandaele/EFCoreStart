using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model.Entities.Configurations;

class LandConfiguration : IEntityTypeConfiguration<Land>
{
    public void Configure(EntityTypeBuilder<Land> builder)
    {
        //throw new NotImplementedException();

        builder.ToTable("Landen");

        builder.HasIndex(b => b.Naam)
            .HasDatabaseName("Idx_LandNaam")
            .IsUnique();

        builder.HasKey(c => c.LandCode);

        builder.Property(b => b.LandCode)
            .ValueGeneratedNever()
            .HasMaxLength(2);

        builder.Property(b => b.Naam)
            .HasMaxLength(25);
    }
}