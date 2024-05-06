
//struct do przechowywania gatunk�w kt�re wezm� udzia� w symulacji
public struct Species
{
    public int Id { get; }
    public string SpeciesName { get; }
    public string GenomCode { get; }
    public int Count { get; }
    public bool Moving { get; }
    public bool Armor { get; }

    public Species(int id, string speciesName, string genomCode, int count, bool moving, bool armor)
    {
        Id = id;
        SpeciesName = speciesName;
        GenomCode = genomCode;
        Count = count;
        Moving = moving;
        Armor = armor;
    }

    public override string ToString()
    {
        return $"Id: {Id}, SpeciesName: {SpeciesName}, Genom: {GenomCode}, Count: {Count}, Moving: {Moving}, Armor: {Armor}";
    }
}