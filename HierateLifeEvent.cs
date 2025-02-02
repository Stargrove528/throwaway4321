
// Type: com.digitalarcsystems.Traveller.DataModel.Events.HierateLifeEvent




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class HierateLifeEvent : Event
  {
    protected static readonly List<Outcome> lifeEvents;

    public override void handleOutcome(GenerationState currentState)
    {
      RollEffect rollEffect = Dice.D6Roll(2, 0, "Hierate Life Event Roll");
      Outcome lifeEvent = HierateLifeEvent.lifeEvents[rollEffect.rawResult - 2];
      currentState.decisionMaker.present(new Presentation(lifeEvent.Name, lifeEvent.Description));
      lifeEvent.handleOutcome(currentState);
    }

    static HierateLifeEvent()
    {
      List<Outcome> outcomeList1 = new List<Outcome>();
      outcomeList1.Add((Outcome) new Event.Injury(false));
      outcomeList1.Add((Outcome) new Event("Life Event: (Birth or Death)", "Someone close to the character dies, such as a friend or pride member. Alternatively, someone close to the character gives birth (or is born!). The character is involved in some fashion (father or mother, pridekin, honour guard)."));
      outcomeList1.Add((Outcome) new StatBasedOutcome("Life Event: Territory Challenge:", "A rival attempts to claim some of your Territory (or your sire's or mate's Territory, if you have none of your own). Make a Diplomat, Broker, Melee(natural), or Gun Combat check, and increase or decrease, your TER(or Soc if you have no Territory) by the Effect of the roll. ", "ter", (Outcome) new Event.ChooseChallenge("Life Event: Territory Challenge:", "A rival attempts to claim some of your Territory (or your sire's or mate's Territory, if you have none of your own). Make a Diplomat, Broker, Melee(natural), or Gun Combat check, and increase or decrease, your TER(or Soc if you have no Territory) by the Effect of the roll. ", new List<Outcome>(), new List<Outcome>(), new Event.Challenge[7]
      {
        (Event.Challenge) new ModifyStatByEffect("Socially Outmaneuver Them! ", "Move through the halls of power, and out maneuver the challenger. (Diplomat + SOC)", "Diplomat", "soc", 8, "ter"),
        (Event.Challenge) new ModifyStatByEffect("Crush them in their own social sphere!", "Manipulate their own social circumstance to paint them in a bad light. (Diplomat + Int)", "Diplomat", "int", 8, "ter"),
        (Event.Challenge) new ModifyStatByEffect("Manipulate the market, and dash their profits!", "Make the acquisition too expensive for them to sustain. (Broker + Int)", "Broker", "int", 8, "ter"),
        (Event.Challenge) new ModifyStatByEffect("Gauge the market and simply outperform them!", "Buy when the cost is low, sell when high.  Let the power of the market be your claws. (Broker + Int)", "Broker", "edu", 8, "ter"),
        (Event.Challenge) new ModifyStatByEffect("Violence solves everything!", "Might makes right.  Physically beat them down with blows. (Melee(natural) + STR)", "Natural", "str", 8, "ter"),
        (Event.Challenge) new ModifyStatByEffect("Finesse over Raw Power!", "Float like a butterfly, sting like a bee... (Melee(natural) + DEX)", "Natural", "dex", 8, "ter"),
        (Event.Challenge) new ModifyStatByEffect("Bullets and Beams are better!", "You use gunfire to drive the challenger away. (Gun Combat + DEX)", "Gun Combat", "dex", 8, "ter")
      }), (Outcome) new Event.ChooseChallenge("", "", new List<Outcome>(), new List<Outcome>(), new Event.Challenge[7]
      {
        (Event.Challenge) new ModifyStatByEffect("Socially Outmaneuver Them! ", "Move through the halls of power, and out maneuver the challenger. (Diplomat + SOC)", "Diplomat", "soc", 8, "soc"),
        (Event.Challenge) new ModifyStatByEffect("Crush them in their own social sphere!", "Manipulate their own social circumstance to paint them in a bad light. (Diplomat + Int)", "Diplomat", "int", 8, "soc"),
        (Event.Challenge) new ModifyStatByEffect("Manipulate the market, and dash their profits!", "Make the acquisition too expensive for them to sustain. (Broker + Int)", "Broker", "int", 8, "soc"),
        (Event.Challenge) new ModifyStatByEffect("Gauge the market and simply outperform them!", "Buy when the cost is low, sell when high.  Let the power of the market be your claws. (Broker + Int)", "Broker", "edu", 8, "soc"),
        (Event.Challenge) new ModifyStatByEffect("Violence solves everything!", "Might makes right.  Physically beat them down with blows. (Melee(natural) + STR)", "Natural", "str", 8, "soc"),
        (Event.Challenge) new ModifyStatByEffect("Finesse over Raw Power!", "Float like a butterfly, sting like a bee... (Melee(natural) + DEX)", "Natural", "dex", 8, "soc"),
        (Event.Challenge) new ModifyStatByEffect("Bullets and Beams are better!", "You use gunfire to drive the challenger away. (Gun Combat + DEX)", "Gun Combat", "dex", 8, "soc")
      })));
      outcomeList1.Add((Outcome) new Event("Life Event: (Change in Marriage Status)", "If not married, you are now married (either to an existing Ally or Contact, or a mate arranged by your clan).  If already married, decide if  another female is added to the pride or if your previous mate was slain."));
      outcomeList1.Add((Outcome) new HierateClanEvent("Clan Event", "Things that happen to your clan impact you. Roll again"));
      outcomeList1.Add((Outcome) new Event.SingleOutcome("Life Event: (New Contact)", "The character gains a new contact", (Outcome) new Outcome.Contacts(1)));
      outcomeList1.Add((Outcome) new HierateClanEvent("Clan Event", "Things that happen to your clan impact you. Roll again"));
      outcomeList1.Add((Outcome) new Event.SingleOutcome("Life Event: (Travel)", "You move to another world. You gain a +2 DM to your next Qualification roll.", (Outcome) new Outcome.QualificationModifier(2)));
      outcomeList1.Add((Outcome) new Event.ChoiceOutcome(1, "Life Event: (Duel)", "You are challenged to a duel over a matter of family honour. If you refuse, lose SOC -1.  If you accept, roll Melee(natural) 8+. If you succeed, gain SOC +1, otherwise, lose Soc-2.", new Outcome[2]
      {
        (Outcome) new Event.SingleOutcome("Decline the Challenge", "You decide not to fight the duel", (Outcome) new Outcome.StatModified("Soc", -1)),
        (Outcome) new Event.ChoiceOutcome(1, "Defend Your Clan's Honour", "You fight the duel. ", new Outcome[2]
        {
          (Outcome) new Event.Challenge("Raw Power!", "Crush them. Pierce through any defence. Let their insults fall to sniveling for their very life!\nStrRoll Melee(Natural) + STR.  Succeed, and gain Soc +1.  Fail, and lose Soc -2.", "Natural", "STR", 8, (IList<Outcome>) new List<Outcome>()
          {
            (Outcome) new Outcome.StatModified("Soc", 1)
          }, (IList<Outcome>) new List<Outcome>()
          {
            (Outcome) new Outcome.StatModified("Soc", -2)
          }),
          (Outcome) new Event.Challenge("Finesse and Cunning!", "Their strongest offense can be turned into their weakest defense.  Slip around their attacks until your dew claw is at their very throat!\nRoll Natural + DEX.  Succeed, and gain Soc +1.  Fail, and lose Soc -2.", "Natural", "Dex", 8, (IList<Outcome>) new List<Outcome>()
          {
            (Outcome) new Outcome.StatModified("Soc", 1)
          }, (IList<Outcome>) new List<Outcome>()
          {
            (Outcome) new Outcome.StatModified("Soc", -2)
          })
        })
      }));
      List<Outcome> outcomeList2 = outcomeList1;
      Outcome[] outcomeArray = new Outcome[2];
      List<Outcome> successOutcomes1 = new List<Outcome>();
      List<Outcome> failureOutcomes1 = new List<Outcome>();
      failureOutcomes1.Add((Outcome) new EnsureStatMax("Soc", 2));
      failureOutcomes1.Add((Outcome) new Outcome.NextCareerMustBe("Outcast", true));
      Event.Challenge[] challengeArray1 = new Event.Challenge[2]
      {
        new Event.Challenge("Might Makes Right", "Overpower him.  Roll Natural + STR.  Fail, and drop to Soc -2, and become an outcast.", "Natural", "STR", 8),
        new Event.Challenge("Fast And Smooth", "Out maneuver him.  Roll Natural + DEX.  Fail, and drop to Soc -2, and become an outcast.", "Natural", "DEX", 8)
      };
      outcomeArray[0] = (Outcome) new Event.ChooseChallenge("Fight For Your Honor", "You face your accuser on the field of honour.  Roll Melee(natural) 10 + to defend yourself ", successOutcomes1, failureOutcomes1, challengeArray1);
      List<Outcome> successOutcomes2 = new List<Outcome>();
      List<Outcome> failureOutcomes2 = new List<Outcome>();
      failureOutcomes2.Add((Outcome) new EnsureStatMax("Soc", 2));
      failureOutcomes2.Add((Outcome) new Outcome.NextCareerMustBe("Outcast", true));
      Event.Challenge[] challengeArray2 = new Event.Challenge[2]
      {
        new Event.Challenge("Friend In High Places", "If You have a Contact, or Ally, within your clan, you may have them speak on your behalf, giving you a DM +2.  Roll Advocate + SOC.  Fail, and drop to Soc -2, and become an outcast.", "Advocate", "SOC", 6),
        new Event.Challenge("Stand Tall", "Even if you alone.  Roll Advocate + SOC.  Fail, and drop to Soc -2, and become an outcast.", "Natural", "DEX", 8)
      };
      outcomeArray[1] = (Outcome) new Event.ChooseChallenge("Taking Your Case To Court", "You take the accusation before the authorities of you clan.  Roll Advocate 8+ to defend yourself ", successOutcomes2, failureOutcomes2, challengeArray2);
      Event.ChoiceOutcome choiceOutcome = new Event.ChoiceOutcome(1, "Life Event: (Dishonoured)", "You are accused of a crime.  Roll Advocate 8+ or Melee(natural) 10 + to defend yourself (if you have any Contacts or Allies in the clan, you gain DM +2 to the Advocate roll).  If you cannot defend yourself, you drop to SOC 2 and become Outcast.", outcomeArray);
      outcomeList2.Add((Outcome) choiceOutcome);
      outcomeList1.Add((Outcome) new UnusualHierateLifeEvent("Unusual Event!", "Something weird. Roll again"));
      HierateLifeEvent.lifeEvents = outcomeList1;
    }
  }
}
