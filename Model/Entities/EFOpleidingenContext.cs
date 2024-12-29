global using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Model.Entities.Configurations;
using Model.Entities.Seedings;

namespace Model.Entities;

public class EFOpleidingenContext : DbContext
{
    // ------------------
    // Instance variables
    // ------------------
    private static IConfigurationRoot Configuration = null!;
    private const string ConnectionString = "efopleidingen";
    private static bool TestMode;		// = false;

    // -----
    // DbSet
    // -----
    public DbSet<Campus> Campussen { get; set; } = null!;
    public DbSet<Docent> Docenten { get; set; } = null!;
    public DbSet<Land> Landen { get; set; } = null!;

    // ------------
    // Constructors
    // ------------
    public EFOpleidingenContext() { }
    public EFOpleidingenContext(DbContextOptions<EFOpleidingenContext> options) : base(options) { }

    // -------
    // Methods
    // -------
    // OnConfiguring
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information, DbContextLoggerOptions.DefaultWithUtcTime);

        if (!optionsBuilder.IsConfigured)
        {
            // Zoek de naam in de connectionStrings section - appsettings.json
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var connectionString = Configuration.GetConnectionString(ConnectionString);

            if (connectionString != null) // Indien de naam is gevonden
            {
                optionsBuilder.UseSqlServer(
                    connectionString
                    // Max aantal SQL commands die kunnen doorgestuurd worden naar de database
                    , options => options.MaxBatchSize(150)) //;
                    .LogTo(Console.WriteLine
                        , new[] { DbLoggerCategory.Database.Command.Name }
                        , LogLevel.Information
                        , DbContextLoggerOptions.Level | DbContextLoggerOptions.LocalTime)
						// Toont de waarden van de parameters bij de logging
						.EnableSensitiveDataLogging(true)//;
                        .UseLazyLoadingProxies();
            }
		}
        else
        {
            TestMode = true;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // -------------
        // Configuration
        // -------------
        modelBuilder.ApplyConfiguration(new CampusConfiguration());
        modelBuilder.ApplyConfiguration(new LandConfiguration());
        modelBuilder.ApplyConfiguration(new DocentConfiguration());

        // -------
        // Seeding
        // -------
        if (!TestMode)
        {
            modelBuilder.ApplyConfiguration(new CampusSeeding());
            modelBuilder.ApplyConfiguration(new DocentSeeding());
            modelBuilder.ApplyConfiguration(new LandSeeding());
        }

        //// ------
        //// Docent
        //// ------
        //// [Table("Docenten")]
        //modelBuilder.Entity<Docent>().ToTable("Docenten");

        //// [Index(nameof(Voornaam), nameof(Familienaam), Name ="Idx_DocentNaam")]
        //modelBuilder.Entity<Docent>()
        //    .HasIndex(b => new { b.Voornaam, b.Familienaam })
        //    .HasDatabaseName("Idx_DocentNaam");

        ////// [Index(nameof(CampusId), Name ="Idx_DocentCampus")]
        ////modelBuilder.Entity<Docent>()
        ////	.HasIndex(b => b.CampusId)
        ////	.HasDatabaseName("Idx_DocentCampus");

        ////// [Index(nameof(LandCode), Name ="Idx_DocentLand")]
        ////modelBuilder.Entity<Docent>()
        ////	.HasIndex(b => b.LandCode)
        ////	.HasDatabaseName("Idx_DocentLand");

        //// [Key]
        //modelBuilder.Entity<Docent>().HasKey(c => c.DocentId);

        //// [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //modelBuilder.Entity<Docent>().Property(b => b.DocentId)
        //    .ValueGeneratedOnAdd();

        //modelBuilder.Entity<Docent>().Property(b => b.Voornaam)
        //    .IsRequired()               // [Required]
        //    .HasMaxLength(20);          // [MaxLength(20)]

        //modelBuilder.Entity<Docent>().Property(b => b.Familienaam)
        //    .IsRequired()               // [Required]
        //    .HasMaxLength(30);          // [MaxLength(30)]

        //// [Column("Maandwedde", TypeName = "decimal(18,4)")]
        //modelBuilder.Entity<Docent>().Property(b => b.Wedde)
        //    .HasColumnName("Maandwedde")
        //    .HasColumnType("decimal(18, 4)");

        //modelBuilder.Entity<Docent>().Property(b => b.InDienst)
        //    .HasColumnType("date");   // [Column(TypeName = "date")]

        ////modelBuilder.Entity<Docent>()
        ////	.HasOne(b => b.Campus)
        ////	.WithMany(c => c.Docenten)
        ////	.HasForeignKey(b => b.CampusId);

        ////modelBuilder.Entity<Docent>()
        ////	.HasOne(b => b.Land)
        ////	.WithMany(c => c.Docenten)
        ////	.HasForeignKey(b => b.LandCode);

        //// ------
        //// Campus
        //// ------
        //// [Table("Campussen")]
        //modelBuilder.Entity<Campus>().ToTable("Campussen");

        //// [Index(nameof(Naam), IsUnique = true, Name ="Idx_CampusNaam")]
        //modelBuilder.Entity<Campus>()
        //    .HasIndex(b => b.Naam)
        //    .HasDatabaseName("Idx_CampusNaam")
        //    .IsUnique();

        //// [Key]
        //modelBuilder.Entity<Campus>().HasKey(c => c.CampusId);

        //// [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //modelBuilder.Entity<Campus>().Property(b => b.CampusId)
        //    .ValueGeneratedOnAdd();

        //modelBuilder.Entity<Campus>().Property(b => b.Naam)
        //    .HasColumnName("CampusNaam")    // [Column("CampusNaam")]
        //    .IsRequired();                  // [Required]

        //modelBuilder.Entity<Campus>().Property(b => b.Straat)
        //    .HasMaxLength(50);              // [StringLength(50)]

        //modelBuilder.Entity<Campus>().Property(b => b.Huisnummer)
        //    .HasMaxLength(5);               // [StringLength(5)]

        //modelBuilder.Entity<Campus>().Property(b => b.Postcode)
        //    .HasMaxLength(4);               // [StringLength(4)]

        //modelBuilder.Entity<Campus>().Property(b => b.Gemeente)
        //    .HasMaxLength(50);              // [StringLength(50)]

        //// [NotMapped]
        //modelBuilder.Entity<Campus>().Ignore(c => c.Commentaar);

        //// ----
        //// Land
        //// ----
        //// [Table("Landen")]
        //modelBuilder.Entity<Land>().ToTable("Landen");

        //// [Index(nameof(Naam), IsUnique = true, Name ="Idx_LandNaam")]
        //modelBuilder.Entity<Land>()
        //    .HasIndex(b => b.Naam)
        //    .HasDatabaseName("Idx_LandNaam")
        //    .IsUnique();

        //// [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //modelBuilder.Entity<Land>().HasKey(c => c.LandCode);

        //modelBuilder.Entity<Land>().Property(b => b.LandCode)
        //    .ValueGeneratedNever()
        //    .HasMaxLength(2);

        //modelBuilder.Entity<Land>().Property(b => b.Naam)
        //    .HasMaxLength(25);          // [StringLength(50)]

        base.OnModelCreating(modelBuilder);
    }
}