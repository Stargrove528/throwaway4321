
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ConsumableRangedWeapon




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ConsumableRangedWeapon : 
    RangedWeapon,
    IConsumableRangedWeapon,
    IConsumableWeapon,
    IWeapon,
    IUpgradable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IConsumable,
    IRangedWeapon,
    IRangedEquipment
  {
    [JsonProperty]
    protected IConsumable consumable = (IConsumable) new Consumable();

    [JsonConstructor]
    public ConsumableRangedWeapon()
    {
    }

    public ConsumableRangedWeapon(IRangedWeapon copyMe)
      : base(copyMe)
    {
    }

    [JsonIgnore]
    public bool Rechargable
    {
      get => this.consumable.Rechargable;
      set => this.consumable.Rechargable = value;
    }

    [JsonIgnore]
    public string ConsumableType
    {
      get => this.consumable.ConsumableType;
      set => this.consumable.ConsumableType = value;
    }

    [JsonIgnore]
    public string AmountName
    {
      get => this.consumable.AmountName;
      set => this.consumable.AmountName = value;
    }

    [JsonIgnore]
    public int MaxAmount
    {
      get => this.consumable.MaxAmount;
      set => this.consumable.MaxAmount = value;
    }

    [JsonIgnore]
    public int CurrentAmount
    {
      get => this.consumable.CurrentAmount;
      set => this.consumable.CurrentAmount = value;
    }
  }
}
