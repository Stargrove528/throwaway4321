
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ComputerOption




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ComputerOption : AbstractEquipmentOption
  {
    [JsonConstructor]
    public ComputerOption()
    {
    }

    public ComputerOption(ComputerOption copyMe)
      : base((AbstractEquipmentOption) copyMe)
    {
      this.Rating = copyMe.Rating;
    }

    [JsonProperty]
    public int Rating { get; set; }

    public override int CalculatePrice(IEquipment equipmentToBeAddedTo) => this.Cost;
  }
}
