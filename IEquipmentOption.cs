
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IEquipmentOption




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IEquipmentOption : 
    IEquipmentModifier,
    IModifier<IEquipment>,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    List<string> UpgradeCategories { get; set; }

    int CalculatePrice(IEquipment equipmentToBeAddedTo);

    void AddEquipmentAction(IEquipmentAction addMe);
  }
}
