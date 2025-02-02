
// Type: com.digitalarcsystems.Traveller.utility.Utility




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using com.digitalarcsystems.Traveller.DataModel.Events;
using NCalc;
using RPGSuiteCloud;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility
{
  public class Utility
  {
    public static readonly Guid DnsNamespace = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
    public static readonly Guid UrlNamespace = new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8");
    public static readonly Guid IsoOidNamespace = new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8");
    public const int NO_VALUE = -99999;

    public static ISkill handleIfFreeFormSkill(
      ISkill potentialFreeFormSkill,
      GenerationState currentState)
    {
      ISkill skill = potentialFreeFormSkill;
      if (potentialFreeFormSkill.Name.Equals("Custom Skill", StringComparison.InvariantCultureIgnoreCase) && !(potentialFreeFormSkill is FreeFormSkill))
        potentialFreeFormSkill = (ISkill) new FreeFormSkill(potentialFreeFormSkill.Name, potentialFreeFormSkill.Description, potentialFreeFormSkill.Parent);
      if (potentialFreeFormSkill is FreeFormSkill)
      {
        FreeFormSkill freeFormSkill = (FreeFormSkill) potentialFreeFormSkill;
        string description;
        string title;
        if (freeFormSkill.Parent != null && freeFormSkill.Parent.Name != null)
        {
          description = "Please provide details for your custom " + freeFormSkill.Parent.Name + " specialization.";
          title = "Define Custom " + freeFormSkill.Parent.Name + " Specialization";
        }
        else
        {
          EngineLog.Warning("FreeFormSkill didn't have parent");
          description = "Please provide details for you custom skill.";
          title = "Define Custom Skill";
        }
        IDescribable describable = currentState.decisionMaker.provideFreeString(description, title, RequiredInformation.NAME_AND_DESCRIPTION);
        CustomSkill customSkill = freeFormSkill.CreateCustomSkill(describable);
        customSkill.Parent = freeFormSkill.Parent;
        customSkill.Description += freeFormSkill.Parent == null || string.IsNullOrEmpty(freeFormSkill.Parent.Name) ? "This is a custom specialization." : "This is a " + freeFormSkill.Parent.Name + " specialization";
        skill = (ISkill) customSkill;
      }
      return skill;
    }

    public static int getRandomNumber(int maxNumber) => new Random().Next(maxNumber + 1);

    public static T pickRandomOne<T>(IList<T> fromHere)
    {
      return fromHere.Count > 0 ? fromHere[new Random().Next(fromHere.Count)] : default (T);
    }

    public static string getLastCapitalizedWord(string camelCase)
    {
      string lastCapitalizedWord = (string) null;
      if (camelCase != null)
      {
        int num = camelCase.Length - 1;
        while (num >= 0 && !char.IsUpper(camelCase[num]))
          --num;
        if (char.IsUpper(camelCase[num]))
          lastCapitalizedWord = camelCase.Substring(num);
      }
      return lastCapitalizedWord;
    }

    public static ISkill GetHighestCascadeSkill(ISkill skill, Character character)
    {
      ISkill highestCascadeSkill = skill;
      if (highestCascadeSkill.Cascade)
      {
        ISkill skill1 = (ISkill) null;
        foreach (ISkill specializationSkill in (List<ISkill>) highestCascadeSkill.SpecializationSkills)
        {
          if (character.hasSkill(specializationSkill) && (skill1 == null || character.getSkill(specializationSkill).Level > skill1.Level))
            skill1 = character.getSkill(specializationSkill);
        }
        if (skill1 != null && skill1.Level > skill.Level)
          highestCascadeSkill = skill1;
      }
      return highestCascadeSkill;
    }

    public static string toTitleCase(string str)
    {
      if (string.IsNullOrEmpty(str))
        return str;
      StringBuilder stringBuilder = new StringBuilder();
      char c1 = ' ';
      foreach (char c2 in str)
      {
        if (c2 == '_')
          stringBuilder.Append(' ');
        else if (c1 == ' ' || c1 == '_')
          stringBuilder.Append(char.ToUpper(c2));
        else if (char.IsUpper(c2) && !char.IsUpper(c1))
        {
          stringBuilder.Append(' ');
          stringBuilder.Append(char.ToUpper(c2));
        }
        else
          stringBuilder.Append(c2);
        c1 = c2;
      }
      return stringBuilder.ToString();
    }

    public static void processAllContainedOutcomes(OutcomeOperator oo, Outcome outcome)
    {
      oo.operateOnOutcome(outcome);
      if (!(outcome is OutcomeContainer))
        return;
      Utility.processAllContainedOutcomes(oo, (ICollection<Outcome>) ((OutcomeContainer) outcome).Outcomes);
    }

    public static void processAllContainedOutcomes(
      OutcomeOperator oo,
      ICollection<Outcome> outcomes)
    {
      foreach (Outcome outcome in (IEnumerable<Outcome>) outcomes)
        Utility.processAllContainedOutcomes(oo, outcome);
    }

    public static void processAllContainedOutcomes(OutcomeOperator oo, params Outcome[] outcomes)
    {
      foreach (Outcome outcome in outcomes)
        Utility.processAllContainedOutcomes(oo, outcome);
    }

    public static IEquipment nextUpgrade(IEquipment doIHaveAnUpgrade, string category)
    {
      return Utility.getAllUpgrades(doIHaveAnUpgrade).FirstOrDefault<IEquipment>();
    }

    public static List<IEquipment> getAllUpgrades(IEquipment doIHaveUpgrades)
    {
      List<IEquipment> source = new List<IEquipment>();
      string name = doIHaveUpgrades.Name.Trim();
      if (name.ToLowerInvariant().Contains(" "))
      {
        string str = name;
        char[] chArray = new char[1]{ ' ' };
        foreach (string s in str.Split(chArray))
        {
          int result = 0;
          if (!s.ToLowerInvariant().Contains("tl") && !int.TryParse(s, out result) && !s.ToLowerInvariant().Contains(")"))
          {
            name = s;
            break;
          }
        }
      }
      List<IEquipment> list = DataManager.Instance.GetAsset<IEquipment>((Func<IEquipment, bool>) (arg => arg.GetType().Equals(doIHaveUpgrades.GetType()) && arg.Name.IndexOf(name, StringComparison.Ordinal) >= 0)).OrderBy<IEquipment, int>((Func<IEquipment, int>) (x => x.TechLevel)).ToList<IEquipment>();
      if (doIHaveUpgrades is IAugmentation)
      {
        int bonus = ((IAugmentation) doIHaveUpgrades).AugmentationBonus;
        source = !list.All<IEquipment>((Func<IEquipment, bool>) (x => x is IAugmentation && ((IAugmentation) x).AugmentationBonus == ((IAugmentation) doIHaveUpgrades).AugmentationBonus)) ? list.Where<IEquipment>((Func<IEquipment, bool>) (x => x is IAugmentation && ((IAugmentation) x).AugmentationBonus > bonus)).OrderBy<IEquipment, int>((Func<IEquipment, int>) (x => ((IAugmentation) x).AugmentationBonus)).ToList<IEquipment>() : list.Where<IEquipment>((Func<IEquipment, bool>) (x => x is IAugmentation && x.TechLevel > doIHaveUpgrades.TechLevel)).OrderBy<IEquipment, int>((Func<IEquipment, int>) (x => x.TechLevel)).ToList<IEquipment>();
      }
      else
      {
        int num = -1;
        for (int index = 0; index < list.Count; ++index)
        {
          if (!list[index].Id.Equals(doIHaveUpgrades.Id) && list[index].TechLevel > doIHaveUpgrades.TechLevel)
          {
            source.Add(list[index]);
            num = list[index].TechLevel;
          }
        }
        if (source.Any<IEquipment>())
          source.OrderBy<IEquipment, int>((Func<IEquipment, int>) (x => x.TechLevel));
      }
      return source;
    }

    public static bool upgradePosessed<T>(
      IEquipment amIAlreadyUpgraded,
      List<T> ownedEquipment,
      string equipmentType = "Augmentation")
      where T : IEquipment
    {
      bool flag = false;
      if (ownedEquipment != null && ownedEquipment.Count != 0)
      {
        List<IEquipment> allUpgrades = Utility.getAllUpgrades(amIAlreadyUpgraded);
        if (allUpgrades != null && allUpgrades.Any<IEquipment>())
        {
          foreach (T obj in ownedEquipment)
          {
            IEquipment equipment1 = (IEquipment) obj;
            foreach (IEquipment equipment2 in allUpgrades)
            {
              if (equipment1.Id.Equals(equipment2.Id))
              {
                flag = true;
                break;
              }
            }
            if (flag)
              break;
          }
        }
      }
      return flag;
    }

    public static bool upgradeAvailable(IEquipment doIHaveAnUpgrade, string equipmentType = "Augmentation")
    {
      return Utility.nextUpgrade(doIHaveAnUpgrade, equipmentType) != null;
    }

    public static List<Outcome> prunePosessedSkillsAndTalents(
      Character character,
      IList<Outcome> pruneThese,
      int min_level = 0)
    {
      List<Outcome> outcomeList = new List<Outcome>((IEnumerable<Outcome>) pruneThese);
      foreach (Outcome outcome in (IEnumerable<Outcome>) pruneThese)
      {
        bool flag = false;
        Utility.OutcomeType outcomeType = Utility.OutcomeType.Other;
        switch (outcome)
        {
          case Outcome.GainSkill _:
            outcomeType = Utility.OutcomeType.GainSkill;
            break;
          case Outcome.GainTalent _:
            outcomeType = Utility.OutcomeType.GainTalent;
            break;
          case Outcome.EnsureSkillAtLevel _:
            outcomeType = Utility.OutcomeType.EnsureSkill;
            break;
          case Outcome.EnsureTalentAtLevel _:
            outcomeType = Utility.OutcomeType.EnsureTalent;
            break;
          case GainIndependence _:
            outcomeType = Utility.OutcomeType.GainIndependence;
            break;
        }
        switch (outcomeType)
        {
          case Utility.OutcomeType.GainSkill:
            Outcome.GainSkill gainSkill = (Outcome.GainSkill) outcome;
            if (Utility.pruneMe(character, gainSkill.skillName, true, min_level))
            {
              flag = true;
              break;
            }
            break;
          case Utility.OutcomeType.GainTalent:
            Outcome.GainTalent gainTalent = (Outcome.GainTalent) outcome;
            if (Utility.pruneMe(character, gainTalent.talent_name, false, min_level))
            {
              flag = true;
              break;
            }
            break;
          case Utility.OutcomeType.EnsureSkill:
            Outcome.EnsureSkillAtLevel ensureSkillAtLevel = (Outcome.EnsureSkillAtLevel) outcome;
            int min_level1 = ensureSkillAtLevel.minLevel > min_level ? ensureSkillAtLevel.minLevel : min_level;
            if (Utility.pruneMe(character, ensureSkillAtLevel.Text, true, min_level1))
            {
              flag = true;
              break;
            }
            break;
          case Utility.OutcomeType.EnsureTalent:
            Outcome.EnsureTalentAtLevel ensureTalentAtLevel = (Outcome.EnsureTalentAtLevel) outcome;
            int min_level2 = ensureTalentAtLevel.minLevel > min_level ? ensureTalentAtLevel.minLevel : min_level;
            if (Utility.pruneMe(character, ensureTalentAtLevel.talentName, false, min_level2))
            {
              flag = true;
              break;
            }
            break;
          case Utility.OutcomeType.GainIndependence:
            if (Utility.pruneMe(character, "Independence", true, min_level))
            {
              flag = true;
              break;
            }
            break;
        }
        if (flag)
          outcomeList.Remove(outcome);
      }
      return outcomeList;
    }

    private static bool pruneMe(
      Character character,
      string target_name,
      bool isSkill,
      int min_level = 0)
    {
      bool flag = false;
      if (isSkill)
      {
        if (character.hasSkill(target_name) && character.getSkill(target_name).Level >= min_level)
          flag = true;
      }
      else if (character.hasTalent(target_name) && character.getTalent(target_name).Level >= min_level)
        flag = true;
      return flag;
    }

    public static IList<T> prune<T>(IList<T> source, IList<T> pruneThese, int min_level = 0) where T : IAddable<T>
    {
      for (int index = 0; index < source.Count; ++index)
      {
        bool flag = false;
        T obj = source[index];
        if ((object) obj != null && pruneThese.Count > 0 && pruneThese.Contains(obj) && pruneThese[pruneThese.IndexOf(obj)].Level >= min_level)
        {
          source.Remove(obj);
          --index;
          flag = true;
        }
        if (!flag && (!((object) obj is ISkill) || (object) obj is ISkill && !((ISkill) (object) obj).Cascade))
          obj.Level = min_level;
      }
      return (IList<T>) source.OrderBy<T, string>((Func<T, string>) (t => t.Name)).ToList<T>();
    }

    public static bool equalToOrSubSkillOf(string parent_skill, string target_skill)
    {
      bool orSubSkillOf = parent_skill == target_skill;
      if (!orSubSkillOf)
      {
        ISkill skill1 = DataManager.Instance.Skills.FirstOrDefault<ISkill>((Func<ISkill, bool>) (arg => arg.Name.ToLowerInvariant().Trim() == parent_skill.ToLowerInvariant().Trim()));
        if (skill1 != null)
        {
          ISkill skill2 = DataManager.Instance.Skills.FirstOrDefault<ISkill>((Func<ISkill, bool>) (arg => arg.Name.ToLowerInvariant().Trim() == target_skill.ToLowerInvariant().Trim()));
          if (skill2 != null && (skill1 == skill2 || skill2.Parent == skill1))
            orSubSkillOf = true;
        }
      }
      return orSubSkillOf;
    }

    public static int? TryParseNullable(string val)
    {
      int result;
      return int.TryParse(val, out result) ? new int?(result) : new int?();
    }

    public static Guid CreateGuid(Guid namespaceId, string name)
    {
      return Utility.CreateGuid(namespaceId, name, 5);
    }

    public static Guid CreateGuid(Guid namespaceId, string name, int version)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (version != 3 && version != 5)
        throw new ArgumentOutOfRangeException(nameof (version), "version must be either 3 or 5.");
      byte[] bytes = Encoding.UTF8.GetBytes(name);
      byte[] byteArray = namespaceId.ToByteArray();
      Utility.SwapByteOrder(byteArray);
      byte[] hash;
      using (HashAlgorithm hashAlgorithm = version == 3 ? (HashAlgorithm) MD5.Create() : (HashAlgorithm) SHA1.Create())
      {
        hashAlgorithm.TransformBlock(byteArray, 0, byteArray.Length, (byte[]) null, 0);
        hashAlgorithm.TransformFinalBlock(bytes, 0, bytes.Length);
        hash = hashAlgorithm.Hash;
      }
      byte[] numArray = new byte[16];
      Array.Copy((Array) hash, 0, (Array) numArray, 0, 16);
      numArray[6] = (byte) ((int) numArray[6] & 15 | version << 4);
      numArray[8] = (byte) ((int) numArray[8] & 63 | 128);
      Utility.SwapByteOrder(numArray);
      return new Guid(numArray);
    }

    internal static void SwapByteOrder(byte[] guid)
    {
      Utility.SwapBytes(guid, 0, 3);
      Utility.SwapBytes(guid, 1, 2);
      Utility.SwapBytes(guid, 4, 5);
      Utility.SwapBytes(guid, 6, 7);
    }

    private static void SwapBytes(byte[] guid, int left, int right)
    {
      byte num = guid[left];
      guid[left] = guid[right];
      guid[right] = num;
    }

    public static bool IsNumeric(object expression)
    {
      return expression != null && double.TryParse(Convert.ToString(expression, (IFormatProvider) CultureInfo.InvariantCulture), NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out double _);
    }

    public static List<string> tokenize(string formula)
    {
      return ((IEnumerable<string>) formula.Split(new string[1]
      {
        "<<"
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (s => s.Contains(">>"))).Select<string, string>((Func<string, string>) (s => s.Split(new string[1]
      {
        ">>"
      }, StringSplitOptions.RemoveEmptyEntries)[0])).ToList<string>();
    }

    public static int calculateValue(string formula, object targetObject)
    {
      string expression = formula;
      foreach (string name in Utility.tokenize(formula))
      {
        string newValue;
        if (name.ToLower().Contains("traits:") || name.ToLower().Contains("trait:"))
        {
          IEquipment equipment = (IEquipment) targetObject;
          newValue = Math.Max(0, Utility.getIntTrait(name.Substring(name.IndexOf(":") + 1), equipment.Traits)).ToString() ?? "";
        }
        else
          newValue = targetObject.GetType().GetProperty(name).GetValue(targetObject, (object[]) null)?.ToString() ?? "";
        expression = expression.Replace("<<" + name + ">>", newValue);
      }
      string s = new Expression(expression).Evaluate().ToString();
      int result;
      if (!int.TryParse(s, out result))
        result = (int) Math.Round(double.Parse(s));
      return result;
    }

    public static void WriteLBA_Archive(Stream destination, List<LicensedBinaryAsset> images)
    {
      using (BinaryWriter binaryWriter = new BinaryWriter(destination))
      {
        binaryWriter.Write(images.Count);
        foreach (LicensedBinaryAsset image in images)
        {
          if (image.Contents == null)
            throw new ArgumentNullException("ERROR: " + image.Name + " has null contents. [" + image.SourceFilename + "]");
          binaryWriter.Write(image.AsSerialized());
          binaryWriter.Write(((IEnumerable<byte>) image.Contents).Count<byte>());
          binaryWriter.Write(image.Contents);
        }
      }
    }

    public static List<LicensedBinaryAsset> ReadLBA_Archive(Stream source)
    {
      List<LicensedBinaryAsset> licensedBinaryAssetList = new List<LicensedBinaryAsset>();
      using (BinaryReader binaryReader = new BinaryReader(source))
      {
        int num = binaryReader.ReadInt32();
        for (int index = 0; index < num; ++index)
        {
          LicensedBinaryAsset licensedBinaryAsset = binaryReader.ReadString().AsDeserialized<LicensedBinaryAsset>();
          int count = binaryReader.ReadInt32();
          licensedBinaryAsset.Contents = binaryReader.ReadBytes(count);
          licensedBinaryAssetList.Add(licensedBinaryAsset);
        }
      }
      return licensedBinaryAssetList;
    }

    public static int getIntTrait(string traitName, string traitsString)
    {
      int intTrait = -99999;
      if (traitName == null || traitName.Length == 0 || traitsString == null || traitsString.Length == 0)
        return intTrait;
      List<EquipmentExtensions.Trait> traits = new com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment("TEMP")
      {
        Traits = traitsString
      }.GetTraits();
      traitName = traitName.ToLower().Trim();
      for (int index = 0; index < traits.Count<EquipmentExtensions.Trait>() && intTrait < 0; ++index)
      {
        if (traits[index].name.ToLower().Contains(traitName) && traits[index].name.Length == traitName.Length)
        {
          intTrait = traits[index].value;
          break;
        }
      }
      return intTrait;
    }

    public static bool anyCommaSeperatedMatches(string allowed, string possessed)
    {
      if (allowed == null || possessed == null || allowed.Count<char>() == 0 || possessed.Count<char>() == 0)
        return false;
      return Utility.anyCommaSeperatedMatches(((IEnumerable<string>) allowed.Split(',')).ToList<string>().Select<string, string>((Func<string, string>) (t => t.Trim())).ToList<string>(), ((IEnumerable<string>) possessed.Split(',')).ToList<string>().Select<string, string>((Func<string, string>) (t => t.Trim())).ToList<string>());
    }

    public static bool anyCommaSeperatedMatches(List<string> allowed, string possessed)
    {
      if (allowed == null || possessed == null)
        return false;
      List<string> list = ((IEnumerable<string>) possessed.Split(',')).ToList<string>().Select<string, string>((Func<string, string>) (t => t.Trim())).ToList<string>();
      return Utility.anyCommaSeperatedMatches(allowed, list);
    }

    public static bool anyCommaSeperatedMatches(
      List<string> allowed_items,
      List<string> possessed_items)
    {
      if (allowed_items == null || possessed_items == null)
        return false;
      bool flag = false;
      foreach (string allowedItem in allowed_items)
      {
        foreach (string possessedItem in possessed_items)
        {
          if (allowedItem.ToLowerInvariant().Equals(possessedItem.ToLowerInvariant()))
          {
            flag = true;
            break;
          }
        }
        if (flag)
          break;
      }
      return flag;
    }

    internal static string CommaSeperatedStringDifferences(string original_csv, string final_csv)
    {
      List<string> list1 = ((IEnumerable<string>) original_csv.Split(',')).ToList<string>().Select<string, string>((Func<string, string>) (t => t.Trim())).ToList<string>();
      List<string> list2 = ((IEnumerable<string>) final_csv.Split(',')).ToList<string>().Select<string, string>((Func<string, string>) (t => t.Trim())).ToList<string>();
      List<string> list3 = list2.Except<string>((IEnumerable<string>) list1).ToList<string>();
      List<string> list4 = list1.Except<string>((IEnumerable<string>) list2).Select<string, string>((Func<string, string>) (x => "-" + x)).ToList<string>();
      return (list3.Any<string>() ? string.Join(", ", list3.ToArray()) : "") + (list4.Any<string>() ? string.Join(",", list4.ToArray()) : "");
    }

    public static List<IAmmunition> GenerateAmmunition(IMultishotRangedWeapon forMe)
    {
      List<IAmmunition> ammunition1 = new List<IAmmunition>();
      if (forMe.ConsumableTypeAllowed.ToLowerInvariant().Contains("grenade"))
      {
        foreach (IAmmunition instance in DataManager.Instance.GetAsset<Grenade>().Select<Grenade, IAmmunition>((Func<Grenade, IAmmunition>) (g => (IAmmunition) g)))
        {
          IAmmunition ammunition2 = instance.Clone<IAmmunition>();
          ammunition2.MaxAmount = forMe.Capacity;
          ammunition2.CurrentAmount = forMe.Capacity;
          ammunition2.DamageString = Utility.GetDamageString(instance as IWeapon);
          IAmmunition ammunition3 = ammunition2;
          Guid urlNamespace = Utility.UrlNamespace;
          Guid id = instance.Id;
          string str1 = id.ToString();
          id = forMe.Id;
          string str2 = id.ToString();
          string name = "com.digitalarcsystems.Traveller." + str1 + str2;
          Guid guid = Utility.CreateGuid(urlNamespace, name);
          ammunition3.Id = guid;
          IAmmunition ammunition4 = ammunition2;
          id = forMe.Id;
          string str3 = id.ToString();
          ammunition4.ConsumableType = str3;
          ammunition2.RangeInMeters = forMe.RangeInMeters;
          ammunition1.Add(ammunition2);
          ammunition2.Cost *= forMe.Capacity;
          if (ammunition2.InstanceID == Guid.Empty)
            ammunition2.InstanceID = Guid.NewGuid();
        }
      }
      List<AmmunitionTemplate> templates = new List<AmmunitionTemplate>();
      if (DataManager.Instance.GetAsset<AmmunitionTemplate>().FirstOrDefault<AmmunitionTemplate>() != null)
        templates = DataManager.Instance.GetAsset<AmmunitionTemplate>(true).ToList<AmmunitionTemplate>();
      List<IAmmunition> ammunition5 = Utility.GenerateAmmunition(forMe, templates);
      if (ammunition5 != null && ammunition5.Where<IAmmunition>((Func<IAmmunition, bool>) (a => a != null)).Any<IAmmunition>())
        ammunition1.AddRange((IEnumerable<IAmmunition>) ammunition5.Where<IAmmunition>((Func<IAmmunition, bool>) (a => a != null)).ToList<IAmmunition>());
      return ammunition1;
    }

    public static List<IAmmunition> GenerateAmmunition(
      IMultishotRangedWeapon forMe,
      List<AmmunitionTemplate> templates)
    {
      List<IAmmunition> ammunition1 = new List<IAmmunition>();
      if (!forMe.Name.ToLowerInvariant().Contains("laser"))
      {
        List<AmmunitionTemplate> list = templates.Where<AmmunitionTemplate>((Func<AmmunitionTemplate, bool>) (t => Utility.anyCommaSeperatedMatches(forMe.ConsumableTypeAllowed, t.ConsumableType))).ToList<AmmunitionTemplate>();
        if (list.Any<AmmunitionTemplate>())
        {
          foreach (AmmunitionTemplate ammunitionTemplate in list)
          {
            IMultishotRangedWeapon multishotRangedWeapon = forMe.Clone<IMultishotRangedWeapon>();
            multishotRangedWeapon.Load((IConsumable) ammunitionTemplate);
            string str1 = ammunitionTemplate.Name.Length > 7 ? Utility.makeAcronymn(ammunitionTemplate.Name) : ammunitionTemplate.Name;
            List<IAmmunition> ammunitionList = ammunition1;
            Ammunition ammunition2 = new Ammunition(ammunitionTemplate);
            ammunition2.Name = str1 + " [" + forMe.Name + "]";
            ammunition2.Description = ammunitionTemplate.Description;
            Guid urlNamespace = Utility.UrlNamespace;
            Guid id = ammunitionTemplate.Id;
            string str2 = id.ToString();
            id = forMe.Id;
            string str3 = id.ToString();
            string name = "com.digitalarcsystems.Traveller." + str2 + str3;
            ammunition2.Id = Utility.CreateGuid(urlNamespace, name);
            ammunition2.Cost = ammunitionTemplate.CalculatePrice(forMe);
            ammunition2.Traits = Utility.CommaSeperatedStringDifferences(forMe.Traits, multishotRangedWeapon.Traits);
            ammunition2.RangeInMeters = multishotRangedWeapon.RangeInMeters;
            ammunition2.MaxAmount = forMe.Capacity;
            ammunition2.CurrentAmount = forMe.Capacity;
            ammunition2.DamageString = Utility.GetDamageString((IWeapon) multishotRangedWeapon);
            id = forMe.Id;
            ammunition2.ConsumableType = id.ToString();
            ammunitionList.Add((IAmmunition) ammunition2);
          }
        }
      }
      if (forMe.Id == Guid.Parse("418ed6a0-3f87-4f13-ba6d-e6ef63e6b90f"))
      {
        List<IAmmunition> ammunitionList = ammunition1;
        Ammunition ammunition3 = new Ammunition();
        ammunition3.Name = "STANDARD CLIP [" + forMe.Name + "]";
        ammunition3.Description = "This is the standard ammunition for a/an " + forMe.Name;
        ammunition3.Id = Utility.CreateGuid(Utility.UrlNamespace, "com.digitalarcsystems.Traveller.standard" + forMe.Id.ToString());
        ammunition3.Cost = 30;
        ammunition3.Traits = "Blast 9";
        ammunition3.RangeInMeters = forMe.RangeInMeters;
        ammunition3.MaxAmount = forMe.Capacity;
        ammunition3.CurrentAmount = forMe.Capacity;
        ammunition3.DamageString = "5D";
        ammunition3.ConsumableType = forMe.Id.ToString();
        ammunitionList.Add((IAmmunition) ammunition3);
      }
      else
      {
        List<IAmmunition> ammunitionList = ammunition1;
        Ammunition ammunition4 = new Ammunition();
        ammunition4.Name = "STANDARD CLIP [" + forMe.Name + "]";
        ammunition4.Description = "This is the standard ammunition for a/an " + forMe.Name;
        ammunition4.Id = Utility.CreateGuid(Utility.UrlNamespace, "com.digitalarcsystems.Traveller.standard" + forMe.Id.ToString());
        ammunition4.Cost = forMe.StandardConsumableCost;
        ammunition4.Traits = "";
        ammunition4.RangeInMeters = forMe.RangeInMeters;
        ammunition4.MaxAmount = forMe.Capacity;
        ammunition4.CurrentAmount = forMe.Capacity;
        ammunition4.DamageString = Utility.GetDamageString((IWeapon) forMe);
        ammunition4.ConsumableType = forMe.Id.ToString();
        ammunitionList.Add((IAmmunition) ammunition4);
      }
      return ammunition1;
    }

    public static string Abbreviate(string name, int too_big_length = 7)
    {
      string source1 = name;
      if (name.Length > too_big_length)
      {
        string[] strArray;
        if (!source1.Contains<char>(' '))
          strArray = new string[1]{ source1 };
        else
          strArray = name.Split(' ');
        string[] source2 = strArray;
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
          {
            "advanced",
            "Adv"
          },
          {
            "communications",
            "Comms"
          },
          {
            "communication",
            "Comm"
          },
          {
            "temperature",
            "Temp"
          },
          {
            "autonomous",
            "Auto"
          },
          {
            "battle",
            "Btl"
          },
          {
            "dress",
            "Drss"
          },
          {
            "computerized",
            "Comp"
          },
          {
            "augmentation",
            "Augmnt"
          },
          {
            "holographic",
            "Holo"
          },
          {
            "laboratory",
            "Lab"
          },
          {
            "anti-missile",
            "Antimssl"
          },
          {
            "telekinesis",
            "TK"
          },
          {
            "telekinetic",
            "TK"
          },
          {
            "very_distant",
            "vry dstnt"
          },
          {
            "Jack-of-All-Trades",
            "JoAT"
          },
          {
            "jack-of-all-trades",
            "JoAT"
          }
        };
        int num1 = 4;
        int num2 = ((IEnumerable<string>) source2).Sum<string>((Func<string, int>) (s => s.Length)) + source2.Length - 1;
        for (int index1 = 0; index1 < num1 && num2 > too_big_length; ++index1)
        {
          switch (index1)
          {
            case 0:
              for (int index2 = 0; index2 < source2.Length; ++index2)
              {
                string source3 = source2[index2];
                int result = 0;
                if (source3.Any<char>() && (source3.ToLowerInvariant().Equals("tl") && index2 + 1 < source2.Length && int.TryParse(source2[index2 + 1], out result) || source3.ToLowerInvariant().Equals("(tl") && index2 + 1 < source2.Length && source2[index2 + 1].ToLowerInvariant().EndsWith(")")))
                {
                  source2[index2] = "";
                  source2[index2 + 1] = "";
                  if (index2 - 1 >= 0 && source2[index2 - 1].EndsWith(","))
                    source2[index2 - 1] = source2[index2 - 1].Length <= 1 ? "" : source2[index2 - 1].Substring(0, source2[index2 - 1].Length - 1);
                  ++index2;
                }
              }
              break;
            case 1:
              for (int index3 = 0; index3 < source2.Length; ++index3)
              {
                string source4 = source2[index3].StartsWith("(") ? "(" : "";
                string str1 = source4.Any<char>() ? source2[index3].Substring(1) : source2[index3];
                if (str1.Length > 4 && dictionary.ContainsKey(str1.ToLowerInvariant()))
                {
                  string str2 = source4 + dictionary[str1.ToLowerInvariant()];
                  source2[index3] = str2;
                }
              }
              break;
            case 2:
              for (int index4 = 0; index4 < source2.Length && num2 >= too_big_length; ++index4)
              {
                string name1 = source2[index4];
                if (name1.Length > 4 && !dictionary.ContainsValue(name1))
                  source2[index4] = Utility.makeAcronymn(name1);
                num2 = ((IEnumerable<string>) source2).Where<string>((Func<string, bool>) (w => w.Any<char>())).Sum<string>((Func<string, int>) (s => s.Length)) + source2.Length - 1;
              }
              break;
            case 3:
              for (int index5 = 0; index5 < source2.Length; ++index5)
              {
                if (source2[index5].Equals("-"))
                {
                  if (index5 > 0)
                  {
                    // ISSUE: explicit reference operation
                    ^ref source2[index5 - 1] += "-";
                  }
                  if (index5 + 1 < source2.Length)
                  {
                    // ISSUE: explicit reference operation
                    ^ref source2[index5 - 1] += source2[index5 + 1];
                    source2[index5 + 1] = "";
                  }
                  source2[index5] = "";
                }
              }
              break;
          }
          num2 = ((IEnumerable<string>) source2).Where<string>((Func<string, bool>) (w => w.Any<char>())).Sum<string>((Func<string, int>) (s => s.Length)) + source2.Length - 1;
        }
        source1 = string.Join(" ", ((IEnumerable<string>) source2).Where<string>((Func<string, bool>) (w => w.Any<char>())).ToArray<string>());
        source1.Trim();
      }
      return source1;
    }

    public static string TrimTimeSegment(string power_description)
    {
      power_description = power_description.Replace("_", " ");
      power_description = power_description.Replace("  ", " ");
      power_description = power_description.Trim();
      power_description = Regex.Replace(power_description, " to ", "-", RegexOptions.IgnoreCase);
      return power_description;
    }

    public static string makeAcronymn(string name, int too_big_length = 4)
    {
      string str1 = "";
      if (name.Length > too_big_length)
      {
        string pattern = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";
        string str2 = Regex.Replace(name, pattern, "").Replace(",", "");
        if (str2 != null && str2.Length > 0)
          name = str2;
        string[] strArray = name.Split(' ');
        if (strArray.Length > 1)
        {
          foreach (string str3 in strArray)
          {
            if (str3 != null & str3.Length > 0)
              str1 += str3.ToUpper().ElementAt<char>(0).ToString();
          }
        }
        else
        {
          string vowels = "aeiouAEIOU";
          char ch = name.ElementAt<char>(0);
          int index = ch.Equals('(') ? 1 : 0;
          ch = name.ElementAt<char>(index);
          string upper = ch.ToString().ToUpper();
          ch = name.Last<char>();
          string str4 = ch.ToString();
          if (!char.IsUpper(name.ElementAt<char>(name.Length - 2)))
            str4.ToLower();
          string str5;
          if (index == 0)
          {
            str5 = upper + new string(name.Substring(index + 1, name.Length - 2).Where<char>((Func<char, bool>) (c => !vowels.Contains<char>(c))).ToArray<char>()) + str4;
          }
          else
          {
            ch = name[0];
            str5 = ch.ToString() + upper + new string(name.Substring(index + 1, name.Length - 3).Where<char>((Func<char, bool>) (c => !vowels.Contains<char>(c))).ToArray<char>()) + str4;
          }
          str1 = str5;
          if (str1.Length > 7)
            str1 = str1.Substring(0, 3 + index) + ".";
        }
      }
      else
        str1 = name;
      return str1;
    }

    public static string GetDamageString(IWeapon weapon)
    {
      int num;
      string str1;
      if (weapon.DamageModifier == 0)
        str1 = "";
      else if (weapon.DamageModifier <= 0)
      {
        num = weapon.DamageModifier;
        str1 = num.ToString() ?? "";
      }
      else
      {
        num = weapon.DamageModifier;
        str1 = "+" + num.ToString();
      }
      string str2 = str1;
      string str3 = new string('D', weapon.DamageExponentiator);
      num = weapon.Damage;
      return num.ToString() + str3 + str2;
    }

    public static List<IWeapon> GetBattleDressWeapons(BattleDress forMe, bool clone)
    {
      List<IWeapon> source1 = (List<IWeapon>) null;
      IList<IWeapon> list = (IList<IWeapon>) DataManager.Instance.GetAsset<IWeapon>((Func<IWeapon, bool>) (w =>
      {
        if (w.Weight <= 1001.0)
        {
          switch (w)
          {
            case IAugmentation _:
            case Grenade _:
            case Shield _:
              break;
            default:
              if (w.Cost != 0)
                return !w.Skill.ToLower().Contains("explos");
              break;
          }
        }
        return false;
      }), true).ToList<IWeapon>();
      if (list.Any<IWeapon>())
      {
        source1 = new List<IWeapon>();
        foreach (IWeapon weapon in (IEnumerable<IWeapon>) list)
        {
          IntegratedIWeaponMount integratedIweaponMount1 = (IntegratedIWeaponMount) null;
          List<string> source2 = new List<string>()
          {
            weapon.Skill.ToLowerInvariant(),
            weapon.SubSkill.ToLowerInvariant()
          };
          string lowerInvariant = weapon.Name.ToLowerInvariant();
          if (lowerInvariant.Contains("artil") || lowerInvariant.Contains("vehicle"))
            source2.AddRange((IEnumerable<string>) weapon.Name.ToLowerInvariant().Split(' '));
          if (weapon is IConsumer)
            source2.AddRange((IEnumerable<string>) ((IConsumer) weapon).ConsumableTypeAllowed.ToLowerInvariant().Split(','));
          int cost = 1000 + weapon.Cost;
          int num = 1;
          if (source2.Any<string>((Func<string, bool>) (x => x.Contains("vehicle") || x.Contains("artillery"))) && weapon.Weight > 250.0)
          {
            cost = 10000 + weapon.Cost;
            num = 15;
          }
          else if (source2.Any<string>((Func<string, bool>) (x => x.Contains("heavy") && !x.Contains("gun combat"))) && weapon.Weight <= 250.0)
          {
            if (!source2.Any<string>((Func<string, bool>) (x => x.Contains("man portable"))))
            {
              cost = 10000 + weapon.Cost;
              num = 10;
            }
            else
            {
              cost = 5000 + weapon.Cost;
              num = 10;
            }
          }
          else if (source2.Any<string>((Func<string, bool>) (x => x.Contains("rifle") || x.Contains("shotgun") || x.Contains("carbine"))))
            num = 2;
          else if (source2.Any<string>((Func<string, bool>) (x => x.Contains("pistol") || x.Contains("gun combat"))))
            cost = 500 + weapon.Cost;
          switch (weapon)
          {
            case IAutomaticWeapon _:
              IntegratedIAutomaticWeaponMount iautomaticWeaponMount = new IntegratedIAutomaticWeaponMount((IAutomaticWeapon) weapon, cost);
              iautomaticWeaponMount.Slots = num;
              integratedIweaponMount1 = (IntegratedIWeaponMount) iautomaticWeaponMount;
              goto case null;
            case IMultishotRangedWeapon _:
              IntegratedIMultishotRangedWeaponMount rangedWeaponMount = new IntegratedIMultishotRangedWeaponMount((IMultishotRangedWeapon) weapon, cost);
              rangedWeaponMount.Slots = num;
              integratedIweaponMount1 = (IntegratedIWeaponMount) rangedWeaponMount;
              goto case null;
            case IRangedWeapon _:
              IntegratedIRangedWeaponMount irangedWeaponMount = new IntegratedIRangedWeaponMount((IRangedWeapon) weapon, cost);
              irangedWeaponMount.Slots = num;
              integratedIweaponMount1 = (IntegratedIWeaponMount) irangedWeaponMount;
              goto case null;
            case null:
              integratedIweaponMount1.AllowedUpgradeCategories.Add(integratedIweaponMount1.Id.ToString());
              if (clone)
                integratedIweaponMount1.InstanceID = Guid.NewGuid();
              else
                integratedIweaponMount1.InstanceID = Guid.Empty;
              source1.Add((IWeapon) integratedIweaponMount1);
              continue;
            default:
              IntegratedIWeaponMount integratedIweaponMount2 = new IntegratedIWeaponMount(weapon, cost);
              integratedIweaponMount2.Slots = num;
              integratedIweaponMount1 = integratedIweaponMount2;
              goto case null;
          }
        }
      }
      source1.Select<IWeapon, BattleDressMod>((Func<IWeapon, BattleDressMod>) (x => x as BattleDressMod)).ToList<BattleDressMod>();
      return source1.OrderBy<IWeapon, string>((Func<IWeapon, string>) (o => o.Name)).ThenBy<IWeapon, int>((Func<IWeapon, int>) (or => or.TechLevel)).ToList<IWeapon>();
    }

    public static List<BattleDressMod> GetAvailableOptions(BattleDress forMe, bool clone = false)
    {
      return DataManager.Instance.GetAsset<IEquipmentOption>().Where<IEquipmentOption>((Func<IEquipmentOption, bool>) (eo => !(eo is IWeapon) && Utility.anyCommaSeperatedMatches(forMe.AllowedUpgradeCategories, eo.UpgradeCategories))).Select<IEquipmentOption, BattleDressMod>((Func<IEquipmentOption, BattleDressMod>) (eo =>
      {
        if (eo is BattleDressMod)
          return eo as BattleDressMod;
        return new BattleDressMod(eo)
        {
          Slots = 1,
          UpgradeCategories = new List<string>()
          {
            "Battle Dress Mod"
          }
        };
      })).ToList<BattleDressMod>().OrderBy<BattleDressMod, string>((Func<BattleDressMod, string>) (o => o.Name)).ThenBy<BattleDressMod, int>((Func<BattleDressMod, int>) (or => or.TechLevel)).ToList<BattleDressMod>();
    }

    public static List<IEquipmentOption> GetAvailableOptions(IUpgradable forMe, bool clone = false)
    {
      if (forMe is BattleDress)
        return Utility.GetAvailableOptions((BattleDress) forMe).Select<BattleDressMod, IEquipmentOption>((Func<BattleDressMod, IEquipmentOption>) (bdm => (IEquipmentOption) bdm)).ToList<IEquipmentOption>();
      List<IEquipmentOption> list = DataManager.Instance.GetAsset<IEquipmentOption>((Func<IEquipmentOption, bool>) (eo => Utility.anyCommaSeperatedMatches(forMe.AllowedUpgradeCategories, eo.UpgradeCategories)), clone).ToList<IEquipmentOption>();
      if (forMe is IntegratedIMultishotRangedWeaponMount)
      {
        IntegratedIMultishotRangedWeaponMount rangedWeaponMount = forMe as IntegratedIMultishotRangedWeaponMount;
        if (!rangedWeaponMount.UnlimitedAmmunition && !rangedWeaponMount.Options.Any<IEquipmentOption>((Func<IEquipmentOption, bool>) (o => o is ExtendedAmmunition)))
        {
          ExtendedAmmunition extendedAmmunition1 = new ExtendedAmmunition();
          extendedAmmunition1.Name = "Ext. Ammo Autoloader [" + (rangedWeaponMount.Capacity * 10).ToString() + "]";
          extendedAmmunition1.Description = "By using up an additonal " + rangedWeaponMount.Slots.ToString() + " slots, this integrated weapon mount will be equipped with autoloaders and additional magazines, increasing the " + rangedWeaponMount.Name + "'s totalcapacity to " + (rangedWeaponMount.Capacity * 10).ToString() + ". The cost of this will be " + (rangedWeaponMount.StandardConsumableCost * 9 * 2).ToString() + " which is twice that of the additional magazines.";
          ExtendedAmmunition extendedAmmunition2 = extendedAmmunition1;
          extendedAmmunition2.UpgradeCategories = new List<string>()
          {
            rangedWeaponMount.Id.ToString()
          };
          extendedAmmunition2.TechLevel = rangedWeaponMount.TechLevel;
          list.Add((IEquipmentOption) extendedAmmunition2);
        }
      }
      if (forMe is UpgradeWeapon)
        list = list.Where<IEquipmentOption>((Func<IEquipmentOption, bool>) (o => !(o is UpgradeWeapon))).ToList<IEquipmentOption>();
      return list.OrderBy<IEquipmentOption, string>((Func<IEquipmentOption, string>) (o => o.Name)).ThenBy<IEquipmentOption, int>((Func<IEquipmentOption, int>) (or => or.TechLevel)).ToList<IEquipmentOption>();
    }

    public static string formatCost(int credits)
    {
      return Utility.formatNumberWithDecade((double) credits) + "cr";
    }

    public static string formatNumberWithDecade(double number)
    {
      int num1 = (int) (Math.Floor(Math.Log10(number)) / 3.0);
      double num2 = Math.Pow(10.0, (double) (num1 * 3));
      double num3 = number / num2;
      string str;
      switch (num1)
      {
        case 0:
          str = string.Empty;
          break;
        case 1:
          str = "k";
          break;
        case 2:
          str = "M";
          break;
        case 3:
          str = "B";
          break;
        default:
          str = num1.ToString();
          break;
      }
      return str == string.Empty ? number.ToString() ?? "" : (num3.ToString("N1").EndsWith("0", StringComparison.Ordinal) ? num3.ToString("N0") + str : num3.ToString("N1") + str);
    }

    public static string GetStatistics(IEquipment eq)
    {
      string str1 = !eq.Weight.Equals(0.0) ? Utility.formatNumberWithDecade(eq.Weight * 1000.0) + "g " : "";
      string str2 = eq.Cost != 0 ? string.Format("{0:n0}", (object) eq.Cost) : "- ";
      string str3 = "TL: " + eq.TechLevel.ToString() + " " + str1;
      if (eq is IWeapon)
      {
        if (eq.GetType() == typeof (WeaponAugment))
        {
          if (Utility.GetDamageString((IWeapon) eq).Contains<char>('+'))
          {
            string[] strArray = Utility.GetDamageString((IWeapon) eq).Split('+');
            str3 = str3 + "DMG: " + strArray[0] + "D+" + strArray[1] + " ";
          }
          else if (Utility.GetDamageString((IWeapon) eq).Contains<char>('-'))
          {
            string[] strArray = Utility.GetDamageString((IWeapon) eq).Split('-');
            str3 = str3 + "DMG: " + strArray[0] + "D-" + strArray[1] + " ";
          }
          else
            str3 = str3 + "DMG: " + Utility.GetDamageString((IWeapon) eq) + "D ";
        }
        else
          str3 = str3 + "DMG: " + Utility.GetDamageString((IWeapon) eq) + " ";
      }
      if (eq is IAutomaticWeapon)
        str3 = str3 + "Auto: " + ((IAutomaticWeapon) eq).AutoRating.ToString() + " ";
      if (eq is IRangedEquipment && ((IRangedEquipment) eq).RangeInMeters != 0)
        str3 = str3 + "Rng: " + Utility.formatNumberWithDecade((double) ((IRangedEquipment) eq).RangeInMeters) + "m ";
      int num;
      if (eq is Armor)
      {
        Armor armor = (Armor) eq;
        bool flag1 = armor.ProtectionEnergy != armor.ProtectionKinetic;
        bool flag2 = armor.ProtectionLaser != armor.ProtectionKinetic && armor.ProtectionLaser != armor.ProtectionEnergy;
        string str4 = str3;
        num = ((Armor) eq).ProtectionKinetic;
        string str5 = num.ToString();
        string str6 = str4 + "P: " + str5;
        if (flag2)
        {
          string str7 = str6;
          num = armor.ProtectionLaser;
          string str8 = num.ToString();
          str6 = str7 + "/" + str8 + "L";
        }
        if (flag1)
        {
          string str9 = str6;
          num = armor.ProtectionEnergy;
          string str10 = num.ToString();
          str6 = str9 + "/" + str10 + "E";
        }
        str3 = str6 + " ";
      }
      if (eq is com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship)
      {
        string[] strArray = new string[10];
        strArray[0] = str3;
        strArray[1] = "Hull: ";
        num = ((Vehicle) eq).Hull;
        strArray[2] = num.ToString();
        strArray[3] = " Armor: ";
        num = ((Vehicle) eq).Armor;
        strArray[4] = num.ToString();
        strArray[5] = " Jump: ";
        num = ((com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship) eq).Jump;
        strArray[6] = num.ToString();
        strArray[7] = " Mnvr: ";
        num = ((com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship) eq).Thrust;
        strArray[8] = num.ToString();
        strArray[9] = " ";
        str3 = string.Concat(strArray);
      }
      else if (eq is Vehicle)
      {
        string[] strArray = new string[8];
        strArray[0] = str3;
        strArray[1] = "Hull: ";
        num = ((Vehicle) eq).Hull;
        strArray[2] = num.ToString();
        strArray[3] = " Armor: ";
        num = ((Vehicle) eq).Armor;
        strArray[4] = num.ToString();
        strArray[5] = " Speed: ";
        strArray[6] = ((Vehicle) eq).Speed.ToString();
        strArray[7] = " ";
        str3 = string.Concat(strArray);
      }
      if (eq is IComputer)
      {
        string str11 = str3;
        num = ((IComputer) eq).Rating;
        string str12 = num.ToString();
        str3 = str11 + "Rtng: " + str12 + " ";
      }
      if (eq.hasTrait("blast"))
        str3 = str3 + "Blst: " + eq.GetTrait("blast").value.ToString() + "m ";
      string statistics = str3 + str2 + "cr ";
      if (eq is BattleDress)
      {
        string str13 = statistics;
        num = ((BattleDress) eq).TotalSlots;
        string str14 = num.ToString();
        statistics = str13 + "Slots: " + str14 + " ";
      }
      if (!string.IsNullOrEmpty(eq.Traits))
        statistics = statistics + "[" + eq.Traits + "]";
      return statistics;
    }

    public static void ConsolidateConsumables(Character onMe)
    {
      List<IConsumable> list = onMe.Equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e is IConsumable && !((IConsumable) e).Rechargable && ((IConsumable) e).CurrentAmount < ((IConsumable) e).MaxAmount)).OrderBy<IEquipment, Guid>((Func<IEquipment, Guid>) (ie => ie.Id)).Select<IEquipment, IConsumable>((Func<IEquipment, IConsumable>) (x => x as IConsumable)).ToList<IConsumable>();
      for (int index1 = 0; index1 < list.Count<IConsumable>(); ++index1)
      {
        IConsumable removeMe = list.ElementAt<IConsumable>(index1);
        if (removeMe.CurrentAmount <= 0)
        {
          onMe.removeEquipment((IEquipment) removeMe);
        }
        else
        {
          for (int index2 = index1 + 1; index2 < list.Count<IConsumable>() && removeMe.CurrentAmount != removeMe.MaxAmount && list[index2].Id.Equals(removeMe.Id); ++index2)
          {
            IConsumable consumable = list.ElementAt<IConsumable>(index2);
            int num = Math.Min(removeMe.MaxAmount - removeMe.CurrentAmount, consumable.CurrentAmount);
            consumable.CurrentAmount -= num;
            removeMe.CurrentAmount += num;
          }
        }
      }
    }

    public static byte[] ReadStreamFully(Stream input)
    {
      byte[] buffer = new byte[16384];
      using (MemoryStream memoryStream = new MemoryStream())
      {
        int count;
        while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
          memoryStream.Write(buffer, 0, count);
        return memoryStream.ToArray();
      }
    }

    public static int ConvertTravellerCodeToNumber(char code)
    {
      code = code.ToString().ToLower()[0];
      int num1 = 48;
      int num2 = 57;
      int num3 = 97;
      int num4 = 122;
      int num5 = (int) code;
      if (num5 <= num2 && num5 >= num1)
        return num5 - num1;
      return num5 <= num4 && num5 >= num3 ? num5 - num3 : 0;
    }

    public static IList<string> FormatSkillsByNameAndCascade(IList<ISkill> skills)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (ISkill skill in (IEnumerable<ISkill>) skills)
      {
        if (skill.Cascade || skill.SpecializationSkills != null && skill.SpecializationSkills.Count<ISkill>() > 0)
        {
          foreach (ISkill specializationSkill in (List<ISkill>) skill.SpecializationSkills)
            stringList2.Add(specializationSkill.Name);
        }
      }
      foreach (ISkill skill1 in (IEnumerable<ISkill>) skills)
      {
        ISkill skill = skill1;
        if (!skill.IsSpecialization && !stringList2.Contains(skill.Name))
        {
          string str = skill.Cascade ? skill.Name.ToUpper() : skill.Name;
          stringList1.Add(str + " " + skill.Level.ToString());
          if (skill.Cascade)
          {
            IList<ISkill> source = (IList<ISkill>) new List<ISkill>((IEnumerable<ISkill>) skill.SpecializationSkills);
            if (source.Any<ISkill>((Func<ISkill, bool>) (s => s is FreeFormSkill)))
            {
              source = (IList<ISkill>) source.Where<ISkill>((Func<ISkill, bool>) (s => !(s is FreeFormSkill))).ToList<ISkill>();
              foreach (ISkill skill2 in skills.Where<ISkill>((Func<ISkill, bool>) (s => s is FreeFormSkill && s.Parent.Equals((object) skill))))
                source.Add(skill2);
            }
            source.OrderBy<ISkill, string>((Func<ISkill, string>) (s => s.Name));
            foreach (ISkill skill3 in (IEnumerable<ISkill>) source)
            {
              ISkill child = skill3;
              ISkill skill4 = skills.FirstOrDefault<ISkill>((Func<ISkill, bool>) (s => s.Name.Equals(child.Name, StringComparison.InvariantCultureIgnoreCase)));
              int level = skill4 != null ? skill4.Level : 0;
              stringList1.Add("     " + child.Name + " " + level.ToString());
            }
          }
        }
      }
      return (IList<string>) stringList1;
    }

    public static bool ImportCharacter(DataManager dm, string filename)
    {
      if (Utility.ValidateFileExtension(filename))
      {
        try
        {
          Character character = File.ReadAllText(filename).AsDeserialized<Character>();
          character.Id = Guid.NewGuid();
          dm.Save<Character>(character, true);
        }
        catch (Exception ex)
        {
          Console.WriteLine((object) ex);
          return false;
        }
        return true;
      }
      Console.WriteLine("Incorrect Filetype");
      return false;
    }

    public static bool ExportCharacter(Character character, string filename)
    {
      string path = filename;
      if (!Utility.ValidateFileExtension(filename))
        path = Utility.EnsureCorrectFileExtension(filename);
      try
      {
        string contents = character.AsSerialized();
        File.WriteAllText(path, contents);
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
        return false;
      }
      return true;
    }

    public static bool ValidateFileExtension(string filename, string file_extension = ".tchjson")
    {
      string extension = Path.GetExtension(filename);
      if (!file_extension.Substring(0, 1).Equals("."))
        file_extension = "." + file_extension;
      return string.Equals(extension, file_extension);
    }

    public static string EnsureCorrectFileExtension(string filename, string file_extension = ".tchjson")
    {
      string extension = Path.GetExtension(filename);
      string[] strArray1 = new string[0];
      string str;
      if (!string.Equals(extension, file_extension))
      {
        string[] strArray2 = filename.Split('.');
        str = file_extension.Contains(".") ? strArray2[0] + file_extension : strArray2[0] + "." + file_extension;
      }
      else
        str = filename;
      return str;
    }

    private enum OutcomeType
    {
      GainSkill,
      GainTalent,
      EnsureSkill,
      EnsureTalent,
      GainIndependence,
      Other,
    }
  }
}
