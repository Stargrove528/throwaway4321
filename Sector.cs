
// Type: com.digitalarcsystems.Traveller.DataModel.Sector




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Sector : ILicensedAsset, IAssetBase, IGenericAsset<string>, IAsset, IDescribable
  {
    [JsonProperty]
    private Dictionary<string, string> _subsectors;
    [JsonIgnore]
    public static string[] subsectorLabels = new string[16]
    {
      "A",
      "B",
      "C",
      "D",
      "E",
      "F",
      "G",
      "H",
      "I",
      "J",
      "K",
      "L",
      "M",
      "N",
      "O",
      "P"
    };

    public Guid Id { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    [JsonProperty]
    public string Contents { get; private set; }

    [JsonIgnore]
    public List<World> Worlds
    {
      get
      {
        List<World> worlds = World.LoadFromString(this.Contents);
        foreach (World world in worlds)
        {
          world.SubsectorName = this.getSubsector(world.hexNumber);
          world.SectorName = this.Name;
        }
        return worlds;
      }
    }

    [JsonProperty]
    public string Description { get; set; }

    [JsonProperty]
    public string Name { get; set; }

    [JsonProperty]
    public List<string> Subsectors
    {
      get => new List<string>((IEnumerable<string>) this._subsectors.Values);
    }

    [JsonConstructor]
    public Sector()
    {
      this._subsectors = new Dictionary<string, string>();
      this.Tags = new List<AssetTag>();
      this.Tags.AddDistinct<AssetTag>(AssetTag.World);
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
    }

    public static Sector Create(string secFile, Guid id)
    {
      int index1 = secFile.IndexOf("Name: ") + 6;
      string lineStartingAtIndex1 = secFile.GetLineStartingAtIndex(index1);
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (string subsectorLabel in Sector.subsectorLabels)
      {
        int index2 = secFile.IndexOf("Subsector " + subsectorLabel + ": ") + 13;
        string lineStartingAtIndex2 = secFile.GetLineStartingAtIndex(index2);
        dictionary.Add(subsectorLabel, lineStartingAtIndex2);
      }
      if (dictionary.Count != 16)
        Console.WriteLine("ERROR: Not the right amount of SUBSECTORS! Found [" + dictionary.Count.ToString() + "/16]");
      Sector sector = Sector.Create(secFile, lineStartingAtIndex1, "", id);
      sector._subsectors = dictionary;
      return sector;
    }

    public static Sector Create(string secFile, string sectorName, string description, Guid id)
    {
      return new Sector()
      {
        Contents = secFile,
        Name = sectorName,
        Id = id,
        Description = description
      };
    }

    public string getSubsector(int hex)
    {
      int num1 = hex / 100;
      int num2 = hex - num1 * 100;
      int index = Math.Max((num1 - 1) / 8, 0) + Math.Max((num2 - 1) / 10 * 4, 0);
      return index >= 0 && index <= 15 ? this._subsectors[Sector.subsectorLabels[index]] : "";
    }
  }
}
