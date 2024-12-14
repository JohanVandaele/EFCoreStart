using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("Landen")]
[Index(nameof(Naam), IsUnique = true, Name = "Idx_LandNaam")]
public class Land
{
    // ----------
    // Properties
    // ----------
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(2)]
    public required string LandCode { get; set; }

    [StringLength(25)]
    public required string Naam { get; set; }

    // ---------------------
    // Navigation Properties
    // ---------------------
    public required ICollection<Docent> Docenten { get; set; }
}