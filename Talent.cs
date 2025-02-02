
// Type: com.digitalarcsystems.Traveller.DataModel.Talent




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  [Serializable]
  public class Talent : IAsset, IDescribable, IAssetBase, ILicensedAsset, IAddable<Talent>
  {
    public string Name { get; set; }

    public int Level { get; set; }

    public Trait associatedTrait { get; set; }

    public List<Power> powers { get; set; }

    public int learning_dm { get; set; }

    public string Description { get; set; }

    public Talent()
    {
      this.powers = new List<Power>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
    }

    public Talent(Talent copy)
    {
      this.Id = copy.Id;
      this.Name = copy.Name;
      this.Description = copy.Description;
      this.Level = copy.Level;
      this.associatedTrait = copy.associatedTrait;
      this.powers = new List<Power>();
      foreach (Power power in copy.powers)
        this.powers.Add(new Power(power));
      this.learning_dm = copy.learning_dm;
    }

    public Talent(string name, string description, Trait trait, int learning_dm)
    {
      this.Name = name;
      this.Description = description;
      this.powers = new List<Power>();
      this.associatedTrait = trait;
      this.learning_dm = learning_dm;
    }

    [JsonConstructor]
    public Talent(
      string name,
      string description,
      Trait trait,
      int learning_dm,
      List<Power> powers)
      : this(name, description, trait, learning_dm)
    {
      foreach (Power power in powers)
        this.addPower(power);
    }

    public void Add(Talent addMe)
    {
      if (addMe == null || !addMe.Name.Equals(this.Name, StringComparison.CurrentCultureIgnoreCase))
        return;
      this.Level += addMe.Level;
    }

    public void addPower(Power power)
    {
      if (this.powers == null)
        this.powers = new List<Power>();
      power.parentTalentName = this.Name;
      this.powers.Add(power);
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Talent)
      {
        Talent talent = (Talent) obj;
        flag = this.Id == talent.Id || this.GetHashCode() == talent.GetHashCode();
      }
      return flag;
    }

    public override int GetHashCode()
    {
      return 31 * (31 * 17321 + (this.Name != null ? this.Name.GetHashCode() : 0)) + (this.associatedTrait != null ? this.associatedTrait.GetHashCode() : 0);
    }

    public Guid Id { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    public override string ToString() => this.Name + ": " + this.Level.ToString();
  }
}
