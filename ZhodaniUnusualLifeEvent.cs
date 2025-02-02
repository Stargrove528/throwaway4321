
// Type: com.digitalarcsystems.Traveller.Zhodani.ZhodaniUnusualLifeEvent




using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.Zhodani
{
  public class ZhodaniUnusualLifeEvent : Event
  {
    [JsonConstructor]
    public ZhodaniUnusualLifeEvent()
    {
    }

    public ZhodaniUnusualLifeEvent(string title, string description)
      : base(title, description)
    {
    }

    public virtual Outcome getUnusualLifeEvent(GenerationState currentState)
    {
      return Dice.RollRandomResult<Outcome>("Something unusual is happening to you. Roll to determine what.", (IList<Outcome>) new List<Outcome>()
      {
        (Outcome) new Event.OptionalEvent("Life Event: Psionics", "You encounter a Psionic institute. You may immediately test your Psionic Strength and, if you qualify, take the Psion career in your next term.", (Outcome) new Outcome.StatModified("Psi", 2)),
        (Outcome) new Event.MultipleOutcomes("Life Event: Aliens", "You spend time among an alien race. Gain Science 1 and a Contact among an alien race.", new Outcome[2]
        {
          (Outcome) new Outcome.GainSkill("Science"),
          (Outcome) new Outcome.GainSpecifiedContact("Alien Race Contact: " + currentState.peopleSource.EntityToMeet?.ToString())
        }),
        (Outcome) new Event.SingleOutcome("Life Event: Alien Artifact", "You have a strange and unusual device from an alien culture that is not normally available to humans.", (Outcome) new com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment("Alien Artifact", "A strange and unusual device from an alien culture that is not normally available to humans", 12, 0, 1)),
        (Outcome) new Event("Life Event: Amnesia", "Something happened to you, but you don't know what it was."),
        (Outcome) new Event("Life Event: Contact with government", "You briefly come into contact with the highest echelons of the Imperium, an Archduke or the Emperor, perhaps, or Imperial intelligence."),
        (Outcome) new Event.SingleOutcome("Life Event: Ancient technology", "You have something older than the Imperium, or even something older than humanity.", (Outcome) new com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment("Ancient Technology", "Something older than the Imperium, or even something older than humanity.", 20, 0, 1))
      }, ContextKeys.OUTCOMES);
    }

    public override void handleOutcome(GenerationState currentState)
    {
      this.getUnusualLifeEvent(currentState).handleOutcome(currentState);
    }
  }
}
