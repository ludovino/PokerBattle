using UnityEngine;

[CreateAssetMenu(menuName ="Level/ShopEncounterType")]
public class ShopEncounerType : EncounterType
{
    public override Encounter GetEncounter()
    {
        return new ShopEncounter(this);
    }
}

internal class ShopEncounter : Encounter
{
    private ShopEncounerType _encounerType;

    public ShopEncounter(ShopEncounerType encounerType)
    {
        _encounerType = encounerType;
    }

    public override string Name => "Shop Encounter";

    public override EncounterType EncounterType => _encounerType;

    public override string Tooltip => "Shop";

    public override void BeginEncounter()
    {
        GameController.Instance.OpenShop();
    }

    public override void SkipEncounter(){}
}