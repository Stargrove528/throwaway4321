
// Type: com.digitalarcsystems.Traveller.DataModel.TravellerLocation




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class TravellerLocation
  {
    [JsonIgnore]
    public const int UNDEFINED = -2147483648;
    [JsonProperty]
    private string _manualName;
    [JsonProperty]
    private World _world;

    [JsonIgnore]
    public string ManualName
    {
      get => this._manualName;
      set => this._manualName = value;
    }

    [JsonIgnore]
    public World world
    {
      get => this._world;
      set => this._world = value;
    }

    [JsonProperty]
    public int WSC_X { get; set; }

    [JsonProperty]
    public int WSC_Y { get; set; }

    [JsonProperty]
    public string Sector { get; set; }

    [JsonProperty]
    public int Hex { get; set; }

    public TravellerLocation()
    {
      this.Hex = int.MinValue;
      this.WSC_X = int.MinValue;
      this.WSC_Y = int.MinValue;
      this.Sector = "";
      this.ManualName = "";
    }
  }
}
