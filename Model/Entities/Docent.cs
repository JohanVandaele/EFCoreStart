using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("Docenten")]
[Index(nameof(Voornaam), nameof(Familienaam), Name = "Idx_DocentNaam")]
//[Index(nameof(CampusId), Name = "Idx_DocentCampus")]
//[Index(nameof(LandCode), Name = "Idx_DocentLand")]
public partial class Docent
{
    // ----------
    // Properties
    // ----------
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DocentId { get; set; }

    [Required]
    [MaxLength(20)]
    public required string Voornaam { get; set; }

    [Required]
    [MaxLength(30)]
    public required string Familienaam { get; set; }

    [Column("Maandwedde", TypeName = "decimal(18,4)")]
    public decimal Wedde { get; set; }

    [Column(TypeName = "date")]
    public DateOnly InDienst { get; set; }

    //public bool HeeftRijbewijs { get; set; }
    public bool? HeeftRijbewijs { get; set; }

    //public int CampusId { get; set; }

    // ---------------------
    // Navigation properties
    // ---------------------
    public required Campus Campus { get; set; }		// Een Docent heeft één Campus
    public required Land Land { get; set; }			// Een Docent heeft één Land
}