
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.AgentProgram




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class AgentProgram : Software
  {
    public AgentProgram()
    {
    }

    public AgentProgram(Software copyMe)
      : base(copyMe)
    {
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      Attribute stat)
    {
      int num = 0;
      if (skillName.ToLowerInvariant().Contains("computers") && stat.Ordinal > 2 && difficulty <= 2 * this.CurrentRating + 8)
        num = -1 * charactersSkillLevel + -1 * stat.Modifier + this.CurrentRating;
      return num;
    }
  }
}
