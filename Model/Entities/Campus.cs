﻿//using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities;

[Table("Campussen")] //using System.ComponentModel.DataAnnotations.Schema;
[Index(nameof(Naam), IsUnique = true, Name = "Idx_CampusNaam")]
//[Table("Campussen", Schema = "Admin")] //using System.ComponentModel.DataAnnotations.Schema;
public class Campus
{
	//private readonly ILazyLoader lazyLoader = null!;        // Een private variable voor de ILazyLoader instantieµ

	//-----------
	//Constructor
	//-----------
	//public Campus()
	//{
	//	Docenten = new List<Docent>();
	//}

	//public Campus() { }

	//private Campus(ILazyLoader lazyLoader) => this.lazyLoader = lazyLoader;

	// ----------
	// Properties
	// ----------
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int CampusId { get; set; }

	[Required]
	[Column("CampusNaam")]
	[StringLength(50)]
	public required string Naam { get; set; }

	[StringLength(50)]
	public required string Straat { get; set; }

	[StringLength(5)]
	public required string Huisnummer { get; set; }

	[StringLength(4)]
	public required string Postcode { get; set; }

	[StringLength(50)]
	public required string Gemeente { get; set; }

	[NotMapped]
	public string Commentaar { get; set; } = null!;

	// ---------------------
	// Navigation Properties
	// ---------------------
	//public required ICollection<Docent> Docenten { get; set; }  // Een Campus kan meerdere Docenten hebben
	public virtual required ICollection<Docent> Docenten { get; set; }  // Een Campus kan meerdere Docenten hebben

	//private ICollection<Docent> docenten = null!;

	//public ICollection<Docent> Docenten
	//{
	//	get => lazyLoader.Load(this, ref docenten!)!;
	//	set => docenten = value;
	//}
}