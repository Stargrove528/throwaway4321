
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_ScientificEquipment




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_ScientificEquipment : Outcome
  {
    public MusteringOutBenefit_ScientificEquipment()
    {
      this.Name = "Scientific Equipment";
      this.Description = "Gain a piece of scientific equipment.  If you already have some, you'll have the option to gain a level in either Electronics, or Science";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      List<Outcome> choices = new List<Outcome>();
      bool flag;
      try
      {
        flag = currentState.character.FindEquipment<IEquipment>((Func<IEquipment, bool>) (sb => (sb is Computer || sb is com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Sensors || sb.Name != null && sb.Name.IndexOf("Toolkit", StringComparison.InvariantCultureIgnoreCase) >= 0) && sb.Cost < 1001 && sb.TechLevel <= 12)).Any<IEquipment>();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Caught an exception");
        throw ex;
      }
      List<IEquipment> list = DataManager.Instance.GetAsset<IEquipment>((Func<IEquipment, bool>) (sb => (sb is Computer || sb is com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Sensors || sb.Name.IndexOf("Toolkit", StringComparison.InvariantCultureIgnoreCase) >= 0) && sb.Cost < 1001 && sb.TechLevel <= 12)).ToList<IEquipment>();
      if (flag)
      {
        choices.Add((Outcome) new Outcome.GainSkill("Electronics"));
        choices.Add((Outcome) new Outcome.GainSkill("Science"));
      }
      foreach (IEquipment equipment in list)
        choices.Add((Outcome) new Outcome.AddEquipment(equipment));
      currentState.decisionMaker.ChooseOne<Outcome>("Please choose one of the folowing as your mustering out benefit.", (IList<Outcome>) choices).handleOutcome(currentState);
    }
  }
}
