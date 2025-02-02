
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_Weapon




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_Weapon : Outcome
  {
    public MusteringOutBenefit_Weapon()
    {
      this.Name = "Weapon";
      this.Description = "Congratulations!  You muster out with a weapon.  If you already have a weapon, you'll get the oportunity to increase the associated skill.";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      if (string.IsNullOrEmpty(this.Description))
        this.Description = "Armaments acquired.";
      List<IWeapon> list1 = currentState.character.FindEquipment<IWeapon>((Func<IEquipment, bool>) (weapon => !(weapon is WeaponOption) && weapon.Cost < 1001 && weapon.TechLevel <= 12)).ToList<IWeapon>();
      List<string> source = new List<string>();
      foreach (IWeapon weapon in list1)
      {
        string str = (string) null;
        if (!string.IsNullOrEmpty(weapon.SubSkill))
          str = weapon.SubSkill;
        else if (!string.IsNullOrEmpty(weapon.Skill))
          str = weapon.Skill;
        if (str != null && !source.Contains(str))
          source.Add(str);
      }
      List<IWeapon> list2 = DataManager.Instance.GetAsset<IWeapon>((Func<IWeapon, bool>) (weapon => !(weapon is WeaponOption) && weapon.Cost < 1001 && weapon.TechLevel <= 12)).OrderBy<IWeapon, string>((Func<IWeapon, string>) (a => a.Name)).ToList<IWeapon>();
      List<Outcome> choices = new List<Outcome>();
      choices.AddRange(source.Select<string, Outcome.GainSkill>((Func<string, Outcome.GainSkill>) (skillName => new Outcome.GainSkill(skillName))).Cast<Outcome>());
      choices.AddRange(list2.Select<IWeapon, Outcome.AddEquipment>((Func<IWeapon, Outcome.AddEquipment>) (weapon => new Outcome.AddEquipment((IEquipment) weapon))).Cast<Outcome>());
      currentState.decisionMaker.ChooseOne<Outcome>((IList<Outcome>) choices).handleOutcome(currentState);
    }
  }
}
