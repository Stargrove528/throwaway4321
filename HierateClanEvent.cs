
// Type: com.digitalarcsystems.Traveller.DataModel.Events.HierateClanEvent




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class HierateClanEvent : Event
  {
    [JsonIgnore]
    private static readonly List<Outcome> HierateClanEvents;

    [JsonConstructor]
    public HierateClanEvent()
    {
    }

    public HierateClanEvent(string name, string description)
      : base(name, description)
    {
    }

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.decisionMaker.present(new Presentation(this.Name, this._description));
      RollEffect rollEffect = Dice.D6Roll(1, 0, "Clan Event Roll");
      Outcome hierateClanEvent = HierateClanEvent.HierateClanEvents[rollEffect.totalResult - 1];
      this.Name = hierateClanEvent.Name;
      this.Description = hierateClanEvent.Description;
      currentState.decisionMaker.present(new Presentation(this.Name, this.Description));
      hierateClanEvent.handleOutcome(currentState);
    }

    static HierateClanEvent()
    {
      List<Outcome> outcomeList1 = new List<Outcome>();
      outcomeList1.Add((Outcome) new Event.ChoiceOutcome(1, "CLAN EVENT: Prosperous Times", "The clan acquires new Territories or trade routes.  Gain an extra Benefit roll, or DM +2 to your next advancement roll. ", new Outcome[2]
      {
        (Outcome) new Outcome.MusteringOutBenefitModifier(1),
        (Outcome) new Outcome.AdvancementModifier(2)
      }));
      outcomeList1.Add((Outcome) new Event.SingleOutcome("CLAN EVENT: Rising Fortunes", "Your clan's political standing improves.  Gain SOC +1.", (Outcome) new Outcome.StatModified("SOC", 1)));
      outcomeList1.Add((Outcome) new Event.SingleOutcome("CLAN EVENT: New Ally", "A member of your clan rises to an influential position.  Gain him or her as an Ally.", (Outcome) new Outcome.GainSpecifiedAlly("Fellow Clan Member")));
      outcomeList1.Add((Outcome) new Event.SingleOutcome("CLAN EVENT: Feud", "Your family is now feuding with another Aslan Family.  Gain the enemy family as an Enemy.", (Outcome) new Outcome.GainSpecifiedEnemy("Enemy Family Members")));
      List<Outcome> outcomeList2 = outcomeList1;
      GenderBasedOutcome genderBasedOutcome1 = new GenderBasedOutcome("CLAN EVENT: WAR!", "Your clan, or the clan in which you find yourself go to War.  If you are male, you suffer a DM -2 to survival rolls next term.  If you are female, you lose one benefit roll.");
      Outcome.SurvivalModifier survivalModifier = new Outcome.SurvivalModifier();
      survivalModifier.modifier = -2;
      survivalModifier.Name = "War";
      survivalModifier.Description = "Your clan goes to war.  You suffer a DM -2 to survival rolls next term.";
      GenderBasedOutcome genderBasedOutcome2 = genderBasedOutcome1.addGenderOutcome("male", (Outcome) survivalModifier).addGenderOutcome("female", (Outcome) new Outcome.MusteringOutBenefitModifier("War", "Your clan goes to war.  You lose one Benefit roll.", -1));
      outcomeList2.Add((Outcome) genderBasedOutcome2);
      outcomeList1.Add((Outcome) new Event.MultipleOutcomes("CLAN EVENT: Hard Times", "Your clan suffers economic hardship.  You suffer DM-4 to advancement rolls this term, and gain no Benefit rolls for it.", new Outcome[2]
      {
        (Outcome) new Outcome.AdvancementModifier(-4),
        (Outcome) new Outcome.MusteringOutBenefitModifier(-1)
      }));
      HierateClanEvent.HierateClanEvents = outcomeList1;
    }
  }
}
