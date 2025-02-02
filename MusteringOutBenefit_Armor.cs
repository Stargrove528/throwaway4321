
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_Armor




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_Armor : Outcome
  {
    public MusteringOutBenefit_Armor()
    {
      this.Name = "Armor";
      this.Description = "Over the course of your career you managed to obtain Armor.  If you already have a suite, you'll have the opportunity to upgrade.";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      List<Armor> list1 = currentState.character.FindEquipment<Armor>((Func<IEquipment, bool>) (x => !(x is ArmorOption))).OrderBy<Armor, string>((Func<Armor, string>) (a => a.Name)).ToList<Armor>();
      List<Armor> list2 = DataManager.Instance.GetAsset<Armor>((Func<Armor, bool>) (e => e.Cost > 10000 && e.Cost < 200001 && e.TechLevel <= 12)).OrderBy<Armor, string>((Func<Armor, string>) (a => a.Name)).ToList<Armor>();
      List<Armor> list3 = DataManager.Instance.GetAsset<Armor>((Func<Armor, bool>) (e => e.Cost < 10001 && e.TechLevel <= 12)).OrderBy<Armor, string>((Func<Armor, string>) (a => a.Name)).ToList<Armor>();
      List<Outcome> choices = new List<Outcome>();
      if (list1 != null && list1.Count > 0)
      {
        foreach (IEquipment replaceMe in list1.Where<Armor>((Func<Armor, bool>) (x => x.Cost < 1000)))
        {
          foreach (IEquipment addMe in list2)
            choices.Add((Outcome) new Event.SwapEquipment("Upgrade To " + addMe.Name, " Would you like to trade in your " + replaceMe.Name + " for " + addMe.Name, replaceMe, addMe));
        }
      }
      foreach (Armor armor in list3)
      {
        List<Outcome> outcomeList = choices;
        Outcome.AddEquipment addEquipment = new Outcome.AddEquipment((IEquipment) armor);
        addEquipment.Name = "Gain " + armor.Name;
        addEquipment.Description = "Add " + armor.Name + "to your equipment.  Description: " + armor.Description;
        outcomeList.Add((Outcome) addEquipment);
      }
      currentState.decisionMaker.ChooseOne<Outcome>((IList<Outcome>) choices).handleOutcome(currentState);
    }
  }
}
