
// Type: com.digitalarcsystems.Traveller.RandomCharacterCreationAlgorithm




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using com.digitalarcsystems.Traveller.utility;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class RandomCharacterCreationAlgorithm : AbstractCharacterCreationAlgorithm, IDecisionMaker
  {
    protected Career _career = (Career) null;

    public Career PreferredCareer
    {
      get => this._career;
      set => this._career = value;
    }

    public ISkill PrioritySkill { get; set; }

    public int PrioritySkillLevel { get; set; }

    public Race PreferredRace { get; set; }

    public Gender PreferredGender { get; set; }

    public bool AutomaticallyBuyEquipment { get; set; }

    public int CreditsForEquipment { get; set; }

    public int CreditsOnHand { get; set; }

    public bool Psionic { get; set; }

    public bool ForceTerms { get; set; }

    public virtual int MaxTerms { get; set; }

    public RandomCharacterCreationAlgorithm()
    {
      this.MaxTerms = 8;
      this.theDecider = (IDecisionMaker) this;
    }

    public RandomCharacterCreationAlgorithm(
      int numTerms,
      Career career,
      Skill prioritySkill,
      int skillLevel,
      Race race,
      Gender gender,
      bool psionic,
      bool forceTerms)
    {
      bool flag = race.Name.ToLowerInvariant().Contains("aslan");
      this.MaxTerms = numTerms;
      this.theDecider = (IDecisionMaker) this;
      this.PreferredCareer = career;
      this.PrioritySkill = (ISkill) prioritySkill;
      this.PrioritySkillLevel = skillLevel;
      this.PreferredRace = race;
      this.PreferredGender = gender;
      this.Psionic = psionic;
      this.ForceTerms = forceTerms;
      this.careers = (IList<Career>) new List<Career>()
      {
        career
      };
      this.races = (IList<Race>) new List<Race>() { race };
      if (flag && career.Category.ToLowerInvariant().Contains("aslan"))
        this.drifter = DataManager.Instance.Careers.FirstOrDefault<Career>((Func<Career, bool>) (c => c.Name.ToLowerInvariant().Equals("outcast")));
      else
        this.drifter = DataManager.Instance.Careers.FirstOrDefault<Career>((Func<Career, bool>) (c => c.Name.ToLowerInvariant().Equals("drifter")));
    }

    public override GenerationState processBeforeNextOperation(GenerationState currentState)
    {
      string str = currentState.character.CurrentTerm == null || currentState.character.CurrentTerm.specializationName == null ? "NONE" : currentState.character.CurrentTerm.specializationName;
      Console.WriteLine("Next state: [" + currentState.nextOperation.ToString() + "], CurrentCareer: " + currentState.currentCareer?.ToString() + ", Specialization [" + str + "] Current Age: " + currentState.character.Age.ToString() + " Term: " + currentState.character.TotalNumberOfTerms.ToString() + " Rank: " + (currentState.character.CurrentTerm != null ? currentState.character.CurrentTerm.rank.ToString() ?? "" : "No Term"));
      if (currentState.character.CurrentTerm != null && currentState.character.Age > 18 + 4 * currentState.character.TotalNumberOfTerms)
        Console.WriteLine("=-=-=-=--=-=- AGING ERROR =-=-=-=-=-=-");
      if (currentState.character.getAttributes().Any<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (stat => !stat.Name.ToLower().Contains("psi") && stat.UninjuredValue <= 0)))
        Console.WriteLine("===== Uninjured Stat <= zero ======");
      if (this.ForceTerms && currentState.QueryUseOfAnagathics)
        currentState.QueryUseOfAnagathics = false;
      return currentState;
    }

    public override GenerationState selectRace(GenerationState currentState)
    {
      if (this.PreferredRace != null)
      {
        this.PreferredRace = this.PreferredRace.Clone<Race>();
        IList<Race> races = this.races;
        this.races = (IList<Race>) new List<Race>()
        {
          this.PreferredRace
        };
      }
      GenerationState generationState = base.selectRace(currentState);
      if (this.PreferredGender != null)
        generationState.character.Gender = this.PreferredGender;
      return generationState;
    }

    public override GenerationState chooseCareer(GenerationState currentState)
    {
      Career career1 = currentState.currentCareer;
      if (this.ForceTerms)
      {
        career1 = this.PreferredCareer;
      }
      else
      {
        int num = 0;
        while (career1 == null && num++ < 20)
        {
          Career career2 = this.PreferredCareer == null || !this.Careers.Contains(this.PreferredCareer) ? Utility.pickRandomOne<Career>((IList<Career>) this.Careers.Where<Career>((Func<Career, bool>) (c => !(c is PreCareer) && !(c is CaptiveCareer))).ToList<Career>()) : this.PreferredCareer;
          if (!currentState.character.haveHadCareer(career2) || career2.Name.Equals("Drifter", StringComparison.CurrentCultureIgnoreCase) || !(career2 is PreCareer))
          {
            if (career2.Name.Equals("University", StringComparison.InvariantCultureIgnoreCase))
              EngineLog.Print("We hit university, wrongly.  Why?");
            career1 = career2;
          }
        }
        if (career1 == null)
        {
          Console.WriteLine("Not Enough Careers");
          return currentState.createNextCharacterGenerationState(CreationOperation.FINISHED);
        }
      }
      int qualificationModifier = currentState.getQualificationModifier(career1.Name);
      currentState.clearQualificationModifier(career1.Name);
      if ((career1.Name.Equals("Psion", StringComparison.InvariantCultureIgnoreCase) || this.Psionic) && !currentState.character.hasTrait(com.digitalarcsystems.Traveller.DataModel.Trait.PSIONIC))
      {
        new Event.TestForTalent(com.digitalarcsystems.Traveller.DataModel.Trait.PSIONIC).handleOutcome(currentState);
        if (currentState.character.getAttribute(6).UninjuredValue <= 0)
        {
          com.digitalarcsystems.Traveller.DataModel.Attribute attribute = currentState.character.getAttribute(6);
          int num = Math.Max(1, attribute.Value);
          attribute.Value = num;
          attribute.UninjuredValue = num;
        }
      }
      if (!this.ForceTerms && !career1.qualifyForCareer(currentState.character, qualificationModifier))
      {
        if (!currentState.character.hasAlreadyBeenDrafted())
        {
          career1 = Utility.pickRandomOne<Career>(this.draftCareers);
          currentState.character.setDrafted();
        }
        else
          career1 = this.drifter;
      }
      CreationOperation nextOperation = CreationOperation.CHOOSE_CAREER_SPECIALIZATION;
      GenerationState characterGenerationState = currentState.createNextCharacterGenerationState(nextOperation);
      characterGenerationState.currentCareer = career1;
      EngineLog.Print("Career Chosen: " + career1.Name);
      Console.WriteLine("Career Chosen: " + career1.Name);
      return characterGenerationState;
    }

    public override GenerationState rollForSurvival(GenerationState currentState)
    {
      return !this.ForceTerms || currentState.character.TotalNumberOfTerms - 1 > this.MaxTerms ? base.rollForSurvival(currentState) : currentState.createNextCharacterGenerationState(CreationOperation.ROLL_EVENTS);
    }

    public override GenerationState chooseEndOfTermAction(GenerationState currentState)
    {
      if (currentState.mustContinueInCareer && currentState.currentCareer == null)
        EngineLog.Error("We have a problem.  Must Continue in Career, but Career Null");
      if (this.ForceTerms && this.PreferredCareer != null && currentState.currentCareer != null && this.currentCharacter.TotalNumberOfTerms - 1 <= this.MaxTerms)
      {
        currentState.mustContinueInCareer = true;
        currentState.endCurrentCareer = false;
      }
      GenerationState generationState = base.chooseEndOfTermAction(currentState);
      try
      {
        if (generationState.mustContinueInCareer)
          EngineLog.Error("We have a problem.  Shouldn't have a MustContinueInCareer after base chooses end of term action.");
        if (generationState.character.TotalNumberOfTerms - 1 >= this.MaxTerms)
        {
          if (generationState.mustContinueInCareer)
            generationState.mustContinueInCareer = false;
          generationState = generationState.currentCareer == null ? generationState.createNextCharacterGenerationState(CreationOperation.ESTABLISH_CONNECTION_WITH_OTHER_CHARCTER) : generationState.createNextCharacterGenerationState(CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT);
        }
        else if (generationState.nextOperation != CreationOperation.CHOOSE_CAREER_SPECIALIZATION && generationState.nextOperation != CreationOperation.CHOOSE_CAREER && generationState.nextOperation != CreationOperation.HANDLE_PRECAREER && generationState.nextOperation != CreationOperation.CHOOSE_END_OF_TERM_ACTION && generationState.currentCareer == null)
          generationState = generationState.createNextCharacterGenerationState(CreationOperation.CHOOSE_CAREER);
        if (generationState.mustContinueInCareer)
          EngineLog.Error("We have a problem. Shouldn't end chooseEndOfTermAction with MustContinueInCareer.");
      }
      catch (Exception ex)
      {
        EngineLog.Error(ex.StackTrace);
      }
      return generationState;
    }

    public override GenerationState establishConnectionWithOtherCharacter(
      GenerationState currentState,
      Character newFriend)
    {
      currentState.createNextCharacterGenerationState(CreationOperation.PURCHASE_EQUIPMENT);
      return base.establishConnectionWithOtherCharacter(currentState, (Character) null);
    }

    public override GenerationState selectSkillPackage(GenerationState currentState)
    {
      GenerationState generationState = base.selectSkillPackage(currentState);
      Character character = generationState.character;
      if (this.PrioritySkill != null)
      {
        if (!character.hasSkill(this.PrioritySkill))
          new Outcome.GainSkill(this.PrioritySkill).handleOutcome(currentState);
        ISkill skill = character.getSkill(this.PrioritySkill);
        if (skill.Level < this.PrioritySkillLevel)
          skill.Level = this.PrioritySkillLevel;
      }
      return generationState;
    }

    public IList<T> ResolveAllocation<T>(int quantity_to_allocate, IList<T> allocations) where T : IAllocatable
    {
      throw new NotImplementedException();
    }

    public virtual IList<T> Choose<T>(int num_to_choose, IList<T> choices)
    {
      List<T> objList = new List<T>();
      if (choices.Count <= num_to_choose)
        return choices;
      for (int index = 0; index < num_to_choose && choices.Count > 0; ++index)
      {
        T obj = Utility.pickRandomOne<T>(choices);
        objList.Add(obj);
        choices.Remove(obj);
      }
      return (IList<T>) objList;
    }

    public void setInfoForUI(CreationOperation currentOperation)
    {
    }

    public virtual T ChooseOne<T>(IList<T> choices)
    {
      try
      {
        if (choices.Any<T>((Func<T, bool>) (ch => ch.ToString().Contains("Benefit"))))
          return choices.Where<T>((Func<T, bool>) (ch => ch.ToString().Contains("Benefit"))).FirstOrDefault<T>();
        if (choices.Any<T>((Func<T, bool>) (ch => ch.ToString().Contains("University"))))
          return choices.Where<T>((Func<T, bool>) (ch => ch.ToString().Contains("University"))).FirstOrDefault<T>();
        if (choices.Any<T>((Func<T, bool>) (ch => (object) ch is ISkill)))
        {
          bool flag1 = choices.Any<T>((Func<T, bool>) (ch => ((IDescribable) (object) ch).Name.ToLowerInvariant().Contains("slug")));
          bool flag2 = choices.Any<T>((Func<T, bool>) (ch => ((IDescribable) (object) ch).Name.ToLowerInvariant().Contains("energy")));
          if (flag1 & flag2)
            return new Random().Next() % 2 == 0 ? choices.Where<T>((Func<T, bool>) (ch => ((IDescribable) (object) ch).Name.ToLowerInvariant().Contains("slug"))).FirstOrDefault<T>() : choices.Where<T>((Func<T, bool>) (ch => ((IDescribable) (object) ch).Name.ToLowerInvariant().Contains("energy"))).FirstOrDefault<T>();
          if (flag1 && !flag2)
            return choices.Where<T>((Func<T, bool>) (ch => ((IDescribable) (object) ch).Name.ToLowerInvariant().Contains("slug"))).FirstOrDefault<T>();
          if (flag2 && !flag1)
            return choices.Where<T>((Func<T, bool>) (ch => ((IDescribable) (object) ch).Name.ToLowerInvariant().Contains("energy"))).FirstOrDefault<T>();
        }
        T obj = Utility.pickRandomOne<T>(choices);
        if ((object) obj is Event.Death && choices.Count<T>() > 1)
          obj = choices.IndexOf(obj) <= 0 ? choices.ElementAt<T>(1) : choices.ElementAt<T>(0);
        return obj;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public virtual T ChooseOne<T>(string description, IList<T> choices)
    {
      EngineLog.Print("RandomChoiceGenerator.chooseOne<T>() - desc: " + description);
      return this.ChooseOne<T>(choices);
    }

    public virtual IList<T> Choose<T>(string description, int num_to_choose, IList<T> choices)
    {
      EngineLog.Print("RandomChoiceGenerator.choose<T>() - desc: " + description);
      return this.Choose<T>(num_to_choose, choices);
    }

    public virtual void setQueryKey(ContextKeys key)
    {
    }

    public virtual void setResultKey(ContextKeys key)
    {
    }

    public IDescribable provideFreeString(
      string description,
      string title = "",
      RequiredInformation requiredInfo = RequiredInformation.DESCRIPTION)
    {
      return (IDescribable) new Describable("", "Manual Information");
    }

    public void present(Presentation presentMeToUser)
    {
      if (this.decisionMaker == this)
        return;
      this.decisionMaker.present(presentMeToUser);
    }

    public override GenerationState purchaseEquipment(GenerationState currentState)
    {
      Console.WriteLine("Start of purchase Equipment");
      Console.WriteLine("Character: " + currentState.character.Race?.ToString() + " - " + currentState.character.Gender?.ToString());
      Random random = new Random();
      if (this.AutomaticallyBuyEquipment)
      {
        Console.WriteLine("Automatically Buying equipment");
        Character character = currentState.character;
        character.Credits = this.CreditsForEquipment;
        bool flag = false;
        List<ISkill> list1 = character.Skills.OrderByDescending<ISkill, int>((Func<ISkill, int>) (s => s.Level)).ToList<ISkill>();
        Console.WriteLine("Credits: " + character.Credits.ToString());
        foreach (ISkill skill1 in list1)
        {
          ISkill skill = skill1;
          if (character.Credits > 20)
          {
            Console.WriteLine("Buying for " + skill.Name);
            if (!this.isASillySkill(skill) || skill.Level > 0)
            {
              try
              {
                if (!flag && skill.Categories != null && skill.Categories.Contains("Combat") && !skill.Categories.Contains("starship"))
                {
                  Console.WriteLine("Buying Armor");
                  List<Armor> list2 = DataManager.Instance.Equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e is Armor && ((com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment) e).TechLevel >= 8 && e.Cost <= character.Credits)).Select<IEquipment, Armor>((Func<IEquipment, Armor>) (e => (Armor) e)).ToList<Armor>();
                  if (list2.Any<Armor>())
                  {
                    list2.OrderByDescending<Armor, int>((Func<Armor, int>) (a => a.Cost));
                    Armor eq = list2[0];
                    this.buyEqIfUnowned(character, (IEquipment) eq);
                    flag = true;
                    Console.WriteLine("Bought armor - " + eq.Name);
                  }
                }
              }
              catch (Exception ex)
              {
                throw ex;
              }
              List<IEquipment> list3 = DataManager.Instance.Equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Skill != null && (e.SubSkill.ToLower().Equals(skill.Name.ToLower()) || e.Skill.ToLower().Equals(skill.Name.ToLower())) && e.Cost <= character.Credits)).ToList<IEquipment>();
              int num;
              List<IEquipment> list4;
              if (this.skillTechLevel(skill) <= 8)
              {
                Console.WriteLine("In if line 518 RCCA");
                string name1 = skill.Name;
                num = this.skillTechLevel(skill);
                string str1 = num.ToString();
                Console.WriteLine("skillTechLevel(" + name1 + "): " + str1);
                Console.WriteLine("Possible Equipment List Before: ");
                foreach (IEquipment equipment in list3)
                {
                  string name2 = equipment.Name;
                  num = equipment.TechLevel;
                  string str2 = num.ToString();
                  Console.WriteLine(name2 + ": TL " + str2);
                }
                list4 = list3.Where<IEquipment>((Func<IEquipment, bool>) (e => e.TechLevel >= this.skillTechLevel(skill) - 2)).ToList<IEquipment>();
                Console.WriteLine("Possible Equipment List After: ");
                foreach (IEquipment equipment in list4)
                  Console.WriteLine(equipment.Name + ": TL " + equipment.TechLevel.ToString());
              }
              else
              {
                Console.WriteLine("In else line 535 RCCA");
                string name3 = skill.Name;
                num = this.skillTechLevel(skill);
                string str3 = num.ToString();
                Console.WriteLine("skillTechLevel(" + name3 + "): " + str3);
                Console.WriteLine("Possible Equipment List Before: ");
                foreach (IEquipment equipment in list3)
                {
                  string name4 = equipment.Name;
                  num = equipment.TechLevel;
                  string str4 = num.ToString();
                  Console.WriteLine(name4 + ": TL " + str4);
                }
                list4 = list3.Where<IEquipment>((Func<IEquipment, bool>) (e => e.TechLevel >= 8)).ToList<IEquipment>();
                Console.WriteLine("Possible Equipment List After: ");
                foreach (IEquipment equipment in list4)
                  Console.WriteLine(equipment.Name + ": TL " + equipment.TechLevel.ToString());
              }
              num = list4.Count;
              Console.WriteLine("possibleEquipment.Count: " + num.ToString());
              if (list4.Any<IEquipment>())
              {
                IEquipment eq = list4.ElementAt<IEquipment>(random.Next(0, list4.Count - 1));
                Console.WriteLine("Trying to buy " + eq.Name);
                this.buyEqIfUnowned(character, eq);
              }
            }
          }
          else
            break;
        }
        character.Credits = this.CreditsOnHand;
      }
      return currentState.createNextCharacterGenerationState(CreationOperation.FINISHED);
    }

    private void buyEqIfUnowned(Character character, IEquipment eq)
    {
      if (!character.Equipment.Contains(eq))
      {
        character.Credits -= eq.Cost;
        if (eq is Armor || eq is IWeapon)
          character.Equip(eq, true);
        else
          character.addEquipment(eq, true);
      }
      Console.WriteLine("Buying " + eq.Name);
    }

    public int skillTechLevel(ISkill skill)
    {
      int num = 0;
      foreach (IEquipment equipment in (IEnumerable<IEquipment>) DataManager.Instance.Equipment)
      {
        if (equipment.Skill != null && (equipment.Skill.ToLower().Equals(skill.Name.ToLower()) || equipment.SubSkill.ToLower().Equals(skill.Name.ToLower())) && equipment.TechLevel > num)
          num = equipment.TechLevel;
      }
      return num;
    }

    public bool isASillySkill(ISkill skill)
    {
      string lowerInvariant = skill.Name.ToLowerInvariant();
      return skill.Cascade || lowerInvariant.Equals("bola") || lowerInvariant.Equals("monofilament bola") || lowerInvariant.Equals("boomerang") || lowerInvariant.Equals("net") || lowerInvariant.Equals("archaic") || lowerInvariant.Equals("whip") || lowerInvariant.Equals("shock whip") || lowerInvariant.Equals("natural");
    }

    public string ArrayAsSingleLine<T>(T[] arr)
    {
      string str = "{" + arr[0].ToString();
      for (int index = 1; index < arr.Length; ++index)
        str = str + ", " + arr[index].ToString();
      return str + "}";
    }
  }
}
