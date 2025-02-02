
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.AutomaticWeapon




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class AutomaticWeapon : 
    MultishotRangedWeapon,
    IAutomaticWeapon,
    IMultishotRangedWeapon,
    IRangedWeapon,
    IRangedEquipment,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IWeapon,
    IUpgradable,
    IConsumer
  {
    public int AutoRating { get; set; }

    [JsonConstructor]
    public AutomaticWeapon()
    {
    }

    public AutomaticWeapon(IMultishotRangedWeapon copyMe)
      : base(copyMe)
    {
    }

    public AutomaticWeapon(IWeapon copyMe)
      : base(copyMe)
    {
    }

    public bool FireBurst() => this.Use(this.AutoRating);

    public bool FireAuto() => this.Use(3 * this.AutoRating);
  }
}
