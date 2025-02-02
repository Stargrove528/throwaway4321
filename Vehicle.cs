
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Vehicle




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Vehicle : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
  {
    public int Agility { get; set; }

    public Vehicle.SpeedBand Speed { get; set; }

    public int Crew { get; set; }

    public int Passengers { get; set; }

    public float Cargo { get; set; }

    public int Hull { get; set; }

    public float Shipping { get; set; }

    public int Armor { get; set; }

    public string EquipmentAndTraits { get; set; }

    public string Notes { get; set; }

    public int RangeMeters { get; set; }

    [JsonConstructor]
    public Vehicle()
    {
    }

    public enum SpeedBand
    {
      Stopped,
      Idle,
      VerySlow,
      Slow,
      Medium,
      High,
      Fast,
      VeryFast,
      Subsonic,
      Supersonic,
      Hypersonic,
    }
  }
}
