
// Type: com.digitalarcsystems.Traveller.DataModel.ISkill




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface ISkill : IAsset, IDescribable, IAssetBase, ICategorizable, IAddable<ISkill>
  {
    ISkill Parent { get; set; }

    bool Cascade { get; }

    SkillList SpecializationSkills { get; set; }

    bool IsSpecialization { get; }

    List<Attribute> GetAttributes();

    List<Attribute> Attributes { get; }
  }
}
