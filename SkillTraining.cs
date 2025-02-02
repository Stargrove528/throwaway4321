
// Type: com.digitalarcsystems.Traveller.DataModel.SkillTraining




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class SkillTraining
  {
    public Skill SkillBeingTraining { get; private set; }

    public int DesiredSkillLevel { get; private set; }

    public int NOW_Studied { get; private set; }

    public int TotalNOW_Required => this.DesiredSkillLevel;

    public int NOW_Remaining => this.TotalNOW_Required - this.NOW_Studied;

    public bool IsComplete => this.NOW_Remaining <= 0;

    public SkillTraining(Skill skillNameToTrain, int desiredSkillLevel, int currentSkillLevel)
    {
      this.SkillBeingTraining = skillNameToTrain;
      this.DesiredSkillLevel = desiredSkillLevel;
      if (this.DesiredSkillLevel - currentSkillLevel > 1)
        throw new ArgumentException("CurrentSkillLevel can't be more than 1 level higher than DesiredSkillLevel.  You must train up one level at a time.");
      this.NOW_Studied = 0;
    }

    public string getAttributeNameForStatChallenge()
    {
      if (this.SkillBeingTraining.Name.Equals("Dexterity", StringComparison.InvariantCultureIgnoreCase))
        return "Dex";
      if (this.SkillBeingTraining.Name.Equals("Strength", StringComparison.InvariantCultureIgnoreCase))
        return "Str";
      return this.SkillBeingTraining.Name.Equals("Endurance", StringComparison.InvariantCultureIgnoreCase) ? "End" : "Edu";
    }

    public bool increment()
    {
      bool flag = false;
      if (this.NOW_Remaining > 0)
        ++this.NOW_Studied;
      if (this.NOW_Remaining <= 0)
        flag = true;
      return flag;
    }
  }
}
