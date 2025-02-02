
// Type: com.digitalarcsystems.Traveller.DataModel.Events.UnusualHierateLifeEvent




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class UnusualHierateLifeEvent : Event
  {
    [JsonConstructor]
    public UnusualHierateLifeEvent()
    {
    }

    public UnusualHierateLifeEvent(string title, string description)
      : base(title, description)
    {
    }

    public virtual Outcome getUnusualHierateLifeEvent(GenerationState currentState)
    {
      List<Outcome> outcomeList = new List<Outcome>();
      outcomeList.Add((Outcome) new Event("Life Event: (Psionics)", "You have an encounter with a psionic phenomenon, such as a human Psionic institute, Zhodani agent, or a telepathic plant. "));
      outcomeList.Add((Outcome) new Event.SingleOutcome("Life Event: (Aliens)", "You travel extensively with non-Aslan.  Gain Tolerance.", (Outcome) new Outcome.GainSkill("Tolerance")));
      com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment equipment1 = new com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment("Alien Artifact");
      equipment1.Description = "A strange and unusual device from an alien culture that is not normally available to Aslan";
      equipment1.TechLevel = 12;
      equipment1.Cost = 2500;
      equipment1.Weight = 1.0;
      outcomeList.Add((Outcome) new Event.SingleOutcome("Life Event: (Alien artifact)", "You come into possession of a curious piece of alien technology or an archaeological relic.", (Outcome) equipment1));
      outcomeList.Add((Outcome) new Event("Life Event: (Amnesia)", "There is a gap in your memory."));
      outcomeList.Add((Outcome) new Event("Life Event: (Contact with Clan Leaders)", "The elders of your clan entrust you with a mission or a secret."));
      com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment equipment2 = new com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment("Ancient Technology");
      equipment2.Description = "Something older than the Aslan.";
      equipment2.TechLevel = 20;
      equipment2.Cost = 0;
      equipment2.Weight = 1.0;
      outcomeList.Add((Outcome) new Event.SingleOutcome("Life Event: (Ancient technology)", "You have an item that is older than the Aslan race.", (Outcome) equipment2));
      return outcomeList[Dice.D6Roll(1, 0, "Something unusual is happening to you.").rawResult - 1];
    }

    public override void handleOutcome(GenerationState currentState)
    {
      Outcome hierateLifeEvent = this.getUnusualHierateLifeEvent(currentState);
      currentState.decisionMaker.present(new Presentation((IDescribable) hierateLifeEvent));
      hierateLifeEvent.handleOutcome(currentState);
    }
  }
}
