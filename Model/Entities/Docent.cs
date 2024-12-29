//using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("Docenten")]
[Index(nameof(Voornaam), nameof(Familienaam), Name = "Idx_DocentNaam")]
//[Index(nameof(CampusId), Name = "Idx_DocentCampus")]
//[Index(nameof(LandCode), Name = "Idx_DocentLand")]
public partial class Docent
{
	//private readonly ILazyLoader lazyLoader = null!;        // Een private variable voor de ILazyLoader instantie

	// -----------
	// Constructor
	// -----------
	//public Docent() { }

	//private Docent(ILazyLoader lazyLoader) => this.lazyLoader = lazyLoader;

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
	//public required Campus Campus { get; set; }     		// Een Docent heeft één Campus
	//public required Land Land { get; set; }        		// Een Docent heeft één Land
	public virtual required Campus Campus { get; set; }     // Een Docent heeft één Campus
	public virtual required Land Land { get; set; }         // Een Docent heeft één Land

	//// Lazy Loading Campus (DI)
	//private Campus campus = null!;

	//public Campus Campus
	//{
	//	get => lazyLoader.Load(this, ref campus!)!;
	//	set => campus = value;
	//}

	//// Lazy Loading Land (DI)
	//private Land land = null!;

	//public Land Land
	//{
	//	get => lazyLoader.Load(this, ref land!)!;
	//	set => land = value;
	//}
}