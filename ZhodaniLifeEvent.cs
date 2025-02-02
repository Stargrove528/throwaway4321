
// Type: com.digitalarcsystems.Traveller.Zhodani.ZhodaniLifeEvent




using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.Zhodani
{
  public class ZhodaniLifeEvent : Event
  {
    [JsonConstructor]
    public ZhodaniLifeEvent()
    {
    }

    public ZhodaniLifeEvent(string withName, string withDescription)
      : base(withName, withDescription)
    {
    }

    public ZhodaniLifeEvent(Event.LifeEvent copyMe)
      : base((Event) copyMe)
    {
    }

    public virtual Outcome getLifeEvent(GenerationState currentState)
    {
      List<Outcome> choices = new List<Outcome>();
      choices.Add((Outcome) new Event.Injury(false));
      choices.Add((Outcome) new Event("Life Event: Birth or Death", "Someone close to you dies, such as a friend or family member. Alternatively, someone close to you gives birth (or is born!). You are involved in some fashion (father or mother, relative, godparent, or similar)."));
      choices.Add((Outcome) new Event.ChoiceOutcome(1, "Life Event: Ending of Relationship", "A romantic relationship involving you ends. Badly. Gain a Rival or Enemy.", new Outcome[2]
      {
        (Outcome) new Outcome.Rivals(1),
        (Outcome) new Outcome.Enemies(1)
      }));
      List<Outcome> outcomeList1 = choices;
      Outcome.Allies allies1 = new Outcome.Allies(1);
      allies1.Name = "Life Event: Improved Relationship";
      allies1.Description = "A romantic relationship involving you deepens, possibly leading to marriage or some other emotional commitment. Gain an Ally.";
      outcomeList1.Add((Outcome) allies1);
      List<Outcome> outcomeList2 = choices;
      Outcome.Allies allies2 = new Outcome.Allies(1);
      allies2.Name = "Life Event: New Relationship";
      allies2.Description = "You become involved in a romantic relationship. Gain an Ally.";
      outcomeList2.Add((Outcome) allies2);
      choices.Add((Outcome) new Event.SingleOutcome("Life Event: New Contact", "You gain a new Contact", (Outcome) new Outcome.Contacts(1)));
      choices.Add((Outcome) new Event.Betrayal("Life Event: Betrayal", "You are betrayed in some fashion by a friend. If you have any Contacts or Allies, convert one into a Rival or Enemy. Otherwise, gain a Rival or an Enemy."));
      choices.Add((Outcome) new Event.SingleOutcome("Life Event: Travel", "You move to another world. You gain a +2 DM to your next Qualification roll.", (Outcome) new Outcome.QualificationModifier(2)));
      choices.Add((Outcome) new Event.SingleOutcome("Life Event: Good Fortune", "Something good happens to you; you come into money unexpectedly, have a lifelong dream come true, get a book published or have some other stroke of good fortune. Gain a +2 DM to any one Benefit roll.", (Outcome) new Outcome.MusteringOutBenefitRollModifier(2)));
      List<Outcome> outcomeList3 = choices;
      Outcome[] outcomeArray = new Outcome[2]
      {
        (Outcome) new Outcome.MusteringOutBenefitModifier(-1),
        null
      };
      Outcome.NextCareerMustBe nextCareerMustBe = new Outcome.NextCareerMustBe("Prison");
      nextCareerMustBe.Description = "Imprisoned for a crime you may or may not have committed, you'll report to prison at the beginning of your next term.";
      outcomeArray[1] = (Outcome) nextCareerMustBe;
      Event.ChoiceOutcome choiceOutcome = new Event.ChoiceOutcome(1, "Life Event: Crime", "You commit a crime, are accused of a crime, or are the victim of a crime. Lose one Benefit roll or take the Prison Career in your next term", outcomeArray);
      outcomeList3.Add((Outcome) choiceOutcome);
      choices.Add((Outcome) new ZhodaniUnusualLifeEvent("Unusual Life Event!", "Something VERY unusual has happened to you. Roll again"));
      return Dice.RollRandomResult<Outcome>("Life happens... Roll for Life Event", (IList<Outcome>) choices, ContextKeys.OUTCOMES);
    }

    public override void handleOutcome(GenerationState currentState)
    {
      this.getLifeEvent(currentState).handleOutcome(currentState);
    }
  }
}
