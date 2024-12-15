using Model.Entities;

namespace Services;

public class DocentService(EFOpleidingenContext ctx)
{
    private readonly EFOpleidingenContext context = ctx;

    // -------
    // Methods
    // -------
    // GetDocentenVoorCampus
    public IEnumerable<Docent> GetDocentenVoorCampus(Campus campus)
    {
        //throw new NotImplementedException();
        return context.Docenten.Where(x => x.Campus == campus).ToList();
    }

    // GetDocent
    public Docent? GetDocent(int id)
    {
        //throw new NotImplementedException();

        if (id == 0)
        {
            throw new ArgumentException(nameof(id));
        }

        return context.Docenten.FirstOrDefault(x => x.DocentId == id);
    }

    // ToevoegenDocent
    public void ToevoegenDocent(Docent docent)
    {
        //throw new NotImplementedException();

        if (docent != null)
        {
            docent.Land ??= new Land { LandCode = "UA", Naam = "Oekraïne", Docenten = null! };
            context.Docenten.Add(docent);
        }
    }
}