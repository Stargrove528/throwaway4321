
// Type: com.digitalarcsystems.Traveller.DataModel.CustomSkill




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public sealed class CustomSkill : 
    ISkill,
    IAsset,
    IDescribable,
    IAssetBase,
    ICategorizable,
    IAddable<ISkill>,
    INonLicensedAsset
  {
    [JsonProperty]
    private readonly List<Attribute> _attributes;

    public List<string> Categories { get; private set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public int CreatingUser { get; set; }

    public string DefaultFileName => this.Id.ToString("D") + ".skjson";

    public string Description { get; set; }

    public Guid Id { get; set; }

    public int Level { get; set; }

    public string Name { get; set; }

    public List<AssetTag> Tags { get; set; }

    public void Add(ISkill addMe)
    {
      if (!this.Equals((object) addMe))
        return;
      this.Level += addMe.Level;
    }

    public List<Attribute> GetAttributes()
    {
      return new List<Attribute>((IEnumerable<Attribute>) this._attributes);
    }

    public List<Attribute> Attributes => this.GetAttributes();

    public override bool Equals(object amIEqual)
    {
      bool flag = false;
      if (amIEqual is CustomSkill)
        flag = string.Compare(this.Name, ((CustomSkill) amIEqual).Name, StringComparison.CurrentCultureIgnoreCase) == 0;
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 47;
      return string.IsNullOrEmpty(this.Name) ? num * 23 : num * 23 + this.Name.GetHashCode();
    }

    [JsonConstructor]
    public CustomSkill()
    {
      this.Name = "Custom Skill";
      this.Description = "This is a custom skill that you can define.";
      this.Tags = new List<AssetTag>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.Categories = new List<string>();
      this.Id = Guid.NewGuid();
      this.CreatingUser = DataManager.UserID;
      this._attributes = new List<Attribute>();
      if (this.CreatingUser == 0)
        this.CreatingUser = 1;
      this.SpecializationSkills = new SkillList();
    }

    public CustomSkill(string name, string description, int creatingUser, ISkill parent, Guid id = default (Guid))
    {
      this.Tags = new List<AssetTag>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.Categories = new List<string>();
      this.Name = name;
      this.Description = description;
      this.SetParent(parent);
      this.CreatingUser = creatingUser;
      this.Id = id != new Guid() ? id : Guid.NewGuid();
      this._attributes = parent.GetAttributes() != null ? new List<Attribute>((IEnumerable<Attribute>) parent.GetAttributes()) : new List<Attribute>();
      this.SpecializationSkills = new SkillList();
      this.Parent = parent;
    }

    public List<ISkill> _cascadeSkills { get; set; }

    public ISkill Parent { get; set; }

    public bool Cascade => this._cascadeSkills != null && this._cascadeSkills.Count > 0;

    public SkillList SpecializationSkills { get; set; }

    public bool IsSpecialization { get; private set; }

    public CustomSkill(List<Skill> cascades, Skill parent, List<Skill> specializations)
    {
      this._cascadeSkills = new List<ISkill>();
      foreach (ISkill cascade in cascades)
        this._cascadeSkills.Add(cascade);
      this.Parent = (ISkill) parent;
      this._attributes = parent.GetAttributes() != null ? new List<Attribute>((IEnumerable<Attribute>) parent.GetAttributes()) : new List<Attribute>();
      this.SpecializationSkills = new SkillList();
      foreach (ISkill specialization in specializations)
        this.SpecializationSkills.Add(specialization);
    }

    public static explicit operator CustomSkill(FreeFormSkill skill)
    {
      return skill.CreateCustomSkill(skill.Name, skill.Description);
    }
  }
}
