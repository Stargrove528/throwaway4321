
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ModifyPowerArmorStatBonus




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ModifyPowerArmorStatBonus : 
    IEquipmentAction,
    IEquipmentModifier,
    IModifier<IEquipment>
  {
    public string Stat { get; set; }

    public int Value { get; set; }

    public void ActionsOnAdd(IEquipment addedToThis)
    {
      if (addedToThis is PoweredArmor)
        ((PoweredArmor) addedToThis).addStatModification(this.Stat, this.Value);
      else
        EngineLog.Warning("Stat modification attempted to be added to nonPower Armor [" + addedToThis.Name + ", " + addedToThis.GetType().Name + "]");
    }

    public void ActionsOnRemoval(IEquipment removedFromThis)
    {
      if (removedFromThis is PoweredArmor)
        ((PoweredArmor) removedFromThis).removeStatModification(this.Stat, this.Value);
      else
        EngineLog.Warning("Stat modification attempted to be added to nonPower Armor [" + removedFromThis.Name + ", " + removedFromThis.GetType().Name + "]");
    }
  }
}
