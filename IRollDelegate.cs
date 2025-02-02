
// Type: com.digitalarcsystems.Traveller.IRollDelegate




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IRollDelegate
  {
    int StatRoll(Attribute stat, int targetNumber, string description);

    int SkillRoll(Attribute stat, Skill skill, int targetNumber, string description);

    int Roll(int targetNumber, string description);

    int RollD6(int diceCount, int modifier, string description);

    int GetRandomNumber(int numChoices, int minimumNumber, string description);

    RollEffect AnimateRoll(RollEffect setting);
  }
}
