
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.BattleDressMod




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class BattleDressMod : AbstractEquipmentOption
  {
    [JsonProperty]
    protected IEquipmentOption wrapped;

    [JsonConstructor]
    public BattleDressMod()
    {
    }

    public BattleDressMod(BattleDressMod copyMe)
      : base((AbstractEquipmentOption) copyMe)
    {
      this.Slots = copyMe.Slots;
      this.wrapped = copyMe.wrapped;
    }

    public BattleDressMod(IEquipmentOption basedOnMe)
    {
      this.Copy((IEquipment) basedOnMe);
      this.Name = basedOnMe.Name;
      this.Description = basedOnMe.Description;
      this.Slots = 1;
      this.wrapped = basedOnMe;
    }

    [JsonProperty]
    public int Slots { get; set; }

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      bool flag = base.ModifiesSkillTask(skillName, statName);
      if (!flag && this.wrapped != null)
        flag = this.wrapped.ModifiesSkillTask(skillName, statName);
      return flag;
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      Attribute stat)
    {
      int num = base.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
      if (this.wrapped != null)
        num += this.wrapped.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
      return num;
    }

    public override void ActionsOnAdd(IEquipment addedToThis)
    {
      if (this.wrapped != null)
        this.wrapped.ActionsOnAdd(addedToThis);
      else
        base.ActionsOnAdd(addedToThis);
    }

    public override void ActionsOnRemoval(IEquipment removedFromThis)
    {
      if (this.wrapped != null)
        this.wrapped.ActionsOnRemoval(removedFromThis);
      else
        base.ActionsOnRemoval(removedFromThis);
    }

    public override int CalculatePrice(IEquipment equipmentToBeAddedTo)
    {
      return this.wrapped != null ? this.wrapped.CalculatePrice(equipmentToBeAddedTo) : this.Cost;
    }
  }
}
