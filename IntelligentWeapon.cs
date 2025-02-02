
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IntelligentWeapon




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class IntelligentWeapon : UpgradeComputer
  {
    public IntelligentWeapon(UpgradeComputer copyMe)
      : base(copyMe)
    {
    }

    [JsonConstructor]
    public IntelligentWeapon()
    {
    }

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      skillName = skillName.ToLowerInvariant();
      if (((IEnumerable<string>) new string[5]
      {
        "slug",
        "energy",
        "heavy",
        "gun",
        "port"
      }).Any<string>(new Func<string, bool>(skillName.Contains)))
        statName = "edu";
      int canonicalOrdinalForStat = com.digitalarcsystems.Traveller.DataModel.Attribute.GetCanonicalOrdinalForStat(statName);
      return this.GetBonusProvidedFromSubComponents(skillName, 0, -3, new com.digitalarcsystems.Traveller.DataModel.Attribute(statName, canonicalOrdinalForStat)) != 0;
    }
  }
}
