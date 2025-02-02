
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Consumer




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Consumer : 
    com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment,
    IConsumer,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    [JsonProperty]
    public bool UnlimitedAmmunition { get; set; }

    [JsonProperty]
    public bool ConsumableIsRechargable { get; set; }

    [JsonProperty]
    public int Capacity { get; set; }

    [JsonProperty]
    public int StandardConsumableCost { get; set; }

    [JsonProperty]
    public string ConsumableAmountName { get; set; }

    [JsonProperty]
    public IConsumable ConsumableLoaded { get; private set; }

    [JsonProperty]
    public string ConsumableTypeAllowed { get; set; }

    public bool Load(IConsumable loadMe)
    {
      string[] allowed = Array.ConvertAll<string, string>(this.ConsumableTypeAllowed.Split(','), (Converter<string, string>) (p => p.Trim()));
      if (!((IEnumerable<string>) Array.ConvertAll<string, string>(loadMe.ConsumableType.Split(','), (Converter<string, string>) (p => p.Trim()))).Any<string>((Func<string, bool>) (ammo_type => ((IEnumerable<string>) allowed).Contains<string>(ammo_type))))
        return false;
      this.ConsumableLoaded = loadMe;
      return true;
    }

    public bool Unload()
    {
      this.ConsumableLoaded = (IConsumable) null;
      return true;
    }

    public bool Use()
    {
      bool flag = false;
      if (this.ConsumableLoaded != null && this.ConsumableLoaded.CurrentAmount > 0)
      {
        if (!this.UnlimitedAmmunition)
          --this.ConsumableLoaded.CurrentAmount;
        flag = true;
      }
      return flag;
    }

    public bool Use(int amount)
    {
      bool flag = false;
      if (!this.UnlimitedAmmunition)
      {
        if (this.ConsumableLoaded != null && this.ConsumableLoaded.CurrentAmount >= amount)
        {
          this.ConsumableLoaded.CurrentAmount -= amount;
          flag = true;
        }
      }
      else
        flag = true;
      return flag;
    }
  }
}
