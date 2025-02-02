
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.CharacterModifyingArmorOption




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class CharacterModifyingArmorOption : ArmorOption, ICharacterModifier, IModifier<Character>
  {
    [JsonProperty]
    protected Augmentation augmentation;

    [JsonConstructor]
    public CharacterModifyingArmorOption(List<IEquipmentAction> actions)
      : base(actions)
    {
    }

    public CharacterModifyingArmorOption(Augmentation augment) => this.augmentation = augment;

    void IModifier<Character>.ActionsOnAdd(Character addedToThis)
    {
      this.augmentation.ActionsOnAdd(addedToThis);
    }

    void IModifier<Character>.ActionsOnRemoval(Character removedFromThis)
    {
      this.augmentation.ActionsOnAdd(removedFromThis);
    }
  }
}
