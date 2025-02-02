
// Type: com.digitalarcsystems.Traveller.DataModel.AvatarInfo




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  [Serializable]
  public class AvatarInfo
  {
    [JsonProperty]
    public string ModelName { get; set; }

    [JsonProperty]
    public Dictionary<string, float> PropertyValues { get; set; }

    [JsonProperty]
    public string CustomRace { get; set; }

    [JsonProperty]
    public string CustomGender { get; set; }

    [JsonProperty]
    public string CustomCareer { get; set; }

    [JsonProperty]
    public string BodyType { get; set; }

    [JsonProperty]
    public string AlternativeModel { get; set; }

    public AvatarInfo() => this.PropertyValues = new Dictionary<string, float>();

    [JsonConstructor]
    public AvatarInfo(
      string modelName,
      Dictionary<string, float> propertyValues,
      string customRace,
      string customGender,
      string customCareer,
      string bodyType,
      string alternativeModel)
    {
      this.ModelName = modelName;
      this.PropertyValues = propertyValues;
      this.CustomRace = customRace;
      this.CustomGender = customGender;
      this.CustomCareer = customCareer;
      this.BodyType = bodyType;
      this.AlternativeModel = alternativeModel;
    }
  }
}
