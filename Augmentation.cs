
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Augmentation




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Augmentation : 
    UpgradableEquipment,
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
    public static readonly List<string> UPGRADECATEGORIES = new List<string>()
    {
      "AugmentOption"
    };
    [JsonProperty]
    private List<AugmentationType> _augmentationType;

    [JsonConstructor]
    public Augmentation(List<string> allowedUpgradeCategories)
    {
      this.AllowedUpgradeCategories = allowedUpgradeCategories;
    }

    public Augmentation()
    {
      this.AllowedUpgradeCategories = new List<string>((IEnumerable<string>) Augmentation.UPGRADECATEGORIES);
    }

    public Augmentation(IAugmentation copyMe)
      : base((IUpgradable) copyMe)
    {
      this.AllowedUpgradeCategories = copyMe.AllowedUpgradeCategories;
      this.IsPartOfAnotherAugment = copyMe.IsPartOfAnotherAugment;
      this.AugmentationBonus = copyMe.AugmentationBonus;
    }

    public bool IsPartOfAnotherAugment { get; set; }

    public virtual int AugmentationBonus { get; set; }

    [JsonIgnore]
    public List<AugmentationType> AugmentationTypes
    {
      get
      {
        if (this._augmentationType == null)
        {
          List<AugmentationType> augmentationTypeList = new List<AugmentationType>();
          if (this.Name.ToLower().Contains("str") || this.Name.ToLower().Contains("end") || this.Name.ToLower().Contains("dex") || this.Name.ToLower().Contains("int") || this.Name.ToLower().Contains("edu") || this.Name.ToLower().Contains("soc") || this.Name.ToLower().Contains("cha") || this.Name.ToLower().Contains("cst") || this.Name.ToLower().Contains("psi") || this.Name.ToLower().Contains("ter"))
            augmentationTypeList.Add(AugmentationType.STAT);
          else if (this.Name.ToLower().Contains("subdermal"))
            augmentationTypeList.Add(AugmentationType.PROTECTION);
          else if (this.Name.ToLower().Contains("natural attack"))
            augmentationTypeList.Add(AugmentationType.NATURAL_ATTACK_ENHANCEMENT);
          else if (this.Name.ToLower().Contains("combat"))
            augmentationTypeList.Add(AugmentationType.IMPROVE_COMBAT_ABILITIES);
          else if (this.Name.ToLower().Contains("waferjack"))
            augmentationTypeList.Add(AugmentationType.COMPUTER);
          else if (this.Name.ToLower().Contains("headcomp"))
            augmentationTypeList.Add(AugmentationType.COMPUTER);
          else
            augmentationTypeList.Add(AugmentationType.OTHER);
        }
        return this._augmentationType;
      }
      set => this._augmentationType = value;
    }

    public virtual void ActionsOnAdd(Character changeMe)
    {
    }

    public virtual void ActionsOnRemoval(Character restoreMe)
    {
    }
  }
}
