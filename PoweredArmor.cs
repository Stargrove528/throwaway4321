
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.PoweredArmor




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class PoweredArmor : Armor, ICharacterModifier, IModifier<Character>
  {
    [JsonProperty]
    protected Dictionary<string, int> _statsModified = new Dictionary<string, int>();
    [JsonProperty]
    protected bool nervePlugResponseRigEquipped = false;
    [JsonProperty]
    protected Guid wearingCharacterID;
    [JsonIgnore]
    protected Character wearingCharacter;

    [JsonConstructor]
    protected PoweredArmor(List<string> _AllowedUpgradeCategories)
      : base(_AllowedUpgradeCategories)
    {
    }

    public PoweredArmor()
    {
    }

    public PoweredArmor(Armor basedOnMe)
      : base(basedOnMe)
    {
    }

    public PoweredArmor addStatModification(string stat, int value)
    {
      if (this._statsModified.ContainsKey(stat.ToLowerInvariant()))
        this._statsModified[stat.ToLowerInvariant()] += value;
      else
        this._statsModified.Add(stat.ToLowerInvariant(), value);
      return this;
    }

    public PoweredArmor removeStatModification(string stat, int value)
    {
      if (this._statsModified.ContainsKey(stat.ToLowerInvariant()))
      {
        this._statsModified[stat.ToLower()] -= value;
        if (this._statsModified[stat.ToLower()] == 0)
          this._statsModified.Remove(stat.ToLower());
      }
      return this;
    }

    public List<string> getStatsModified()
    {
      return new List<string>((IEnumerable<string>) this._statsModified.Keys);
    }

    public int statBonus(string statName)
    {
      int num = 0;
      if (this._statsModified.ContainsKey(statName))
        num = this._statsModified[statName];
      return num;
    }

    public void ActionsOnAdd(Character addedToThis)
    {
      this.nervePlugResponseRigEquipped = false;
      this.wearingCharacterID = addedToThis.Id;
      this.wearingCharacter = addedToThis;
      if (addedToThis.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (ag => ag.Id.Equals(new Guid("5405894d-b808-464e-8a83-85ed4ff3cf0d")))) && this.Options.Any<IEquipmentOption>((Func<IEquipmentOption, bool>) (ag => ag.Id.Equals(new Guid("f6cfc791-e5a9-454a-a98e-978ff101e52f")))))
        this.nervePlugResponseRigEquipped = true;
      foreach (KeyValuePair<string, int> keyValuePair in this._statsModified)
        addedToThis.getAttribute(keyValuePair.Key).TemporaryAugmentation += keyValuePair.Value;
    }

    public void ActionsOnRemoval(Character removedFromThis)
    {
      this.wearingCharacterID = Guid.Empty;
      this.wearingCharacter = (Character) null;
      this.nervePlugResponseRigEquipped = false;
      foreach (KeyValuePair<string, int> keyValuePair in this._statsModified)
        removedFromThis.getAttribute(keyValuePair.Key).TemporaryAugmentation -= keyValuePair.Value;
    }

    public override bool AddOption(IEquipmentOption addMe)
    {
      Character wearingCharacter = this.wearingCharacter;
      Guid wearingCharacterId = this.wearingCharacterID;
      this.AddOrRemoveEquipmentFromCharacter(wearingCharacterId, wearingCharacter, PoweredArmor.AddOrRemove.REMOVE);
      bool flag = base.AddOption(addMe);
      this.AddOrRemoveEquipmentFromCharacter(wearingCharacterId, wearingCharacter, PoweredArmor.AddOrRemove.ADD);
      return flag;
    }

    public override void RemoveOption(IEquipmentOption removeMe)
    {
      Character wearingCharacter = this.wearingCharacter;
      Guid wearingCharacterId = this.wearingCharacterID;
      this.AddOrRemoveEquipmentFromCharacter(wearingCharacterId, wearingCharacter, PoweredArmor.AddOrRemove.REMOVE);
      base.RemoveOption(removeMe);
      this.AddOrRemoveEquipmentFromCharacter(wearingCharacterId, wearingCharacter, PoweredArmor.AddOrRemove.ADD);
    }

    private void AddOrRemoveEquipmentFromCharacter(
      Guid characterID,
      Character character,
      PoweredArmor.AddOrRemove desiredAction)
    {
      if (!(characterID != Guid.Empty))
        return;
      if (character == null)
        character = DataManager.Instance.GetAsset<Character>(this.wearingCharacterID);
      if (character != null)
      {
        if (desiredAction == PoweredArmor.AddOrRemove.REMOVE)
          character.UnEquip((IEquipment) this);
        else
          character.Equip((IEquipment) this);
      }
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      int num = base.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
      if (this.nervePlugResponseRigEquipped && stat.Ordinal == com.digitalarcsystems.Traveller.DataModel.Attribute.GetCanonicalOrdinalForStat("dex"))
        num += 2;
      return num;
    }

    private enum AddOrRemove
    {
      ADD,
      REMOVE,
    }
  }
}
