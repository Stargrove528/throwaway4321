
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ClanShares




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ClanShares : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
  {
    public const string CLAN_SHARES_ID = "16f7e066-579a-41b3-a5d3-792352cb3067";
    [JsonIgnore]
    private int _number = 1;

    [JsonProperty]
    public int Number
    {
      get => this._number;
      set
      {
        this._number = value;
        this.Name = "Clan Shares [" + value.ToString() + "]";
        this.Description = value.ToString() + " x various clan benefits.";
      }
    }

    public ClanShares()
    {
      this.Number = 1;
      this.Id = new Guid("16f7e066-579a-41b3-a5d3-792352cb3067");
      this.Categories = new List<string>() { "General" };
      if (!(this.InstanceID == Guid.Empty))
        return;
      this.InstanceID = Guid.NewGuid();
    }

    public ClanShares(int numShares)
      : this()
    {
      this.Number = numShares;
    }

    [JsonConstructor]
    public ClanShares(Guid instanceID)
    {
      this.InstanceID = instanceID;
      if (!(instanceID == Guid.Empty))
        return;
      this.InstanceID = Guid.NewGuid();
    }
  }
}
