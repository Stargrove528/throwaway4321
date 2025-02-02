
// Type: com.digitalarcsystems.Traveller.DataModel.JournalEntry




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class JournalEntry : IAsset, IDescribable, IAssetBase, INonLicensedAsset
  {
    public string Title { get; set; }

    public string Notes { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    [JsonProperty]
    public List<AssetTag> Tags { get; set; }

    public GameDate EntryDate { get; set; }

    public DateTime CreationDate { get; set; }

    public TravellerLocation Location { get; set; }

    [JsonConstructor]
    public JournalEntry(int creatingUser)
    {
      this.Tags = new List<AssetTag>();
      this.Id = Guid.NewGuid();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.CreatingUser = creatingUser;
    }

    public string Description { get; set; }

    public string Name { get; set; }

    public Guid Id { get; set; }

    public string DefaultFileName => this.Id.ToString("D") + ".trjnl";

    public int CreatingUser { get; set; }
  }
}
