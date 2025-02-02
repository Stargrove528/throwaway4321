
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IRangedEquipment




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IRangedEquipment : 
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    int RangeInMeters { get; set; }
  }
}
