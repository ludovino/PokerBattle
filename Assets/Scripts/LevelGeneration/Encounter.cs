public abstract class Encounter
{
    public abstract string Name { get; }
    public abstract EncounterType EncounterType { get; }
    public abstract string Tooltip { get; }
    public abstract void BeginEncounter();
    public abstract void SkipEncounter();
}

