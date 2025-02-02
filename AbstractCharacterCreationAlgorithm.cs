
// Type: com.digitalarcsystems.Traveller.AbstractCharacterCreationAlgorithm




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using com.digitalarcsystems.Traveller.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public abstract class AbstractCharacterCreationAlgorithm : 
    ICharacterCreationAlgorithm,
    ISkillSource,
    IPeopleSource,
    IBenefitRecorder,
    ICareerSource
  {
    protected internal IList<Race> races;
    protected internal Dictionary<string, ISkill> skills;
    protected internal Dictionary<string, Talent> talents;
    protected internal Dictionary<string, Configurator> configurators;
    protected internal IList<World> worlds;
    protected internal IList<Career> careers;
    protected internal IList<Character> partyMembers;
    protected internal IList<Contact> peopleToMeet;
    private IList<Career> _draftCareers = (IList<Career>) new List<Career>();
    protected internal Career drifter = (Career) null;
    protected GenerationState currentState = (GenerationState) null;
    protected GenerationState savedState = new GenerationState(DataManager.UserID);
    internal Character currentCharacter;
    protected internal IDecisionMaker theDecider = (IDecisionMaker) null;
    private List<PreCareer> preCareers = (List<PreCareer>) null;
    public bool OnlyRemoveLastCareerAndNotAllCareers = true;
    public static readonly ICareerFilter defaultCareerFilter = (ICareerFilter) new CategoryStringFilter("aslan", "outcast", CategoryStringFilter.FilterBehavior.FILTER_OUT);
    private ICareerFilter _careerFilter = AbstractCharacterCreationAlgorithm.defaultCareerFilter;

    protected internal IList<Career> draftCareers
    {
      get
      {
        return (IList<Career>) this._draftCareers.Where<Career>((Func<Career, bool>) (dc => this.careerFilter.filter(dc))).ToList<Career>();
      }
      set => this._draftCareers = value;
    }

    public bool QualifyOnAssignmentChange { get; set; }

    public AbstractCharacterCreationAlgorithm()
    {
      this.configurators = new Dictionary<string, Configurator>();
      Configurator configurator = (Configurator) new PsionicConfigurator((ICharacterCreationAlgorithm) this);
      this.configurators.Add(configurator.Name, configurator);
    }

    public CreationOperation nextOperation() => this.savedState.nextOperation;

    public GenerationState getCurrentGenerationState() => this.savedState;

    public virtual GenerationState selectRace(GenerationState currentState)
    {
      Race race = currentState.decisionMaker.ChooseOne<Race>("Select your character's race", this.races);
      IList<Skill> racialSkills = race.RacialSkills;
      IList<Outcome> racialBenefits = race.RacialBenefits;
      if (racialSkills.Count > 0)
      {
        foreach (Skill addMe in (IEnumerable<Skill>) racialSkills)
          currentState.character.addSkill((ISkill) addMe);
      }
      if (racialBenefits.Count > 0)
      {
        foreach (Outcome outcome in (IEnumerable<Outcome>) racialBenefits)
          outcome.handleOutcome(currentState);
      }
      currentState.character.Race = race;
      return currentState.createNextCharacterGenerationState(CreationOperation.GENERATE_STATS);
    }

    public virtual GenerationState generateStats(GenerationState currentState)
    {
      List<NamedList<com.digitalarcsystems.Traveller.DataModel.Attribute>> choices = new List<NamedList<com.digitalarcsystems.Traveller.DataModel.Attribute>>();
      for (int index = 1; index < 4; ++index)
        choices.Add(new NamedList<com.digitalarcsystems.Traveller.DataModel.Attribute>("Possibility " + index.ToString(), (ICollection<com.digitalarcsystems.Traveller.DataModel.Attribute>) currentState.character.generateCharacteristics()));
      NamedList<com.digitalarcsystems.Traveller.DataModel.Attribute> newAttributes = currentState.decisionMaker.ChooseOne<NamedList<com.digitalarcsystems.Traveller.DataModel.Attribute>>("Pick a set of stats for the character", (IList<NamedList<com.digitalarcsystems.Traveller.DataModel.Attribute>>) choices);
      currentState.character.setAttributes((List<com.digitalarcsystems.Traveller.DataModel.Attribute>) newAttributes);
      return currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_HOMEWORLD);
    }

    public virtual GenerationState chooseHomeworld(GenerationState currentState)
    {
      World world = currentState.decisionMaker.ChooseOne<World>("Pick a home world", this.worlds);
      currentState.character.HomeWorld = world;
      return currentState.createNextCharacterGenerationState(CreationOperation.GAIN_BACKGROUND_SKILLS);
    }

    public virtual GenerationState chooseCareerSpecialization(GenerationState currentState)
    {
      bool flag1 = true;
      if (currentState.currentCareer == null)
        throw new NullReferenceException("chooseCareerSpecialization called but current career null");
      if (currentState.currentCareer is PreCareer)
        throw new Exception("Precareers Don't Have Specializations");
      currentState.character.startNewTerm(currentState.currentCareer);
      if (currentState.currentCareer.AutomaticCommissionIfStatIsAtOrOverTarget && currentState.currentCareer.hasCommissions() && !currentState.character.CurrentTerm.officer && currentState.character.CurrentTerm.numberOfTermsInCareerIncludingThisOne == 1)
      {
        Career currentCareer = currentState.currentCareer;
        com.digitalarcsystems.Traveller.DataModel.Attribute attribute = currentState.character.getAttribute(currentCareer.CommissionStat);
        if (attribute != null && attribute.Value >= currentCareer.Commission)
        {
          this.currentState.decisionMaker.present(new Presentation("Automatic Commission", "Because of your high " + currentCareer.CommissionStat + " you automatically receive a commission"));
          currentState.character.CurrentTerm.officer = true;
        }
      }
      if (currentState.startNextCareerAtRank != 0)
      {
        currentState.character.CurrentTerm.rank = currentState.startNextCareerAtRank;
        currentState.startNextCareerAtRank = 0;
      }
      if (currentState.actionsOnQualifications.Count > 0)
      {
        for (int index = 0; index < currentState.actionsOnQualifications.Count; ++index)
        {
          Event.ActionsOnQualification actionsOnQualification = currentState.actionsOnQualifications[index];
          actionsOnQualification.handleOutcome(currentState);
          if (actionsOnQualification.delete)
            currentState.actionsOnQualifications.RemoveAt(index--);
        }
      }
      this.Log("calling decision maker to choose one of given specializations for " + currentState.currentCareer.Name);
      currentState.decisionMaker.setInfoForUI(CreationOperation.CHOOSE_CAREER_SPECIALIZATION);
      string description = "";
      Career.Specialization specialization1 = (Career.Specialization) null;
      if (!currentState.character.CurrentTermNewCareer && currentState.previousState.character.hasAlreadyBeenDrafted() == currentState.character.hasAlreadyBeenDrafted() && this.QualifyOnAssignmentChange && currentState.currentCareer.qualificationChancePercent(currentState.character) < 100)
      {
        if (currentState.currentCareer.CareerFinishedOnFailedAssignmentChange)
          description = " Failing to qualify means ENDING this career and becoming a Drifter" + (!this.driftOrDraftAvailable() || currentState.character.hasAlreadyBeenDrafted() ? ". " : " or entering the Draft.") + description;
        description = "Changing assignments will required a qualification roll. " + description;
        specialization1 = currentState.currentCareer.getSpecialization(currentState.character.getPreviousTermOfCareer(currentState.currentCareer.Name, 1).specializationName);
      }
      Career.Specialization specialization2 = currentState.decisionMaker.ChooseOne<Career.Specialization>(description, currentState.currentCareer.GetApplicableSpecializations(currentState.character));
      if (specialization2 == null)
        Console.WriteLine("Bingo");
      if (specialization1 != null && specialization1 != specialization2)
      {
        List<DescribableContainer<Career.Specialization>> choices = new List<DescribableContainer<Career.Specialization>>()
        {
          new DescribableContainer<Career.Specialization>("Don't Change Assignments. [" + specialization1.Name + "]", "Play it safe and stay a " + specialization1.Name + ".", specialization1)
          {
            PrependValueDescription = false
          },
          new DescribableContainer<Career.Specialization>("Make that Change! [" + specialization2.Name + "]", "It's time to go for a new opportunity and be a " + specialization2.Name, specialization2)
          {
            PrependValueDescription = false
          }
        };
        specialization2 = currentState.decisionMaker.ChooseOne<DescribableContainer<Career.Specialization>>(description + "  Are you sure you want to switch specializations?", (IList<DescribableContainer<Career.Specialization>>) choices).Value;
      }
      if (this.QualifyOnAssignmentChange && specialization1 != null && !specialization2.Name.Equals(specialization1.Name, StringComparison.InvariantCultureIgnoreCase))
      {
        flag1 = currentState.currentCareer.qualifyForCareer(currentState.character, currentState.getQualificationModifier());
        FeedbackStream.Send(flag1 ? "Succeeded in Changing Assignments." : "Failed to Change Assignments");
        if (!flag1 && !currentState.currentCareer.CareerFinishedOnFailedAssignmentChange)
        {
          specialization2 = specialization1;
          flag1 = true;
        }
      }
      GenerationState characterGenerationState;
      if (!flag1)
      {
        characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT);
        characterGenerationState.nextCarrerMustBe = this.handleDriftOrDraft(characterGenerationState, currentState.currentCareer.Name + ": " + specialization2.Name).Name;
      }
      else
      {
        currentState.character.CurrentTerm.specializationName = specialization2.Name;
        this.Log(string.Format(" d'oh chooseCareerSpecialization about to test current term new career"));
        if (currentState.character.CurrentTermNewCareer)
        {
          this.Log(string.Format(" dd'oh about to compute rank zero. current career null state: {0}, specialization name {1}", (object) currentState.currentCareer, (object) specialization2.Name));
          Rank rank = currentState.currentCareer.getRank(false, 0, specialization2.Name);
          this.Log(string.Format("chooseCareerSpecialization - rank zero is null? {0}", (object) (rank != null)));
          if (rank != null)
          {
            bool flag2 = rank.benefits != null && rank.benefits.Count > 0;
            this.Log(string.Format("chooseCareerSpecialization - has benefits? {0}", (object) flag2));
            if (flag2)
            {
              FeedbackStream.Send("Rank 0 Benefits:");
              foreach (Outcome benefit in rank.benefits)
                benefit.handleOutcome(currentState);
            }
            Term currentTerm = currentState.character.CurrentTerm;
            if (rank.title != null)
              currentTerm.title = rank.title;
          }
          characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.GET_BASIC_TRAINING);
        }
        else
          characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_SKILLS_AND_TRAINING_TABLE_OBTAIN_SKILL);
      }
      return characterGenerationState;
    }

    protected static List<Outcome> getBasicTrainingSkills(GenerationState currentState)
    {
      List<Outcome> source = new List<Outcome>();
      if (currentState.currentCareer.useSpecialistSkillListForBasicTraining())
        source.AddRange((IEnumerable<Outcome>) currentState.currentCareer.getSpecializationSkills(currentState.character.CurrentTerm.specializationName));
      else
        source.AddRange((IEnumerable<Outcome>) currentState.currentCareer.ServiceSkills);
      Outcome outcome = source.FirstOrDefault<Outcome>((Func<Outcome, bool>) (o => o is Outcome.GainSkill && ((Outcome.GainSkill) o).skillName.ToLowerInvariant().Contains("jack-of-all-trade")));
      if (outcome != null)
        source.Remove(outcome);
      return source.Where<Outcome>((Func<Outcome, bool>) (oc => !(oc is Outcome.StatModified))).ToList<Outcome>();
    }

    public virtual GenerationState getBasicTraining(GenerationState currentState)
    {
      this.Log("getBasicTraining() called");
      List<Outcome> outcomeList = new List<Outcome>();
      List<Outcome> choices = Utility.prunePosessedSkillsAndTalents(currentState.character, (IList<Outcome>) AbstractCharacterCreationAlgorithm.getBasicTrainingSkills(currentState));
      if (currentState.character.NumberOfCareers == 1)
      {
        FeedbackStream.Send("Basic Training Skills Added:");
        foreach (Outcome outcome in choices)
        {
          if (outcome is Outcome.GainSkill)
          {
            if (!(((Outcome.GainSkill) outcome).skillName.ToLowerInvariant() == "jack-of-all-trades"))
              outcome.handleOutcome(currentState);
          }
          else
            outcome.handleOutcome(currentState);
        }
      }
      else if (choices.Count > 0)
      {
        foreach (Outcome outcome in choices)
        {
          outcome.Description = outcome.Description.Replace("+1", "0");
          outcome.Name = outcome.Name.Replace("+1", "0");
        }
        FeedbackStream.Send("Basic Training Skill Added:");
        currentState.decisionMaker.ChooseOne<Outcome>("Select a single 0 level skill for your basic training", (IList<Outcome>) choices).handleOutcome(currentState);
      }
      else
        new Outcome.InformUser("No Benefit from Basic Training", "There wasn't anything basic about this career you didn't already know.").handleOutcome(currentState);
      return currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_SKILLS_AND_TRAINING_TABLE_OBTAIN_SKILL);
    }

    public virtual GenerationState chooseSkillsAndTrainingTableAndObtainSkill(
      GenerationState currentState)
    {
      bool flag = currentState.nextOperation == CreationOperation.ROLL_EVENTS || currentState.nextOperation == CreationOperation.PROCESS_ADVANCEMENT_OR_COMISSION;
      string description = "Which table would you like to roll on?";
      if (flag)
      {
        string str = "Congratulations on your promotion to Rank: " + currentState.character.CurrentTerm.rank.ToString() + " ";
        if (currentState.character.CurrentTerm.title != null)
          str = str + " " + currentState.character.CurrentTerm.title;
        description = str + "!\r\nAs a benefit please gain an additional skill. Which table would you like to roll on?";
      }
      IList<NamedList<Outcome>> availableTables = currentState.currentCareer.getAvailableTables(currentState.character);
      this.Log("calling decision maker to choose table of skills or something like that - after basic training this happens, but maybe not only");
      currentState.decisionMaker.setInfoForUI(CreationOperation.CHOOSE_SKILLS_AND_TRAINING_TABLE_OBTAIN_SKILL);
      currentState.decisionMaker.setQueryKey(ContextKeys.SKILLS_TABLES);
      IList<Outcome> choices = (IList<Outcome>) currentState.decisionMaker.ChooseOne<NamedList<Outcome>>(description, availableTables);
      this.Log("selected table is..." + choices?.ToString());
      Outcome outcome = Dice.RollRandomResult<Outcome>("You improved your skill in...", choices, ContextKeys.OUTCOMES);
      FeedbackStream.Send("Skill Gained: " + outcome?.ToString());
      outcome.handleOutcome(currentState);
      return flag ? currentState : currentState.createNextCharacterGenerationState(CreationOperation.ROLL_FOR_SURVIVAL);
    }

    public virtual GenerationState gainBackgroundSkills(GenerationState currentState)
    {
      Character character = currentState.character;
      currentState.character.applyChildhoodCareer(currentState.character.HomeWorld.Name);
      int backgroundSkills1 = this.determineNumberOfBackgroundSkills(character);
      IList<ISkill> backgroundSkills2 = this.getBackgroundSkills(character);
      foreach (ISkill skill in (IEnumerable<ISkill>) this.currentCharacter.Skills)
        backgroundSkills2.Remove(skill);
      if (backgroundSkills1 > 0)
      {
        IList<ISkill> skillList = currentState.decisionMaker.Choose<ISkill>("Please pick your initial [" + backgroundSkills1.ToString() + "] background skills", backgroundSkills1, backgroundSkills2);
        FeedbackStream.Send("Background Skills Gained: ");
        foreach (ISkill skill in (IEnumerable<ISkill>) skillList)
          new Outcome.GainSkill(skill).handleOutcome(currentState);
      }
      return currentState.createNextCharacterGenerationState(CreationOperation.HANDLE_PRECAREER);
    }

    public virtual GenerationState handlePreCareer(GenerationState currentState)
    {
      if (currentState.character.Race.Id == Guid.Parse("245d5c7d-2fe0-4c8e-a51d-f47351b974ad") && currentState.character.CareerHistory.Count < 1)
      {
        Event.TestForTalent testForTalent = new Event.TestForTalent(com.digitalarcsystems.Traveller.DataModel.Trait.PSIONIC);
        testForTalent.Name = "Train for Psionics";
        testForTalent.Description = "Test your innate psionic ability to determine which psionic talents you will acquire.";
        new Event.ChoiceOutcome(1, "Chokari can be Psionic", "The Chokari Race has been known to possess Psionic Talent, do you wish to pursue Psionic Training?", new Outcome[2]
        {
          (Outcome) testForTalent,
          new Outcome("Decline Psionic Training", "You decide not to follow a Psionic path in life.")
        }).handleOutcome(currentState);
      }
      EngineLog.Print("Entering PreCareer: Num PC=" + this.PreCareers.Count.ToString() + " Char.HadPreCareer: " + currentState.character.HadPreCareer.ToString());
      int num = 0;
      if (!currentState.character.HadPreCareer && currentState.character.TotalNumberOfTerms < 5 && this.PreCareers.Count > 0)
      {
        List<IDescribable> collection = new List<IDescribable>();
        foreach (PreCareer preCareer in this.preCareers)
        {
          num = currentState.getQualificationModifier(preCareer.Name);
          if (preCareer.qualificationChancePercent(currentState.character, num) > 0)
            collection.Add((IDescribable) preCareer);
        }
        if (collection.Count > 0)
        {
          List<IDescribable> choices = new List<IDescribable>((IEnumerable<IDescribable>) collection);
          Describable describable1 = new Describable("Skip University", "College, Academies and Universities are not for you. You want to jump into the real world as soon as possible.");
          choices.Add((IDescribable) describable1);
          IDescribable describable2 = currentState.decisionMaker.ChooseOne<IDescribable>((IList<IDescribable>) choices);
          if (describable2 != describable1)
          {
            PreCareer preCareer = (PreCareer) describable2;
            if (num > 0)
              currentState.clearQualificationModifier(preCareer.Name);
            if (preCareer.qualifyForCareer(currentState.character, num))
            {
              currentState.currentCareer = (Career) preCareer;
              currentState.character.startNewTerm(currentState.currentCareer);
              PreCareer currentCareer = (PreCareer) currentState.currentCareer;
              foreach (Outcome serviceSkill in (IEnumerable<Outcome>) currentCareer.ServiceSkills)
                serviceSkill.handleOutcome(currentState);
              currentState = this.rollEvents(currentState);
              int advancementModifier = currentState.character.CurrentTerm.advancementModifier;
              if (!currentState.endCurrentCareer)
              {
                IList<Outcome> outcomeList = currentCareer.qualifyForAdvancement(currentState.character, "", advancementModifier);
                if (outcomeList != null && outcomeList.Count > 0)
                {
                  foreach (Outcome outcome in (IEnumerable<Outcome>) outcomeList)
                    outcome.handleOutcome(currentState);
                }
              }
              currentState.character.Age += 4;
            }
          }
        }
      }
      currentState.currentCareer = (Career) null;
      return currentState.nextCarrerMustBe == null || currentState.nextCarrerMustBe.Length <= 0 ? currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CAREER) : this.handleNextCareerMustBe(currentState);
    }

    private bool driftOrDraftAvailable()
    {
      return this.drifter != null || !this.currentState.character.hasAlreadyBeenDrafted() && this.draftCareers != null && this.draftCareers.Count > 0;
    }

    protected virtual Career handleDriftOrDraft(GenerationState currentState, string careerName)
    {
      Career career;
      if (currentState.character.hasAlreadyBeenDrafted() || this.draftCareers == null || this.draftCareers.Count == 0)
      {
        FeedbackStream.Send("Unfortunately, you didn't qualify for the career (" + careerName + ") of your choice, and the draft is also no longer available. You spend the next four years as a Drifter");
        career = this.drifter;
      }
      else
      {
        Describable describable1 = new Describable("Drifter", "Forget regular jobs. Who wants to have a reliable pension and a boring life?! You want to be independent and you decide to live on your own - without bosses for sure, but hopefully with some comrades along the way.");
        string str = "";
        for (int index = 0; index < this.draftCareers.Count; ++index)
        {
          Career draftCareer = this.draftCareers[index];
          if (index != 0)
            str += ", ";
          if (index == this.draftCareers.Count - 1 && this.draftCareers.Count > 1)
            str += "and ";
          str += draftCareer.Name;
        }
        Describable describable2 = new Describable("Draft", "Maybe you didn't make it into the career of your choice, but there's always a chance you'll get picked up in the Draft: the " + str + " always need cannon fodder - that is, mandatory volunteers...");
        Describable[] choices = new Describable[2]
        {
          describable1,
          describable2
        };
        currentState.decisionMaker.setResultKey(ContextKeys.CHOICE_RESULT);
        if (currentState.decisionMaker.ChooseOne<Describable>("Unfortunately, you didn't qualify for the career of your choice. You can either become a Drifter for a term, or see what the Draft holds for you", (IList<Describable>) choices) == describable1)
        {
          career = this.drifter;
        }
        else
        {
          career = Dice.RollRandomResult<Career>("Searching for opportunities...", this.draftCareers, ContextKeys.CAREERS);
          FeedbackStream.Send("You've been drafted: (" + career.Name + ")");
          currentState.character.setDrafted();
        }
      }
      return career;
    }

    protected List<Career> getAvailableCareers(
      GenerationState currentState,
      bool ensure_drifter_in_choices = true)
    {
      List<Career> list = this.Careers.Where<Career>((Func<Career, bool>) (c => !(c is CaptiveCareer))).ToList<Career>();
      if (this.OnlyRemoveLastCareerAndNotAllCareers)
      {
        EngineLog.Print("Number of Careers: " + currentState.character.NumberOfCareers.ToString());
        if (currentState.character.NumberOfCareers > 0)
        {
          string lastCareerName = currentState.character.CareerHistory.Last<Term>().careerName;
          EngineLog.Print("Last Career Name: " + lastCareerName);
          list = list.Where<Career>((Func<Career, bool>) (c => c.Name != lastCareerName)).ToList<Career>();
          EngineLog.Print("Available Careers: " + list.Select<Career, string>((Func<Career, string>) (a => a.Name)).ToArray<string>()?.ToString());
        }
      }
      else
        list = list.Where<Career>((Func<Career, bool>) (c => !currentState.character.haveHadCareer(c))).ToList<Career>();
      if (this.drifter != null & ensure_drifter_in_choices && !list.Contains(this.drifter))
        list.Add(this.drifter);
      return list;
    }

    public virtual GenerationState chooseCareer(GenerationState currentState)
    {
      int num = 0;
      List<Career> availableCareers = this.getAvailableCareers(currentState);
      GenerationState characterGenerationState;
      if (availableCareers.Count != 0)
      {
        currentState.decisionMaker.setInfoForUI(CreationOperation.CHOOSE_CAREER);
        Career career = currentState.decisionMaker.ChooseOne<Career>("These are the available careers. Pick one.", (IList<Career>) availableCareers);
        bool flag;
        if (this.driftOrDraftAvailable())
        {
          if (currentState.getQualificationModifier(career.Name) > 0)
          {
            num = currentState.getQualificationModifier(career.Name);
            currentState.clearQualificationModifier(career.Name);
            FeedbackStream.Send("Because of your history, you're attempting to qualify for (" + career?.ToString() + ") with a modifier of " + (num > 0 ? "+" : "-") + num.ToString());
          }
          flag = career.qualificationChancePercent(currentState.character, num) >= 100 || career.qualifyForCareer(currentState.character, num);
        }
        else
        {
          flag = true;
          EngineLog.Print("Qualifying because there was no alternative");
        }
        this.Log("after feedback,  isQualified == " + flag.ToString() + " for career: " + career.Name);
        if (flag)
          FeedbackStream.Send("Congratulations! You qualified for the career of your choice (" + career.Name + ")");
        else
          career = this.handleDriftOrDraft(currentState, career.Name);
        CreationOperation nextOperation = CreationOperation.CHOOSE_CAREER_SPECIALIZATION;
        characterGenerationState = currentState.createNextCharacterGenerationState(nextOperation);
        characterGenerationState.currentCareer = career;
      }
      else
      {
        EngineLog.Error("There were no careers, but there should have been at least Drifter.  Sending character to connections");
        characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.ESTABLISH_CONNECTION_WITH_OTHER_CHARCTER);
      }
      if (characterGenerationState.currentCareer == null)
        EngineLog.Error("ChooseCareer ended in a Null career.  Last Stage was " + currentState.previousState.nextOperation.ToString());
      EngineLog.Print("at the end of chooseCareer()");
      return characterGenerationState;
    }

    public virtual GenerationState rollForSurvival(GenerationState currentState)
    {
      int num = currentState.character.OnAnagathics ? 2 : 1;
      CreationOperation nextOperation = CreationOperation.UNDEFINED;
      bool flag = false;
      for (int index = 0; index < num && !flag; ++index)
      {
        if (index == 1)
          currentState.decisionMaker.present(new Presentation("Anagathics Are Risky!", "Because you are on anagathics, you must make an additional survival roll.  This represents the risks you must take to find and/or hide anagathics, as well as the side effects of anagathics on your biochemistry."));
        this.Log("RollForSurvival() executed");
        RollParam settings = currentState.currentCareer.getSpecialization(currentState.character.CurrentTerm.specializationName).survivalRoll(currentState.character, currentState.careerSurvivalModifier);
        currentState.character.CurrentTerm.survivalModifier = 0;
        settings.AddResultDescriptions("You made it through the term.", "Uh oh. Things didn't go as planned...");
        if (Dice.Roll(settings).isSuccessful)
        {
          nextOperation = CreationOperation.ROLL_EVENTS;
          FeedbackStream.Send("You survived your term (" + currentState.character.TotalNumberOfTerms.ToString() + ") without incident.");
        }
        else
        {
          FeedbackStream.Send("There was a mishap during this term!");
          new Event("Failed Survival Roll", "There was a mishap during this term!").handleOutcome(currentState);
          nextOperation = CreationOperation.ROLL_MISHAP;
          flag = true;
        }
      }
      GenerationState characterGenerationState = currentState.createNextCharacterGenerationState(nextOperation);
      if (nextOperation == CreationOperation.ROLL_MISHAP)
        --characterGenerationState.character.CurrentTerm.additional_mustering_out_benefits;
      characterGenerationState.endCurrentCareer = flag;
      return characterGenerationState;
    }

    public virtual GenerationState processAdvancementOrCommission(GenerationState currentState)
    {
      IList<Outcome> outcomeList = (IList<Outcome>) new List<Outcome>();
      bool flag = false;
      if (currentState.skipProcessAdvancementOrCommission)
      {
        currentState.decisionMaker.present(new Presentation("Skipping Advancement", "Due to events during this term, you are not given an opportunity to advance in your career."));
        currentState.skipProcessAdvancementOrCommission = false;
        return currentState.createNextCharacterGenerationState(CreationOperation.HANDLE_AGING);
      }
      if (!currentState.character.CurrentTerm.officer && currentState.currentCareer.hasCommissions() && (currentState.character.CurrentTerm.numberOfTermsInCareerIncludingThisOne == 1 || currentState.character.getAttributeModifier(5) >= 1))
      {
        string[] choices = new string[2]
        {
          "Yes, I belong in command",
          "Nah, I work for a living"
        };
        flag = currentState.decisionMaker.ChooseOne<string>("You may try for a commission or a normal advancement. Would you like a commission?", (IList<string>) choices) == choices[0];
      }
      IList<Outcome> outcomes = !flag ? currentState.currentCareer.qualifyForAdvancement(currentState.character, currentState.character.CurrentTerm.specializationName, currentState.careerAdvancementModifier) : currentState.currentCareer.qualifyForCommision(currentState.character, currentState.careerAdvancementModifier);
      this.handleAdvancementOutcomes(currentState, outcomes);
      return currentState.createNextCharacterGenerationState(CreationOperation.HANDLE_AGING);
    }

    private void handleAdvancementOutcomes(GenerationState currentState, IList<Outcome> outcomes)
    {
      if (outcomes == null || outcomes.Count <= 0)
        return;
      foreach (Outcome outcome in (IEnumerable<Outcome>) outcomes)
      {
        outcome.handleOutcome(currentState);
        if (outcome is Outcome.GainCommision || outcome is Event.GainPromotion)
          this.chooseSkillsAndTrainingTableAndObtainSkill(currentState);
      }
    }

    public virtual GenerationState chooseEndOfTermAction(GenerationState currentState)
    {
      GenerationState generationState;
      if (currentState.mustContinueInCareer)
      {
        generationState = currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CAREER_SPECIALIZATION);
        generationState.mustContinueInCareer = false;
        FeedbackStream.Send("You find yourself in another term of (" + currentState.character.CurrentTerm.careerName + ").");
        currentState.decisionMaker.present(new Presentation("Another year as in the " + currentState.currentCareer.Name + " Career", "Due to events this term, you are obliged to do another term in the " + currentState.currentCareer.Name + " career."));
      }
      else if (currentState.nextCarrerMustBe != null && currentState.nextCarrerMustBe.Length > 0)
      {
        generationState = this.handleNextCareerMustBe(currentState);
      }
      else
      {
        Dictionary<IDescribable, CreationOperation> dictionary = new Dictionary<IDescribable, CreationOperation>();
        if (currentState.currentCareer != null)
        {
          if (!currentState.endCurrentCareer)
            dictionary[(IDescribable) new Describable("CONTINUE in current career", "Keep going, and do at least one more term.  Why stop now?")] = CreationOperation.CHOOSE_CAREER_SPECIALIZATION;
          dictionary[(IDescribable) new Describable("RETIRE from current career", "That's enough of this career.  Time to get your benefits and check out.")] = CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT;
        }
        else
        {
          bool flag = currentState.character.CareerHistory.Count >= 2;
          dictionary[(IDescribable) new Describable("Choose a DIFFERENT CAREER", "You're not done! Try your luck at a different career before starting the real adventure.")] = CreationOperation.HANDLE_PRECAREER;
          if (flag)
            dictionary[(IDescribable) new Describable("FINISH UP: Make Connections", "No more careers for you!  It's time to make connections with other characters, buy equipment, and finish up.\n\nWhen building characters as a group activity (highly recommended) if two players agree, any event rolled for one character can involve another. This is called a CONNECTION. Only one character needs to have rolled the specific event.\n\nAs a benefit for doing this, each connection (maximum 2) grants the character a level in any skill (existing or new). A skill can't be raised above 3, and you aren't allowed to take Jack-of-all-Trades.\n\nIf you don't have other players to share events with, you are allowed to make up two non-player characters with whom to share events.\n\nEquipment can be purchased after making connections.")] = CreationOperation.ESTABLISH_CONNECTION_WITH_OTHER_CHARCTER;
          dictionary[(IDescribable) new Describable("FINISH UP: Buy Equipment", "Time to retire - Go straight to buying equipment!")] = CreationOperation.PURCHASE_EQUIPMENT;
        }
        IDescribable key = currentState.decisionMaker.ChooseOne<IDescribable>("What's the next step in your character's life?", (IList<IDescribable>) new List<IDescribable>((IEnumerable<IDescribable>) dictionary.Keys));
        generationState = currentState.createNextCharacterGenerationState(dictionary[key]);
      }
      return generationState;
    }

    protected GenerationState handleNextCareerMustBe(GenerationState currentState)
    {
      GenerationState generationState = currentState;
      if (currentState.nextCarrerMustBe != null)
      {
        string nextCarrerMustBe = currentState.nextCarrerMustBe;
        generationState = currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CAREER_SPECIALIZATION);
        generationState.currentCareer = this.GetCareer(nextCarrerMustBe);
        string str;
        Presentation presentMeToUser;
        if (generationState.currentCareer == null)
        {
          str = "Oops!  It looks like you were supposed to enter the " + nextCarrerMustBe + " career. Its not available for your account and were very sorry!  You can purchase the  " + nextCarrerMustBe + " career at RPGSUITE.COM, and it will become available as soon as you login again (save now to resume here afterwards!). If you've already purchased the career, hmmm something went wrong and we apologize.  You can try logging in again to restore your cache, or contact us at rpgsuite.zendesk.com for technical assistance.  In the meantime, you can move forward by choosing a different career.";
          FeedbackStream.Send(str);
          presentMeToUser = new Presentation(nextCarrerMustBe + " Career Not Available!", str);
          generationState = currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_END_OF_TERM_ACTION);
          generationState.currentCareer = (Career) null;
        }
        else
        {
          str = "Due to the events of this term, you must now enter the " + nextCarrerMustBe + " career.";
          presentMeToUser = new Presentation("Going into the " + nextCarrerMustBe + " career", "Due to events this term, you're going directly into the " + nextCarrerMustBe + " career.  There is no qualification roll.");
        }
        FeedbackStream.Send(str);
        this.decisionMaker.present(presentMeToUser);
        generationState.nextCarrerMustBe = (string) null;
      }
      return generationState;
    }

    public virtual Character initialize(
      IList<Race> races,
      IList<ISkill> skillList,
      IList<World> worlds,
      IList<Career> careers,
      IList<Career> draftCareers,
      IList<Character> partyMembers,
      IList<Contact> peopleToMeet,
      IList<Talent> talentList)
    {
      this.races = races;
      this.skills = new Dictionary<string, ISkill>();
      foreach (ISkill skill in (IEnumerable<ISkill>) skillList)
        this.skills[skill.Name] = skill;
      this.talents = new Dictionary<string, Talent>();
      foreach (Talent talent in (IEnumerable<Talent>) talentList)
        this.talents[talent.Name] = talent;
      this.worlds = worlds;
      this.careers = careers;
      this.partyMembers = partyMembers;
      this.currentCharacter = new Character(DataManager.UserID);
      this.peopleToMeet = peopleToMeet;
      this.draftCareers = draftCareers;
      this.drifter = this.careerFilter.GetDrifterEquivalentCareer(careers);
      if (this.drifter == null)
        EngineLog.Error("Drifter career not found");
      this.Log("Returning from Character.initialize()");
      return this.currentCharacter;
    }

    public virtual GenerationState rollMishap(GenerationState currentState)
    {
      currentState.currentCareer.Mishap.handleOutcome(currentState);
      GenerationState characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.HANDLE_AGING);
      if (characterGenerationState.currentCareer is CaptiveCareer)
      {
        characterGenerationState.endCurrentCareer = false;
        characterGenerationState.nextOperation = CreationOperation.PROCESS_ADVANCEMENT_OR_COMISSION;
      }
      return characterGenerationState;
    }

    public virtual GenerationState rollEvents(GenerationState currentState)
    {
      currentState.currentCareer.Event.handleOutcome(currentState);
      GenerationState characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.PROCESS_ADVANCEMENT_OR_COMISSION);
      if (characterGenerationState.endCurrentCareer)
        characterGenerationState.nextOperation = CreationOperation.HANDLE_AGING;
      return characterGenerationState;
    }

    public virtual GenerationState handleAging(GenerationState currentState)
    {
      GenerationState characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.HANDLE_AGING);
      List<string> stringList = new List<string>();
      Character character = currentState.character;
      character.Age += 4;
      string str1 = "Do you want to stop aging? Anagathics are a highly illegal, but effective way to stop your body from growing older. They're hard to find (SOC roll 10+), but if you do, you stay the way you are now! There are a number of risks.";
      string str2 = "You are currently on Anagathics. You are " + character.Age.ToString() + " but are aging like you are a " + (character.Age - character.NumberOfYearsOnAnagathics).ToString() + ". You'll have to find more Anagathics, which is always a risk, and they are still terribly expensive (between 200,000 cr and 1.2 million cr.). They also complicate your career's survival roll. However, if you discontinue, you'll have to make two aging rolls this term, and if you do find them, they actually work! Do you want to continue on Anagathics?  ";
      Describable describable1 = new Describable("Start Anagathics!", "Delay the ravages of time, and keep your attributes right where they are. However, there are four risks you should know about:\n1) Once your on Anagathics and are forced to stop (because you're in prison, or because you can't find them) your system will go into shock, and you'll age.\n2) They are EXPENSIVE (200,000 cr -1.2 million cr.) \n3) They are ILLEGAL.   Just looking for them can send you to prison.\n4) Using Anagathics increases the likelyhood of a mishap during your term (roll twice for survival instead of once). \nSo, the risks are high, but the reward could be eternal youth.");
      Describable describable2 = new Describable("Stop Anagathics.", "The debt is too much, and even the tiniest chance you'll be going to jail is still a chance to go to jail. You will go through extra aging this term, but your chance of going through next term without a mishap is much better. You're still younger than you would be, but you're ready to age normally again.");
      Describable describable3 = new Describable("Keep Taking Anagathics!", "You know the risks well, and you're already in so much debt that at this point adding a little bit more doesn't matter anymore. You've gone this far, you might as well continue to stop aging.");
      Describable describable4 = new Describable("Maybe Later", "This term isn't the right time to start taking Anagathics, but maybe you'll need them down the road. You'll think about it for another four years.");
      Describable describable5 = new Describable("Never Use Anagathics", "The risks are too high, the money is too much. You'll never take Anagathics, and you don't want to be asked again.");
      bool onAnagathics = characterGenerationState.character.OnAnagathics;
      characterGenerationState.character.OnAnagathics = false;
      if (characterGenerationState.QueryUseOfAnagathics && currentState.currentCareer.AllowUseOfAnagathics)
      {
        string description;
        Describable describable6;
        Describable describable7;
        if (onAnagathics)
        {
          description = str2;
          describable6 = describable3;
          describable7 = describable2;
        }
        else
        {
          description = str1;
          describable6 = describable1;
          describable7 = describable4;
        }
        Describable describable8 = characterGenerationState.decisionMaker.ChooseOne<Describable>(description, (IList<Describable>) new List<Describable>()
        {
          describable6,
          describable7,
          describable5
        });
        if (describable8 == describable6)
        {
          new Event.ChallengeWithCriticalFailure((Event.Challenge) new Event.StatChallenge("Seeking Anagathics", "You try to find a supply of Anagathics. Quietly you reach out through social means to get what you need.", "Soc", 10, (IList<Outcome>) new List<Outcome>()
          {
            (Outcome) new Event.SetAnagathicsState("Taking Anagathics", "Found a supply of Anagathics for your " + characterGenerationState.character.TotalNumberOfTerms.ToString() + " term.", true)
          }, (IList<Outcome>) new List<Outcome>()
          {
            (Outcome) new Event.SetAnagathicsState("Unable to Find Anagathics", "You looked everywhere, desperately asked everyone, but you couldn't find Anagathics.", false)
          }), 2, new Outcome[3]
          {
            (Outcome) new Event.EndOfCareer("The authorities found out you're searching for Anagathics! Not only is your career over, you are even on your way to Prison."),
            (Outcome) new Outcome.NextCareerMustBe("Prison"),
            (Outcome) new Outcome.InformUser("Going to Prison!", "The authorities found out you're searching for Anagathics! Not only is your career over, you are even on your way to Prison.")
          }).handleOutcome(characterGenerationState);
        }
        else
        {
          characterGenerationState.character.OnAnagathics = false;
          if (describable8 == describable5)
            characterGenerationState.QueryUseOfAnagathics = false;
        }
      }
      if (onAnagathics && !characterGenerationState.character.OnAnagathics)
        stringList.Add("The shock to your system as the anagathics wear off causes an additional Aging Roll.");
      int num1 = characterGenerationState.character.TotalNumberOfTerms - characterGenerationState.character.NumberOfTermsOnAnagathics;
      bool flag1 = characterGenerationState.character.Race.Name.Equals("Darrian", StringComparison.CurrentCultureIgnoreCase);
      bool flag2 = currentState.character.Race.Name.ToLowerInvariant().Contains("aslan");
      int num2 = flag2 ? 40 : 34;
      int num3;
      if (character.Age >= 34 + currentState.character.NumberOfYearsOnAnagathics && !currentState.character.OnAnagathics && character.Race.Name != "Hierate Aslan")
      {
        string str3 = "Roll to determine Normal Aging Effects.";
        if (flag1)
        {
          num1 /= 2;
          str3 = str3 + "As a Darrian, you age as if you'd only had " + num1.ToString() + " terms of service rather than " + characterGenerationState.character.TotalNumberOfTerms.ToString();
        }
        else if (flag2 && character.Age >= 44)
        {
          num1 *= 2;
          str3 = str3 + "As an Aslan, you don't start aging until 40, but you age as if you'd have " + num1.ToString() + " terms of service rather than " + characterGenerationState.character.TotalNumberOfTerms.ToString();
        }
        stringList.Add(str3);
      }
      else if (character.Age >= num2 && character.Race.Name == "Hierate Aslan")
      {
        string str4 = "Roll to determine Normal Aging Effects.";
        num1 *= 2;
        string[] strArray = new string[5]
        {
          str4,
          "As an Aslan, you don't start aging until 40, but you age as if you'd have ",
          num1.ToString(),
          " terms of service rather than ",
          null
        };
        num3 = characterGenerationState.character.TotalNumberOfTerms;
        strArray[4] = num3.ToString();
        string str5 = string.Concat(strArray);
        stringList.Add(str5);
      }
      num3 = character.Age;
      Console.WriteLine("AGING To [" + num3.ToString() + "]");
      for (int index = 0; index < stringList.Count && num1 >= 1; ++index)
      {
        int num4 = Dice.TwoDiceRollEffect(num1 + 1, stringList[index], "You age gracefully.", "Time is taking it's toll on you.") + 1;
        if (num4 <= 0)
        {
          List<Outcome> outcomeList = new List<Outcome>();
          switch (num4 - -6)
          {
            case 1:
              outcomeList.Add((Outcome) new Event.SingleOutcome("Serious age problems", "Reduce three physical attributes by 2", (Outcome) new Outcome.ReducePhysicalAttribute(3, 2)));
              break;
            case 2:
              outcomeList.Add((Outcome) new Event.MultipleOutcomes("Aging", "Reduce two physical attribute by 2, and one physical attribute by 1", new Outcome[2]
              {
                (Outcome) new Outcome.ReducePhysicalAttribute(2, 2),
                (Outcome) new Outcome.ReducePhysicalAttribute(1, 1)
              }));
              break;
            case 3:
              outcomeList.Add((Outcome) new Event.MultipleOutcomes("Aging", "Reduce one physical attribute by 2, and two physical attributes by 1", new Outcome[2]
              {
                (Outcome) new Outcome.ReducePhysicalAttribute(1, 2),
                (Outcome) new Outcome.ReducePhysicalAttribute(2, 1)
              }));
              break;
            case 4:
              outcomeList.Add((Outcome) new Event.SingleOutcome("Aging", "Reduce three physical attributes by 1", (Outcome) new Outcome.ReducePhysicalAttribute(3, 1)));
              break;
            case 5:
              outcomeList.Add((Outcome) new Event.SingleOutcome("Aging", "Reduce two physical attributes by 1", (Outcome) new Outcome.ReducePhysicalAttribute(2, 1)));
              break;
            case 6:
              outcomeList.Add((Outcome) new Event.SingleOutcome("Aging well", "Reduce one physical attribute by 1", (Outcome) new Outcome.ReducePhysicalAttribute(1, 1)));
              break;
            default:
              outcomeList.Add((Outcome) new Event.MultipleOutcomes("Age health crisis", "Reduce three physical attributes by 2, and one mental attribute by 1", new Outcome[2]
              {
                (Outcome) new Outcome.ReducePhysicalAttribute(3, 2),
                (Outcome) new Outcome.ReduceMentalAttribute(1, 1)
              }));
              break;
          }
          foreach (Outcome outcome in outcomeList)
            outcome.handleOutcome(characterGenerationState);
          if (character.InCrisis)
            new Event.AgingCrisis(character.StatsInCrisis).handleOutcome(characterGenerationState);
        }
      }
      if (characterGenerationState.currentCareer is CaptiveCareer && !characterGenerationState.endCurrentCareer)
      {
        characterGenerationState.nextOperation = CreationOperation.CHOOSE_CAREER_SPECIALIZATION;
      }
      else
      {
        int num5 = !characterGenerationState.endCurrentCareer ? 0 : (!characterGenerationState.mustContinueInCareer ? 1 : 0);
        characterGenerationState.nextOperation = num5 == 0 ? CreationOperation.CHOOSE_END_OF_TERM_ACTION : CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT;
      }
      return characterGenerationState;
    }

    public virtual GenerationState generateMusteringOutBenefits(GenerationState currentState)
    {
      if (!currentState.currentCareer.Name.ToLowerInvariant().Contains(currentState.character.CurrentTerm.careerName.ToLowerInvariant()))
      {
        EngineLog.Print("ERROR CAUGHT- MAXTERM EXCEEDED?");
        GenerationState characterGenerationState = currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_END_OF_TERM_ACTION);
        characterGenerationState.currentCareer = (Career) null;
        return characterGenerationState;
      }
      int permMobModifiers = this.GetPermMOB_Modifiers(currentState);
      EngineLog.Print("In Generation State 1");
      GenerationState characterGenerationState1 = currentState.createNextCharacterGenerationState(CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT);
      Term currentTerm = characterGenerationState1.character.CurrentTerm;
      Character character = characterGenerationState1.character;
      bool flag1 = this.IsAslanFemale(character);
      bool flag2 = this.IsAslanMale(character);
      int val2 = currentState.currentCareer.getNumBenefits(character);
      List<int> intList = new List<int>();
      List<Term> termList = new List<Term>();
      foreach (Term term in character.CareerHistory)
      {
        if (currentState.currentCareer.Name == term.careerName)
          termList.Add(term);
      }
      foreach (Term term in termList)
        intList.Add(term.musteringOutBenefitRollModifierCurrentTerm);
      EngineLog.Print("In Generation State 2");
      List<AbstractCharacterCreationAlgorithm.CashDescription> collection1 = new List<AbstractCharacterCreationAlgorithm.CashDescription>();
      List<Outcome> collection2 = new List<Outcome>();
      foreach (MusteringOutBenefit mustringOutBenefit in (IEnumerable<MusteringOutBenefit>) characterGenerationState1.currentCareer.MustringOutBenefitList)
      {
        int amount = flag2 ? mustringOutBenefit.cash / 2 : mustringOutBenefit.cash;
        collection1.Add(new AbstractCharacterCreationAlgorithm.CashDescription(amount, amount.ToString() + "Cr Earned", "You managed to save " + amount.ToString() + " credits."));
        collection2.Add(mustringOutBenefit.benefit);
      }
      EngineLog.Print("In Generation State 3");
      if (characterGenerationState1.character.CurrentTerm.rank >= 5)
      {
        EngineLog.Print("In Generation State 4");
        ++permMobModifiers;
        FeedbackStream.Send("In recognition of your many years of service in the " + currentState.currentCareer.Name + " they reward you with better benefits when mustering out [+1 to all rolls]");
      }
      if (val2 < 0)
        val2 = 0;
      EngineLog.Print("In Generation State 5");
      FeedbackStream.Send("Mustering Out: You have " + val2.ToString() + " benefit" + (val2 > 1 ? "s." : "."));
      int num1;
      for (; val2 > 0; val2 = currentState.currentCareer.getNumBenefits(character))
      {
        int num2 = 0;
        if (intList.Count > 0)
        {
          num2 = intList[0];
          intList.RemoveAt(0);
        }
        int num3 = permMobModifiers;
        FeedbackStream.Send("Your benefit quality is being modified because of an event during your career.");
        try
        {
          if (num2 > 0)
            num3 += num2;
          else
            num3 = 0;
        }
        catch (Exception ex)
        {
          EngineLog.Error("what on earth... there are things called Queque and Stack.. why using IEnumerator... Exception is: " + ex.Message);
        }
        EngineLog.Print("In Generation State 5.5");
        string[] choices = new string[2]
        {
          "Cash",
          "Benefit"
        };
        string str1 = choices[1];
        int val1 = flag2 ? (character.hasSkill("Independence") ? character.getSkill("Independence").Level - character.CashRolls : 0) : 3 - character.CashRolls;
        if (flag1 || val1 > 0)
        {
          EngineLog.Print("In Generation State 5.6");
          string str2;
          if (!flag1)
          {
            num1 = Math.Min(val1, val2);
            str2 = "a maximum of " + num1.ToString() + " for cash rolls.";
          }
          else
            str2 = "any and all of them for cash rolls.";
          string str3 = str2;
          str1 = currentState.decisionMaker.ChooseOne<string>("You have " + val2.ToString() + " benefit rolls left, of which you can use " + str3 + " Would you like to roll for cash or benefits?", (IList<string>) choices);
          EngineLog.Print("In Generation State 5.7");
        }
        bool flag3 = str1 == choices[0];
        bool flag4 = character.hasSkill("Gambler") && character.getSkill("Gambler").Level >= 1;
        int benefitMobModifier = this.GetBenefitMOB_Modifier(currentState);
        int cashMobModifier = this.GetCashMOB_Modifier(currentState);
        int num4 = flag3 ? cashMobModifier : benefitMobModifier;
        string description = "Mustering Out: Roll for " + str1 + (flag4 & flag3 ? ". You get +1 from Gambling!" : ".");
        if (num3 + num4 > 0)
        {
          string str4 = description;
          num1 = num3 + num4;
          string str5 = num1.ToString();
          description = str4 + " Events and circumstances have granted you an additional " + str5 + " on this roll.";
        }
        EngineLog.Print("In Generation State 6");
        Outcome outcome;
        if (flag3)
        {
          int num5 = num3 + cashMobModifier;
          if (flag4)
            ++num5;
          character.incrementCashRolls();
          List<AbstractCharacterCreationAlgorithm.CashDescription> cashDescriptionList = new List<AbstractCharacterCreationAlgorithm.CashDescription>((IEnumerable<AbstractCharacterCreationAlgorithm.CashDescription>) collection1);
          if (num5 > 0)
          {
            for (int index = 0; index < num5; ++index)
              cashDescriptionList.RemoveAt(0);
            while (cashDescriptionList.Count < 6)
              cashDescriptionList.Add(cashDescriptionList.Last<AbstractCharacterCreationAlgorithm.CashDescription>());
          }
          else
          {
            while (cashDescriptionList.Count > 6)
              cashDescriptionList.Remove(cashDescriptionList.Last<AbstractCharacterCreationAlgorithm.CashDescription>());
          }
          Cash cash = new Cash(Dice.RollRandomResult<AbstractCharacterCreationAlgorithm.CashDescription>(description, (IList<AbstractCharacterCreationAlgorithm.CashDescription>) cashDescriptionList, ContextKeys.STRING_CHOICES).cash);
          cash.MusteringOutBenefit = true;
          outcome = (Outcome) cash;
        }
        else
        {
          List<Outcome> outcomeList = new List<Outcome>((IEnumerable<Outcome>) collection2);
          if (num3 + benefitMobModifier > 0)
          {
            for (int index = 0; index < num3; ++index)
              outcomeList.RemoveAt(0);
            while (outcomeList.Count < 6)
              outcomeList.Add(outcomeList.Last<Outcome>());
          }
          else
          {
            while (outcomeList.Count > 6)
              outcomeList.Remove(outcomeList.Last<Outcome>());
          }
          EngineLog.Print("In Generation State 7");
          outcome = Dice.RollRandomResult<Outcome>(description, (IList<Outcome>) outcomeList, ContextKeys.STRING_CHOICES);
        }
        if (outcome != null)
        {
          outcome.handleOutcome(currentState);
          currentState.currentCareer.AwardBenefit(character);
        }
        else
          EngineLog.Error("Null Cash/Benefit Roll");
      }
      EngineLog.Print("In Generation State 8");
      if (!character.Race.Name.ToLowerInvariant().Contains("hierate") && character.CurrentTerm.numberOfTermsInCareerIncludingThisOne >= 5 && !character.CurrentTerm.careerName.Equals("Scouts", StringComparison.CurrentCultureIgnoreCase) && !character.CurrentTerm.careerName.Equals("Rogue", StringComparison.CurrentCultureIgnoreCase) && !character.CurrentTerm.careerName.Equals("Drifter", StringComparison.CurrentCultureIgnoreCase) && !character.CurrentTerm.careerName.Equals("Prison", StringComparison.InvariantCultureIgnoreCase))
      {
        int yearlyIncome = 10000 + (character.CurrentTerm.numberOfTermsInCareerIncludingThisOne - 5) * 2000;
        if (character.CurrentTerm.pension_paid_out_for_this_career < yearlyIncome)
        {
          if (character.CurrentTerm.pension_paid_out_for_this_career > 0)
            character.Pension -= character.CurrentTerm.pension_paid_out_for_this_career;
          string[] strArray = new string[5]
          {
            "Congratulations! Because of your ",
            null,
            null,
            null,
            null
          };
          num1 = character.CurrentTerm.numberOfTermsInCareerIncludingThisOne * 4;
          strArray[1] = num1.ToString();
          strArray[2] = " years of service, you can draw a pension of ";
          strArray[3] = yearlyIncome.ToString();
          strArray[4] = " credits on the 1st of every year at any A or B starport.";
          string str = string.Concat(strArray);
          new Event.SingleOutcome("Pension", str, (Outcome) new Outcome.GainPension(yearlyIncome)).handleOutcome(currentState);
          character.CurrentTerm.pension_paid_out_for_this_career = yearlyIncome;
          currentState.decisionMaker.present(new Presentation("Long Service Pays Off", str));
        }
      }
      EngineLog.Print("In Generation State 9");
      currentTerm.additional_mustering_out_benefits = 0;
      characterGenerationState1.nextOperation = CreationOperation.HANDLE_INJURIES;
      characterGenerationState1.currentCareer = (Career) null;
      return characterGenerationState1;
    }

    protected bool IsAslanMale(Character character)
    {
      return character.Race.Name.ToLowerInvariant().Contains("hierate") && !character.Gender.name.ToLowerInvariant().Contains("female");
    }

    protected bool IsAslanFemale(Character character)
    {
      return character.Race.Name.ToLowerInvariant().Contains("hierate") && character.Gender.name.ToLowerInvariant().Contains("female");
    }

    protected virtual int GetPermMOB_Modifiers(GenerationState currentState) => 0;

    protected virtual int GetCashMOB_Modifier(GenerationState currentState) => 0;

    protected virtual int GetBenefitMOB_Modifier(GenerationState currentState) => 0;

    public virtual GenerationState handleInjuries(GenerationState currentState)
    {
      Character character = currentState.character;
      if (character.Injured)
      {
        com.digitalarcsystems.Traveller.DataModel.Attribute attribute1 = character.getAttribute("soc");
        if (attribute1.Injured)
          attribute1.UninjuredValue = attribute1.Value;
        IList<com.digitalarcsystems.Traveller.DataModel.Attribute> injuredStats = character.InjuredStats;
        List<Outcome> outcomes = new List<Outcome>();
        StringBuilder stringBuilder = new StringBuilder();
        int num1 = 0;
        IEnumerator<com.digitalarcsystems.Traveller.DataModel.Attribute> enumerator = injuredStats.GetEnumerator();
        bool flag;
        while (flag = enumerator.MoveNext())
        {
          com.digitalarcsystems.Traveller.DataModel.Attribute current = enumerator.Current;
          int amount = current.UninjuredValue - current.Value;
          stringBuilder.Append(current.Name + ": -" + amount.ToString() + " point" + (amount > 1 ? "s" : "") + (flag ? ", " : ". "));
          num1 += amount * 5000;
          outcomes.Add((Outcome) new Outcome.StatModified(current.Name, amount));
        }
        string[] category1 = new string[3]
        {
          "Army",
          "Navy",
          "Marines"
        };
        string[] category2 = new string[4]
        {
          "Agent",
          "Noble",
          "Scholar",
          "Entertainer"
        };
        int num2 = 0;
        int effect = Dice.D6Roll(2, character.CurrentTerm.total_ranks_in_career, "Roll to determine the amount of medical coverage you will gain from your career").effect;
        if (effect >= 4)
        {
          if (this.isInCategory(character, category1))
            num2 = effect < 8 ? 75 : 100;
          else if (this.isInCategory(character, category2))
            num2 = effect < 12 ? (effect < 8 ? 50 : 75) : 100;
          else if (effect >= 12)
            num2 = 75;
          else if (effect >= 8)
            num2 = 50;
        }
        int num3 = num1 - (int) ((double) num2 / 100.0 * (double) num1);
        if (num3 > 0)
          outcomes.Add((Outcome) new Cash(-1 * num3));
        string str = "Your out of pocket cost will be [" + num3.ToString() + "] credits to heal " + stringBuilder.ToString();
        new Event.ChoiceOutcome(1, "Medical care", "In this career, you were injured [" + stringBuilder.ToString() + "].\n Your previous employer covers (" + num2.ToString() + "%) of the cost, leaving you to pay [" + num3.ToString() + "] credits.\n Let this opportunity (and potential debt) pass, or get professional medical attention?", new Outcome[2]
        {
          (Outcome) new Event("Save money", "You pass up the opportunity to get medical attention."),
          (Outcome) new Event.MultipleOutcomes("Healing", str + "\nYou seek professional medical attention. It's time to heal!", (IList<Outcome>) outcomes)
        }).handleOutcome(currentState);
        foreach (com.digitalarcsystems.Traveller.DataModel.Attribute attribute2 in (IEnumerable<com.digitalarcsystems.Traveller.DataModel.Attribute>) injuredStats)
        {
          if (attribute2.Injured)
            attribute2.UninjuredValue = attribute2.Value;
        }
      }
      return currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_END_OF_TERM_ACTION);
    }

    public virtual GenerationState establishConnectionWithOtherCharacter(
      GenerationState currentState,
      Character newFriend)
    {
      return currentState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CONNECTION_SKILLS);
    }

    public virtual GenerationState chooseConnectionSkills(GenerationState currentState)
    {
      return currentState.createNextCharacterGenerationState(CreationOperation.SELECT_SKILL_PACKAGE);
    }

    public virtual GenerationState selectSkillPackage(GenerationState currentState)
    {
      return currentState.createNextCharacterGenerationState(CreationOperation.SELECT__SKILL_FROM_SHARED_PACKAGE);
    }

    public virtual GenerationState selectSkillFromSharedPackage(GenerationState curentState)
    {
      return this.currentState.createNextCharacterGenerationState(CreationOperation.PURCHASE_EQUIPMENT);
    }

    public virtual GenerationState purchaseEquipment(GenerationState currentState)
    {
      return currentState.createNextCharacterGenerationState(CreationOperation.FINISHED);
    }

    private bool isInCategory(Character character, string[] category)
    {
      string careerName = character.CurrentTerm.careerName;
      foreach (string str in category)
      {
        if (careerName.ToLower().Contains(str.ToLower()))
          return true;
      }
      return false;
    }

    public virtual GenerationState processBeforeNextOperation(GenerationState currentState)
    {
      return currentState;
    }

    public virtual Character generateCharacter(GenerationState restoreState)
    {
      this.currentState = restoreState;
      restoreState.careerSource = (ICareerSource) this;
      restoreState.decisionMaker = this.decisionMaker;
      restoreState.draftCareers = this.draftCareers;
      restoreState.peopleSource = (IPeopleSource) this;
      restoreState.skillSource = (ISkillSource) this;
      restoreState.recorder = (IBenefitRecorder) this;
      if (restoreState.nextOperation == CreationOperation.UNDEFINED || restoreState.nextOperation == CreationOperation.FINISHED)
        restoreState.nextOperation = CreationOperation.PURCHASE_EQUIPMENT;
      this.runEngine();
      this.Log("CREATION FINISHED, returning from ACCA");
      return this.currentState.character;
    }

    public virtual Character generateCharacter()
    {
      this.currentState = this.createInitialCharacterGenerationState();
      this.runEngine();
      this.Log("CREATION FINISHED, returning from ACCA");
      return this.currentState.character;
    }

    public void setConfiguration(Configuration setMe)
    {
      if (this.configurators == null || this.configurators.Count <= 0 || !this.configurators.ContainsKey(setMe.ConfiguratorName))
        return;
      this.configurators[setMe.ConfiguratorName].handleConfiguration(setMe);
    }

    public void setConfiguration(List<Configuration> setUs)
    {
      if (setUs == null || setUs.Count <= 0)
        return;
      setUs.ForEach((Action<Configuration>) (c => this.setConfiguration(c)));
    }

    public List<Configuration> CurrentConfiguration
    {
      get
      {
        List<Configuration> currentConfiguration = new List<Configuration>();
        if (this.configurators != null && this.configurators.Count > 0)
        {
          foreach (Configurator configurator in this.configurators.Values)
            currentConfiguration.Add(configurator.CurrentConfiguration);
        }
        return currentConfiguration;
      }
    }

    private void processConfigurators(GenerationState currentState)
    {
      if (this.configurators == null || this.configurators.Count <= 0)
        return;
      foreach (Configurator configurator in this.configurators.Values)
      {
        if (configurator.RequiresProcessBeforeNextOperation)
          configurator.processBeforeNextOperation(currentState);
      }
    }

    protected void runEngine()
    {
      while (this.currentState.nextOperation != CreationOperation.FINISHED && !this.currentState.character.hasDied())
      {
        CreationOperation nextOperation = this.currentState.nextOperation;
        this.savedState = this.currentState.createNextCharacterGenerationState(this.currentState.nextOperation);
        this.currentState.decisionMaker = this.theDecider;
        this.currentState.careerSource = (ICareerSource) this;
        this.processConfigurators(this.currentState);
        this.currentState = this.processBeforeNextOperation(this.currentState);
        if (this.currentState.nextOperation != this.savedState.nextOperation)
          this.savedState = this.currentState.createNextCharacterGenerationState(this.currentState.nextOperation);
        this.Log("switching current state to next operation, which is: " + this.currentState.nextOperation.ToString());
        switch (this.currentState.nextOperation)
        {
          case CreationOperation.SELECT_RACE:
            this.currentState = this.selectRace(this.currentState);
            break;
          case CreationOperation.GENERATE_STATS:
            EngineLog.Print("Generating stats for character -> Abstract.... is calling generateStats(currentState)");
            this.currentState = this.generateStats(this.currentState);
            break;
          case CreationOperation.CHOOSE_HOMEWORLD:
            this.currentState = this.chooseHomeworld(this.currentState);
            this.Log("Getting back to our magic launch next operation loop after choosing homeworld");
            break;
          case CreationOperation.GAIN_BACKGROUND_SKILLS:
            this.currentState = this.gainBackgroundSkills(this.currentState);
            break;
          case CreationOperation.HANDLE_PRECAREER:
            this.currentState = this.handlePreCareer(this.currentState);
            break;
          case CreationOperation.CHOOSE_CAREER:
            this.currentState = this.chooseCareer(this.currentState);
            this.Log("career choosen");
            break;
          case CreationOperation.GET_BASIC_TRAINING:
            this.currentState = this.getBasicTraining(this.currentState);
            break;
          case CreationOperation.CHOOSE_CAREER_SPECIALIZATION:
            this.currentState = this.chooseCareerSpecialization(this.currentState);
            break;
          case CreationOperation.CHOOSE_SKILLS_AND_TRAINING_TABLE_OBTAIN_SKILL:
            this.currentState = this.chooseSkillsAndTrainingTableAndObtainSkill(this.currentState);
            break;
          case CreationOperation.ROLL_FOR_SURVIVAL:
            this.currentState = this.rollForSurvival(this.currentState);
            break;
          case CreationOperation.ROLL_MISHAP:
            this.currentState = this.rollMishap(this.currentState);
            break;
          case CreationOperation.ROLL_EVENTS:
            this.currentState = this.rollEvents(this.currentState);
            break;
          case CreationOperation.PROCESS_ADVANCEMENT_OR_COMISSION:
            this.currentState = this.processAdvancementOrCommission(this.currentState);
            break;
          case CreationOperation.HANDLE_AGING:
            this.currentState = this.handleAging(this.currentState);
            break;
          case CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT:
            this.currentState = this.generateMusteringOutBenefits(this.currentState);
            break;
          case CreationOperation.HANDLE_INJURIES:
            this.currentState = this.handleInjuries(this.currentState);
            break;
          case CreationOperation.CHOOSE_END_OF_TERM_ACTION:
            this.currentState = this.chooseEndOfTermAction(this.currentState);
            break;
          case CreationOperation.ESTABLISH_CONNECTION_WITH_OTHER_CHARCTER:
            this.currentState = this.establishConnectionWithOtherCharacter(this.currentState, (Character) null);
            break;
          case CreationOperation.CHOOSE_CONNECTION_SKILLS:
            this.currentState = this.chooseConnectionSkills(this.currentState);
            break;
          case CreationOperation.SELECT_SKILL_PACKAGE:
            this.currentState = this.selectSkillPackage(this.currentState);
            break;
          case CreationOperation.SELECT__SKILL_FROM_SHARED_PACKAGE:
            this.currentState = this.selectSkillFromSharedPackage(this.currentState);
            break;
          case CreationOperation.CHOOSE_ONE_FREE_SKILL:
            this.currentState = this.chooseOneFreeSkill(this.currentState);
            break;
          case CreationOperation.PURCHASE_EQUIPMENT:
            this.currentState = this.purchaseEquipment(this.currentState);
            break;
        }
        this.Log("operation finished: " + nextOperation.ToString());
      }
    }

    protected internal virtual GenerationState createInitialCharacterGenerationState()
    {
      return new GenerationState(DataManager.UserID)
      {
        engineID = this.GetType().Name,
        character = Character.BlankCharacter,
        nextOperation = CreationOperation.SELECT_RACE,
        previousState = (GenerationState) null,
        peopleSource = (IPeopleSource) this,
        recorder = (IBenefitRecorder) this,
        skillSource = (ISkillSource) this,
        draftCareers = this.draftCareers,
        decisionMaker = this.theDecider
      };
    }

    protected internal virtual int determineNumberOfBackgroundSkills(Character character)
    {
      return 3 + character.getAttributeModifier("Edu");
    }

    protected internal virtual IList<ISkill> getBackgroundSkills(Character character)
    {
      this.Log("ABCCA: getBackgroundSkills [START]");
      List<ISkill> backgroundSkills = new List<ISkill>();
      string[] strArray = new string[17]
      {
        "Admin",
        "Animals",
        "Art",
        "Athletics",
        "Carouse",
        "Drive",
        "Electronics",
        "Flyer",
        "Language",
        "Mechanic",
        "Medic",
        "Profession",
        "Science",
        "Seafarer",
        "Streetwise",
        "Survival",
        "Vacc Suit"
      };
      foreach (string skillName in strArray)
      {
        this.Log("ABCCA: adding[" + skillName + "] as basicTrainingSkill");
        backgroundSkills.Add(this.getBasicTrainingSkill(skillName));
      }
      this.Log("ABCCA: getBackgroundSkills [FINISHED]");
      return (IList<ISkill>) backgroundSkills;
    }

    public virtual ISkill getSkill(string skillName)
    {
      if (this.skills.ContainsKey(skillName + "s"))
        skillName += "s";
      ISkill skill;
      if (!this.skills.TryGetValue(skillName, out ISkill _))
      {
        EngineLog.Error("ABCCA: making Skill(" + skillName + ")");
        skill = (ISkill) new Skill(skillName, "Unknown skill")
        {
          Level = 1
        };
      }
      else
      {
        skill = this.skills[skillName].Clone<ISkill>();
        if (skill.Cascade)
          skill.Level = 0;
        else
          skill.Level = 1;
      }
      return skill;
    }

    public virtual ISkill getBasicTrainingSkill(string skillName)
    {
      ISkill skill = this.getSkill(skillName);
      skill.Level = 0;
      return skill;
    }

    public virtual IList<ISkill> Skills
    {
      get
      {
        return (IList<ISkill>) this.skills.Select<KeyValuePair<string, ISkill>, ISkill>((Func<KeyValuePair<string, ISkill>, ISkill>) (entry => entry.Value.Clone<ISkill>())).ToList<ISkill>();
      }
    }

    public Talent getBasicTrainingTalent(string talentName)
    {
      Talent talent = this.getTalent(talentName);
      if (talent != null)
        talent.Level = 0;
      return talent;
    }

    public Talent getTalent(string talentName)
    {
      Talent copy = this.Talents.Where<Talent>((Func<Talent, bool>) (tal => tal.Name.Equals(talentName, StringComparison.InvariantCultureIgnoreCase))).FirstOrDefault<Talent>();
      if (copy == null)
        return (Talent) null;
      return new Talent(copy) { Level = 1 };
    }

    public virtual IList<Talent> Talents => DataManager.Instance.Talents;

    public virtual IList<Talent> getTalentsForTrait(com.digitalarcsystems.Traveller.DataModel.Trait trait)
    {
      return (IList<Talent>) this.getTalentsForTrait(trait.Name);
    }

    public List<Talent> getTalentsForTrait(string traitName)
    {
      List<Talent> talentsForTrait = new List<Talent>();
      foreach (Talent instance in this.talents.Values)
      {
        if (instance.associatedTrait.Name.Equals(traitName, StringComparison.InvariantCultureIgnoreCase))
          talentsForTrait.Add(instance.Clone<Talent>());
      }
      return talentsForTrait;
    }

    public virtual IList<string> getHomeWorldSkills(Character character)
    {
      List<string> homeWorldSkills = new List<string>();
      foreach (World.WorldType worldType in character.HomeWorld.worldTypes)
      {
        switch (worldType.InnerEnumValue())
        {
          case World.WorldType.InnerEnum.Agricultural:
            homeWorldSkills.Add("Animals");
            break;
          case World.WorldType.InnerEnum.Asteroid:
            homeWorldSkills.Add("Zero-G");
            break;
          case World.WorldType.InnerEnum.Desert:
            homeWorldSkills.Add("Survival");
            break;
          case World.WorldType.InnerEnum.Fluid_Oceans:
            homeWorldSkills.Add("Seafarer");
            break;
          case World.WorldType.InnerEnum.Garden:
            homeWorldSkills.Add("Animals");
            break;
          case World.WorldType.InnerEnum.High_Technology:
            homeWorldSkills.Add("Computers");
            break;
          case World.WorldType.InnerEnum.Ice_Capped:
            homeWorldSkills.Add("Vacc Suit");
            break;
          case World.WorldType.InnerEnum.Industrial:
            homeWorldSkills.Add("Trade");
            break;
          case World.WorldType.InnerEnum.Low_Population:
            homeWorldSkills.Add("Streetwise");
            break;
          case World.WorldType.InnerEnum.Low_Technology:
            homeWorldSkills.Add("Survival");
            break;
          case World.WorldType.InnerEnum.Poor:
            homeWorldSkills.Add("Animals");
            break;
          case World.WorldType.InnerEnum.Rich:
            homeWorldSkills.Add("Carouse");
            break;
          case World.WorldType.InnerEnum.Vacuum:
            homeWorldSkills.Add("Vacc Suit");
            break;
          case World.WorldType.InnerEnum.Water_World:
            homeWorldSkills.Add("Seafarer");
            break;
        }
      }
      return (IList<string>) homeWorldSkills;
    }

    public virtual GenerationState chooseOneFreeSkill(GenerationState currentState)
    {
      new Outcome.GainAnySkill().handleOutcome(currentState);
      return currentState.createNextCharacterGenerationState(CreationOperation.PURCHASE_EQUIPMENT);
    }

    public virtual void RecordBenefit(Outcome benefit, GenerationState currentState)
    {
      Term currentTerm = currentState.character.CurrentTerm;
      if (currentTerm != null)
      {
        if (currentState.nextOperation == CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT)
        {
          Event @event;
          switch (benefit)
          {
            case Event.ChoiceOutcome _:
              return;
            case Event.MusteringOutBenefitWraper _:
              @event = (Event) (benefit as Event.MusteringOutBenefitWraper);
              break;
            default:
              @event = (Event) new Event.MusteringOutBenefitWraper(benefit);
              break;
          }
          this.RecordBenefit(@event, currentState);
        }
        else
        {
          FeedbackStream.Send(benefit.ToString());
          currentTerm.benefits.Add(benefit);
        }
      }
      else
        this.Log("Got benefits before there was a term");
    }

    public virtual void RecordBenefit(Event @event, GenerationState currentState)
    {
      if (@event == null)
        throw new Exception("Event is null");
      Term currentTerm = currentState.character.CurrentTerm;
      if (currentTerm != null)
      {
        if (@event.Recorded)
          return;
        FeedbackStream.Send(@event.ToString());
        currentTerm.events.Add(@event);
        @event.Recorded = true;
      }
      else
        this.Log("Got an event before there was a term");
    }

    public virtual Contact EntityToMeet => this.getEntityToMeet(1, "").First<Contact>();

    public virtual IList<Contact> getEntityToMeet(int num, string description = "")
    {
      List<Contact> entityToMeet = new List<Contact>();
      for (int index = 0; index < num; ++index)
        entityToMeet.Add(Dice.RollRandomResult<Contact>(description, this.peopleToMeet, ContextKeys.STRING_CHOICES));
      return (IList<Contact>) entityToMeet;
    }

    public virtual IDecisionMaker decisionMaker
    {
      get => this.theDecider;
      set => this.theDecider = value;
    }

    protected void Log(string msg, bool isThisSerious = false)
    {
      msg = "AbstractChar... " + msg;
      if (isThisSerious)
        msg = ">>>ERROR: " + msg;
      EngineLog.Print(msg);
    }

    private ICareerFilter careerFilter
    {
      get
      {
        if (this.drifter == null)
          this.drifter = this._careerFilter.GetDrifterEquivalentCareer(this.careers);
        return this._careerFilter;
      }
      set
      {
        this._careerFilter = value;
        this.drifter = value.GetDrifterEquivalentCareer(this.careers);
      }
    }

    public virtual IList<Career> Careers
    {
      get
      {
        return (IList<Career>) this.careers.Where<Career>((Func<Career, bool>) (c => !(c is PreCareer) && this.careerFilter.filter(c))).ToList<Career>();
      }
    }

    public virtual Career GetCareer(string careerName)
    {
      return this.careers.Where<Career>((Func<Career, bool>) (c => c.Name.Equals(careerName, StringComparison.InvariantCultureIgnoreCase))).ToList<Career>().FirstOrDefault<Career>();
    }

    public virtual IList<Career> GetCareersByCategory(string category)
    {
      return (IList<Career>) this.careers.Where<Career>((Func<Career, bool>) (c => c.Category != null && c.Category.Equals(category, StringComparison.InvariantCultureIgnoreCase) && this.careerFilter.filter(c))).ToList<Career>();
    }

    public IList<PreCareer> PreCareers
    {
      get
      {
        if (this.preCareers == null)
        {
          this.preCareers = new List<PreCareer>();
          foreach (Career career in (IEnumerable<Career>) this.GetCareersByCategory("PRECAREER"))
          {
            if (career is PreCareer && this.careerFilter.filter(career))
              this.preCareers.Add((PreCareer) career);
          }
        }
        return (IList<PreCareer>) this.preCareers;
      }
    }

    public void SetCareerFilter(ICareerFilter filter)
    {
      if (filter == null)
        this.careerFilter = AbstractCharacterCreationAlgorithm.defaultCareerFilter;
      else
        this.careerFilter = filter;
    }

    private class CashDescription : Describable
    {
      public int cash;

      public CashDescription(int amount, string title, string description)
        : base(title, description)
      {
        this.cash = amount;
      }
    }
  }
}
