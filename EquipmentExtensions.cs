
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.EquipmentExtensions




using com.digitalarcsystems.Traveller.utility;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public static class EquipmentExtensions
  {
    public const int NO_VALUE = -99999;

    public static bool hasRequiredSkill(this IEquipment eq) => eq.getRequiredSkillLevel() != -1;

    public static int getRequiredSkillLevel(this IEquipment eq)
    {
      int result = -1;
      if (!string.IsNullOrEmpty(eq.Skill))
      {
        string[] source = eq.Skill.Split(' ');
        if (!int.TryParse(((IEnumerable<string>) source).Last<string>(), out result) || source.Length < 2)
          result = -1;
      }
      return result;
    }

    public static string getRequiredSkill(this IEquipment eq)
    {
      string requiredSkill = (string) null;
      if (eq.hasRequiredSkill())
      {
        char[] chArray = new char[10]
        {
          '0',
          '1',
          '2',
          '3',
          '4',
          '5',
          '6',
          '7',
          '8',
          '9'
        };
        requiredSkill = ((IEnumerable<string>) eq.Skill.Split(chArray)).FirstOrDefault<string>();
        if (!string.IsNullOrEmpty(requiredSkill))
          requiredSkill = requiredSkill.Trim();
      }
      return requiredSkill;
    }

    public static void addTrait(this IEquipment eq, string traitToAdd)
    {
      eq.addTrait(new EquipmentExtensions.Trait(traitToAdd));
    }

    public static void addTrait(this IEquipment eq, EquipmentExtensions.Trait traitToAdd)
    {
      string name = traitToAdd.name.ToLowerInvariant();
      List<EquipmentExtensions.Trait> list = eq.GetTraits().Where<EquipmentExtensions.Trait>((Func<EquipmentExtensions.Trait, bool>) (t => !t.name.ToLowerInvariant().Equals(name))).ToList<EquipmentExtensions.Trait>();
      list.Add(traitToAdd);
      eq.writeOutTraits(list);
    }

    public static void removeTrait(this IEquipment eq, string traitToRemove)
    {
      eq.removeTrait(new EquipmentExtensions.Trait(traitToRemove));
    }

    public static void removeTrait(this IEquipment eq, EquipmentExtensions.Trait traitToRemove)
    {
      string name = traitToRemove.name.ToLowerInvariant();
      if (!eq.Traits.ToLowerInvariant().Contains(name))
        return;
      List<EquipmentExtensions.Trait> list = eq.GetTraits().Where<EquipmentExtensions.Trait>((Func<EquipmentExtensions.Trait, bool>) (t => !t.name.ToLowerInvariant().Equals(name))).ToList<EquipmentExtensions.Trait>();
      eq.writeOutTraits(list);
    }

    public static bool hasTrait(this IEquipment eq, EquipmentExtensions.Trait checkForMe)
    {
      bool flag = false;
      if (checkForMe == null || string.IsNullOrEmpty(checkForMe.name))
        throw new ArgumentException("Trait to check for must be nonnull and not empty");
      if (eq.Traits.Any<char>())
      {
        foreach (object trait in eq.GetTraits())
        {
          flag = trait.Equals((object) checkForMe);
          if (flag)
            break;
        }
      }
      return flag;
    }

    public static bool hasTrait(this IEquipment eq, string checkForMe)
    {
      checkForMe = checkForMe != null && checkForMe.Count<char>() != 0 ? checkForMe.ToLowerInvariant() : throw new ArgumentException("Trait to check for must be nonnull and not empty");
      return eq.GetTraits().Any<EquipmentExtensions.Trait>((Func<EquipmentExtensions.Trait, bool>) (t => t.name.ToLowerInvariant().Equals(checkForMe)));
    }

    public static int getTraitValue(this IEquipment eq, string traitToGetValueFor)
    {
      return eq.Traits == null || eq.Traits.Count<char>() == 0 ? -99999 : Utility.getIntTrait(traitToGetValueFor, eq.Traits);
    }

    public static EquipmentExtensions.Trait GetTrait(this IEquipment eq, string traitName)
    {
      return eq.Traits != null ? eq.GetTraits().Where<EquipmentExtensions.Trait>((Func<EquipmentExtensions.Trait, bool>) (t => t.name.ToLowerInvariant().Equals(traitName.ToLowerInvariant()))).FirstOrDefault<EquipmentExtensions.Trait>() : (EquipmentExtensions.Trait) null;
    }

    public static List<EquipmentExtensions.Trait> GetTraits(this IEquipment eq)
    {
      char[] chArray = new char[1]{ ',' };
      eq.Traits = EquipmentExtensions.cleanUpTraits(eq.Traits);
      return eq.Traits.Length != 0 ? ((IEnumerable<string>) eq.Traits.Split(chArray)).Select<string, string>((Func<string, string>) (s => s.Trim())).Select<string, EquipmentExtensions.Trait>((Func<string, EquipmentExtensions.Trait>) (s => new EquipmentExtensions.Trait(s))).ToList<EquipmentExtensions.Trait>() : new List<EquipmentExtensions.Trait>();
    }

    private static void writeOutTraits(this IEquipment eq, List<EquipmentExtensions.Trait> traits)
    {
      eq.writeOutTraits(traits.Select<EquipmentExtensions.Trait, string>((Func<EquipmentExtensions.Trait, string>) (t => !t.has_value ? t.name : t.name + " " + t.value.ToString())).ToList<string>());
    }

    private static void writeOutTraits(this IEquipment addedToThis, List<string> traits)
    {
      addedToThis.Traits = "";
      traits = traits.Where<string>((Func<string, bool>) (t => t != null && t.Length > 0)).OrderBy<string, string>((Func<string, string>) (t => t)).ToList<string>();
      for (int index = 0; index < traits.Count<string>(); ++index)
      {
        if (index != 0)
          addedToThis.Traits += ", ";
        addedToThis.Traits += traits.ElementAt<string>(index);
      }
    }

    private static string cleanUpTraits(string traits)
    {
      if (traits == null)
        return "";
      traits.Trim();
      if (traits.StartsWith(","))
        traits = traits.Substring(1);
      if (traits.EndsWith(","))
        traits = traits.Substring(0, traits.Length - 1);
      return traits;
    }

    public class Trait
    {
      public string name;
      public int value = -99999;
      public bool has_value = false;

      public Trait(string s)
      {
        this.name = s.Trim();
        if (!s.Contains<char>(' '))
          return;
        string[] strArray = s.Split(' ');
        if (int.TryParse(strArray[1], out this.value))
        {
          this.name = strArray[0];
          this.has_value = true;
        }
      }

      public override bool Equals(object amIEqual)
      {
        bool flag = false;
        if (amIEqual is EquipmentExtensions.Trait)
        {
          EquipmentExtensions.Trait trait = amIEqual as EquipmentExtensions.Trait;
          flag = trait.name.ToLowerInvariant().Equals(this.name.ToLowerInvariant()) && trait.value == this.value;
        }
        return flag;
      }

      public override int GetHashCode()
      {
        return (13 * 7 + this.name.GetHashCode()) * 7 + this.value.GetHashCode();
      }
    }
  }
}
