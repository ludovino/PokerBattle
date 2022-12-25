
public class ShopEncounerType : EncounterType
{
    public override Encounter GetEncounter()
    {
        return new ShopEncounter();
    }
}

internal class ShopEncounter : Encounter
{
    private ShopEncounerType _encounerType;
    public override string Name => "Shop Encounter";

    public override EncounterType EncounterType => _encounerType;

    public override string Tooltip => "Shop";

    public override void BeginEncounter()
    {
        GameController.Instance.OpenShop();
    }

    public override void SkipEncounter(){}
}