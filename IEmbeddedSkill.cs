
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IEmbeddedSkill




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IEmbeddedSkill
  {
    string EmbeddedSkillName { get; set; }

    int EmbeddedSkillLevel { get; set; }

    bool MustBeAlreadyPossessed { get; }
  }
}
