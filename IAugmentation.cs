
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IAugmentation




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IAugmentation : 
    IUpgradable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    ICharacterModifier,
    IModifier<Character>
  {
    bool IsPartOfAnotherAugment { get; set; }

    int AugmentationBonus { get; set; }
  }
}
