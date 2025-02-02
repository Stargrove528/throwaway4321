
// Type: com.digitalarcsystems.Traveller.DataModel.Skill




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  [Serializable]
  public class Skill : 
    ILicensedAsset,
    IAssetBase,
    ISkill,
    IAsset,
    IDescribable,
    ICategorizable,
    IAddable<ISkill>
  {
    [JsonProperty]
    private int _level;
    private string _description;

    [JsonProperty]
    public List<Attribute> Attributes { get; set; }

    [JsonIgnore]
    public bool Cascade => this._cascadeSkills != null && this._cascadeSkills.Count > 0;

    [JsonProperty]
    public List<string> Categories { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    [JsonProperty]
    public string Description
    {
      get => this._description;
      set => this._description = value;
    }

    [JsonProperty]
    public Guid Id { get; set; }

    [JsonIgnore]
    public int Level
    {
      get => this._level;
      set
      {
        this._level = !this.Cascade || value <= 0 ? value : throw new InvalidOperationException(this.Name + ": is a cascade skill and may not have a level higher than 0.");
      }
    }

    [JsonProperty]
    public string Name { get; set; }

    [JsonProperty]
    public ISkill Parent { get; set; }

    [JsonConstructor]
    public Skill(Skill parent, SkillList cascades)
    {
      this.Parent = (ISkill) parent;
      this._cascadeSkills = new SkillList();
      if (cascades == null)
        return;
      foreach (ISkill cascade in (List<ISkill>) cascades)
        this._cascadeSkills.Add(cascade);
    }

    [JsonIgnore]
    public bool IsSpecialization => this.Parent != null;

    [JsonIgnore]
    public SkillList SpecializationSkills
    {
      get
      {
        if (this._cascadeSkills == null)
          return new SkillList();
        List<ISkill> list = this._cascadeSkills.Select<ISkill, ISkill>((Func<ISkill, ISkill>) (x => x.Clone<ISkill>())).ToList<ISkill>();
        SkillList specializationSkills = new SkillList();
        specializationSkills.AddRange((IEnumerable<ISkill>) list);
        return specializationSkills;
      }
      set => this._cascadeSkills = value;
    }

    public List<AssetTag> Tags { get; set; }

    public static bool operator !=(Skill a, Skill b) => !(a == b);

    public static bool operator ==(Skill a, Skill b)
    {
      return (object) a == null || (object) b == null ? (object) a == (object) b : a.Equals((object) b);
    }

    public void Add(ISkill addMe)
    {
      if (addMe.Name == null || !addMe.Name.Equals(this.Name, StringComparison.CurrentCultureIgnoreCase))
        return;
      this.Level += addMe.Level;
    }

    public override bool Equals(object amIEqual)
    {
      bool flag = false;
      if (amIEqual is Skill && ((Skill) amIEqual).Id == this.Id)
        flag = true;
      return flag;
    }

    public List<Attribute> GetAttributes()
    {
      return this.Attributes != null && this.Attributes.Count > 0 ? this.Attributes.Select<Attribute, Attribute>((Func<Attribute, Attribute>) (x => x.Clone<Attribute>())).ToList<Attribute>() : new List<Attribute>();
    }

    public override int GetHashCode() => this.Name != null ? this.Name.GetHashCode() : 0;

    public Attribute GetPrimaryAttribute()
    {
      return this.Attributes.FirstOrDefault<Attribute>().Clone<Attribute>();
    }

    public void Increment() => ++this.Level;

    public override string ToString() => this.Name + ": " + this.Level.ToString();

    public Skill(string name, string description, int level)
      : this(name, description)
    {
      this.Level = level >= 0 ? level : throw new ArgumentException("Skill levels can't be negative [" + name + ", " + description + ", cascade: " + this.Cascade.ToString() + ", level: " + level.ToString());
    }

    public Skill()
    {
      this.Categories = new List<string>();
      this.Attributes = new List<Attribute>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
    }

    public Skill(string name, string description)
    {
      this.Categories = new List<string>();
      if (name == null)
        throw new ArgumentException("Skills must have names [, " + description + ", cascade: " + this.Cascade.ToString() + "]");
      if (description == null)
        description = "";
      this.Name = name;
      this.Description = description;
      this._cascadeSkills = new SkillList();
    }

    public Skill(string name, string description, List<Attribute> attributes)
    {
      this.Categories = new List<string>();
      this.Name = name;
      this.Description = description;
      this.Attributes = attributes;
    }

    public Skill(
      string name,
      string description,
      List<Attribute> attributes,
      List<ISkill> specialties)
    {
      this.Name = name;
      this.Description = description;
      this.Attributes = attributes;
      this._cascadeSkills = (SkillList) specialties;
    }

    public Skill(string name, string description, params ISkill[] specialties)
    {
      this.Categories = new List<string>();
      this.Name = name;
      this.Description = description;
      this._cascadeSkills.CopyTo(specialties, this._cascadeSkills.Count);
    }

    public Skill(string name, string description, IList<ISkill> specialties)
    {
      this.Categories = new List<string>();
      this.Name = name;
      this.Description = description;
      this._cascadeSkills = specialties != null && specialties.Count != 0 ? (SkillList) specialties.Select<ISkill, ISkill>((Func<ISkill, ISkill>) (x => x.Clone<ISkill>())).ToList<ISkill>() : throw new ArgumentException("Can't have an empty or null specialties in Skill constructor");
    }

    [JsonProperty]
    public SkillList _cascadeSkills { get; set; }
  }
}
