
// Type: com.digitalarcsystems.Traveller.DataModel.Event




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Event : Outcome
  {
    public Event()
    {
    }

    public Event(Event copyMe)
    {
      this.Description = copyMe.Description;
      this.Name = copyMe.Name;
    }

    public Event(string withName, string withDescription)
    {
      this.Description = withDescription;
      this.Name = withName;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.recorder.RecordBenefit(this, currentState);
    }

    public override string ToString() => this.Description;

    public class Death : Event
    {
      public Death()
      {
        this.Description = "How can you read this if you just died?";
        this.Name = "Game Over";
      }

      public Death(string causeOfDeath)
        : this()
      {
        this.Description = causeOfDeath;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.character.killCharacter();
        currentState.recorder.RecordBenefit((Event) this, currentState);
      }
    }

    public class AgingCrisis : Event
    {
      public IList<Attribute> statsInCrisis = (IList<Attribute>) null;

      [JsonConstructor]
      public AgingCrisis()
      {
      }

      public AgingCrisis(IList<Attribute> statsInCrisis)
      {
        this.Description = "Aging Crisis";
        this.Name = "Aging Crisis";
        this.statsInCrisis = statsInCrisis;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        Outcome[] choices = new Outcome[2]
        {
          (Outcome) new Cash(-10000 * Dice.D6Roll(1, 0, "Roll to determine medical debt (x10000 Cr)").effect),
          (Outcome) new Event.Death("Your character suffered a crisis, and died.")
        };
        Outcome outcome = currentState.decisionMaker.ChooseOne<Outcome>("Your character is on the brink of death. Either accept a large debt, or allow the character to die.", (IList<Outcome>) choices);
        currentState.recorder.RecordBenefit((Event) this, currentState);
        outcome.handleOutcome(currentState);
        if (!(outcome is Cash))
          return;
        foreach (Attribute statsInCrisi in (IEnumerable<Attribute>) this.statsInCrisis)
        {
          statsInCrisi.Value = 1;
          statsInCrisi.UninjuredValue = 1;
        }
      }
    }

    public class InjuryCrisis : Event.AgingCrisis
    {
      [JsonConstructor]
      public InjuryCrisis()
      {
      }

      public InjuryCrisis(IList<Attribute> statsInCrisis)
        : base(statsInCrisis)
      {
        string str = "[";
        for (int index = 0; index < statsInCrisis.Count; ++index)
        {
          if (index != 0)
            str += ", ";
          str += statsInCrisis[index].Name;
        }
        this.Description = str + "] in Crisis." + "You are seriously injured, and hospitalized. Your mortality is readily apparent. Your concern about the money is second only to your desire to get better.";
        this.Name = "Injury Crisis";
      }
    }

    public class Mishap : Event
    {
      [JsonProperty]
      public bool ejectedFromCareer = true;

      [JsonConstructor]
      public Mishap(bool ejectFromCareer)
        : this()
      {
        this.ejectedFromCareer = ejectFromCareer;
      }

      public Mishap()
      {
        this.Description = "Sometimes in life, things don't go as planned. Something bad has happened...";
        this.Name = nameof (Mishap);
      }

      public Mishap(Event.Mishap mishap)
        : base((Event) mishap)
      {
        this.ejectedFromCareer = mishap.ejectedFromCareer;
      }

      public Mishap(string withName, string withDescription, bool ejectFromCareer)
        : base(withName, withDescription)
      {
        this.ejectedFromCareer = ejectFromCareer;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        Event mishap = currentState.currentCareer.Mishap;
        mishap.Description = nameof (Mishap) + (this.ejectedFromCareer ? " (EJECTED FROM CAREER)" : "") + ": " + mishap.Description;
        this.Description = mishap.Description;
        mishap.handleOutcome(currentState);
        currentState.endCurrentCareer = this.ejectedFromCareer;
      }
    }

    public class Injury : Event.Mishap
    {
      [JsonProperty]
      public Outcome outcome = (Outcome) null;

      [JsonConstructor]
      public Injury()
      {
        this.Description = nameof (Injury);
        this.Name = nameof (Injury);
      }

      public Injury(Event.Injury copyMe)
        : base((Event.Mishap) copyMe)
      {
        this.outcome = copyMe.outcome;
      }

      public Injury(bool ejectFromCareer)
        : base(ejectFromCareer)
      {
      }

      public virtual List<Outcome> InjuryList()
      {
        List<Outcome> outcomeList1 = new List<Outcome>();
        outcomeList1.Add((Outcome) new Event.MultipleOutcomes("Nearly Killed!", "It was gruesome. Your existence flashed before your eyes. You're lucky to even be alive. Reduce one physical characteristic by 1D, then reduce two other physical characteristics by 2", new Outcome[2]
        {
          (Outcome) new Outcome.ReducePhysicalAttribute(1, 1, true),
          (Outcome) new Outcome.ReducePhysicalAttribute(2, 2, false)
        }));
        List<Outcome> outcomeList2 = outcomeList1;
        Outcome.ReducePhysicalAttribute physicalAttribute1 = new Outcome.ReducePhysicalAttribute(1, 1, true);
        physicalAttribute1.Name = "Severely Injured!";
        physicalAttribute1.Description = "It was pretty bad. Only time will tell how bad. Reduce one physical characteristic by 1D";
        outcomeList2.Add((Outcome) physicalAttribute1);
        outcomeList1.Add((Outcome) new Event.ChoiceOutcome(1, "Missing Eye or Limb!", "You'll never forget the feeling of losing a part of yourself. Choose between reducing your Strength or your Dexterity by 1", new Outcome[2]
        {
          (Outcome) new Outcome.StatModified("Str", -1),
          (Outcome) new Outcome.StatModified("Dex", -1)
        }));
        List<Outcome> outcomeList3 = outcomeList1;
        Outcome.ReducePhysicalAttribute physicalAttribute2 = new Outcome.ReducePhysicalAttribute(1, 2);
        physicalAttribute2.Name = "Scarred";
        physicalAttribute2.Description = "You're pretty beaten, scarred and injured. Reduce any physical characteristic by 2";
        outcomeList3.Add((Outcome) physicalAttribute2);
        List<Outcome> outcomeList4 = outcomeList1;
        Outcome.ReducePhysicalAttribute physicalAttribute3 = new Outcome.ReducePhysicalAttribute(1, 1);
        physicalAttribute3.Name = nameof (Injury);
        physicalAttribute3.Description = "It could have been much worse, but you're still injured. Reduce any physical characteristic by 1";
        outcomeList4.Add((Outcome) physicalAttribute3);
        outcomeList1.Add((Outcome) new Event("Lightly Injured", "You were very, very lucky. You got lightly injured, but there's no lasting damage. You pick yourself up, and move on. The pain subsides eventually."));
        return outcomeList1;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        this.outcome = Dice.RollRandomResult<Outcome>("Roll to determine what kind of injury you have.", (IList<Outcome>) this.InjuryList(), ContextKeys.OUTCOMES);
        currentState.endCurrentCareer = this.ejectedFromCareer;
        currentState.recorder.RecordBenefit((Event) this, currentState);
        if (this.outcome != this)
          this.outcome.handleOutcome(currentState);
        currentState.injuries.Add(this);
        if (!currentState.character.InCrisis)
          return;
        new Event.InjuryCrisis(currentState.character.StatsInCrisis).handleOutcome(currentState);
      }
    }

    public class LifeEvent : Event
    {
      [JsonConstructor]
      public LifeEvent()
      {
      }

      public LifeEvent(string withName, string withDescription)
        : base(withName, withDescription)
      {
      }

      public LifeEvent(Event.LifeEvent copyMe)
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
        choices.Add((Outcome) new Event.UnusualLifeEvent("Unusual Life Event!", "Something VERY unusual has happened to you. Roll again"));
        return Dice.RollRandomResult<Outcome>("Life happens... Roll for Life Event", (IList<Outcome>) choices, ContextKeys.OUTCOMES);
      }

      public override void handleOutcome(GenerationState currentState)
      {
        this.getLifeEvent(currentState).handleOutcome(currentState);
      }
    }

    public class UnusualLifeEvent : Event
    {
      [JsonConstructor]
      public UnusualLifeEvent()
      {
      }

      public UnusualLifeEvent(string title, string description)
        : base(title, description)
      {
      }

      public virtual Outcome getUnusualLifeEvent(GenerationState currentState)
      {
        return Dice.RollRandomResult<Outcome>("Something unusual is happening to you. Roll to determine what.", (IList<Outcome>) new List<Outcome>()
        {
          (Outcome) new Event.OptionalEvent("Life Event: Psionics", "You encounter a Psionic institute. You may immediately test your Psionic Strength and, if you qualify, take the Psion career in your next term.", (Outcome) new Event.TestForTalent(Trait.PSIONIC)),
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

    public class SevereInjury : Event.Injury
    {
      public SevereInjury(Event.SevereInjury copyMe)
        : base((Event.Injury) copyMe)
      {
      }

      [JsonConstructor]
      public SevereInjury(bool mustLeaveCareer = true)
      {
        this.ejectedFromCareer = mustLeaveCareer;
        this.outcome = this.InjuryList()[1];
      }
    }

    public class ChoiceOutcome : Event, OutcomeContainer
    {
      public int numToChoose;

      [JsonConstructor]
      public ChoiceOutcome()
      {
      }

      public ChoiceOutcome(Event.ChoiceOutcome copyMe)
        : base((Event) copyMe)
      {
        this.numToChoose = copyMe.numToChoose;
        this.Outcomes = (IList<Outcome>) new List<Outcome>(copyMe.Outcomes.Select<Outcome, Outcome>((Func<Outcome, Outcome>) (x => x.Clone<Outcome>())));
      }

      public ChoiceOutcome(int toChoose, params Outcome[] choices)
      {
        this.numToChoose = toChoose;
        this.Outcomes = (IList<Outcome>) new List<Outcome>((IEnumerable<Outcome>) choices);
      }

      public ChoiceOutcome(
        int toChoose,
        string withName,
        string withDescription,
        params Outcome[] choices)
        : this(toChoose, choices)
      {
        this.Name = withName;
        this.Description = withDescription;
      }

      public ChoiceOutcome(string description, int toChoose, params Outcome[] outcomes)
        : this(toChoose, outcomes)
      {
        this.Description = description;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        if (this.Description != null && !this.Description.Equals(nameof (ChoiceOutcome), StringComparison.InvariantCultureIgnoreCase))
          currentState.recorder.RecordBenefit((Event) this, currentState);
        foreach (Outcome outcome in this.Description == null || this.Description.Count<char>() <= 13 ? (IEnumerable<Outcome>) currentState.decisionMaker.Choose<Outcome>(this.numToChoose, this.Outcomes) : (IEnumerable<Outcome>) currentState.decisionMaker.Choose<Outcome>(this.Description, this.numToChoose, this.Outcomes))
          outcome.handleOutcome(currentState);
      }

      public override string ToString()
      {
        return !string.IsNullOrEmpty(this._name) ? this._name : base.ToString();
      }

      [JsonProperty]
      public virtual IList<Outcome> Outcomes { get; set; }
    }

    public class Challenge : Event, OutcomeContainer
    {
      public string skillName;
      public string statName;
      public int result_effect;
      public int targetNumber;
      [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
      public List<Outcome> successOutcomes;
      [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
      public List<Outcome> failureOutcomes;
      public RollEffect challengeResult = (RollEffect) null;

      [JsonIgnore]
      public virtual IList<Outcome> Outcomes
      {
        get
        {
          List<Outcome> outcomes = new List<Outcome>();
          if (this.successOutcomes != null)
            outcomes.AddRange((IEnumerable<Outcome>) this.successOutcomes);
          if (this.failureOutcomes != null)
            outcomes.AddRange((IEnumerable<Outcome>) this.failureOutcomes);
          return (IList<Outcome>) outcomes;
        }
      }

      [JsonConstructor]
      public Challenge()
      {
        this.successOutcomes = new List<Outcome>();
        this.failureOutcomes = new List<Outcome>();
      }

      public Challenge(
        string withName,
        string withDescription,
        string skill,
        string stat,
        int target)
        : base(withName, withDescription)
      {
        this.skillName = skill;
        this.statName = stat;
        this.targetNumber = target;
      }

      public Challenge(string withName, string withDescription)
        : base(withName, withDescription)
      {
      }

      public Challenge(
        string withName,
        string withDescription,
        string skill,
        string stat,
        int target,
        IList<Outcome> successOutcomes,
        IList<Outcome> failureOutcomes)
        : this(withName, withDescription, skill, stat, target)
      {
        this.successOutcomes = successOutcomes != null && failureOutcomes != null ? new List<Outcome>((IEnumerable<Outcome>) successOutcomes) : throw new Exception("Success outcome and Failure outcomes must be defined OR use a different constructor.");
        this.failureOutcomes = new List<Outcome>((IEnumerable<Outcome>) failureOutcomes);
      }

      public override void handleOutcome(GenerationState currentState)
      {
        ISkill skill = currentState.skillSource.getSkill(this.skillName);
        skill.Level = -3;
        if (currentState.character.hasSkill(this.skillName))
        {
          skill = currentState.character.getSkill(this.skillName);
          if (skill.Cascade)
            skill = Utility.GetHighestCascadeSkill(skill, currentState.character);
        }
        else if (skill.Parent != null && currentState.character.hasSkill(skill.Parent.Name))
          skill = currentState.character.getSkill(skill.Parent.Name);
        else if (currentState.character.hasSkill("Jack-of-all-Trades"))
        {
          skill.Level += currentState.character.getSkill("Jack-of-all-Trades").Level;
          ((Skill) skill).Name = "Jack-of-all-Trades";
        }
        Attribute stat = (Attribute) null;
        if (this.statName != null && this.statName.Count<char>() > 0)
          stat = currentState.character.getAttribute(this.statName);
        RollParam settings = Dice.SkillRoll(stat, skill, this.targetNumber, this.Description);
        settings.AddResultDescriptions(this.Description + " [SUCCEEDED]", this.Description + " [FAILED]");
        this.challengeResult = Dice.Roll(settings);
        this.result_effect = this.challengeResult.effect;
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
          currentState.recorder.RecordBenefit((Event) this, currentState);
          foreach (Outcome failureOutcome in this.failureOutcomes)
            failureOutcome.handleOutcome(currentState);
        }
      }
    }

    public class RollChallenge : Event.Challenge
    {
      [JsonConstructor]
      public RollChallenge()
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        RollEffect rollEffect = Dice.Roll(new RollParam()
        {
          numOfDices = 2,
          description = this.Description,
          rawMinSuccessValue = this.targetNumber,
          isToBeAnimated = true,
          totalModifier = 0
        });
        List<Outcome> outcomeList = rollEffect.isSuccessful ? this.successOutcomes : this.failureOutcomes;
        if (!rollEffect.isSuccessful)
          return;
        foreach (Outcome outcome in outcomeList)
          outcome.handleOutcome(currentState);
      }
    }

    public class StatChallenge : Event.Challenge
    {
      [JsonConstructor]
      public StatChallenge()
      {
      }

      public StatChallenge(string withName, string withDescription, string stat, int target)
        : base(withName, withDescription, "", stat, target)
      {
      }

      public StatChallenge(
        string withName,
        string withDescription,
        string stat,
        int target,
        IList<Outcome> successOutcomes,
        IList<Outcome> failureOutcomes)
        : base(withName, withDescription, "", stat, target, successOutcomes, failureOutcomes)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        RollParam settings = Dice.StatRoll(currentState.character.getAttribute(this.statName), this.targetNumber, this.Description);
        settings.AddResultDescriptions(this.Description + " [SUCCEEDED]", this.Description + " [FAILED]");
        this.challengeResult = Dice.Roll(settings);
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

    public class ChooseChallenge : Event.Challenge
    {
      [JsonProperty]
      public List<Event.Challenge> challenges = new List<Event.Challenge>();

      [JsonConstructor]
      public ChooseChallenge()
      {
      }

      public ChooseChallenge(
        string challengeTitle,
        string challengeDescription,
        List<Outcome> successOutcomes,
        List<Outcome> failureOutcomes,
        params Event.Challenge[] challenges)
        : base(challengeTitle, challengeDescription)
      {
        this.successOutcomes = successOutcomes;
        this.failureOutcomes = failureOutcomes;
        this.challenges.AddRange((IEnumerable<Event.Challenge>) challenges);
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.recorder.RecordBenefit((Event) this, currentState);
        Event.Challenge challenge = currentState.decisionMaker.ChooseOne<Event.Challenge>((IList<Event.Challenge>) this.challenges);
        challenge.successOutcomes = this.successOutcomes;
        challenge.failureOutcomes = this.failureOutcomes;
        challenge.handleOutcome(currentState);
      }
    }

    public class SpecializationBasedChallenge : Event, OutcomeContainer
    {
      [JsonProperty]
      public Dictionary<string, Event.Challenge> challenges = new Dictionary<string, Event.Challenge>();
      public IList<Outcome> failureOutcomes;
      public IList<Outcome> successOutcomes;

      [JsonConstructor]
      public SpecializationBasedChallenge()
      {
      }

      public SpecializationBasedChallenge(
        string challengeName,
        string challengeDescription,
        IList<Outcome> successOutcomes,
        IList<Outcome> failureOutcomes,
        string specialization1,
        Event.Challenge challenge1,
        string specialization2,
        Event.Challenge challenge2,
        string specialization3,
        Event.Challenge challenge3)
        : base(challengeName, challengeDescription)
      {
        this.challenges[specialization1] = challenge1;
        this.challenges[specialization2] = challenge2;
        this.challenges[specialization3] = challenge3;
        this.successOutcomes = successOutcomes;
        this.failureOutcomes = failureOutcomes;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        Event.Challenge challenge = this.challenges[currentState.character.CurrentTerm.specializationName];
        challenge.failureOutcomes = new List<Outcome>((IEnumerable<Outcome>) this.failureOutcomes);
        challenge.successOutcomes = new List<Outcome>((IEnumerable<Outcome>) this.successOutcomes);
        currentState.recorder.RecordBenefit((Event) this, currentState);
        challenge.handleOutcome(currentState);
      }

      public virtual IList<Outcome> Outcomes
      {
        get
        {
          List<Outcome> outcomes = new List<Outcome>((IEnumerable<Outcome>) this.successOutcomes);
          outcomes.AddRange((IEnumerable<Outcome>) this.failureOutcomes);
          return (IList<Outcome>) outcomes;
        }
      }
    }

    public new class RandomAmount : Event, OutcomeContainer
    {
      [JsonProperty]
      public int min;
      [JsonProperty]
      public int max;
      [JsonProperty]
      public bool inform_player;
      public Outcome outcome;

      [JsonConstructor]
      public RandomAmount()
      {
      }

      public RandomAmount(string withTitle, string withDescription)
        : base(withTitle, withDescription)
      {
      }

      public RandomAmount(
        string withTitle,
        string withDescription,
        int min_amount,
        int max_amount,
        Outcome repeatMe,
        bool inform_player = false)
        : base(withTitle, withDescription)
      {
        this.min = min_amount;
        this.max = max_amount;
        this.outcome = repeatMe;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.recorder.RecordBenefit((Event) this, currentState);
        int num = Dice.random(this.min, this.max);
        if (this.inform_player)
          currentState.decisionMaker.present(new Presentation("Number Rolled", " [" + num.ToString() + "] " + this.outcome.Name + "(s) "));
        for (int index = 0; index < num; ++index)
          this.outcome.handleOutcome(currentState);
      }

      public virtual IList<Outcome> Outcomes
      {
        get
        {
          return (IList<Outcome>) new List<Outcome>()
          {
            this.outcome
          };
        }
      }
    }

    public class MultipleOutcomes : Event, OutcomeContainer
    {
      [JsonProperty]
      public List<Outcome> outcomes;

      [JsonConstructor]
      public MultipleOutcomes()
      {
      }

      public MultipleOutcomes(params Outcome[] outcomes)
      {
        this.outcomes = new List<Outcome>((IEnumerable<Outcome>) outcomes);
      }

      public MultipleOutcomes(string withName, string withDescription, params Outcome[] outcomes)
        : this(outcomes)
      {
        this.Description = withDescription;
        this.Name = withName;
      }

      public MultipleOutcomes(string withName, string withDescription, IList<Outcome> outcomes)
      {
        this.outcomes = new List<Outcome>((IEnumerable<Outcome>) outcomes);
        this.Description = withDescription;
        this.Name = withName;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.recorder.RecordBenefit((Event) this, currentState);
        foreach (Outcome outcome in this.outcomes)
          outcome.handleOutcome(currentState);
      }

      [JsonIgnore]
      public virtual IList<Outcome> Outcomes => (IList<Outcome>) this.outcomes;
    }

    public class SingleOutcome : Event, OutcomeContainer
    {
      public Outcome outcome;

      public SingleOutcome()
      {
      }

      public SingleOutcome(Outcome outcome)
        : this(outcome.Name, outcome.Description, outcome)
      {
      }

      [JsonConstructor]
      public SingleOutcome(string withName, string withDescription, Outcome outcome)
        : base(withName, withDescription)
      {
        this.outcome = outcome;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.recorder.RecordBenefit((Event) this, currentState);
        this.outcome.handleOutcome(currentState);
      }

      [JsonIgnore]
      public virtual IList<Outcome> Outcomes
      {
        get
        {
          return (IList<Outcome>) new List<Outcome>()
          {
            this.outcome
          };
        }
      }
    }

    public class MustContinueCareer : Event
    {
      [JsonConstructor]
      public MustContinueCareer()
      {
      }

      public MustContinueCareer(string withName, string withDescription)
        : base(withName, withDescription)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.endCurrentCareer = false;
        currentState.mustContinueInCareer = true;
        currentState.recorder.RecordBenefit((Event) this, currentState);
      }
    }

    public class EndOfCareer : Event
    {
      [JsonConstructor]
      public EndOfCareer()
        : base("End of Career", "Your career is over.")
      {
      }

      public EndOfCareer(string reason)
        : base("End of career", reason)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.mustContinueInCareer = false;
        currentState.endCurrentCareer = true;
        currentState.decisionMaker.present(new Presentation((IDescribable) this));
        currentState.recorder.RecordBenefit((Event) this, currentState);
      }
    }

    public class Betrayal : Event
    {
      [JsonConstructor]
      public Betrayal()
      {
      }

      public Betrayal(string withName, string withDescription)
        : base(withName, withDescription)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        Character character = currentState.character;
        NamedList<Contact> namedList1 = new NamedList<Contact>("Allies", (ICollection<Contact>) character.Allies);
        NamedList<Contact> namedList2 = new NamedList<Contact>("Contacts", (ICollection<Contact>) character.Contacts);
        if (namedList1.Count == 0 && namedList2.Count == 0)
        {
          currentState.decisionMaker.ChooseOne<Outcome.EntityToMeet>("Pick either a Rival or Enemy", (IList<Outcome.EntityToMeet>) new List<Outcome.EntityToMeet>()
          {
            (Outcome.EntityToMeet) new Outcome.Enemies(1),
            (Outcome.EntityToMeet) new Outcome.Rivals(1)
          }).handleOutcome(currentState);
        }
        else
        {
          NamedList<Contact> choices;
          if (namedList1.Count > 0 && namedList2.Count > 0)
            choices = currentState.decisionMaker.ChooseOne<NamedList<Contact>>("Choose whether to convert and Ally or contact to an Enemy", (IList<NamedList<Contact>>) new List<NamedList<Contact>>()
            {
              namedList1,
              namedList2
            });
          else
            choices = namedList1.Count <= 0 ? namedList2 : namedList1;
          Contact contact = currentState.decisionMaker.ChooseOne<Contact>("Which entity should be converted?", (IList<Contact>) choices);
          character.removeEntityIKnow(contact);
          Outcome.GainSpecifiedEntityToMeet specifiedEntityToMeet = currentState.decisionMaker.ChooseOne<Outcome.GainSpecifiedEntityToMeet>("Pick either a Rival or Enemy", (IList<Outcome.GainSpecifiedEntityToMeet>) new List<Outcome.GainSpecifiedEntityToMeet>()
          {
            (Outcome.GainSpecifiedEntityToMeet) new Outcome.GainSpecifiedEnemy(contact),
            (Outcome.GainSpecifiedEntityToMeet) new Outcome.GainSpecifiedRival(contact)
          });
          this.Description = this.Description + "\n\t[" + contact?.ToString() + "] changed from [" + choices.Name.ToLower() + "] to [" + Utility.getLastCapitalizedWord(specifiedEntityToMeet.GetType().Name) + "]. ";
          currentState.recorder.RecordBenefit((Event) this, currentState);
          specifiedEntityToMeet.handleOutcome(currentState);
        }
      }
    }

    public class OptionalEvent : Event, OutcomeContainer
    {
      [JsonProperty]
      public Outcome outcome;

      [JsonConstructor]
      public OptionalEvent()
      {
      }

      public OptionalEvent(string withName, string withDescription, Outcome outcomeIfAccepted)
        : base(withName, withDescription)
      {
        this.outcome = outcomeIfAccepted;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        string[] choices = new string[2]
        {
          "Accept",
          "Reject"
        };
        string str = currentState.decisionMaker.ChooseOne<string>(this.Description + "  Choose to Accept, or Reject.", (IList<string>) choices);
        this.Description = this.Description + " [" + str + "ed]";
        currentState.recorder.RecordBenefit((Event) this, currentState);
        if (!str.Equals(choices[0]))
          return;
        this.outcome.handleOutcome(currentState);
      }

      [JsonIgnore]
      public virtual IList<Outcome> Outcomes
      {
        get
        {
          return (IList<Outcome>) new List<Outcome>()
          {
            this.outcome
          };
        }
      }
    }

    public class EquipementBenefitOutcome : OutcomeOperator
    {
      [JsonConstructor]
      public EquipementBenefitOutcome()
      {
      }

      public void operateOnOutcome(Outcome outcome)
      {
        if (!(outcome is com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment))
          return;
        ((com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment) outcome).MusteringOutBenefit = true;
      }
    }

    public class MusteringOutBenefitWraper : Event
    {
      public Outcome benefit = (Outcome) null;
      private Event.EquipementBenefitOutcome oo = new Event.EquipementBenefitOutcome();

      [JsonConstructor]
      public MusteringOutBenefitWraper()
      {
      }

      public MusteringOutBenefitWraper(Outcome wrapMe)
      {
        this.benefit = wrapMe;
        this.Description = "Mustering Out Benefit: " + wrapMe?.ToString();
        Utility.processAllContainedOutcomes((OutcomeOperator) this.oo, wrapMe);
      }
    }

    public class GainPromotion : Event
    {
      [JsonConstructor]
      public GainPromotion()
      {
        this.Name = "Promotion";
        this.Description = "You were promoted";
      }

      public GainPromotion(string description)
        : base("Promotion", description)
      {
      }

      public GainPromotion(string eventName, string eventDescription)
        : base(eventName, eventDescription)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        Rank title = currentState.character.incrementRankAndGetTitle(currentState.currentCareer);
        currentState.decisionMaker.present(new Presentation("Congratulations!", "You've been promoted to  " + currentState.currentCareer.Name + " Rank: " + currentState.character.CurrentTerm.rank.ToString() + " " + (title == null || title.title == null || title.title.Length <= 0 ? "" : " Title: " + title.title)));
        currentState.recorder.RecordBenefit((Event) this, currentState);
        if (title != null && title.benefits != null)
        {
          foreach (Outcome benefit in title.benefits)
            benefit.handleOutcome(currentState);
        }
        if (!(currentState.decisionMaker is RandomCharacterCreationAlgorithm))
          return;
        new Outcome.AdditionalRollOnSkillsAndTrainingTable().handleOutcome(currentState);
      }
    }

    public class WagerMusteringOutBenefits : Event
    {
      public Event.Challenge myChallenge;
      public const int NO_MAX = 10000;
      public int limit_benefits_wager_to = 10000;
      public bool double_or_nothing = false;

      [JsonConstructor]
      public WagerMusteringOutBenefits()
      {
      }

      public WagerMusteringOutBenefits(
        string eventName,
        string eventDescription,
        Event.Challenge challenge)
        : base(eventName, eventDescription)
      {
        this.myChallenge = challenge;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        int numBenefits = currentState.currentCareer.getNumBenefits(currentState.character);
        if (numBenefits > 0)
        {
          List<int> choices = new List<int>();
          for (int index = 0; index <= numBenefits && this.limit_benefits_wager_to > index; ++index)
            choices.Add(index);
          int num = currentState.decisionMaker.ChooseOne<int>("How many benefits will you wager?", (IList<int>) choices);
          List<Outcome> outcomeList1 = new List<Outcome>();
          List<Outcome> outcomeList2 = new List<Outcome>();
          if (num == 0)
          {
            this.Description += "[Nothing Ventured, Nothing Gained]";
            currentState.decisionMaker.present(new Presentation("You Wager Nothing", "Nothing ventured, nothing gained.  You don't lose anything, but you don't gain anthing either."));
          }
          else
          {
            int num_of_benefits = this.double_or_nothing ? num * 2 : (int) Math.Max(1.0, Math.Ceiling((double) num / 2.0));
            outcomeList1.Add((Outcome) new Outcome.MusteringOutBenefitModifier(num_of_benefits));
            outcomeList1.Add((Outcome) new Outcome.InformUser("You Did It!", "Your skills were sharp and you were in good form.  You've won " + num_of_benefits.ToString() + " additional benefit" + (num_of_benefits != 1 ? "s." : ".")));
            outcomeList2.Add((Outcome) new Outcome.MusteringOutBenefitModifier(-1 * num));
            outcomeList2.Add((Outcome) new Outcome.InformUser("No Luck!", "Maybe because you weren't on your toes, or maybe because it just wasn't your day, it all fell through.  You lost " + num.ToString() + " benefit" + (num != 1 ? "s." : ".")));
            this.myChallenge.successOutcomes = outcomeList1;
            this.myChallenge.failureOutcomes = outcomeList2;
            this.myChallenge.handleOutcome(currentState);
          }
        }
        else
        {
          this.Description += "\n[No Benefits to Wager With]";
          FeedbackStream.Send("No benefits to wager with.");
          currentState.decisionMaker.present(new Presentation("Oops.  No benefits!", "Unfortunately, you don't have any benefits to wager with. The opportunity was nice, but you couldn't take advantage of it."));
        }
        currentState.recorder.RecordBenefit((Event) this, currentState);
      }
    }

    public class LoseAllMusteringOutBenefits : Event
    {
      [JsonConstructor]
      public LoseAllMusteringOutBenefits()
      {
      }

      public LoseAllMusteringOutBenefits(string eventName, string eventDescription)
        : base(eventName, eventDescription)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.character.CurrentTerm.additional_mustering_out_benefits = 0;
        int numBenefits = currentState.currentCareer.getNumBenefits(currentState.character);
        currentState.character.CurrentTerm.additional_mustering_out_benefits = -1 * numBenefits;
        this.Description = this.Description + "\n[" + numBenefits.ToString() + " lost]";
        currentState.recorder.RecordBenefit((Event) this, currentState);
      }
    }

    public class ForciblyDrafted : Event
    {
      [JsonConstructor]
      public ForciblyDrafted()
      {
        this.Name = "Drafted";
        this.Description = "Waiting in your cell for your new masters. You are recalling as your grandpa was telling you stories, old ancient stories about something called British Navy from some old planet where they didn't know even basic of physics and were forcing people to service.";
      }

      public override void handleOutcome(GenerationState currentState)
      {
        if (currentState.draftCareers != null && currentState.draftCareers.Count > 0)
        {
          Career career = Dice.RollRandomResult<Career>("", currentState.draftCareers, ContextKeys.CAREERS);
          currentState.nextCarrerMustBe = career.Name;
          currentState.endCurrentCareer = true;
          this.Description = this.Description + " [" + currentState.nextCarrerMustBe + "]";
          currentState.recorder.RecordBenefit((Event) this, currentState);
        }
        else
          currentState.decisionMaker.present(new Presentation("Sorry!  No Draft Careers Available", "There aren't any careers available that are part of the draft.  You can obtain them from RPGsuite.com.  Draft careers include Agent, Merchant, Scouts, Army, Navy, and Marines."));
      }
    }

    public class FriendsAndFamilyInjured : Event
    {
      [JsonConstructor]
      public FriendsAndFamilyInjured()
      {
      }

      public FriendsAndFamilyInjured(string eventName, string eventDescription)
        : base(eventName, eventDescription)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        IList<Contact> contacts = (IList<Contact>) currentState.character.Contacts;
        IList<Contact> allies = (IList<Contact>) currentState.character.Allies;
        Contact[] collection = new Contact[6]
        {
          new Contact("Family: Mother"),
          new Contact("Family: Father")
          {
            Type = Contact.ContactType.Ally
          },
          new Contact("Family: Sister")
          {
            Type = Contact.ContactType.Ally
          },
          new Contact("Family: Brother")
          {
            Type = Contact.ContactType.Ally
          },
          new Contact("Family: Child")
          {
            Type = Contact.ContactType.Ally
          },
          new Contact("Family: Spouse")
          {
            Type = Contact.ContactType.Ally
          }
        };
        List<Contact> choices = new List<Contact>((IEnumerable<Contact>) contacts);
        choices.AddRange((IEnumerable<Contact>) allies);
        choices.AddRange((IEnumerable<Contact>) collection);
        Contact addMe = currentState.decisionMaker.ChooseOne<Contact>("Which friend or family member was injured?", (IList<Contact>) choices);
        int num1 = Dice.RawRollResult(1);
        int num2 = Dice.RawRollResult(1);
        int num3 = num1 > num2 ? num2 : num1;
        Outcome injury = new Event.Injury().InjuryList()[num3 - 1];
        Contact contact = addMe;
        contact.Notes = contact.Notes + "In term " + currentState.character.CurrentTerm.term.ToString() + " suffered an injury [" + injury.Name + "] because [" + this.Description + "]";
        new Event(addMe.Name + " injury", addMe.Name + " suffered an injury [" + injury.Name + "]").handleOutcome(currentState);
        if (currentState.character.EntityIKnow.Contains(addMe))
          return;
        currentState.character.addEntityIKnow(addMe);
      }
    }

    public class GainASkillAndTestIt : Event.Challenge
    {
      [JsonConstructor]
      public GainASkillAndTestIt()
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        if (this.skillName == null)
          EngineLog.Print("GAINASKILLANDTESTIT HAD NULL skillName");
        new Outcome.GainSkill(this.skillName).handleOutcome(currentState);
        base.handleOutcome(currentState);
      }
    }

    public class EnsureSkillAndTestIt : Event.Challenge
    {
      public int min_skill_level = 1;

      [JsonConstructor]
      public EnsureSkillAndTestIt()
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        new Outcome.EnsureSkillAtLevel(this.skillName, this.min_skill_level).handleOutcome(currentState);
        base.handleOutcome(currentState);
      }
    }

    public class GainSpecialistSkillFromSpecifiedCareer : Event
    {
      public string careerName;

      [JsonConstructor]
      public GainSpecialistSkillFromSpecifiedCareer()
      {
      }

      public GainSpecialistSkillFromSpecifiedCareer(
        string careerName,
        string eventName,
        string eventDescription)
        : base(eventName, eventDescription)
      {
        this.careerName = careerName;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        IList<Career.Specialization> applicableSpecializations = currentState.careerSource.GetCareer(this.careerName).GetApplicableSpecializations(currentState.character);
        List<NamedList<Outcome>> choices = new List<NamedList<Outcome>>();
        foreach (Career.Specialization specialization in (IEnumerable<Career.Specialization>) applicableSpecializations)
        {
          NamedList<Outcome> namedList = new NamedList<Outcome>(specialization.Name + " skills");
          ICollection<Outcome> specializationOutcomeList = (ICollection<Outcome>) specialization.SpecializationOutcomeList;
          namedList.AddRange((IEnumerable<Outcome>) specializationOutcomeList);
          choices.Add(namedList);
        }
        Dice.RollRandomResult<Outcome>("In the end you learned...", (IList<Outcome>) currentState.decisionMaker.ChooseOne<NamedList<Outcome>>("Choose one of the specialization skills tables from " + this.careerName + " to gain a skill from.", (IList<NamedList<Outcome>>) choices), ContextKeys.OUTCOMES).handleOutcome(currentState);
      }
    }

    public class RollMishapFromSpecifiedCareer : Event
    {
      public string careerName;

      [JsonConstructor]
      public RollMishapFromSpecifiedCareer()
      {
      }

      public RollMishapFromSpecifiedCareer(
        string careerName,
        string eventName,
        string eventDescription)
        : base(eventName, eventDescription)
      {
        this.careerName = careerName;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        (currentState.careerSource.GetCareer(this.careerName) ?? throw new NullReferenceException("specified career [" + this.careerName + "] didn't exist in the careerSource")).Mishap.handleOutcome(currentState);
      }
    }

    public class QualificationModifierForSpecifiedCareers : Event
    {
      public List<string> careerNames = new List<string>();
      public int modifier = 0;

      [JsonConstructor]
      public QualificationModifierForSpecifiedCareers()
      {
      }

      public QualificationModifierForSpecifiedCareers(
        List<string> careers,
        int mod,
        string description)
        : base("Qual Mods", description)
      {
        if (careers != null)
          this.careerNames = careers;
        this.modifier = mod;
      }

      public QualificationModifierForSpecifiedCareers(string career, int mod, string description)
        : base(career + " qualification modifier", description)
      {
        this.careerNames.Add(career);
        this.modifier = mod;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        if (this.careerNames.Count <= 0)
          return;
        foreach (string careerName in this.careerNames)
        {
          int modifier = this.modifier;
          if (currentState.qualificationModifiers.ContainsKey(careerName))
            modifier += currentState.qualificationModifiers[careerName];
          currentState.qualificationModifiers[careerName] = modifier;
        }
      }
    }

    public abstract class ActionsOnQualification : Event
    {
      protected bool installed = false;
      public bool delete = false;

      [JsonConstructor]
      public ActionsOnQualification()
      {
      }

      public ActionsOnQualification(string description)
        : base("Qualifying Action", description)
      {
      }

      public abstract bool performActionsOnQualification(GenerationState currentState);

      public ActionsOnQualification(string name, string description)
        : base(name, description)
      {
      }

      public override void handleOutcome(GenerationState currentState)
      {
        if (!this.installed)
        {
          EngineLog.Print("Installing [" + this.Name + "] to ActionsOnQualifications");
          currentState.actionsOnQualifications.Add(this);
          this.installed = true;
        }
        else
        {
          EngineLog.Print("Firing actions on qualification [" + this.Name + "]");
          this.delete = this.performActionsOnQualification(currentState);
        }
      }
    }

    public class AutomaticCommissionIfNextCareerMilitary : Event.ActionsOnQualification
    {
      [JsonConstructor]
      public AutomaticCommissionIfNextCareerMilitary()
      {
      }

      public AutomaticCommissionIfNextCareerMilitary(string name, string description)
        : base(name, description)
      {
      }

      public override bool performActionsOnQualification(GenerationState currentState)
      {
        bool flag = true;
        if (!currentState.character.CurrentTerm.officer && currentState.currentCareer != null && currentState.currentCareer.hasCommissions() && currentState.currentCareer.Category.ToLower().Contains("military"))
        {
          Outcome.AutomaticPromotionOrCommission promotionOrCommission = new Outcome.AutomaticPromotionOrCommission();
          promotionOrCommission.apocChoice = Outcome.AutomaticPromotionOrCommission.Choice.COMMISION_ONLY;
          promotionOrCommission.Description = this.Description;
          promotionOrCommission.Name = this.Name;
          promotionOrCommission.AdditionalSkill = false;
          promotionOrCommission.handleOutcome(currentState);
        }
        return flag;
      }
    }

    public class AttemptCommisionFirstIfNextCareerMilitary : Event.ActionsOnQualification
    {
      public int modifier = 0;

      [JsonConstructor]
      public AttemptCommisionFirstIfNextCareerMilitary()
      {
      }

      public AttemptCommisionFirstIfNextCareerMilitary(string description)
        : base(description)
      {
      }

      public AttemptCommisionFirstIfNextCareerMilitary(int commision_modifier, string description)
        : base(description)
      {
        this.modifier = commision_modifier;
      }

      public override bool performActionsOnQualification(GenerationState currentState)
      {
        bool flag = true;
        Career currentCareer = currentState.currentCareer;
        if (!currentState.character.CurrentTerm.officer && currentCareer != null && currentCareer.Category.ToLower().Contains("military") && currentCareer.hasCommissions())
        {
          currentState.decisionMaker.present(new Presentation("Attempting Commission Before Enrollment!", this.Description + (this.modifier != 0 ? "\n Commision Bonus: " + this.modifier.ToString() : "")));
          IList<Outcome> outcomeList = currentState.currentCareer.qualifyForCommision(currentState.character, this.modifier);
          if (outcomeList != null && outcomeList.Count > 0)
          {
            foreach (Outcome outcome in (IEnumerable<Outcome>) outcomeList)
              outcome.handleOutcome(currentState);
          }
        }
        else if (currentCareer == null)
          EngineLog.Error("There was NO Career when performing AttemptCommisionFirstIfNextCareerMilitary");
        return flag;
      }
    }

    public class RollEventFromSpecifiedCareer : Event
    {
      public string careerName;

      [JsonConstructor]
      public RollEventFromSpecifiedCareer()
      {
      }

      public RollEventFromSpecifiedCareer(
        string careerName,
        string eventName,
        string eventDescription)
        : base(eventName, eventDescription)
      {
        this.careerName = careerName;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        (currentState.careerSource.GetCareer(this.careerName) ?? throw new NullReferenceException("specified career [" + this.careerName + "] didn't exist in the careerSource")).Event.handleOutcome(currentState);
      }
    }

    public class GainTrait : Event
    {
      public Trait trait { get; set; }

      [JsonConstructor]
      public GainTrait()
      {
      }

      public GainTrait(Trait gainMe)
      {
        this.trait = gainMe;
        this.Description = "You posses the " + this.trait.Name + " trait.";
        this.Name = "Gain " + this.trait.Name + " trait.";
      }

      public GainTrait(Trait gainMe, string eventName, string eventDescription)
        : base(eventName, eventDescription)
      {
        this.trait = gainMe;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.character.addTrait(this.trait);
        currentState.recorder.RecordBenefit((Event) this, currentState);
      }
    }

    public class TestForTalent : Event
    {
      public Trait Trait { get; set; }

      public int TimeInWeeks { get; set; }

      public int Cost { get; set; }

      [JsonConstructor]
      public TestForTalent()
      {
      }

      public TestForTalent(Trait trait) => this.Trait = trait;

      public override void handleOutcome(GenerationState currentState)
      {
        Attribute attribute = currentState.character.getAttribute(this.Trait.associatedAttribute);
        if (attribute == null)
        {
          attribute = new Attribute(this.Trait.associatedAttribute, this.Trait.ordinal);
          attribute.InitializeValue(Dice.D6Roll(2, (currentState.character.TotalNumberOfTerms - 1) * this.Trait.attribute_modifier_per_term_not_possessed, "Testing for your " + this.Trait.Name + " potential. " + (currentState.character.TotalNumberOfTerms > 0 ? "For every term this potential has been untapped, it's modified by " + this.Trait.attribute_modifier_per_term_not_possessed.ToString() : "")).effect);
          IList<Attribute> attributes = currentState.character.getAttributes();
          if (attribute.Value < 0)
            attribute.InitializeValue(0);
          attributes.Add(attribute);
        }
        string str1;
        if (attribute.Value > 12)
        {
          str1 = " an enormous amount ";
        }
        else
        {
          switch (attribute.Value)
          {
            case 1:
            case 2:
            case 3:
              str1 = " barely any, but some amount ";
              break;
            case 4:
            case 5:
              str1 = " a small but important amount ";
              break;
            case 6:
            case 7:
            case 8:
              str1 = " an average amount ";
              break;
            case 9:
              str1 = " a significant amount ";
              break;
            case 10:
            case 11:
            case 12:
              str1 = " a great amount ";
              break;
            default:
              str1 = " absolutely no amount ";
              break;
          }
        }
        if (attribute.Value > 0)
        {
          string str2 = "After testing you've found that you have " + str1 + " of " + this.Trait.Name + " potential that you've only just begun to understand.";
          currentState.decisionMaker.present(new Presentation("Testing Results", str2));
          if (!currentState.character.hasTrait(this.Trait))
            new Event.GainTrait(this.Trait, this.Trait.Name + " gained", str2).handleOutcome(currentState);
          Event.TrainForTalent trainForTalent = new Event.TrainForTalent();
          trainForTalent.Trait = this.Trait;
          trainForTalent.cumulative_dm_per_check = -1;
          trainForTalent.handleOutcome(currentState);
        }
        else
        {
          Event aevent = new Event("Failed test for " + this.Trait.Name, "Unfortunately, you weren't able to find any " + this.Trait.Name + " potential within you.");
          currentState.recorder.RecordBenefit(aevent, currentState);
        }
      }
    }

    public class TrainForTalent : Event.TestForTalent
    {
      public int cumulative_dm_per_check { get; set; }

      [JsonConstructor]
      public TrainForTalent()
      {
      }

      private bool HasBeenTrained(Character character)
      {
        return character.Talents.Any<Talent>((Func<Talent, bool>) (i => i.associatedTrait != null));
      }

      public override void handleOutcome(GenerationState currentState)
      {
        if (!this.HasBeenTrained(currentState.character))
        {
          if (currentState.character.hasTrait(this.Trait))
          {
            currentState.character.Credits -= this.Cost;
            string[] strArray = new string[6]
            {
              this.Description,
              "You spend ",
              null,
              null,
              null,
              null
            };
            int num = this.Cost;
            strArray[2] = num.ToString();
            strArray[3] = " credits attempting to gain mastery of your ";
            strArray[4] = this.Trait.Name;
            strArray[5] = " potential.\n";
            this.Description = string.Concat(strArray);
            string description1 = this.Description;
            num = this.TimeInWeeks;
            string str = num.ToString();
            this.Description = description1 + "You train across the span of " + str + " weeks.\n";
            new Outcome.InformUser(this.Trait.Name + " Training", "Now that you've discovered the potential within you, " + this.Trait.Name.ToLower() + " training can commence. This is your opportunity to open new horizons and learn each of the basic " + this.Trait.Name.ToLower() + " talents. Each talent may only be attempted once, and each attempt gets more and more difficult. Additionally, not all talents are equally easy to learn, so pay close attention to the talent difficulties.  Good luck!").handleOutcome(currentState);
            List<DescribableContainer<Talent>> dc_talents = new List<DescribableContainer<Talent>>();
            foreach (Talent talent in currentState.skillSource.getTalentsForTrait(this.Trait.Name))
            {
              if (!currentState.character.hasTalent(talent))
                dc_talents.Add(new DescribableContainer<Talent>(talent.Name, talent));
            }
            int targetNumber = 8;
            while (dc_talents.Count > 0)
            {
              for (int i = 0; i < dc_talents.Count; i++)
              {
                if (targetNumber - currentState.character.getAttributeModifier(this.Trait.associatedAttribute) - dc_talents[i].Value.learning_dm > 12)
                {
                  dc_talents.RemoveAt(i);
                  --i;
                }
                else
                {
                  string description2 = DataManager.Instance.Talents.Select<Talent, Talent>((Func<Talent, Talent>) (t => t.Clone<Talent>())).ToList<Talent>().FirstOrDefault<Talent>((Func<Talent, bool>) (t => t.Name == dc_talents[i].Name)).Description;
                  dc_talents[i].SetDescription("Your likelyhood of learning this talent is [" + Dice.ProbabilityPercent(new RollParam(currentState.character.getAttribute(this.Trait.associatedAttribute), targetNumber, dc_talents[i].Value.learning_dm, "")).ToString() + "%].\n" + description2);
                }
              }
              bool flag = !currentState.character.Talents.Any<Talent>();
              if (dc_talents.Count > 0)
              {
                string description3 = dc_talents.Count == dc_talents.Count ? "Which talent would you like to try to train first?  Each successive try is at a cumulative " + this.cumulative_dm_per_check.ToString() + "." : "Which talent would you like to try to train next?";
                DescribableContainer<Talent> describableContainer = currentState.decisionMaker.ChooseOne<DescribableContainer<Talent>>(description3, (IList<DescribableContainer<Talent>>) dc_talents);
                if (flag && describableContainer.Name.ToLowerInvariant().Contains("telepathy"))
                {
                  currentState.character.addTalent(describableContainer.Value);
                  new Outcome.InformUser("Gain Telepathy", "Because you chose telepathy first, you automatically learn it. Congratulations!").handleOutcome(currentState);
                }
                else
                {
                  RollParam setup = new RollParam(currentState.character.getAttribute(this.Trait.associatedAttribute), targetNumber, describableContainer.Value.learning_dm, "Attempting to gain " + describableContainer.Name);
                  string successMessage = "You did it! You learned " + describableContainer.Name;
                  string failureMessage = "It just didn't come to you.  " + describableContainer.Name + " is currently beyond your grasp.";
                  RollEffect rollEffect = Dice.Roll(setup, successMessage, failureMessage);
                  this.Description += rollEffect.isSuccessful ? successMessage : failureMessage;
                  if (rollEffect.isSuccessful)
                  {
                    describableContainer.Value.Level = 0;
                    currentState.character.addTalent(describableContainer.Value);
                  }
                }
                dc_talents.Remove(describableContainer);
                targetNumber -= this.cumulative_dm_per_check;
              }
            }
            List<Career> source = currentState.careerSource.Careers.Where<Career>((Func<Career, bool>) (c => c.getRequiredTraits().Contains(this.Trait))).ToList<Career>() ?? new List<Career>();
            new Outcome.InformUser("What's Next?", "Now that you have tested and trained your " + this.Trait.Name + " potential, " + (source.Count != 0 ? (source.Count > 0 ? " new careers become open to you." : "the " + source.FirstOrDefault<Career>().Name + "Career becomes open to you. ") : "who knows what the world has in store for you? ") + "If your GM allows, and you choose to pursue your talents further, you can strengthen what you've learned, and potentially learn new ones. Either way, your world has just gotten a whole lot bigger.").handleOutcome(currentState);
          }
          else
          {
            this.Description = "You couldn't train to be " + this.Trait.Name + " because you didn't possess the potential.";
            new Outcome.InformUser("Unable to Train", this.Description).handleOutcome(currentState);
          }
          currentState.recorder.RecordBenefit((Event) this, currentState);
        }
        else
        {
          new Outcome.InformUser("Already Trained Psionics", "You have already trained Psionics, you will gain a new contact from the Psionic Institute.").handleOutcome(currentState);
          new Outcome.GainSpecifiedContact("Psionic Mentor").handleOutcome(currentState);
        }
      }
    }

    public class ChallengeWithCriticalFailure : Event
    {
      [JsonProperty]
      public Event.Challenge wrappedChallenge = (Event.Challenge) null;
      [JsonProperty]
      public int critical_failure_threshold = 2;
      [JsonProperty]
      public Outcome[] critical_failure_outcomes = (Outcome[]) null;

      [JsonConstructor]
      public ChallengeWithCriticalFailure()
      {
      }

      public ChallengeWithCriticalFailure(
        Event.Challenge wrapMe,
        int critical_failure_threshold,
        params Outcome[] critFailureOutcomes)
      {
        this.critical_failure_threshold = critical_failure_threshold;
        this.wrappedChallenge = wrapMe;
        this.critical_failure_outcomes = critFailureOutcomes;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        this.Description = this.Description + "  On a " + this.critical_failure_threshold.ToString() + " or less, you risk ";
        for (int index = 0; index < this.critical_failure_outcomes.Length; ++index)
        {
          if (this.critical_failure_outcomes.Length > 2 && index > 0)
            this.Description += ", ";
          if (index == this.critical_failure_outcomes.Length - 1 && this.critical_failure_outcomes.Length > 1)
            this.Description += "and ";
          this.Description += this.critical_failure_outcomes[index].Name;
        }
        this.wrappedChallenge.handleOutcome(currentState);
        if (this.wrappedChallenge.challengeResult.rawResult > this.critical_failure_threshold)
          return;
        foreach (Outcome criticalFailureOutcome in this.critical_failure_outcomes)
          criticalFailureOutcome.handleOutcome(currentState);
        currentState.recorder.RecordBenefit((Event) this, currentState);
      }
    }

    public class SetAnagathicsState : Event
    {
      [JsonProperty]
      public bool on_anagathics = false;

      [JsonConstructor]
      public SetAnagathicsState()
      {
      }

      public SetAnagathicsState(string name, string description, bool on_anagathics)
        : base(name, description)
      {
        this.on_anagathics = on_anagathics;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        if (this.on_anagathics)
        {
          currentState.character.OnAnagathics = true;
          currentState.character.NumberOfYearsOnAnagathics += 4;
          ++currentState.character.NumberOfTermsOnAnagathics;
          int credits = Dice.RawRollResult(1) * -200000;
          Outcome.GainMoney gainMoney = new Outcome.GainMoney(credits);
          gainMoney.Name = "Paid " + credits.ToString() + "cr for Anagathics";
          gainMoney.Description = "You found a 4 year anagathics supply, but it cost you " + credits.ToString() + "credits.";
          gainMoney.handleOutcome(currentState);
        }
        else
          currentState.character.OnAnagathics = false;
        currentState.recorder.RecordBenefit((Event) this, currentState);
        currentState.decisionMaker.present(new Presentation((IDescribable) this));
      }
    }

    public class GainSkillsAtDifferentLevels : Event
    {
      [JsonProperty]
      protected int[] skill_levels = (int[]) null;
      [JsonProperty]
      protected string[] skills = (string[]) null;
      [JsonProperty]
      protected bool mutually_exclusive = false;
      [JsonProperty]
      protected Dictionary<int, int> skillHistogram = new Dictionary<int, int>();

      [JsonConstructor]
      public GainSkillsAtDifferentLevels()
      {
      }

      public GainSkillsAtDifferentLevels(int[] levels, string[] skills, string description)
        : base("Gain Skills", description)
      {
        Array.Sort<int>(levels);
        this.skill_levels = levels;
        this.skills = skills;
        this.buildSkillHistogram();
      }

      protected void buildSkillHistogram()
      {
        this.skillHistogram.Clear();
        foreach (int skillLevel in this.skill_levels)
        {
          int num1 = 0;
          if (this.skillHistogram.ContainsKey(skillLevel))
            num1 = this.skillHistogram[skillLevel];
          int num2;
          this.skillHistogram[skillLevel] = num2 = num1 + 1;
          foreach (int key in this.skillHistogram.Keys)
            this._description = this._description + "Gain " + this.skillHistogram[key].ToString() + " skills at " + key.ToString() + ". ";
        }
      }

      public override void handleOutcome(GenerationState currentState)
      {
        List<ISkill> skillList1 = new List<ISkill>();
        List<ISkill> skillList2 = new List<ISkill>();
        foreach (string skill1 in this.skills)
        {
          ISkill basicTrainingSkill = currentState.skillSource.getBasicTrainingSkill(skill1);
          ISkill skill2 = basicTrainingSkill.Parent != null ? currentState.skillSource.getBasicTrainingSkill(basicTrainingSkill.Parent.Name) : basicTrainingSkill;
          skillList1.Add(basicTrainingSkill);
          ISkill skill3 = basicTrainingSkill.Id != skill2.Id ? skill2 : basicTrainingSkill;
          if (!skillList2.Contains(skill3))
            skillList2.Add(skill3);
        }
        foreach (int key in this.skillHistogram.Keys)
        {
          int numToChoose = this.skillHistogram[key];
          string str = numToChoose == 1 ? "" : "s";
          if (skillList1.Count != 0)
          {
            IList<ISkill> skillList3 = currentState.decisionMaker.Choose<ISkill>("Pick " + numToChoose.ToString() + " skill" + str + " at level " + key.ToString(), numToChoose, (IList<ISkill>) (key == 0 ? skillList2 : skillList1));
            if (this.mutually_exclusive)
            {
              foreach (ISkill skill in (IEnumerable<ISkill>) skillList3)
                skillList1.Remove(skill);
            }
            foreach (IDescribable describable in (IEnumerable<ISkill>) skillList3)
              new Outcome.EnsureSkillAtLevel(describable.Name, key).handleOutcome(currentState);
          }
        }
      }
    }

    public class IncreaseLastSkillsObtained : Event
    {
      [JsonProperty]
      protected int num_of_skills = 0;
      [JsonProperty]
      protected int skill_modifier = 0;

      public IncreaseLastSkillsObtained(int num_of_skills, int amount, string description)
        : base("Improve Skills", description)
      {
        this.num_of_skills = num_of_skills;
        this.skill_modifier = amount;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        List<Outcome> list = currentState.character.CurrentTerm.benefits.Where<Outcome>((Func<Outcome, bool>) (skl => skl is Outcome.EnsureSkillAtLevel)).ToList<Outcome>().Take<Outcome>(this.num_of_skills).ToList<Outcome>();
        if (list.Count <= 0)
          return;
        foreach (Outcome outcome in list)
        {
          for (int index = 0; index < this.skill_modifier; ++index)
            new Outcome.GainSkill(((Outcome.EnsureSkillAtLevel) outcome).Text).handleOutcome(currentState);
        }
      }
    }

    public class SwapEquipment : Outcome.AddEquipment
    {
      public IEquipment RemoveMe { get; set; }

      public SwapEquipment(
        string Name,
        string Description,
        IEquipment replaceMe,
        IEquipment addMe)
        : base(addMe)
      {
        this.Name = Name;
        this.Description = Description;
        this.RemoveMe = replaceMe;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        currentState.character.removeEquipment(this.RemoveMe);
        base.handleOutcome(currentState);
      }
    }

    public class TestRecentlyAcquiredSkill : Event.Challenge
    {
      public int number_of_skills = 2;

      public TestRecentlyAcquiredSkill(
        string name,
        string description,
        int number_of_skills,
        int target)
        : base(name, description)
      {
        this.number_of_skills = number_of_skills;
        this.targetNumber = target;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        EngineLog.Print("TestRecentlyAcquiredSkil.handleOutcome() entered");
        List<ISkill> skillList = new List<ISkill>();
        List<ISkill> source = new List<ISkill>((IEnumerable<ISkill>) currentState.character.Skills);
        for (int index = 0; source.Count > 0 && index < this.number_of_skills; ++index)
        {
          skillList.Add(source.Last<ISkill>());
          source.Remove(source.Last<ISkill>());
        }
        EngineLog.Print("Skill Selection Set[" + skillList.Count.ToString() + "] " + skillList.Select<ISkill, string>((Func<ISkill, string>) (sk => sk.Name + " ")).ToArray<string>()?.ToString());
        if (skillList.Count > 0)
        {
          if (skillList.Count > 1)
          {
            EngineLog.Print("Call to DecisionMaker.ChooseOne");
            this.skillName = currentState.decisionMaker.ChooseOne<ISkill>("Which skill would you like to test?", (IList<ISkill>) skillList).Name;
          }
          else
            this.skillName = skillList.Last<ISkill>().Name;
          EngineLog.Print("Skill Chosen: " + this.skillName);
          if (this.successOutcomes == null)
            this.successOutcomes = new List<Outcome>();
          this.successOutcomes.Add((Outcome) new Outcome.GainSkill(this.skillName));
          this.successOutcomes.Insert(0, (Outcome) new Outcome.InformUser("Success!", "You did it. " + this.skillName + " will be increased by 1"));
          if (this.failureOutcomes == null)
            this.failureOutcomes = new List<Outcome>();
          this.failureOutcomes.Insert(0, (Outcome) new Outcome.InformUser("Failure", "You tried, but you just couldn't pull it off."));
          base.handleOutcome(currentState);
        }
        else
          currentState.decisionMaker.present(new Presentation("Nothing Learned!", "Since nothing has been recently learned, it isn't possible to test it.  Sorry"));
        EngineLog.Print("TestRecentlyAcquiredSkil.handleOutcome() exited");
      }
    }
  }
}
