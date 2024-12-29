//using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("Landen")]
[Index(nameof(Naam), IsUnique = true, Name = "Idx_LandNaam")]
public class Land
{
	//private readonly ILazyLoader lazyLoader = null!;        // Een private variable voor de ILazyLoader instantie

	// -----------
	// Constructor
	// -----------
	//public Land() { }

	//private Land(ILazyLoader lazyLoader) => this.lazyLoader = lazyLoader;

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
	//public required ICollection<Docent> Docenten { get; set; }
	public virtual required ICollection<Docent> Docenten { get; set; }

	//// Lazy Loading Docent (DI)
	//private ICollection<Docent> docenten = null!;

	//public ICollection<Docent> Docenten
	//{
	//	get => lazyLoader.Load(this, ref docenten!)!;
	//	set => docenten = value;
	//}
}