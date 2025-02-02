
// Type: com.digitalarcsystems.Traveller.DataModel.HireALawyer




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class HireALawyer : Event
  {
    public int SkillLevel { get; set; }

    public int ParoleReducedBy { get; set; }

    public int Target { get; set; }

    public int Cost { get; set; }

    public bool is_dice { get; set; }

    public List<Outcome> SuccessOutcomes { get; set; }

    public List<Outcome> FailureOutcomes { get; set; }

    public HireALawyer()
    {
    }

    public HireALawyer(
      string title,
      string description,
      int cost,
      int target,
      int parole_reduced_by,
      bool is_dice = false)
      : base(title, description)
    {
      this.Cost = cost;
      this.Target = target;
      this.ParoleReducedBy = parole_reduced_by;
      this.SkillLevel = 1;
      this.is_dice = is_dice;
    }

    public HireALawyer(
      string title,
      string description,
      int cost,
      int target,
      List<Outcome> successOutcomes,
      List<Outcome> failureOutcomes)
      : base(title, description)
    {
      this.Cost = cost;
      this.Target = target;
      this.SuccessOutcomes = successOutcomes;
      this.FailureOutcomes = failureOutcomes;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      CaptiveCareer captiveCareer = (CaptiveCareer) null;
      if (currentState.currentCareer is CaptiveCareer)
        captiveCareer = (CaptiveCareer) currentState.currentCareer;
      IDescribable[] choices = new IDescribable[5]
      {
        (IDescribable) new Event("New Lawyer [1]", "An inexperienced lawyer fresh out of law school.  This lawyer will at least give you a chance, and the price is more \"affordable\". [" + this.Cost.ToString() + "cr.]"),
        (IDescribable) new Event("Staff Lawyer [2]", "A competent professional.  This lawyer has enough experience to be proud of their work.  They charge the standard rate of [" + (this.Cost * 2 * 2).ToString() + "cr.]"),
        (IDescribable) new Event("Seasoned Lawyer [3]", "A true professional.  This lawyer is seasoned and has several complicated but successes in their history.  They are more expensive, but they have proven results.  [" + (this.Cost * 3 * 3).ToString() + "cr.]"),
        (IDescribable) new Event("Senior Lawyer [4]", "Well known in this subsector, this powerful lawyer has performed legal manueverings that have triggered the rewrite of some laws.  They charge [" + (this.Cost * 4 * 4).ToString() + "cr.]"),
        (IDescribable) new Event("Celebrity Lawyer [5]", "Known throughout the sector, you've attracted the eye of a household name.  The clout of this lawyer alone might get your released.  As with their skills, the skies the limit in price [" + (this.Cost * 5 * 5).ToString() + "cr.]")
      };
      IDescribable describable = currentState.decisionMaker.ChooseOne<IDescribable>((IList<IDescribable>) choices);
      for (int index = 0; index < choices.Length; ++index)
      {
        if (describable == choices[index])
        {
          this.SkillLevel = index + 1;
          this.Cost = this.Cost * this.SkillLevel * this.SkillLevel;
          break;
        }
      }
      ISkill skill = currentState.skillSource.getSkill("Advocate");
      skill.Level = this.SkillLevel;
      Attribute stat = new Attribute("Edu", 4);
      stat.InitializeValue(7);
      string str = captiveCareer != null ? " attempts to reduce your " + captiveCareer.release_threshold_label : " attempts to resolve your legal problem.";
      RollEffect rollEffect = Dice.Roll(new RollParam(skill, stat, this.Target, skill.Level + stat.Modifier, describable.Name + str));
      new Outcome.GainMoney(-1 * this.Cost).handleOutcome(currentState);
      if (rollEffect.isSuccessful)
      {
        if (captiveCareer != null)
        {
          int num1;
          if (!this.is_dice)
            num1 = this.ParoleReducedBy;
          else
            num1 = Dice.D6Roll(this.ParoleReducedBy, 0, "Determine the amount your " + captiveCareer.release_threshold_label + ", currently at " + captiveCareer.ReleaseThreshold.ToString() + ", is reduced by.").totalResult;
          int num2 = num1;
          new ParoleModifier(describable.Name + " reduces " + captiveCareer.release_threshold_label, "You hired a " + describable.Name + " for " + this.Cost.ToString() + " and they reduced your " + captiveCareer.release_threshold_label + " by " + num2.ToString(), -1 * num2).handleOutcome(currentState);
        }
        if (this.SuccessOutcomes == null || this.SuccessOutcomes.Count <= 0)
          return;
        foreach (Outcome successOutcome in this.SuccessOutcomes)
          successOutcome.handleOutcome(currentState);
      }
      else if (this.FailureOutcomes != null && this.FailureOutcomes.Count > 0)
      {
        foreach (Outcome failureOutcome in this.FailureOutcomes)
          failureOutcome.handleOutcome(currentState);
      }
      else
        new Event("Waste of Money", "The lawyer was a waste of time an money.").handleOutcome(currentState);
    }
  }
}
