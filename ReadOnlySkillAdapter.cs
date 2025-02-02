
// Type: com.digitalarcsystems.Traveller.DataModel.ReadOnlySkillAdapter




using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class ReadOnlySkillAdapter : 
    ISkill,
    IAsset,
    IDescribable,
    IAssetBase,
    ICategorizable,
    IAddable<ISkill>,
    ICloneable
  {
    private ISkill skill;

    public ReadOnlySkillAdapter(ISkill wrapMe) => this.skill = wrapMe;

    public ISkill Parent
    {
      get => this.skill.Parent;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public bool Cascade => this.skill.Cascade;

    public SkillList SpecializationSkills
    {
      get => this.skill.SpecializationSkills;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public bool IsSpecialization => this.skill.IsSpecialization;

    public List<Attribute> Attributes => this.skill.Attributes;

    public Dictionary<Guid, AssetMetadata> ChildAssets
    {
      get => this.skill.ChildAssets;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public List<AssetTag> Tags
    {
      get => this.skill.Tags;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public Guid Id
    {
      get => this.skill.Id;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public List<string> Categories => this.skill.Categories;

    public int Level
    {
      get => this.skill.Level;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public string Description
    {
      get => this.skill.Description;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public string Name
    {
      get => this.skill.Name;
      set => throw new Exception("TRIED TO CHANGE A BASE SKILL");
    }

    public void Add(ISkill addMe) => this.skill.Add(addMe);

    public string AsSerialized() => this.skill.AsSerialized();

    public object Clone() => (object) this.skill.Clone<ISkill>();

    public List<Attribute> GetAttributes() => this.skill.GetAttributes();
  }
}
