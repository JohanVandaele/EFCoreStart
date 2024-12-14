namespace Model.Entities;

public partial class Docent
{
    public string Naam => $"{Voornaam} {Familienaam}";
}