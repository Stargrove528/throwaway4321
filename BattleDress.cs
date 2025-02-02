
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.BattleDress




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class BattleDress : PoweredArmor
  {
    [JsonProperty]
    public int TotalSlots { get; set; }

    [JsonIgnore]
    public int AvailableSlots
    {
      get
      {
        int num = 0;
        foreach (IEquipmentOption option in this._options)
        {
          if (option is BattleDressMod)
            num += ((BattleDressMod) option).Slots;
        }
        return this.TotalSlots - num;
      }
    }

    [JsonConstructor]
    public BattleDress(List<string> allowedUpgradeCategories)
      : base(allowedUpgradeCategories)
    {
    }

    public BattleDress() => this.AllowedUpgradeCategories.Add("Battle Dress Mod");

    public BattleDress(Armor copyMe)
      : base(copyMe)
    {
      if (this.AllowedUpgradeCategories == null)
        this.AllowedUpgradeCategories = new List<string>();
      this.AllowedUpgradeCategories.Add("Battle Dress Mod");
    }

    public override bool AddOption(IEquipmentOption addMe)
    {
      BattleDressMod battleDressMod = (BattleDressMod) null;
      if (addMe is BattleDressMod)
        battleDressMod = addMe as BattleDressMod;
      else if (addMe != null)
        battleDressMod = new BattleDressMod(addMe);
      int availableSlots = this.AvailableSlots;
      int? slots = battleDressMod?.Slots;
      int valueOrDefault = slots.GetValueOrDefault();
      return availableSlots >= valueOrDefault & slots.HasValue && base.AddOption(addMe);
    }
  }
}
