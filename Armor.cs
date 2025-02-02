
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Armor




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Armor : UpgradableEquipment
  {
    [JsonConstructor]
    public Armor(List<string> _AllowedUpgradeCategories)
    {
      this.AllowedUpgradeCategories = _AllowedUpgradeCategories;
    }

    public Armor()
    {
      this.AllowedUpgradeCategories = new List<string>()
      {
        "Armor Option",
        "Armor Upgrade",
        "Armor Modification"
      };
    }

    public Armor(IEquipment copyMe)
      : base(copyMe)
    {
    }

    public Armor(Armor basedOnMe)
      : base((IUpgradable) basedOnMe)
    {
      this.ProtectionLaser = basedOnMe.ProtectionLaser;
      this.ProtectionEnergy = basedOnMe.ProtectionEnergy;
      this.ProtectionPsionic = basedOnMe.ProtectionPsionic;
      this.ProtectionKinetic = basedOnMe.ProtectionKinetic;
      this.ProtectionRadiation = basedOnMe.ProtectionRadiation;
      this.Strata = basedOnMe.Strata;
    }

    [JsonIgnore]
    public int Protection
    {
      get => this.ProtectionKinetic;
      set => this.ProtectionKinetic = value;
    }

    public int protectionByDamageType(DamageType damage)
    {
      switch (damage)
      {
        case DamageType.PSIONIC:
          return this.ProtectionPsionic;
        case DamageType.KINETIC:
          return this.ProtectionKinetic;
        case DamageType.ENERGY:
          return this.ProtectionEnergy;
        case DamageType.LASER:
          return this.ProtectionLaser;
        default:
          return this.ProtectionKinetic;
      }
    }

    [JsonProperty]
    public int ProtectionKinetic { get; set; }

    [JsonProperty]
    public int ProtectionEnergy { get; set; }

    [JsonProperty]
    public int ProtectionLaser { get; set; }

    [JsonProperty]
    public int ProtectionPsionic { get; set; }

    [JsonProperty]
    public int ProtectionRadiation { get; set; }

    [JsonProperty]
    public int Strata { get; set; }
  }
}
