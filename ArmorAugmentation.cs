
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ArmorAugmentation




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ArmorAugmentation : 
    Armor,
    IAugmentation,
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
    [JsonProperty]
    protected IAugmentation _augmentation;

    [JsonConstructor]
    public ArmorAugmentation(List<string> _AllowedUpgradeCategories)
      : base(_AllowedUpgradeCategories)
    {
    }

    public ArmorAugmentation()
    {
    }

    public ArmorAugmentation(Armor armor2Copy, IAugmentation aug2Copy)
      : base(armor2Copy)
    {
      this._augmentation = aug2Copy;
      this.AugmentationBonus = armor2Copy.Protection;
    }

    [JsonIgnore]
    public bool IsPartOfAnotherAugment
    {
      get => this._augmentation.IsPartOfAnotherAugment;
      set => this._augmentation.IsPartOfAnotherAugment = value;
    }

    [JsonIgnore]
    public int AugmentationBonus
    {
      get => this._augmentation.AugmentationBonus;
      set => this._augmentation.AugmentationBonus = value;
    }

    public void ActionsOnAdd(Character addedToThis) => this._augmentation.ActionsOnAdd(addedToThis);

    public void ActionsOnRemoval(Character removedFromThis)
    {
      this._augmentation.ActionsOnRemoval(removedFromThis);
    }
  }
}
