
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_Gun




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_Gun : Outcome
  {
    public MusteringOutBenefit_Gun()
    {
      this.Name = "Gun";
      this.Description = "Congratulations!  You muster out with a ranged weapon.  If you already have a ranged weapon you'll get the oportunity to increase the associated skill.";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      List<Outcome> source = new List<Outcome>();
      List<IMultishotRangedWeapon> gunsIHave = currentState.character.FindEquipment<IMultishotRangedWeapon>((Func<IEquipment, bool>) (gun => !(gun is WeaponOption) && gun.Skill == "Gun Combat" && gun.Cost < 1001 && gun.TechLevel <= 12)).ToList<IMultishotRangedWeapon>();
      List<string> stringList = new List<string>();
      foreach (IMultishotRangedWeapon multishotRangedWeapon in gunsIHave)
      {
        string str = (string) null;
        if (!string.IsNullOrEmpty(multishotRangedWeapon.SubSkill))
          str = multishotRangedWeapon.SubSkill;
        else if (!string.IsNullOrEmpty(multishotRangedWeapon.Skill))
          str = multishotRangedWeapon.Skill;
        if (str != null && !stringList.Contains(str))
          stringList.Add(str);
      }
      foreach (string skill in stringList)
        source.Add((Outcome) new Outcome.GainSkill(skill));
      List<IMultishotRangedWeapon> list1 = DataManager.Instance.GetAsset<IMultishotRangedWeapon>((Func<IMultishotRangedWeapon, bool>) (e => (e.SubSkill.Equals("Slug", StringComparison.CurrentCultureIgnoreCase) || e.SubSkill.Equals("Energy", StringComparison.CurrentCultureIgnoreCase)) && e.Cost < 1001 && !gunsIHave.Contains(e))).OrderBy<IMultishotRangedWeapon, string>((Func<IMultishotRangedWeapon, string>) (eq => eq.Name)).ToList<IMultishotRangedWeapon>();
      List<Outcome> list2 = source.OrderBy<Outcome, string>((Func<Outcome, string>) (eq => eq.Name)).ToList<Outcome>();
      foreach (IEquipment equipment in list1)
        list2.Add((Outcome) new Outcome.AddEquipment(equipment));
      currentState.decisionMaker.ChooseOne<Outcome>((IList<Outcome>) list2).handleOutcome(currentState);
    }
  }
}
