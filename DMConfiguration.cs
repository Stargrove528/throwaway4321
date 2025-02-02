
// Type: com.digitalarcsystems.Traveller.DMConfiguration




using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class DMConfiguration
  {
    [JsonProperty]
    public Dictionary<Guid, AssetMetadata> Cache { get; set; }

    [JsonProperty]
    public double LastPercentageLicensedSynced { get; set; }

    [JsonProperty]
    public double LastPercentageNonLicensedSynced { get; set; }

    [JsonProperty]
    public double DataModelVersion { get; set; }

    [JsonConstructor]
    public DMConfiguration()
    {
    }
  }
}
