
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.UpgradeComputer




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class UpgradeComputer : 
    Computer,
    IEquipmentOption,
    IEquipmentModifier,
    IModifier<IEquipment>,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    [JsonProperty]
    protected IEquipmentOption _option;

    [JsonIgnore]
    public List<string> UpgradeCategories
    {
      get => this._option.UpgradeCategories;
      set => this._option.UpgradeCategories = value;
    }

    [JsonConstructor]
    protected UpgradeComputer()
    {
    }

    public UpgradeComputer(UpgradeComputer copyMe)
      : base((IComputer) copyMe)
    {
      this._option = copyMe._option;
    }

    public UpgradeComputer(IComputer copyMe, IEquipmentOption option)
      : base(copyMe)
    {
      this._option = option;
    }

    public void ActionsOnAdd(IEquipment addedToThis) => this._option.ActionsOnAdd(addedToThis);

    public void ActionsOnRemoval(IEquipment removedFromThis)
    {
      this._option.ActionsOnRemoval(removedFromThis);
    }

    public int CalculatePrice(IEquipment equipmentToBeAddedTo)
    {
      return this._option.CalculatePrice(equipmentToBeAddedTo);
    }

    public void AddEquipmentAction(IEquipmentAction addMe)
    {
      this._option.AddEquipmentAction(addMe);
    }
  }
}
