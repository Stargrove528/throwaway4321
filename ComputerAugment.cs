
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ComputerAugment




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ComputerAugment : 
    Computer,
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
    private List<AugmentationType> _augmentationTypes = new List<AugmentationType>();

    [JsonConstructor]
    protected ComputerAugment(List<string> allowedUpgradeCategories)
    {
      this.AllowedUpgradeCategories = allowedUpgradeCategories;
    }

    public ComputerAugment()
    {
      if (this.AllowedUpgradeCategories == null)
        this.AllowedUpgradeCategories = new List<string>();
      this.AllowedUpgradeCategories.AddRange((IEnumerable<string>) Augmentation.UPGRADECATEGORIES);
    }

    public ComputerAugment(IAugmentation copyMe)
      : base((IEquipment) copyMe)
    {
      if (this.AllowedUpgradeCategories == null)
        this.AllowedUpgradeCategories = new List<string>();
      this.AllowedUpgradeCategories.AddRange((IEnumerable<string>) Augmentation.UPGRADECATEGORIES);
      this.IsPartOfAnotherAugment = copyMe.IsPartOfAnotherAugment;
      this.AugmentationBonus = copyMe.AugmentationBonus;
    }

    [JsonIgnore]
    public int AugmentationBonus
    {
      get => this.Rating;
      set => this.Rating = value;
    }

    [JsonIgnore]
    public List<AugmentationType> AugmentationTypes
    {
      get
      {
        if (this._augmentationTypes.Count == 0)
          this._augmentationTypes.Add(AugmentationType.COMPUTER);
        return this._augmentationTypes;
      }
      set => this._augmentationTypes = value;
    }

    public bool IsPartOfAnotherAugment { get; set; }

    public void ActionsOnAdd(Character changeMe)
    {
    }

    public void ActionsOnRemoval(Character restoreMe)
    {
    }
  }
}
