
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_Blade




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_Blade : Outcome
  {
    public MusteringOutBenefit_Blade()
    {
      this.Name = "Blade";
      this.Description = "Congratulations!  You muster out with a blade of some kind.  If you already have a blade, you'll get the option to increase the associated skill instead.";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      List<Outcome> choices = new List<Outcome>();
      if (currentState.character.FindEquipment<IWeapon>((Func<IEquipment, bool>) (s => s.SubSkill == "Blade")).Any<IWeapon>())
        choices.Add((Outcome) new Outcome.GainSkill("Blade"));
      foreach (IEquipment equipment in DataManager.Instance.GetAsset<IWeapon>((Func<IWeapon, bool>) (x => x.SubSkill == "Blade")).OrderBy<IWeapon, string>((Func<IWeapon, string>) (a => a.Name)).ToList<IWeapon>())
        choices.Add((Outcome) new Outcome.AddEquipment(equipment));
      currentState.decisionMaker.ChooseOne<Outcome>("Please choose a Blade Mustering Out Benefit", (IList<Outcome>) choices).handleOutcome(currentState);
    }
  }
}
