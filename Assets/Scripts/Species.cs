
//struct do przechowywania gatunków które wezm¹ udzia³ w symulacji
public struct Species
{
    public int Id { get; }
    public string SpeciesName { get; }
    public string GenomCode { get; }
    public int Count { get; }
    public bool Moving { get; }
    public bool Defender { get; }

    public Species(int id, string speciesName, string genomCode, int count, bool moving, bool defender)
    {
        Id = id;
        SpeciesName = speciesName;
        GenomCode = genomCode;
        Count = count;
        Moving = moving;
        Defender = defender;
    }

    public override string ToString()
    {
        return $"Id: {Id}, SpeciesName: {SpeciesName}, Genom: {GenomCode}, Count: {Count}, Moving: {Moving}, Defender: {Defender}";
    }
}