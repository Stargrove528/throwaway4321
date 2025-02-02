
// Type: com.digitalarcsystems.Traveller.DataModel.CareerChallenge




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public abstract class CareerChallenge : IDescribable
  {
    [JsonConstructor]
    public CareerChallenge()
    {
      this.stat_name = string.Empty;
      this.Description = string.Empty;
    }

    public abstract RollParam qualificationRoll(Character forCharacter, int withModifier);

    public abstract int percentChanceOfSuccess(Character character, int totalModifier);

    public virtual RollEffect qualify(Character character, int additionalModifier)
    {
      RollParam setup = this.qualificationRoll(character, additionalModifier);
      if (setup != null)
        return Dice.Roll(setup, "Congratulations you made it!", "You were rejected");
      EngineLog.Error("CareerChallenge.qualify() got null as qualificattion roll parameter. ChallengeType: " + this.GetType()?.ToString());
      return (RollEffect) null;
    }

    public virtual string Name
    {
      get => "Qualification";
      set
      {
      }
    }

    public virtual string Description { get; set; }

    public virtual string stat_name { get; set; }

    public virtual int success_on { get; set; }
  }
}
