
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ConsumableWeapon




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ConsumableWeapon : 
    Weapon,
    IConsumableWeapon,
    IWeapon,
    IUpgradable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IConsumable
  {
    [JsonProperty]
    protected IConsumable consumable = (IConsumable) new Consumable();

    [JsonConstructor]
    protected ConsumableWeapon(List<string> statsModifyingDamage)
      : base(statsModifyingDamage)
    {
    }

    public ConsumableWeapon()
    {
    }

    public ConsumableWeapon(IWeapon copyMe)
      : base(copyMe)
    {
    }

    public bool Rechargable
    {
      get => this.consumable.Rechargable;
      set => this.consumable.Rechargable = value;
    }

    public string ConsumableType
    {
      get => this.consumable.ConsumableType;
      set => this.consumable.ConsumableType = value;
    }

    public string AmountName
    {
      get => this.consumable.AmountName;
      set => this.consumable.AmountName = value;
    }

    public int MaxAmount
    {
      get => this.consumable.MaxAmount;
      set => this.consumable.MaxAmount = value;
    }

    public int CurrentAmount
    {
      get => this.consumable.CurrentAmount;
      set => this.consumable.CurrentAmount = value;
    }
  }
}
