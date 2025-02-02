
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IConsumer




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IConsumer : 
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    bool UnlimitedAmmunition { get; set; }

    int Capacity { get; set; }

    int StandardConsumableCost { get; set; }

    string ConsumableAmountName { get; set; }

    bool ConsumableIsRechargable { get; set; }

    IConsumable ConsumableLoaded { get; }

    string ConsumableTypeAllowed { get; set; }

    bool Load(IConsumable loadMe);

    bool Unload();

    bool Use();

    bool Use(int amount);
  }
}
