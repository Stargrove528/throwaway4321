
// Type: com.digitalarcsystems.Traveller.DataModel.Events.ZhodaniReeducationEvent




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class ZhodaniReeducationEvent : Event
  {
    [JsonIgnore]
    private static readonly List<Outcome> zhodaniReeducationEvents;

    [JsonConstructor]
    public ZhodaniReeducationEvent()
    {
    }

    public ZhodaniReeducationEvent(string name, string description, bool stayInCareer = false)
      : base(name, description)
    {
    }

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.decisionMaker.present(new Presentation(this.Name, this._description));
      RollEffect rollEffect = Dice.D6Roll(1, 0, "Reeducation Event Roll");
      Term term = new Term()
      {
        careerName = "Zhodani Reeducation"
      };
      if (rollEffect.effect == 1)
      {
        currentState.character.CareerHistory.Add(term);
        currentState.character.CareerHistory.Add(term);
      }
      if (rollEffect.effect == 2 || rollEffect.effect == 3)
        currentState.character.CareerHistory.Add(term);
      Outcome reeducationEvent = ZhodaniReeducationEvent.zhodaniReeducationEvents[rollEffect.totalResult - 1];
      this.Name = reeducationEvent.Name;
      this.Description = reeducationEvent.Description;
      currentState.decisionMaker.present(new Presentation(this.Name, this.Description));
      reeducationEvent.handleOutcome(currentState);
    }

    static ZhodaniReeducationEvent()
    {
      List<Outcome> outcomeList1 = new List<Outcome>();
      List<Outcome> outcomeList2 = outcomeList1;
      Outcome[] outcomeArray1 = new Outcome[4]
      {
        (Outcome) new Outcome.StatModified("End", -1),
        null,
        null,
        null
      };
      IfStatOverTarget ifStatOverTarget1 = new IfStatOverTarget("Soc", 11, new Outcome[1]
      {
        (Outcome) new Outcome.StatModified("Soc", -1)
      }, (Outcome[]) null);
      ifStatOverTarget1.Name = "Reduction in Soc";
      ifStatOverTarget1.Description = "If your Soc is higher than 11, it is reduced by 1";
      outcomeArray1[1] = (Outcome) ifStatOverTarget1;
      outcomeArray1[2] = (Outcome) new Event.EndOfCareer();
      outcomeArray1[3] = (Outcome) new AddYearsToCharacter(8);
      Event.MultipleOutcomes multipleOutcomes1 = new Event.MultipleOutcomes("REEDUCATION EVENT: 2 Terms & New Career", "Re-education requires two terms. You must enter a new career and reduce END by –1. If SOC 11+, reduce SOC by –1, to a minimum of 11 ", outcomeArray1);
      outcomeList2.Add((Outcome) multipleOutcomes1);
      List<Outcome> outcomeList3 = outcomeList1;
      Outcome[] outcomeArray2 = new Outcome[4]
      {
        (Outcome) new Outcome.StatModified("End", -1),
        null,
        null,
        null
      };
      IfStatOverTarget ifStatOverTarget2 = new IfStatOverTarget("Soc", 11, new Outcome[1]
      {
        (Outcome) new Outcome.StatModified("Soc", -1)
      }, (Outcome[]) null);
      ifStatOverTarget2.Name = "Reduction in Soc";
      ifStatOverTarget2.Description = "If your Soc is higher than 11, it is reduced by 1";
      outcomeArray2[1] = (Outcome) ifStatOverTarget2;
      outcomeArray2[2] = (Outcome) new Event.EndOfCareer();
      outcomeArray2[3] = (Outcome) new AddYearsToCharacter(4);
      Event.MultipleOutcomes multipleOutcomes2 = new Event.MultipleOutcomes("REEDUCATION EVENT: 1 Term & New Career", "Re-education requires one term. You must enter a new career and reduce END by –1. If SOC 11+, reduce SOC by –1, to a minimum of 11 ", outcomeArray2);
      outcomeList3.Add((Outcome) multipleOutcomes2);
      outcomeList1.Add((Outcome) new Event.SingleOutcome("REEDUCATION EVENT: One Term & Continue Career", "Re-education requires one term but you can continue this career.", (Outcome) new AddYearsToCharacter(4)));
      outcomeList1.Add((Outcome) new Event("REEDUCATION EVENT: Requires Less than 1 Year. Career Unaffected.", "Re-education requires less than a year and your career is not affected."));
      outcomeList1.Add((Outcome) new Event.RandomAmount("REEDUCATION EVENT: Fined but no reeducation", "You are fined 1Dx1000Cr and are not reeducated.", 1, 6, (Outcome) new Outcome.GainMoney(-1000), true));
      outcomeList1.Add((Outcome) new Event.SingleOutcome("REEDUCATION EVENT: Exonerated!", "You are exonerated. Obtain an additional benefit in your current career", (Outcome) new Outcome.MusteringOutBenefitModifier(1)));
      ZhodaniReeducationEvent.zhodaniReeducationEvents = outcomeList1;
    }
  }
}
