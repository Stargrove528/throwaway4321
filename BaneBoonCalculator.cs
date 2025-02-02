
// Type: com.digitalarcsystems.Traveller.DataModel.BaneBoonCalculator




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public static class BaneBoonCalculator
  {
    public static bool isScienceSkill(ISkill selectedSkill)
    {
      bool flag = false;
      ISkill asset = DataManager.Instance.GetAsset<ISkill>(new Guid("0755316f-6579-4303-84c6-5d2451a3d011"));
      IList<ISkill> specializationSkills = (IList<ISkill>) asset.SpecializationSkills;
      ISkill skill = selectedSkill;
      if (skill != null && (skill.Equals((object) asset) || specializationSkills.Contains(skill)))
        flag = true;
      return flag;
    }

    public static ModifierDetails calculate(
      IEquipment equipmentBeingUsed,
      Character character,
      string skillName,
      string stat,
      bool calculateEncumberance)
    {
      stat = stat.ToLowerInvariant();
      ModifierDetails modifierDetails = new ModifierDetails();
      modifierDetails.baneBoonBonus = BaneBoonBonus.NONE;
      int num1 = 0;
      int num2 = 0;
      string str1 = "";
      string str2 = "";
      skillName = skillName.ToLowerInvariant();
      ISkill selectedSkill = DataManager.Instance.Skills.FirstOrDefault<ISkill>((Func<ISkill, bool>) (s => s.Name.ToLowerInvariant().Equals(skillName)));
      if (character.States.Contains(ManualHealthState.Fatigue))
      {
        str1 += "Fatigued";
        ++num1;
      }
      if (calculateEncumberance)
      {
        int num3 = character.getAttributeValue("str") + character.getAttributeValue("end") + (character.hasSkill("strength") ? character.getSkill("strength").Level : 0) + (character.hasSkill("endurance") ? character.getSkill("endurance").Level : 0);
        if (character.EquippedMass > (double) num3)
        {
          ++num1;
          string str3 = str1;
          string str4;
          if (str1.Length <= 0)
            str4 = "Encmbrd " + character.EquippedMass.ToString() + "kg/" + num3.ToString() + "kg";
          else
            str4 = ", ";
          str1 = str3 + str4;
        }
      }
      if (skillName.Contains("stealth") && character.getArmor().Any<Armor>((Func<Armor, bool>) (a => a.OriginalName.ToLowerInvariant().Contains("reflec"))))
      {
        ++num1;
        str1 += str1.Length > 0 ? ", " : "Reflec";
      }
      if (character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString().ToLowerInvariant().Equals("d81f2c-ade0-4b87-9daf-81ec43b9475c"))) && (stat.ToLowerInvariant().Contains("str") || stat.ToLowerInvariant().Contains("end")))
      {
        ++num1;
        if (!str1.Contains("Armor"))
          str1 += str1.Length > 0 ? ", " : "Armor";
      }
      else if (character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e is BattleDress && ((com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment) e).OriginalName.ToLowerInvariant().Contains("artillery"))) && equipmentBeingUsed != null && equipmentBeingUsed is IWeapon && !(equipmentBeingUsed is BattleDressMod))
      {
        ++num1;
        if (!str1.Contains("Armor"))
          str1 += str1.Length > 0 ? ", " : "Armor";
      }
      else if (character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString().ToLowerInvariant().Equals("c4280437-ad69-438a-a780-26a9e3ecd95a"))) && stat.Contains("dex"))
      {
        ++num1;
        if (!str1.Contains("Armor"))
          str1 += str1.Length > 0 ? ", " : "Armor";
      }
      if (DataManager.Instance.GetAsset<IAsset>(skillName) is Talent && character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString().ToLowerInvariant().Equals("a18c3952-4e11-4f2d-afc1-129422553530"))))
      {
        ++num1;
        str1 += str1.Length > 0 ? ", " : "Null-Shielding";
      }
      if (equipmentBeingUsed != null && equipmentBeingUsed.Id.ToString().ToLowerInvariant().Equals("fc4faa28-536d-4733-a15e-13219fcb9d86"))
      {
        ++num1;
        str1 += str1.Length > 0 ? ", " : "Unwieldy Weapon";
      }
      ISkill asset = DataManager.Instance.GetAsset<ISkill>(new Guid("3038c049-75ae-410e-b372-bebbfcd4c49e"));
      if (character.Race.Name.Equals("bwap", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(skillName) && (BaneBoonCalculator.isScienceSkill(selectedSkill) || asset.Equals((object) selectedSkill)))
      {
        ++num2;
        str2 += str2.Length > 0 ? ", " : "Bwap: Structured Mind";
      }
      if (character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString().ToLowerInvariant().Equals("0ea9a478-65a7-4ed8-b250-a17557aa50c0"))) && (skillName.Contains("persuade") || skillName.Contains("intimidate")))
      {
        ++num2;
        str2 += str2.Length > 0 ? ", " : "Armor";
      }
      if (character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e is BattleDress && ((com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment) e).OriginalName.ToLowerInvariant().Contains("command"))) && skillName.Contains("military"))
      {
        ++num2;
        str2 += str2.Length > 0 ? ", " : "Armor";
      }
      if (character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString().ToLowerInvariant().Equals("78c97927-0c1b-492f-a41a-2223c351b3b9"))) && skillName.Contains("surviv"))
      {
        ++num2;
        str2 += str2.Length > 0 ? ", " : "Protein Tap";
      }
      if (DataManager.Instance.GetAsset<IAsset>(skillName) is Talent && character.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString().ToLowerInvariant().Equals("78255704-a5eb-43cd-970e-c9079baac206"))))
      {
        ++num2;
        str2 += str2.Length > 0 ? ", " : "Psi-Amplifier";
      }
      modifierDetails.baneDetails = str1;
      modifierDetails.boonDetails = str2;
      modifierDetails.baneBoonBonus = num1 <= num2 ? (num1 >= num2 ? BaneBoonBonus.NONE : BaneBoonBonus.BOON) : BaneBoonBonus.BANE;
      return modifierDetails;
    }
  }
}
