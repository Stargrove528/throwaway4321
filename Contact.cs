
// Type: com.digitalarcsystems.Traveller.DataModel.Contact




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  [Serializable]
  public class Contact : IAsset, IDescribable, IAssetBase, ILicensedAsset
  {
    [JsonProperty]
    public string Notes { get; set; }

    [JsonProperty]
    public string Location { get; set; }

    [JsonProperty]
    public Contact.ContactType Type { get; set; }

    [JsonConstructor]
    public Contact()
    {
      this.Type = Contact.ContactType.Default;
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
    }

    public Contact(string name)
    {
      this.Name = name;
      this.Type = Contact.ContactType.Default;
    }

    public Contact(Contact copy)
    {
      this.Name = copy.Name;
      this.Id = copy.Id;
      this.Description = copy.Description;
      this.Type = copy.Type;
      this.Notes = copy.Notes;
      this.Location = copy.Location;
      this.ChildAssets = copy.ChildAssets;
    }

    public string Description { get; set; }

    public string Name { get; set; }

    public Guid Id { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    public override string ToString() => this.Name;

    public enum ContactType
    {
      Default,
      Ally,
      Contact,
      Rival,
      Enemy,
    }
  }
}
