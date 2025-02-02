
// Type: com.digitalarcsystems.Traveller.RollParam




using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class RollParam
  {
    public int numOfDices = 2;
    public Attribute attribute = (Attribute) null;
    public ISkill skill = (ISkill) null;
    public string description = string.Empty;
    public int totalModifier = 0;
    public int rawMinSuccessValue = 0;
    public bool isToBeAnimated = true;
    public RollType rollType = RollType.NORMAL;
    public Context possibleResultsAsContext = (Context) null;

    public void AddResultDescriptions(string forSuccess, string forFailure)
    {
      if (this.possibleResultsAsContext == null)
        this.possibleResultsAsContext = new Context();
      this.possibleResultsAsContext.Add(ContextKeys.SUCCESS, (object) forSuccess);
      this.possibleResultsAsContext.Add(ContextKeys.FAILURE, (object) forFailure);
    }

    [JsonConstructor]
    public RollParam()
    {
    }

    public RollParam(Attribute stat, int targetNumber, string description)
    {
      this.attribute = stat;
      this.description = description;
      this.totalModifier = stat.Modifier;
      this.rawMinSuccessValue = targetNumber;
    }

    public RollParam(Attribute stat, int targetNumber, string description, RollType type)
      : this(stat, targetNumber, description)
    {
      this.rollType = type;
    }

    public RollParam(ISkill skill, Attribute stat, int targetNumber, string description)
      : this(stat, targetNumber, description)
    {
      int num = skill == null ? -3 : skill.Level;
      this.skill = skill;
      this.totalModifier += num;
    }

    public RollParam(
      ISkill skill,
      Attribute stat,
      int targetNumber,
      string description,
      RollType type)
      : this(stat, targetNumber, description, type)
    {
      int num = skill == null ? -3 : skill.Level;
      this.skill = skill;
      this.totalModifier += num;
    }

    public RollParam(Attribute stat, int targetNumber, int modifier, string description)
      : this(stat, targetNumber, description)
    {
      this.totalModifier += modifier;
    }

    public RollParam(
      Attribute stat,
      int targetNumber,
      int modifier,
      string description,
      RollType type)
      : this(stat, targetNumber, description, type)
    {
      this.totalModifier += modifier;
    }

    public RollParam(
      ISkill skill,
      Attribute stat,
      int targetNumber,
      int modifier,
      string description)
      : this(skill, stat, targetNumber, description)
    {
      this.totalModifier += modifier;
    }

    public RollParam(
      ISkill skill,
      Attribute stat,
      int targetNumber,
      int modifier,
      string description,
      RollType type)
      : this(skill, stat, targetNumber, description, type)
    {
      this.totalModifier += modifier;
    }
  }
}
