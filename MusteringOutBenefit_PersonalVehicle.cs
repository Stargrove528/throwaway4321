
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_PersonalVehicle




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_PersonalVehicle : Outcome
  {
    public MusteringOutBenefit_PersonalVehicle()
    {
      this.Name = "Personal Vehicle";
      this.Description = "During your career, you managed to obtain a personal conveyance.  You're going to be able to keep it as a retirement benefit!  If you already have a personal vehicle, you'll be able to gain a level in either Drive or Flyer instead.";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      List<Outcome> choices = new List<Outcome>();
      Vehicle vehicle1 = currentState.character.FindEquipment<Vehicle>().ToList<Vehicle>().FirstOrDefault<Vehicle>();
      if (vehicle1 != null)
      {
        choices.Add((Outcome) new Event.ChoiceOutcome(1, "Drive/Flyer", "Since you already have a personal vehicle (" + vehicle1.Name + "), you can take 1 additional level in either Drive or Flyer", new Outcome[2]
        {
          (Outcome) new Outcome.GainSkill("Drive"),
          (Outcome) new Outcome.GainSkill("Flyer")
        }));
      }
      else
      {
        List<Vehicle> list = DataManager.Instance.GetAsset<Vehicle>((Func<Vehicle, bool>) (pv => pv.Cost < 300001 && pv.TechLevel <= 10)).ToList<Vehicle>();
        choices.AddRange(list.Select<Vehicle, Outcome.AddEquipment>((Func<Vehicle, Outcome.AddEquipment>) (vehicle => new Outcome.AddEquipment((IEquipment) vehicle))).Cast<Outcome>());
      }
      currentState.decisionMaker.ChooseOne<Outcome>((IList<Outcome>) choices).handleOutcome(currentState);
    }
  }
}
