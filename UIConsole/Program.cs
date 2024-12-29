// Tot blz 164 (12. Entities Toevoegen)

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace UIConsole;

public partial class Program
{
    private static readonly List<string> Menu =     // Spread operator
    [
        "Voorbeeld gebruik van de logging",															    // 00
		//"Menu Item 1",																				// 01
		//"Menu Item 2",																				// 02
        "De foreach iteratie - beperkt: Je kan niet sorteren of filteren",                              // 01
        "Een Linq-query",                                                                               // 02
        "Query Methods",                                                                                // 03
        "LINQ-Query en Queries met methods vergelijken - LINQ-Query",                                   // 04
        "LINQ-Query en Queries met methods vergelijken - Query-Method",                                 // 05
        "Een entity zoeken op zijn primary key waarde",                                                 // 06
		"Gedeeltelijke objecten ophalen",                                                               // 07
		"Gedeeltelijke objecten ophalen (Query methods)",                                               // 08
		"Groeperen in Queries – LINQ - Tel de docenten met dezelfde voornaam",                          // 09
		"Groeperen in Queries – LINQ - Toon de naam van docenten gegroepeerd per voornaam",             // 10
		"Groeperen in Queries - Query-Methods - Tel de docenten met dezelfde voornaam",                 // 11
		"Groeperen in Queries - Query-Methods - toon de naam van docenten gegroepeerd per voornaam",    // 12
		"Lazy loading",                                                                                 // 13
		"Eager loading (1)",                                                                            // 14
		"Eager loading (2) - LINQ",                                                                     // 15
		"De method ToList() van een query – Met Exception",                                             // 16
		"De method ToList() van een query - Zonder Exception",											// 17
		"De method ToList() van een query - Zonder Exception - 2 methods",								// 18
		// "Menu Item n",	/																		    // 0n
		// . . . .
		// P l a a t s   h i e r   d e   v e r s c h i l l e n d e   m e n u   i t e m s
		// . . . .
	];

    private static string Titel(string t) => $"{Ansi.EraseSavedLines}{Ansi.CURSORHOME}{Ansi.EraseScreen}{new string('=', t.Length)}\n{t}\n{new string('=', t.Length)}\n";

    private static void Main(string[] args)
    {
        UIConsole.Program.StartConsole();

        Console.Title = "EFCoreStart";

        var keuze = string.Empty;

        while (keuze != "X")
        {
            //ResetConsole();	// Problem when using Terminal !!!
            Console.Write($"{UIConsole.Program.ConsBGC}{UIConsole.Program.ConsFGC}{Ansi.EraseScreen}{Ansi.CURSORHOME}");
            Console.WriteLine($"{Ansi.UnderlineOn}MENU{Ansi.UnderlineOf}");

            var item = 0;
            foreach (var m in Menu) Console.WriteLine($"{item++}. {m}");

            keuze = UIConsole.Program.LeesString("Keuze ('X' om te stoppen):", 1, 2, OptionMode.Mandatory)!.ToUpper();

            if (keuze != "X")
            {
                Seperator = true;

                // Reflection
                try
                {
                    //ResetConsole();	// Problem when using Terminal !!!
                    //Console.Write($"{ConsBGC}{ConsFGC}{Ansi.EraseScreen}{Ansi.CURSORHOME}");

                    if (keuze.All(char.IsNumber) && int.Parse(keuze) < Menu.Count) Console.WriteLine(Titel($"{"00"[..(-keuze.Length + 2)] + keuze}" + ". " + Menu.ElementAt(int.Parse(keuze))));

                    //typeof(Program).InvokeMember($"Item{("00".Substring(0, -keuze.Length + 2)) + keuze}"
                    typeof(Program).InvokeMember
                    (
                          $"Item{"00"[..(-keuze.Length + 2)] + keuze}"
                        , BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic
                        , null
                        , null
                        , new object[] { new object[] { Titel($"{"00"[..(-keuze.Length + 2)] + keuze}" + ". " + Menu.ElementAt(int.Parse(keuze))), 123 } }
                    );
                }
                catch (Exception)
                {
                    UIConsole.Program.ToonFoutBoodschap("Ongeldige keuze");
                }
            }

            // ===
            // End
            // ===
            if (keuze == "X") break;

            UIConsole.Program.DrukToets();
        }
    }

    // ---------

    private static bool Seperator = false;

    private static void SeperateLog()
    {
        if (!Seperator) return;

        Thread.Sleep(2000); // Wait to Finish logging threat
        if (Seperator) Console.WriteLine("\n- - - - - - - - - - - - - - - - - - - -\n");
        Seperator = false;
    }

    // ---------

    private static void ProcessException(Exception e)
    {
        Console.WriteLine($"\n{Ansi.UnderlineOn}Exception{Ansi.UnderlineOf}\n\n{e.Message}{(e.InnerException != null ? "\n\n" + e.InnerException.Message : "")}");
        Thread.Sleep(2000); // Wait to Finish logging threat
    }

    // ----------
    // Menu-items
    // ----------
    private static void Item00(object[] args)
    {
        //ToonTekst("Kiekeboe", ConsoleColor.DarkMagenta);
        //ToonTekst("Kiekeboe");
        //ToonInfoBoodschap("InfoTekst");
        //ToonFoutBoodschap("FoutTekst");

        try
        {
            using var context = new EFOpleidingenContext();

            var docenten = from docent in context.Docenten.Include(l => l.Land).Include(c => c.Campus)
                           where docent.Campus.CampusId == 4 & docent.Voornaam.StartsWith("A")
                           orderby docent.Voornaam, docent.Familienaam
                           select docent;

            while (true)
            {
                Console.Clear();
                Console.Write($"{(string)args[0]}");    // Title

                var option = LeesGetal<ushort>($"Optie (1 = foreach, 2 = Linq (ForEach), 3 = LeesLijst en Linq (Select))", 1, 3);

                Seperator = true;

                switch (option)
                {
                    case 1:
                        // Met foreach
                        ToonInfoBoodschap("\nMet foreach\n");

                        foreach (var docent in docenten)
                        {
                            SeperateLog();  // Scheiden van logging en output met een lijn
                            Console.WriteLine($"{docent.Naam} - {docent.Campus.Naam} - {docent.Land.Naam}");
                        }

                        break;

                    case 2:
                        // Met Linq
                        ToonInfoBoodschap("\nMet Linq (ForEach)\n");

                        docenten.ToList().ForEach(d => Console.WriteLine($"{d.Naam} - {d.Campus.Naam} - {d.Land.Naam}"));
                        break;

                    case 3:
                        // Met LeesLijst()
                        ToonInfoBoodschap("\nMet LeesLijst en Linq (Select)\n");

                        //LeesLijst("", docenten, docenten.Select(s => $"{s.Naam} - {s.Campus.Naam} - {s.Land.Naam}").ToList(), SelectionMode.None);
                        LeesLijst("", docenten, [.. docenten.Select(s => $"{s.Naam} - {s.Campus.Naam} - {s.Land.Naam}")], SelectionMode.None);
                        break;

                    default:
                        return;
                }

                DrukToets();
            }
        }
        catch (Exception e)
        {
            ProcessException(e);
        }
    }

    // ---------

    // 01. De foreach iteratie - beperkt: Je kan niet sorteren of filteren
    private static void Item01(object[] args)
    {
        //ToonInfoBoodschap("Hello World");

        try
        {
            using var context = new EFOpleidingenContext();

            while (true)
            {
                Console.Clear();
                Console.Write($"{(string)args[0]}");    // Title

                var option = LeesGetal<ushort>($"Optie (1 = foreach, 2 = Linq (ForEach), 3 = LeesLijst en Linq (Select))", 1, 3);

                Seperator = true;

                switch (option)
                {
                    case 1:
                        // Met foreach
                        ToonInfoBoodschap("\nMet foreach\n");

                        foreach (var docent in context.Docenten)
                        {
                            SeperateLog();
                            Console.WriteLine(docent.Naam);
                        }

                        break;

                    case 2:
                        // Met Linq
                        ToonInfoBoodschap("\nMet Linq\n");

                        context.Docenten.ToList().ForEach(d => Console.WriteLine($"{d.Naam}"));
                        break;

                    case 3:
                        // Met LeesLijst()
                        ToonInfoBoodschap("\nMet LeesLijst\n");

                        LeesLijst("", context.Docenten, context.Docenten.Select(s => $"{s.Naam}").ToList()!, SelectionMode.None);
                        break;

                    default:
                        return;
                }

                DrukToets();
            }
        }
        catch (Exception e)
        {
            ProcessException(e);
        }
    }

    // ---------

    // 02. Een Linq-query
    private static void Item02(object[] args)
    {
        //ToonInfoBoodschap("Hello VDAB");

        try
        {
            var mijnWedde = LeesDecimal("Minimum wedde:", -99999.99m, 99999.99m, OptionMode.Mandatory);

            Console.WriteLine();

            using var context = new EFOpleidingenContext();

            var docenten = from docent in context.Docenten
                where docent.Wedde >= mijnWedde
                orderby docent.Voornaam, docent.Familienaam
                select docent;

            foreach (var docent in docenten)
            {
                SeperateLog();
                Console.WriteLine($"{docent.Naam}: {docent.Wedde}");
            }
        }
        catch (Exception e)
        {
            ProcessException(e);
        }
    }

    // ---------

    // 03. Query Methods
    private static void Item03(object[] args)
    {
        try
        {
            var minWedde = LeesDecimal("Minimum wedde:", -99999.99m, 99999.99m, OptionMode.Mandatory);

            Console.WriteLine();

            using var context = new EFOpleidingenContext();

            var docenten = context.Docenten
                .Where(docent => docent.Wedde >= minWedde)
                .OrderBy(docent => docent.Voornaam)
                .ThenBy(docent => docent.Familienaam);

            foreach (var docent in docenten)
            {
                SeperateLog();
                Console.WriteLine($"{docent.Naam}: {docent.Wedde}");
            }
        }
        catch (Exception e)
        {
            ProcessException(e);
        }
    }

    // ---------

    // 04. LINQ-Query en Queries met methods vergelijken - LINQ-Query
    private static void Item04(object[] args)
    {
        try
        {
            var minWedde = LeesDecimal("Minimum wedde:", -99999.99m, 99999.99m, OptionMode.Mandatory);
            var sorterenOp = LeesInt("Sorteren op:1=Wedde, 2=Familienaam, 3=Voornaam:", 1, 3, OptionMode.Mandatory);

            using var context = new EFOpleidingenContext();

            IQueryable<Docent> docenten;    // Het type van de variabele query is een 
            // LINQ-query die Docent entities teruggeeft
            switch (sorterenOp)
            {
                case 1:
                    ToonInfoBoodschap($"\nSorteren op Wedde\n");

                    docenten = from docent in context.Docenten
                        where docent.Wedde >= minWedde
                        orderby docent.Wedde
                        select docent;
                    break;

                case 2:
                    ToonInfoBoodschap($"\nSorteren op Familienaam\n");

                    docenten = from docent in context.Docenten
                        where docent.Wedde >= minWedde
                        orderby docent.Familienaam
                        select docent;  // deze query lijkt sterk op de vorige
                    break;

                case 3:
                    ToonInfoBoodschap($"\nSorteren op Voornaam\n");

                    docenten = from docent in context.Docenten
                        where docent.Wedde >= minWedde
                        orderby docent.Voornaam
                        select docent;  // deze query lijkt sterk op de vorige
                    break;

                default:
                    Console.WriteLine("Verkeerde keuze");
                    docenten = null!;
                    break;
            }

            if (docenten == null) return;

            foreach (var docent in docenten)
            {
                SeperateLog();
                Console.WriteLine($"{docent.Naam}: {docent.Wedde}");
            }
        }
        catch (Exception e)
        {
            ProcessException(e);
        }
    }

    // ---------

    // 05. LINQ-Query en Queries met methods vergelijken - Query-Method
    private static void Item05(object[] args)
    {
        try
        {
            var minWedde = LeesDecimal("Minimum wedde:", -99999.99m, 99999.99m, OptionMode.Mandatory);
            var sorterenOp = LeesInt("Sorteren op:1=Wedde, 2=Familienaam, 3=Voornaam:", 1, 3, OptionMode.Mandatory);

            Func<Docent, object> sorteerLambda;

            switch (sorterenOp)
            {
                case 1:
                    ToonInfoBoodschap($"\nSorteren op Wedde\n");

                    sorteerLambda = docent => docent.Wedde;
                    break;

                case 2:
                    ToonInfoBoodschap($"\nSorteren op Familienaam\n");

                    sorteerLambda = docent => docent.Familienaam;
                    break;

                case 3:
                    ToonInfoBoodschap($"\nSorteren op Voornaam\n");

                    sorteerLambda = docent => docent.Voornaam;
                    break;

                default:
                    Console.WriteLine("Verkeerde keuze");
                    sorteerLambda = null!;
                    break;
            }

            if (sorteerLambda != null)
            {
                using var context = new EFOpleidingenContext();

                var docenten = context.Docenten
                    .Where(docent => docent.Wedde >= minWedde)
                    .OrderBy(sorteerLambda);

                foreach (var docent in docenten)
                {
                    SeperateLog();
                    Console.WriteLine($"{docent.Naam}: {docent.Wedde}");
                }
            }
            else
            {
                Console.WriteLine("U tikte geen getal");
            }
        }
        catch (Exception e)
        {
            ProcessException(e);
        }
    }

	// ---------

	// 06. Een entity zoeken op zijn primary key waarde
	private static void Item06(object[] args)
	{
		try
		{
			var docentNr = LeesInt("DocentNr:", 1, int.MaxValue, OptionMode.Mandatory);

			Console.WriteLine();

			using var context = new EFOpleidingenContext();

			var docent = context.Docenten.Find(docentNr);

			Console.WriteLine($"\n{(docent == null ? "Docent niet gevonden" : docent.Naam)}");
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 07. Gedeeltelijke objecten ophalen
	private static void Item07(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			var campussen = from campus in context.Campussen
				orderby campus.Naam
				select new { campus.CampusId, campus.Naam };

			foreach (var campusDeel in campussen)
			{
				SeperateLog();
				Console.WriteLine($"{campusDeel.CampusId}: {campusDeel.Naam}");
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 08. Gedeeltelijke objecten ophalen (Query methods)
	private static void Item08(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			//var campussen = from campus in context.Campussen
			//			orderby campus.Naam
			//			select new { campus.CampusNr, campus.Naam };

			var campussen = context.Campussen.OrderBy(campus => campus.Naam)
				.Select(campus => new { campus.CampusId, campus.Naam });

			foreach (var campusDeel in campussen)
			{
				SeperateLog();
				Console.WriteLine($"{campusDeel.CampusId}: {campusDeel.Naam}");
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 09. Groeperen in Queries – LINQ - Tel de docenten met dezelfde voornaam
	private static void Item09(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			while (true)
			{
				Console.Clear();
				Console.Write($"{(string)args[0]}");    // Title

				var option = LeesGetal<ushort>($"Optie (1 = foreach, 2 = LeesLijst en Linq (Select))", 1, 2);

				Seperator = true;

				var docenten = from docent in context.Docenten
					group docent by docent.Voornaam into voornaamGroep
					select new { Voornaam = voornaamGroep.Key, Aantal = voornaamGroep.Count() };

				switch (option)
				{
					case 1:
						// Met foreach
						ToonInfoBoodschap("\nMet foreach\n");

						foreach (var docent in docenten)
						{
							SeperateLog();
							Console.WriteLine($"{docent.Voornaam} - {docent.Aantal}");
						}

						break;

					case 2:
						// Met LeesLijst()
						ToonInfoBoodschap("\nMet LeesLijst\n");

						LeesLijst("", context.Docenten, docenten.Select(docent => $"{docent.Voornaam} - {docent.Aantal}").ToList()!, SelectionMode.None);
						break;

					default:
						return;
				}

				DrukToets();
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 10. Groeperen in Queries – LINQ - Toon de naam van docenten gegroepeerd per voornaam
	private static void Item10(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			var docenten = from docent in context.Docenten.AsEnumerable()
				group docent by docent.Voornaam into voornaamGroep
				select new { voornaamGroep, Voornaam = voornaamGroep.Key };

			foreach (var voornaamStatistiek in docenten)
			{
				SeperateLog();

				Console.WriteLine($"{Ansi.UnderlineOn}{voornaamStatistiek.Voornaam}{Ansi.UnderlineOf} - {voornaamStatistiek.voornaamGroep}");

				foreach (var docent in voornaamStatistiek.voornaamGroep)
					Console.WriteLine(docent.Naam);

				Console.WriteLine();
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 11. Groeperen in Queries - Query-Methods - Tel de docenten met dezelfde voornaam
	private static void Item11(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			var docenten = context.Docenten.GroupBy((docent) => docent.Voornaam)
				.Select(s => new { Voornaam = s.Key, Aantal = s.Count() });

			foreach (var item in docenten)
			{
				SeperateLog();
				Console.WriteLine($"{item.Voornaam} - {item.Aantal}");
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 12. Groeperen in Queries - Query-Methods - toon de naam van docenten gegroepeerd per voornaam
	private static void Item12(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			var docenten = context.Docenten
				.Select(d => d)
				.AsEnumerable()
				.GroupBy(docent => docent.Voornaam)
				.Select(g => new { Voornaam = g.Key, VoornaamGroep = g.ToList<Docent>() });

			foreach (var voornaamStatistiek in docenten)
			{
				SeperateLog();

				Console.WriteLine($"{Ansi.UnderlineOn}{voornaamStatistiek.Voornaam}{Ansi.UnderlineOf}");

				foreach (var docent in voornaamStatistiek.VoornaamGroep)
					Console.WriteLine(docent.Naam);

				Console.WriteLine();
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 13. Lazy loading
	private static void Item13(object[] args)
	{
		try
		{
			var voornaam = LeesString("Voornaam:", 1, 20, OptionMode.Mandatory);

			using var context = new EFOpleidingenContext();

			//context.ChangeTracker.LazyLoadingEnabled = false;

			var docenten = (from docent in context.Docenten
				where docent.Voornaam == voornaam
				select docent).ToList();

			foreach (var docent in docenten)
			{
                SeperateLog();
                Console.WriteLine($"\n{docent.Naam} {docent.Campus.CampusId}: {docent.Campus.Naam}\n");
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 14. Eager loading (1)
	private static void Item14(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			while (true)
			{
				Console.Clear();
				Console.Write($"{(string)args[0]}");    // Title

				var voornaam = LeesString("Voornaam:", 1, 20, OptionMode.Mandatory);
				var option = LeesGetal<ushort>($"Optie (1 = Linq Query Syntax, 2 = Linq Method Syntax)", 1, 2);

				Seperator = true;

				var docenten = new List<Docent>().AsQueryable();

				switch (option)
				{
					case 1:
						// Linq Query Syntax
						ToonInfoBoodschap("\nMet Linq Query Syntax\n");

						docenten = from docent in context.Docenten.Include(c => c.Campus)
							where docent.Voornaam == voornaam
							select docent;

						break;

					case 2:
						// Linq Method Syntax
						ToonInfoBoodschap("\nMet Linq Method Syntax\n");

						docenten = context.Docenten.Where(d => d.Voornaam == voornaam).Include(c => c.Campus);

						break;

					default:
						return;
				}

				foreach (var docent in docenten)
				{
					SeperateLog();
					Console.WriteLine($"{docent.Naam}: {docent.Campus.Naam}");
				}

				DrukToets();
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 15. Eager loading (2) - LINQ
	private static void Item15(object[] args)
	{
		try
		{
			using var context = new EFOpleidingenContext();

			while (true)
			{
				Console.Clear();
				Console.Write($"{(string)args[0]}");    // Title

				var deelNaam = LeesString("Deel naam campus:", 1, 10)!;
				var option = LeesGetal<ushort>($"Optie (1 = Linq Query Syntax, 2 = Linq Method Syntax)", 1, 2);

				Seperator = true;

				var campussen = new List<Campus>().AsQueryable();

				switch (option)
				{
					case 1:
						// Linq Query Syntax
						ToonInfoBoodschap("\nMet Linq Query Syntax\n");

						campussen = from campus in context.Campussen.Include(d => d.Docenten)
							where campus.Naam.Contains(deelNaam)
							orderby campus.Naam
							select campus;

						break;

					case 2:
						// Linq Method Syntax
						ToonInfoBoodschap("\nMet Linq Method Syntax\n");

						campussen = context.Campussen.Where(c => c.Naam.Contains(deelNaam)).OrderBy(c => c.Naam).Include(d => d.Docenten);

						break;

					default:
						return;
				}

				foreach (var campus in campussen)
				{
					SeperateLog();

					var campusNaam = campus.Naam;
					Console.WriteLine($"{Ansi.UnderlineOn}{campusNaam}{Ansi.UnderlineOf}");

					foreach (var docent in campus.Docenten)
						Console.WriteLine(docent.Naam);

					Console.WriteLine();
				}

				DrukToets();
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 16. De method ToList() van een query – Met Exception
	private static void Item16(object[] args)
	{
		try
		{
			IQueryable<Campus> query;

			using (var context = new EFOpleidingenContext())
			{
				query = from campus in context.Campussen
					orderby campus.Naam
					select campus;
			}

			// Itereren na het sluiten van de DbContext (entities) kan niet !!! Exception !!!
			foreach (var campus in query) { }
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 17. De method ToList() van een query - Zonder Exception
	private static void Item17(object[] args)
	{
		try
		{
			List<Campus> campussen;

			using (var context = new EFOpleidingenContext())
			{
				var query = from campus in context.Campussen
					orderby campus.Naam
					select campus;

				campussen = [.. query];
			}

			foreach (var campus in campussen)               // Iteratie 1
			{
				SeperateLog();
				Console.WriteLine(campus.Naam);
			}

			Console.WriteLine();

			foreach (var campus in campussen)               // Iteratie 2
				Console.WriteLine(campus.Naam);
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// ---------

	// 18. De method ToList() van een query - Zonder Exception - 2 methods
	private static void Item18(object[] args)
	{
		try
		{
			foreach (var campus in FindAllCampussen())
			{
				SeperateLog();
				Console.WriteLine(campus.Naam);
			}
		}
		catch (Exception e)
		{
			ProcessException(e);
		}
	}

	// FindAllCampussen
	private static List<Campus> FindAllCampussen()
	{
		try
		{
			using var context = new EFOpleidingenContext();

			return
			[
				.. from campus in context.Campussen
				orderby campus.Naam
				select campus
			];
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}
	}

	// ---------

	//// 02. 
	//static void Item02(object[] args)
	//{
	//    ToonInfoBoodschap("Hello Johan");
	//}
}
