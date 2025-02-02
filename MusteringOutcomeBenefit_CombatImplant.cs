
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutcomeBenefit_CombatImplant




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using com.digitalarcsystems.Traveller.utility;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutcomeBenefit_CombatImplant : Outcome
  {
    public MusteringOutcomeBenefit_CombatImplant()
    {
      this.Name = "Combat Implant";
      this.Description = "Gain a personal augmentation of some kind.  If you get this more than once, you can either take an additional augment, or upgrade an existing one.";
    }

    public static IEnumerable<IAugmentation> CombatImplants
    {
      get
      {
        return (IEnumerable<IAugmentation>) DataManager.Instance.GetAsset<IAugmentation>((Func<IAugmentation, bool>) (e => e.Cost < 50001 && e.TechLevel <= 12)).OrderBy<IAugmentation, string>((Func<IAugmentation, string>) (a => a.Name)).ToList<IAugmentation>();
      }
    }

    public override void handleOutcome(GenerationState currentState)
    {
      List<Outcome> choices = new List<Outcome>();
      List<IAugmentation> ownedAugment = currentState.character.FindEquipment<IAugmentation>().OrderBy<IAugmentation, string>((Func<IAugmentation, string>) (a => a.Name)).ToList<IAugmentation>();
      foreach (IAugmentation augmentation in ownedAugment)
      {
        if (Utility.upgradeAvailable((IEquipment) augmentation))
        {
          IEquipment addMe = Utility.nextUpgrade((IEquipment) augmentation, "Augmentation");
          choices.Add((Outcome) new Event.SwapEquipment(augmentation.Name + " Upgrade", "As a benefit you can upgrade " + augmentation.Name + " to " + addMe.Name, (IEquipment) augmentation, addMe));
        }
      }
      foreach (IAugmentation amIAlreadyUpgraded in MusteringOutcomeBenefit_CombatImplant.CombatImplants.Where<IAugmentation>((Func<IAugmentation, bool>) (e => !ownedAugment.Contains(e))))
      {
        if (!Utility.upgradePosessed<IAugmentation>((IEquipment) amIAlreadyUpgraded, ownedAugment))
          choices.Add((Outcome) new Outcome.AddEquipment((IEquipment) amIAlreadyUpgraded));
      }
      if (choices.Count != 0)
      {
        Outcome outcome = currentState.decisionMaker.ChooseOne<Outcome>((IList<Outcome>) choices);
        if (outcome.Name.ToLowerInvariant().Contains("skill"))
        {
          Outcome.IncreaseAnyExistingSkill anyExistingSkill = new Outcome.IncreaseAnyExistingSkill();
          anyExistingSkill.Name = "Cybernetically Augement Skill";
          anyExistingSkill.Description = "Select a skill you wish to cybernetically enhance.";
          anyExistingSkill.handleOutcome(currentState);
        }
        outcome.handleOutcome(currentState);
      }
      else
      {
        currentState.decisionMaker.present(new Presentation("No More Available Augmentations", "You have all of the available augments that you can have.  Receive 1D X Cr 10,000 instead."));
        Outcome.RandomAmount randomAmount = new Outcome.RandomAmount(1, 6, (Outcome) new Outcome.GainMoney(1000));
        randomAmount.Name = "Money In leu of Augment";
        randomAmount.handleOutcome(currentState);
      }
    }
  }
}
