
// Type: com.digitalarcsystems.Traveller.TravellerCharacterGenerator




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class TravellerCharacterGenerator
  {
    public DataManager DataManager = DataManager.Instance;

    public virtual List<ISkill> Skills { get; private set; }

    public virtual List<Contact> PeopleToMeet { get; private set; }

    public virtual List<Race> Races { get; private set; }

    public virtual List<Career> Careers { get; private set; }

    public virtual List<Career> DraftCareers { get; private set; }

    public virtual List<World> Worlds { get; private set; }

    public virtual List<Talent> Talents { get; private set; }

    public TravellerCharacterGenerator()
    {
      this.Races = new List<Race>((IEnumerable<Race>) this.DataManager.Races);
      this.Careers = new List<Career>((IEnumerable<Career>) this.DataManager.Careers);
      this.PeopleToMeet = new List<Contact>((IEnumerable<Contact>) this.DataManager.EntitiesToMeet);
      this.DraftCareers = this.Careers.Where<Career>((Func<Career, bool>) (c => c.DraftCareer)).ToList<Career>();
      this.Worlds = new List<World>((IEnumerable<World>) this.DataManager.Worlds);
      this.Skills = new List<ISkill>((IEnumerable<ISkill>) this.DataManager.Skills);
      this.Talents = new List<Talent>((IEnumerable<Talent>) this.DataManager.Talents);
    }

    public static Character produceCharacter(
      ICharacterCreationAlgorithm algorithm,
      IDecisionMaker decider)
    {
      TravellerCharacterGenerator characterGenerator = new TravellerCharacterGenerator();
      algorithm.initialize((IList<Race>) characterGenerator.Races, (IList<ISkill>) characterGenerator.Skills, (IList<World>) characterGenerator.Worlds, (IList<Career>) characterGenerator.Careers, (IList<Career>) characterGenerator.DraftCareers, (IList<Character>) new List<Character>(), (IList<Contact>) characterGenerator.PeopleToMeet, (IList<Talent>) characterGenerator.Talents);
      if (decider != null)
      {
        TravellerCharacterGenerator.Log("Set the decider");
        algorithm.decisionMaker = decider;
      }
      try
      {
        TravellerCharacterGenerator.Log("Started Algorithm");
        Character character = algorithm.generateCharacter();
        TravellerCharacterGenerator.Log("Finished TCG Algorithm");
        return character;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Exception in Engine: " + ex.Message + "\n" + ex.StackTrace);
        EngineLog.Error(ex.ToString());
        EngineLog.Error(ex.StackTrace);
        EngineLog.Error("Exception occurred");
      }
      return (Character) null;
    }

    private static void Log(string msg, bool isThisImportant = false)
    {
      string message = "TravelerCharacterGenerator: " + msg;
      if (isThisImportant)
        EngineLog.Warning(message);
      else
        EngineLog.Print(message);
    }
  }
}
