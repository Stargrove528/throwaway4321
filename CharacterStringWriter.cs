
// Type: com.digitalarcsystems.Traveller.CharacterStringWriter




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class CharacterStringWriter
  {
    private readonly string _ls = Environment.NewLine;

    public int ColumnWidth { get; set; }

    public bool ShowDead { get; set; } = true;

    public bool ShowBackgroundInfo { get; set; } = true;

    public bool ShowCharacteristics { get; set; } = true;

    public bool ShowSkills { get; set; } = true;

    public bool ShowContactsEtc { get; set; } = true;

    public bool ShowAugments { get; set; } = true;

    public bool ShowEquipment { get; set; } = true;

    public bool ShowCareerHistory { get; set; } = true;

    private void majorSeperator(StringBuilder sb, int columns)
    {
      int num = columns / 2;
      sb.Append(this._ls);
      for (int index = 0; index < num; ++index)
        sb.Append("-=");
      sb.Append(this._ls);
    }

    private void minorSeperator(StringBuilder sb, int columns)
    {
      int num = columns;
      sb.Append(this._ls);
      for (int index = 0; index < num; ++index)
        sb.Append("-");
      sb.Append(this._ls);
    }

    private void printInColumns(StringBuilder retval, int overall_width, List<string> list)
    {
      int num = overall_width / 20;
      for (int index = 1; index <= list.Count; ++index)
      {
        string format = "{0,-" + 20.ToString() + "}";
        retval.AppendFormat(format, (object) list[index - 1]);
        if (index % num == 0)
          retval.Append(this._ls);
      }
    }

    private void title(StringBuilder sb, string @string, int columnWidth)
    {
      int num = (columnWidth - @string.Length) / 2;
      if (num <= 0)
        num = 0;
      for (int index = 0; index < num; ++index)
        sb.Append(" ");
      sb.Append(@string + this._ls);
    }

    public string write(Character character)
    {
      if (this.ColumnWidth <= 0)
        this.ColumnWidth = 80;
      StringBuilder stringBuilder1 = new StringBuilder();
      this.majorSeperator(stringBuilder1, this.ColumnWidth);
      if (character.hasDied())
      {
        this.title(stringBuilder1, "[DECEASED]", this.ColumnWidth);
        this.majorSeperator(stringBuilder1, this.ColumnWidth);
      }
      StringBuilder stringBuilder2 = stringBuilder1;
      string[] strArray = new string[9];
      strArray[0] = "Name: ";
      strArray[1] = character.Name;
      strArray[2] = "\t\tAge:";
      int num1 = character.Age;
      strArray[3] = num1.ToString();
      strArray[4] = "\t\tRace: ";
      strArray[5] = character.Race.Name;
      strArray[6] = "\tCredits: ";
      num1 = character.Credits;
      strArray[7] = num1.ToString();
      strArray[8] = this._ls;
      string str1 = string.Concat(strArray);
      stringBuilder2.Append(str1);
      stringBuilder1.Append("Homeworld:" + this._ls + "\t" + character.HomeWorld?.ToString() + this._ls);
      stringBuilder1.Append(this._ls + "Racial Traits:" + this._ls + "\t");
      if (character.Race.Notes != null)
        stringBuilder1.Append(character.Race.Notes);
      this.minorSeperator(stringBuilder1, this.ColumnWidth);
      this.title(stringBuilder1, "Characteristics" + this._ls, this.ColumnWidth);
      IList<com.digitalarcsystems.Traveller.DataModel.Attribute> attributes = character.getAttributes();
      for (int index1 = 0; index1 < attributes.Count; ++index1)
      {
        int num2 = 0;
        com.digitalarcsystems.Traveller.DataModel.Attribute attribute = attributes[index1];
        StringBuilder stringBuilder3 = stringBuilder1;
        string name = attribute.Name;
        num1 = attribute.Value;
        string str2 = num1.ToString();
        string str3 = name + ":\t" + str2;
        stringBuilder3.Append(str3);
        if (attribute.Modifier != 0 && !attribute.Name.Equals("Cst", StringComparison.CurrentCultureIgnoreCase))
        {
          StringBuilder stringBuilder4 = stringBuilder1;
          string str4 = attribute.Modifier > 0 ? " " : "";
          num1 = attribute.Modifier;
          string str5 = num1.ToString();
          string str6 = "\t" + str4 + str5;
          stringBuilder4.Append(str6);
          ++num2;
        }
        if (index1 == 0)
        {
          for (int index2 = num2; index2 < 2; ++index2)
            stringBuilder1.Append("\t");
          StringBuilder stringBuilder5 = stringBuilder1;
          num1 = character.getAttributeValue("Str") + character.getAttributeValue("End");
          string str7 = "Carry Cap. (1G):  " + num1.ToString() + "kg";
          stringBuilder5.Append(str7);
        }
        if (index1 == 1)
        {
          for (int index3 = num2; index3 < 2; ++index3)
            stringBuilder1.Append("\t");
          StringBuilder stringBuilder6 = stringBuilder1;
          num1 = character.getAttributeValue("End");
          string str8 = "Max Rounds Melee: " + num1.ToString();
          stringBuilder6.Append(str8);
        }
        stringBuilder1.Append(this._ls);
      }
      this.minorSeperator(stringBuilder1, this.ColumnWidth);
      this.title(stringBuilder1, "Skills" + this._ls, this.ColumnWidth);
      List<string> list = character.Skills.Select<ISkill, string>((Func<ISkill, string>) (entry => (entry.Name.ToLower().Equals("jack-of-all-trades") ? "JoaT" : entry.Name) + ": " + entry.Level.ToString())).ToList<string>();
      list.AddRange((IEnumerable<string>) character.Talents.Select<Talent, string>((Func<Talent, string>) (entry => entry.Name + ": " + entry.Level.ToString())).ToList<string>());
      this.printInColumns(stringBuilder1, this.ColumnWidth, list);
      this.minorSeperator(stringBuilder1, this.ColumnWidth);
      this.title(stringBuilder1, "Contacts, Enemies, Allies, & Rivals" + this._ls, this.ColumnWidth);
      int num3 = 0;
      foreach (Contact contact in character.EntityIKnow)
      {
        if (num3++ >= 3)
        {
          stringBuilder1.Append("\n");
          num3 = 0;
        }
        stringBuilder1.Append(contact.Type.ToString() + ": " + contact.Name + "\t");
      }
      stringBuilder1.Append(this._ls);
      this.minorSeperator(stringBuilder1, this.ColumnWidth);
      this.title(stringBuilder1, "Augments" + this._ls, this.ColumnWidth);
      list.Clear();
      foreach (IEquipment augment in (IEnumerable<IAugmentation>) character.Augments)
        list.Add(augment.Name);
      this.printInColumns(stringBuilder1, this.ColumnWidth, list);
      this.minorSeperator(stringBuilder1, this.ColumnWidth);
      this.title(stringBuilder1, "Equipment" + this._ls, this.ColumnWidth);
      list.Clear();
      foreach (IEquipment of in (IEnumerable<IEquipment>) character.Equipment)
        list.Add(of.Name + " (" + character.equipmentQuantity(of).ToString() + ")");
      this.printInColumns(stringBuilder1, this.ColumnWidth, list);
      Term.column_width = this.ColumnWidth;
      this.minorSeperator(stringBuilder1, this.ColumnWidth);
      this.title(stringBuilder1, "Career History", this.ColumnWidth);
      foreach (Term term in character.CareerHistory)
        stringBuilder1.Append(term?.ToString() + this._ls);
      this.majorSeperator(stringBuilder1, this.ColumnWidth);
      return stringBuilder1.ToString();
    }

    public string CreateEncyclopediaEntry(Character character)
    {
      return character.Race.Name + "   Age:" + character.Age.ToString() + "   Race:    Credits: " + character.Credits.ToString() + "\n";
    }
  }
}
