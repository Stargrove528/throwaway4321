
// Type: com.digitalarcsystems.Traveller.HierateCharacterCreationAlgorithm




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class HierateCharacterCreationAlgorithm : AbstractCharacterCreationAlgorithm
  {
    public HierateCharacterCreationAlgorithm()
    {
      this.drifter = (Career) null;
      this.SetCareerFilter((ICareerFilter) new CategoryStringFilter("aslan", "outcast", CategoryStringFilter.FilterBehavior.MUST_INCLUDE));
    }

    protected internal override GenerationState createInitialCharacterGenerationState()
    {
      Character aslanBlankCharacter = this.CreateHierateAslanBlankCharacter();
      return new GenerationState(DataManager.UserID)
      {
        engineID = this.GetType().Name,
        character = aslanBlankCharacter,
        nextOperation = CreationOperation.HIERATE_CHOOSE_GENDER,
        previousState = (GenerationState) null,
        peopleSource = (IPeopleSource) this,
        recorder = (IBenefitRecorder) this,
        skillSource = (ISkillSource) this,
        draftCareers = this.draftCareers,
        decisionMaker = this.theDecider
      };
    }

    private Character CreateHierateAslanBlankCharacter()
    {
      Race race = new Race(this.races.Where<Race>((Func<Race, bool>) (r => r.Id == new Guid("eacd6065-b96f-4adb-b9da-263c25775340"))).First<Race>())
      {
        Id = new Guid("51c8863d-65c1-4f7c-8bca-9b7bc56cb1f5"),
        Name = "Hierate Aslan"
      };
      race.Characteristics = race.Characteristics.Where<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (a => !a.Name.ToLowerInvariant().Equals("soc") && !a.Name.ToLowerInvariant().Equals("ter"))).ToList<com.digitalarcsystems.Traveller.DataModel.Attribute>();
      if (race == null)
        throw new InvalidOperationException("Need to have core");
      Character blankCharacter = Character.BlankCharacter;
      blankCharacter.Race = race;
      blankCharacter.getAttributes().Remove(blankCharacter.getAttributes().FirstOrDefault<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (a => a.Name.ToLowerInvariant().Equals("soc"))));
      blankCharacter.Age = 16;
      Character character = blankCharacter;
      Weapon weapon = new Weapon("Dew Claw");
      weapon.Damage = 1;
      weapon.DamageModifier = 2;
      weapon.Skill = "Melee";
      weapon.SubSkill = "Natural";
      weapon.AllowedUpgradeCategories = new List<string>()
      {
        "unarmed, natural"
      };
      character.Unarmed = (IWeapon) weapon;
      return blankCharacter;
    }

    public override GenerationState processBeforeNextOperation(GenerationState currentState)
    {
      GenerationState generationState;
      switch (currentState.nextOperation)
      {
        case CreationOperation.HIERATE_CHOOSE_GENDER:
          generationState = this.handleGenderRoll(currentState);
          break;
        case CreationOperation.HIERATE_DETERMINE_ANCESTRAL_HISTORY:
          generationState = this.determineAncestralHistory(currentState);
          break;
        case CreationOperation.HIERATE_RIGHT_OF_PASSAGE:
          generationState = this.handleRightOfPassage(currentState);
          break;
        case CreationOperation.HIERATE_ALLOCATE_CLAN_SHARES:
          generationState = this.handleAllocateClanShares(currentState);
          break;
        default:
          generationState = currentState;
          break;
      }
      return generationState;
    }

    public virtual GenerationState handleGenderRoll(GenerationState currentState)
    {
      Describable describable1 = new Describable("Choose Gender", "Within the Aslan Race gender plays a role in determining what paths a Traveller can take.  What gender will your character be?");
      Describable describable2 = new Describable("Random", "Let's stick with what the book says and roll up gender randomly.");
      Describable describable3 = new Describable("Male", "This character should be male.");
      Describable describable4 = new Describable("Female", "This character should be female.");
      List<Describable> choices = new List<Describable>()
      {
        describable2,
        describable3,
        describable4
      };
      Describable describable5 = currentState.decisionMaker.ChooseOne<Describable>(describable1.Description, (IList<Describable>) choices);
      bool flag = describable5 != describable2 ? describable5 == describable4 : Dice.D6Roll(2, 0, "Roll For Gender (2-6 Male, 7-12 Female)").rawResult >= 7;
      currentState.character.Gender = !flag ? new Gender("Male", Gender.PronounType.MALE) : new Gender("Female", Gender.PronounType.FEMALE);
      return currentState.createNextCharacterGenerationState(CreationOperation.HIERATE_DETERMINE_ANCESTRAL_HISTORY);
    }

    public virtual GenerationState determineAncestralHistory(GenerationState currentState)
    {
      int num1 = 0;
      bool flag1 = currentState.character.Gender.name.ToLowerInvariant().Contains("fe");
      EngineLog.Print("DetermineAncestralHistory: is_female: " + flag1.ToString());
      string skill = flag1 ? "Profession" : "Independence";
      string description1 = "We will now roll to see if your clan is a major one or a minor one.  On a 1 - 3 Minor Clan, on a 4-6 Major Clan.";
      currentState.decisionMaker.setQueryKey(ContextKeys.HIERATE_CLAN_ROLL);
      currentState.decisionMaker.present(new Presentation("CLAN AFFILIATION ROLL", description1));
      bool flag2 = Dice.D6Roll(1, 0, "Clan Status Die").rawResult > 3;
      string description2 = flag2 ? "You are a member of a major clan.  You will receieve a + 1 on your ancestral deeds roll. Don't embarass your clan." : "You are a member of a minor clan. Your clan looks to you for great deads to improve its status and territory.";
      currentState.decisionMaker.present(new Presentation(flag2 ? "Your are of The Best Clans" : "Lesser Clan", description2)
      {
        isResult = true
      });
      currentState.decisionMaker.setQueryKey(ContextKeys.HIERATE_ANCESTOR_ROLL);
      currentState.decisionMaker.present(new Presentation("Roll for Ancestral Deeds", "You are not born alone in the universe. The make up of your being is the collection of deeds that have come before you.  Your clan's territory depend on the deeds of your ancestors, grandfather, and father.  We will roll for your ancestor's deeds now."));
      RollEffect rollEffect1 = Dice.D6Roll(1, flag2 ? 1 : 0, "Ancestral Deeds");
      string description3 = "";
      switch (rollEffect1.totalResult)
      {
        case 1:
          description3 = "Your ancestor shamed the clan, and you come from a branch long dishonoured. +0 Ancestral Territory";
          break;
        case 2:
          description3 = "Your family’s glory days are long gone, all that is left is the tales of great landholdings now lost to upstarts. +0 Ancestral Territory";
          break;
        case 3:
          description3 = "Your family made its fortune in the great expansion after the discovery of jump drive; most family holdings are on distant worlds. +1 Ancestral Territory";
          ++num1;
          break;
        case 4:
          description3 = "Your family are the descendants of an ancient hero forgotten by most Aslan. +1 Ancestral Territory";
          ++num1;
          break;
        case 5:
          description3 = "Your family’s ancestor was a trickster who deceived his enemies. +2 Ancestral Territory";
          num1 += 2;
          break;
        case 6:
          description3 = "Your ancestors were conquerors and great warriors. +2 Ancestral Territor";
          num1 += 2;
          break;
        case 7:
          description3 = "Your family is one of the most influential and wealthy in the Hierate. +3 Ancestral Territory";
          num1 += 3;
          break;
      }
      currentState.decisionMaker.present(new Presentation("Ancestral Result", description3)
      {
        value = num1,
        isResult = true
      });
      currentState.decisionMaker.setQueryKey(ContextKeys.HIERATE_GRANDFATHER_ROLL);
      currentState.decisionMaker.present(new Presentation("Rolling Grandfather's Deeds", "We will now roll for your grandfather's deeds"));
      RollEffect rollEffect2 = Dice.D6Roll(2, 0, "Grandfather's Deeds Roll");
      string description4 = "";
      switch (rollEffect2.totalResult)
      {
        case 2:
          description4 = "Dishonoured! Your grandfather committed some dishonourable act that caused the clan to strip your family of all territory. Gain Independence 0 (if male) or Profession 0 (if female). Lose all Ancestral Territory";
          num1 = 0;
          new Outcome.EnsureSkillAtLevel(skill, 0).handleOutcome(currentState);
          break;
        case 3:
          description4 = "Your grandfather was beset by many foes, one of whom conquered much of your land. Gain an Enemy and Gun Combat 0. -4 Ancestral Territory";
          num1 -= 4;
          new Outcome.EnsureSkillAtLevel("Gun Combat", 0).handleOutcome(currentState);
          new Outcome.GainSpecifiedEnemy("Aslan Conqueror - grandfather").handleOutcome(currentState);
          break;
        case 4:
          description4 = "Your grandfather was a fool who gambled away much of your land. Gain Gamble 0 or Carouse 0. -3 Ancestral Territory";
          num1 -= 3;
          new Event.ChoiceOutcome(1, "Gain Without Fighting", "There are ways to smooth your journey.", new Outcome[2]
          {
            (Outcome) new Event.SingleOutcome("Learn to Gamble", "By choosing good bets, you can make your life better.", (Outcome) new Outcome.EnsureSkillAtLevel("Gambler", 0)),
            (Outcome) new Event.SingleOutcome("Learn to Shmooze", "You learn to move through the informal side of Aslan society.", (Outcome) new Outcome.EnsureSkillAtLevel("Carouse", 0))
          }).handleOutcome(currentState);
          break;
        case 5:
          description4 = "Your grandfather suffered from a degenerative genetic disease that you may have inherited. Gain Medic 0. -2 Ancestral Territory";
          num1 -= 2;
          new Outcome.EnsureSkillAtLevel("Medic", 0).handleOutcome(currentState);
          break;
        case 6:
          description4 = "Your grandfather barely managed to hold onto your landhold. -1 Ancestral Territory";
          --num1;
          break;
        case 7:
          description4 = "Your grandfather held onto your landhold. +1 Ancestral Territory";
          ++num1;
          break;
        case 8:
          description4 = "Your grandfather added some land to your landhold. +2 Ancestral Territory";
          num1 += 2;
          break;
        case 9:
          description4 = "Your grandfather added notably to your landhold. +3 Ancestral Territory";
          num1 += 3;
          break;
        case 10:
          description4 = "Your grandfather was politically adept. +4 Ancestral Territory";
          num1 += 4;
          break;
        case 11:
          description4 = "Your grandfather added a great deal of land to your landhold. +5 Ancestral Territory";
          num1 += 5;
          break;
        case 12:
          description4 = "Your grandfather was a conqueror. +6 Ancestral Territory";
          num1 += 6;
          break;
      }
      if (num1 < 0)
        num1 = 0;
      currentState.decisionMaker.present(new Presentation("Grandfather's History", description4)
      {
        value = num1,
        isResult = true
      });
      currentState.decisionMaker.setQueryKey(ContextKeys.HIERATE_FATHER_ROLL);
      string description5 = "We will now roll for your father's deeds";
      currentState.decisionMaker.present(new Presentation("The deeds of the Father...", description5));
      int rawResult = Dice.D6Roll(2, 0, "Roll For Father's Deeds (higher = better)").rawResult;
      string description6 = "";
      switch (rawResult)
      {
        case 2:
          description6 = "Dishonoured! Your father committed some dishonourable act that caused the clan to strip your family of all territory. Gain Independence 0 (if male) or Profession 0 (if female). Lose all Ancestral Territory";
          num1 = 0;
          new Outcome.EnsureSkillAtLevel(skill, 0).handleOutcome(currentState);
          break;
        case 3:
          description6 = "Your father was beset by many foes, one of whom conquered much of your land. Gain an Enemy and Gun Combat 0. -4 Ancestral Territory";
          num1 -= 4;
          new Outcome.EnsureSkillAtLevel("Gun Combat", 0).handleOutcome(currentState);
          new Outcome.GainSpecifiedEnemy("Aslan Conqueror - father").handleOutcome(currentState);
          break;
        case 4:
          description6 = "Your father was a fool who gambled away much of your land. Gain Gamble 0 or Carouse 0. -3 Ancestral Territory";
          num1 -= 3;
          new Event.ChoiceOutcome(1, "Gain Without Fighting", "There are ways to smooth your journey.", new Outcome[2]
          {
            (Outcome) new Event.SingleOutcome("Learn to Gamble", "By choosing good bets, you can make your life better.", (Outcome) new Outcome.EnsureSkillAtLevel("Gambler", 0)),
            (Outcome) new Event.SingleOutcome("Learn to Shmooze", "You learn to move through the informal side of Aslan society.", (Outcome) new Outcome.EnsureSkillAtLevel("Carouse", 0))
          }).handleOutcome(currentState);
          break;
        case 5:
          description6 = "Your father suffered from a degenerative genetic disease that you may have inherited. Gain Medic 0. -2 Ancestral Territory";
          num1 -= 2;
          new Outcome.EnsureSkillAtLevel("Medic", 0).handleOutcome(currentState);
          break;
        case 6:
          description6 = "Your father barely managed to hold onto your landhold. -1 Ancestral Territory";
          --num1;
          break;
        case 7:
          description6 = "Your father held onto your landhold. +1 Ancestral Territory";
          ++num1;
          break;
        case 8:
          description6 = "Your father added some land to your landhold. +2 Ancestral Territory";
          num1 += 2;
          break;
        case 9:
          description6 = "Your father added notably to your landhold. +3 Ancestral Territory";
          num1 += 3;
          break;
        case 10:
          description6 = "Your father was politically adept. +4 Ancestral Territory";
          num1 += 4;
          break;
        case 11:
          description6 = "Your father added a great deal of land to your landhold. +5 Ancestral Territory";
          num1 += 5;
          break;
        case 12:
          description6 = "Your father was a conqueror. +6 Ancestral Territory";
          num1 += 6;
          break;
      }
      if (num1 < 0)
        num1 = 0;
      currentState.decisionMaker.present(new Presentation("Father's History", description6)
      {
        value = num1,
        isResult = true
      });
      Character currentCharacter = this.currentCharacter;
      currentCharacter.Notes = currentCharacter.Notes + "\nCLAN HISTORY:\n\t" + description3 + "\n\t" + description4 + "\n\t" + description6 + "\n\tCLAN TERRITORY: " + num1.ToString();
      IList<com.digitalarcsystems.Traveller.DataModel.Attribute> attributes1 = currentState.character.getAttributes();
      com.digitalarcsystems.Traveller.DataModel.Attribute attribute1 = new com.digitalarcsystems.Traveller.DataModel.Attribute("Soc", 5);
      attribute1.InitializeValue(num1);
      attributes1.Add(attribute1);
      currentState.decisionMaker.setQueryKey(ContextKeys.HIERATE_BIRTH_ORDER_ROLL);
      currentState.decisionMaker.present(new Presentation("Birth Order", "Birth order is important to the Aslan.  The first male always inherits the Territory of his forbearers.  The first female is always the most important.  We'll now determine your birth order."));
      int totalResult = Dice.D6Roll(2, 0, "Determine Your Birth Order & Inheritance.  (Lower is better).").totalResult;
      int num2;
      string description7;
      if (totalResult <= 3)
      {
        num2 = 1;
        description7 = !flag1 ? "You are the first born son of your family, and in time will inherit the family land." : "You are the eldest daughter your family.";
      }
      else if (totalResult <= 10)
      {
        num2 = 2;
        description7 = !flag1 ? "You are the second born son of your family." : "You are the middle daughter your family.";
      }
      else
      {
        num2 = 3;
        description7 = !flag1 ? "You are the third born son of your family." : "You are the youngest daughter your family.";
      }
      currentState.character.Notes += description7;
      int num3 = 0;
      if (num2 == 1 && !flag1)
        num3 = num1;
      currentState.decisionMaker.present(new Presentation("Birth Order Results", description7)
      {
        value = num3,
        isResult = true
      });
      if (!flag1)
      {
        IList<com.digitalarcsystems.Traveller.DataModel.Attribute> attributes2 = currentState.character.getAttributes();
        com.digitalarcsystems.Traveller.DataModel.Attribute attribute2 = new com.digitalarcsystems.Traveller.DataModel.Attribute("Ter", 7);
        attribute2.InitializeValue(num3);
        attributes2.Add(attribute2);
      }
      currentState.decisionMaker.present(new Presentation("Ready To Move to Rite of Passage", "")
      {
        isResult = false
      });
      return currentState.createNextCharacterGenerationState(CreationOperation.HIERATE_RIGHT_OF_PASSAGE);
    }

    public virtual GenerationState handleRightOfPassage(GenerationState currentState)
    {
      bool flag = currentState.character.Gender.name.ToLowerInvariant().Contains("fe");
      List<int> intList1 = new List<int>() { 3, 4, 5 };
      List<int> intList2 = new List<int>()
      {
        0,
        1,
        2,
        3,
        4,
        5
      };
      int[] numArray = new int[7]
      {
        7,
        9,
        12,
        15,
        18,
        21,
        24
      };
      List<int> ordinals = flag ? intList1 : intList2;
      int multiplier = flag ? 2 : 1;
      string str1 = flag ? "As a female, the Aslan culture values Int, Edu, and Soc.  Your right of passage is simulated by rolling 2D6 and comparing the result with these stats.  For ever stat higher than your roll, you'll recieve an RoP bonus of +2.  The higher the bonus, the easier it will be to get into Hierate careers." : "As a male, your right of passage is simulated by rolling 2D6 and comparing the result with all of your stats.  For ever stat higher than the roll, you'll recieve a RoP bonus of +1.  The higher the bonus, the easier it will be to get into Hierate careers.";
      currentState.decisionMaker.present(new Presentation("Right Of Passage!", "This a fundemental part of Hierate Aslan Culture.  At the age of fifteen (Aslan years), all Aslan Travellers undergo a rite of passage, the Akhuaeuhrekhyeh. This rite tests the individual’s fitness to enter society and has an impact on careers.\nThe actual rite is a test which examines the Traveller’s abilities and qualities, and males and females undergo different rites." + str1));
      RollEffect rollEffect1 = Dice.D6Roll(1, 0, "Right of Passage Roll Die 1");
      RollEffect rollEffect2 = Dice.D6Roll(1, 0, "Right of Passage Roll Die 2");
      string str2 = "";
      if (rollEffect1.rawResult == rollEffect2.rawResult)
        str2 = HierateCharacterCreationAlgorithm.HandleRiteOfPassageEvent(currentState, rollEffect1.rawResult);
      int rop_roll = rollEffect1.rawResult + rollEffect2.rawResult;
      int rop_modifier = 0;
      currentState.character.getAttributes().Where<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (a => ordinals.Contains(a.Ordinal))).ToList<com.digitalarcsystems.Traveller.DataModel.Attribute>().ForEach((Action<com.digitalarcsystems.Traveller.DataModel.Attribute>) (a =>
      {
        if (a.Value <= rop_roll)
          return;
        rop_modifier += multiplier;
      }));
      currentState.decisionMaker.present(new Presentation("Right of Passage Results", "Your RoP score is " + rop_roll.ToString()));
      com.digitalarcsystems.Traveller.DataModel.Attribute attribute = new com.digitalarcsystems.Traveller.DataModel.Attribute("RoP", 10);
      attribute.InitializeValue(6 + 3 * rop_roll);
      HierateCharacterCreationAlgorithm.addAttributeToCharacter(currentState.character, attribute);
      currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CAREER);
      return currentState;
    }

    public static string HandleRiteOfPassageEvent(GenerationState currentState, int rolledDouble)
    {
      string str = "";
      switch (rolledDouble)
      {
        case 1:
          str = "You are believed to have a great destiny, and the clan looks for great things from you. You must excel or disappoint your ancestors. You gain 1D Clan Shares. ";
          new ClanShares(Dice.D6Roll(1, 0, "Gain 1d6 Clan Shares").rawResult).handleOutcome(currentState);
          break;
        case 2:
          str = "Impressive Performance. You are given Cr5000 as a reward for your exemplary performance in the rite.";
          new Outcome.GainMoney(5000).handleOutcome(currentState);
          break;
        case 3:
          str = "You befriend one of the other young Aslan undergoing the rite that day. Gain a Contact.";
          new Outcome.GainSpecifiedContact("Aslan from Rite of Passage").handleOutcome(currentState);
          break;
        case 4:
          str = "One of the other Aslan undergoing the rite tries to outdo you. Gain a Rival";
          new Outcome.Rivals().handleOutcome(currentState);
          break;
        case 5:
          str = "You are wounded in one of the tests, leaving a distinctive scar across your fur.";
          break;
        case 6:
          str = "I Will Not Fail! Your rite tests you to the limit, but you are determined not to give in. Gain END +1";
          new Outcome.StatModified("End", 1).handleOutcome(currentState);
          break;
      }
      return str;
    }

    protected override Career handleDriftOrDraft(GenerationState currentState, string careerName)
    {
      Career career1 = this.GetCareer("Outlaw");
      Career career2 = this.GetCareer("Outcast");
      Career career3 = this.GetCareer("Drifter");
      Career career4 = this.GetCareer("Rogue");
      Character character = currentState.character;
      character.getAttribute("Soc").Value = 2;
      Describable describable1 = new Describable("Outcast", "They didn't want you?  You weren't worthy? This isn't a setback.  It's a challenge!");
      Describable describable2 = new Describable("Outlaw", "Fine. If you can't work with the system, you can work against it! Not everyone is cut out to be an Outlaw. If you can't make it as an Outlaw, you'll be an Outcast.");
      Describable describable3 = new Describable("Rogue", "You just want to put the shame of being unworthy behind you.  You can leave Hierate space and become a Rogue.");
      Describable describable4 = new Describable("Drifter", "There is more to life than what the Hierate offers! It's time you left Hierate space and drifted about seeing what's what, and who's who.");
      Describable describable5 = currentState.decisionMaker.ChooseOne<Describable>("You have FAILED to qualify for " + careerName + ". Your SOC is now " + character.getAttributeValue("Soc").ToString() + ".  Others feel this is shameful, and you have only a few choices left to you.", (IList<Describable>) new List<Describable>()
      {
        describable1,
        describable2,
        describable3,
        describable4
      });
      Career career5;
      switch (describable5.Name)
      {
        case "Outcast":
          career5 = career2;
          break;
        case "Outlaw":
          career5 = !career1.qualifyForCareer(character, 0) ? career2 : career1;
          break;
        case "Rogue":
          career5 = career4;
          this.SetCareerFilter(AbstractCharacterCreationAlgorithm.defaultCareerFilter);
          break;
        case "Drifter":
          career5 = career3;
          this.SetCareerFilter(AbstractCharacterCreationAlgorithm.defaultCareerFilter);
          break;
        default:
          EngineLog.Error("AN UNDEFINED CHOICE WAS MADE IN DRIFTORDRAFT: " + describable5.Name + "\tDEFAULTING TO OUTCAST CAREER.");
          career5 = career2;
          break;
      }
      return career5;
    }

    public override GenerationState chooseEndOfTermAction(GenerationState currentState)
    {
      int includingThisOne = currentState.character.CurrentTerm.numberOfTermsInCareerIncludingThisOne;
      if (currentState.mustContinueInCareer || currentState.currentCareer == null || currentState.currentCareer != null && !currentState.currentCareer.Category.ToLowerInvariant().Contains("aslan"))
        return base.chooseEndOfTermAction(currentState);
      GenerationState characterGenerationState;
      if (includingThisOne > 3)
      {
        Dictionary<IDescribable, CreationOperation> dictionary = new Dictionary<IDescribable, CreationOperation>();
        dictionary[(IDescribable) new Describable("CONTINUE in current career", "Keep going, and do at least one more term.  Why stop now?")] = CreationOperation.CHOOSE_CAREER_SPECIALIZATION;
        dictionary[(IDescribable) new Describable("RETIRE from current career", "That's enough of this career.  Time to get your benefits and check out.")] = CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT;
        IDescribable key = currentState.decisionMaker.ChooseOne<IDescribable>("What's the next step in your character's life?", (IList<IDescribable>) new List<IDescribable>((IEnumerable<IDescribable>) dictionary.Keys));
        characterGenerationState = currentState.createNextCharacterGenerationState(dictionary[key]);
      }
      else
      {
        currentState.decisionMaker.present(new Presentation("Need to Stay in Career", "Hierate society dictates that you can neither retire or change your career before being in it for at least 3 terms. You have " + (3 - includingThisOne).ToString() + " terms remaining."));
        characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CAREER_SPECIALIZATION);
      }
      return characterGenerationState;
    }

    public override GenerationState handlePreCareer(GenerationState currentState)
    {
      currentState.currentCareer = (Career) null;
      return currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CAREER);
    }

    private static void addAttributeToCharacter(Character character, com.digitalarcsystems.Traveller.DataModel.Attribute attribute)
    {
      character.getAttributes().Add(attribute);
    }

    public virtual GenerationState handleAllocateClanShares(GenerationState currentState)
    {
      throw new NotImplementedException();
    }

    protected override int GetPermMOB_Modifiers(GenerationState currentState)
    {
      int permMobModifiers = 0;
      Character character = currentState.character;
      switch (currentState.currentCareer.Name.ToLower())
      {
        case "ceremonial":
        case "management":
        case "military":
        case "military officer":
        case "scientist":
        case "spacer":
        case "spacer officer":
        case "wanderer":
          if (character.getAttribute("Soc").Value >= 9)
          {
            ++permMobModifiers;
            break;
          }
          break;
      }
      return permMobModifiers;
    }

    protected override int GetBenefitMOB_Modifier(GenerationState currentState)
    {
      int benefitMobModifier = 0;
      Character character = currentState.character;
      string name = currentState.currentCareer.Name;
      bool flag = this.IsAslanMale(character);
      string lower = name.ToLower();
      if ((lower == "outlaw" || lower == "spacer officer" || lower == "spacer" || lower == "scientist" || lower == "military officer" || lower == "military") && character.getAttribute("Soc").Value >= 9 && flag)
        ++benefitMobModifier;
      return benefitMobModifier;
    }
  }
}
