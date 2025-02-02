
// Type: com.digitalarcsystems.Traveller.DataModel.Events.ModifyStatByEffect




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class ModifyStatByEffect : Event.StatChallenge
  {
    public Event.Challenge challenge;
    public string statToModify;

    public ModifyStatByEffect()
    {
    }

    public ModifyStatByEffect(string name, string description)
    {
      this.Name = name;
      this.Description = description;
    }

    public ModifyStatByEffect(
      string name,
      string description,
      string stat,
      Event.Challenge challenge)
    {
      this.Name = name;
      this.Description = description;
      this.statName = stat;
      this.statToModify = stat;
      this.challenge = challenge;
    }

    public ModifyStatByEffect(string name, string description, string stat, int target)
    {
      this.Name = name;
      this.Description = description;
      this.statName = stat;
      this.statToModify = stat;
      this.targetNumber = target;
    }

    public ModifyStatByEffect(
      string name,
      string description,
      string skill,
      string stat,
      int target)
    {
      this.Name = name;
      this.Description = description;
      this.statName = stat;
      this.skillName = skill;
      this.statToModify = stat;
      this.targetNumber = target;
    }

    public ModifyStatByEffect(
      string name,
      string description,
      string skill,
      string stat,
      int target,
      string statToModify)
    {
      this.Name = name;
      this.Description = description;
      this.statName = stat;
      this.skillName = skill;
      this.statToModify = statToModify;
      this.targetNumber = target;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      RollParam settings = this.challenge == null ? Dice.StatRoll(currentState.character.getAttribute(this.statName), this.targetNumber, this.Description) : (RollParam) null;
      settings.AddResultDescriptions(this.Description + " [SUCCEEDED]", this.Description + " [FAILED]");
      this.challengeResult = Dice.Roll(settings);
      if (this.challengeResult.effect != 0)
        new Outcome.StatModified(this.statToModify, this.challengeResult.effect).handleOutcome(currentState);
      if (this.challengeResult.isSuccessful)
      {
        this.Description += " [SUCCEEDED]";
        currentState.recorder.RecordBenefit((Event) this, currentState);
        foreach (Outcome successOutcome in this.successOutcomes)
          successOutcome.handleOutcome(currentState);
      }
      else
      {
        this.Description += " [FAILED]";
        foreach (Outcome failureOutcome in this.failureOutcomes)
          failureOutcome.handleOutcome(currentState);
      }
    }
  }
}
