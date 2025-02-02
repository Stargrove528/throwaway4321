
// Type: com.digitalarcsystems.Traveller.DataModel.Gender




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Gender : IComparable<Gender>, IDescribable
  {
    public string name;
    public Gender.PronounType pronoun;

    public string Description
    {
      get => this.name;
      set
      {
      }
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public Gender()
    {
    }

    public Gender(string name, Gender.PronounType pronoun)
    {
      this.name = name;
      this.pronoun = pronoun;
    }

    public int CompareTo(Gender other)
    {
      return string.Compare(this.AsSerialized(), other.AsSerialized(), StringComparison.Ordinal);
    }

    public enum PronounType
    {
      MALE,
      FEMALE,
      OTHER,
    }
  }
}
