
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_SmallCraft




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_SmallCraft : Outcome
  {
    public MusteringOutBenefit_SmallCraft()
    {
      this.Name = "Small Craft";
      this.Description = "You managed to obtain a small spaceworthy vehicle as part of your retirement.  If you've already got one, you'll be able to take a level in Pilot instead.";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      if (!currentState.character.FindEquipment<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>((Func<IEquipment, bool>) (sb => sb.Cost < 10000001 && sb.TechLevel <= 12)).Any<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>())
      {
        List<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship> list = DataManager.Instance.GetAsset<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>((Func<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship, bool>) (sb => sb.Cost <= 10000000 && sb.TechLevel <= 12 && sb.ShipType == com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship.ShipClass.SmallCraft)).OrderBy<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship, string>((Func<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship, string>) (a => a.Name)).ToList<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>();
        List<Outcome> choices = new List<Outcome>();
        foreach (IEquipment equipment in list)
          choices.Add((Outcome) new Outcome.AddEquipment(equipment));
        currentState.decisionMaker.ChooseOne<Outcome>("Which small craft would you like as a mustering out benefit?", (IList<Outcome>) choices).handleOutcome(currentState);
      }
      else
        new Event.SingleOutcome("Increase Pilot", "As you already have a small craft, you gain another level in Pilot as a mustering out benefit.", (Outcome) new Outcome.GainSkill("Pilot")).handleOutcome(currentState);
    }
  }
}
