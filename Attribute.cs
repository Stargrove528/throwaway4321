
// Type: com.digitalarcsystems.Traveller.DataModel.Attribute




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Attribute : IComparable<Attribute>
  {
    [JsonProperty]
    private Attribute.DieRollType _rollType;
    public static string[] CanonicalStats = new string[6]
    {
      "Str",
      "Dex",
      "End",
      "Int",
      "Edu",
      "Soc"
    };
    [JsonIgnore]
    private int _value;
    private static readonly int[] ModifierTable = new int[42]
    {
      -3,
      -2,
      -2,
      -1,
      -1,
      -1,
      0,
      0,
      0,
      1,
      1,
      1,
      2,
      2,
      2,
      3,
      3,
      3,
      4,
      4,
      4,
      5,
      5,
      5,
      6,
      6,
      6,
      7,
      7,
      7,
      8,
      8,
      8,
      9,
      9,
      9,
      10,
      10,
      10,
      11,
      11,
      11
    };
    private bool _hasBeenInCrisis;
    private int[] _rawRollValues = new int[1];
    protected bool can_be_healed = true;

    public Attribute()
    {
      this.NumDice = 2;
      this.Name = "";
      this.Ordinal = 0;
    }

    public Attribute(string name, int ordinal)
    {
      this.NumDice = 2;
      this.Name = name;
      this.Ordinal = ordinal;
    }

    [JsonConstructor]
    public Attribute(string name, int ordinal, int numOfDice, int racialBonus)
    {
      this.Name = name;
      this.Ordinal = ordinal;
      this.NumDice = numOfDice;
      this.RacialBonus = racialBonus;
    }

    public Attribute(
      string name,
      int ordinal,
      int numOfDice,
      int racialBonus,
      Attribute.DieRollType type)
    {
      this.Name = name;
      this.Ordinal = ordinal;
      this.NumDice = numOfDice;
      this.RacialBonus = racialBonus;
      this._rollType = type;
    }

    public Attribute(Attribute stat)
      : this(stat.Name, stat.Ordinal, stat.NumDice, stat.RacialBonus, stat._rollType)
    {
      this._value = stat._value;
      this.UninjuredValue = stat.UninjuredValue;
      this._hasBeenInCrisis = stat._hasBeenInCrisis;
    }

    [JsonIgnore]
    public Attribute.DieRollType RollType
    {
      get => this._rollType;
      set
      {
        if (value.Equals((object) null) || value == Attribute.DieRollType._2d6)
          this._rollType = Attribute.DieRollType._2d6;
        else
          this._rollType = value;
      }
    }

    public int RawRoll { get; set; }

    public int[] _racialRollValues { get; private set; }

    public int[] RawRollValues
    {
      get
      {
        int[] destinationArray = new int[this._rawRollValues.Length];
        Array.Copy((Array) this._rawRollValues, (Array) destinationArray, this._rawRollValues.Length);
        return destinationArray;
      }
      set
      {
        if (((ICollection<int>) value).IsNullOrEmpty<int>() || value[0].Equals(0))
        {
          this._rawRollValues = new int[2];
          this._racialRollValues = new int[2];
          this.RawRoll = 0;
        }
        else
        {
          this._rawRollValues = new int[value.Length];
          Array.Copy((Array) value, (Array) this._rawRollValues, value.Length);
          this.RawRoll = ((IEnumerable<int>) value).Sum();
          this._rawRollValues = value;
          this._racialRollValues = this.BaseValue();
          this._value = this.GetCalculatedRacialValue();
          this.UninjuredValue = this._value;
          this._hasBeenInCrisis = false;
        }
      }
    }

    public int GetCalculatedRacialValue()
    {
      return this._rollType == Attribute.DieRollType._1d3 || this._rollType == Attribute.DieRollType._1d6 ? this._racialRollValues[0] + this.RacialBonus : ((IEnumerable<int>) this._racialRollValues).Sum() + this.RacialBonus;
    }

    public int[] BaseValue()
    {
      int[] numArray1 = new int[1];
      int[] numArray2;
      switch (this._rollType)
      {
        case Attribute.DieRollType._2d6:
          numArray2 = new int[2]
          {
            this.RawRollValues[0],
            this.RawRollValues[1]
          };
          break;
        case Attribute.DieRollType._1d6:
          numArray2 = new int[2]
          {
            ((IEnumerable<int>) this.RawRollValues).Max(),
            ((IEnumerable<int>) this.RawRollValues).Min()
          };
          break;
        case Attribute.DieRollType._3d6:
          numArray2 = new int[2]
          {
            (int) Math.Ceiling(((IEnumerable<int>) this.RawRollValues).Average()),
            ((IEnumerable<int>) this.RawRollValues).Sum()
          };
          break;
        case Attribute.DieRollType._2d3:
          numArray2 = new int[2]
          {
            (int) Math.Ceiling((double) this.RawRollValues[0] / 2.0),
            (int) Math.Ceiling((double) this.RawRollValues[1] / 2.0)
          };
          break;
        case Attribute.DieRollType._1d3:
          numArray2 = new int[2]
          {
            (int) Math.Ceiling((double) ((IEnumerable<int>) this.RawRollValues).Max() / 2.0),
            (int) Math.Ceiling((double) ((IEnumerable<int>) this.RawRollValues).Min() / 2.0)
          };
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return numArray2;
    }

    public bool CanBeHealed
    {
      get => this.can_be_healed;
      set => this.can_be_healed = value;
    }

    public virtual int Ordinal { get; private set; }

    public virtual string Name { get; set; }

    public virtual void InitializeValue(int value)
    {
      this.RawRollValues = this.DeriveRawRollsFromValue(value);
    }

    public virtual int TemporaryAugmentation { get; set; }

    [JsonProperty]
    public virtual int Value
    {
      get
      {
        if (this.RawRollValues == null || ((IEnumerable<int>) this.RawRollValues).Sum() == 0)
          EngineLog.Warning("Attempting to get Attribute Value without first setting RawRollValues Stat Name: " + this.Name + " Value: " + this._value.ToString());
        return this._value;
      }
      set
      {
        if (this.RawRollValues == null || ((IEnumerable<int>) this.RawRollValues).Sum() == 0)
          this.RawRollValues = this.DeriveRawRollsFromValue(value);
        this._value = value;
        if (this._value > 0)
          return;
        this._hasBeenInCrisis = true;
      }
    }

    private int[] DeriveRawRollsFromValue(int currentValue)
    {
      currentValue -= this.RacialBonus;
      int[] numArray;
      switch (this.RollType)
      {
        case Attribute.DieRollType._2d6:
          int num1 = this.Clamp<int>(currentValue, 2, 12);
          int num2 = (double) num1 / 6.0 > 1.0 ? 6 : this.Clamp<int>(num1 % 6 - 1, 1, 6);
          int num3 = num1 - num2;
          numArray = new int[2]{ num2, num3 };
          break;
        case Attribute.DieRollType._1d6:
          numArray = new int[2]
          {
            this.Clamp<int>(currentValue, 1, 6),
            1
          };
          break;
        case Attribute.DieRollType._3d6:
          throw new NotImplementedException("3D6 is not implemented yet");
        case Attribute.DieRollType._2d3:
          float a = (float) this.Clamp<int>(currentValue, 2, 6) / 2f;
          numArray = new int[2]
          {
            (int) Math.Ceiling((double) a),
            (int) a
          };
          break;
        case Attribute.DieRollType._1d3:
          numArray = new int[2]
          {
            2 * this.Clamp<int>(currentValue, 1, 3),
            1
          };
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return numArray;
    }

    public T Clamp<T>(T val, T min, T max) where T : IComparable<T>
    {
      if (val.CompareTo(min) < 0)
        return min;
      return val.CompareTo(max) > 0 ? max : val;
    }

    [JsonIgnore]
    public int TotalValue => this.Value + this.TemporaryAugmentation;

    public virtual int UninjuredValue { get; set; }

    public virtual int Modifier
    {
      get => Attribute.ModifierTable[this.TotalValue < 0 ? 0 : this.TotalValue];
    }

    [JsonProperty]
    public virtual int NumDice { get; private set; }

    public virtual int RacialBonus { get; set; }

    public virtual void Increment() => this.Add(1);

    public virtual void Subtract(int subtractMe)
    {
      this._value -= Math.Abs(subtractMe);
      if (this._value > 0)
        return;
      this._hasBeenInCrisis = true;
      EngineLog.Print("Attribute.Subract() and here also setting crisis to true for " + this.Name);
    }

    public virtual void Add(int addMe) => this._value += addMe;

    public virtual void Age(int subtractMe)
    {
      subtractMe = Math.Abs(subtractMe);
      this.Subtract(subtractMe);
      if (this.UninjuredValue - subtractMe >= 0)
        this.UninjuredValue -= subtractMe;
      else
        this.UninjuredValue = 0;
    }

    public virtual bool InCrisis() => this._value <= 0;

    public virtual bool Injured => this.UninjuredValue > this._value;

    public virtual bool HasBeenInCrisis() => !this.can_be_healed && this._hasBeenInCrisis;

    public virtual void Generate()
    {
      this.RawRollValues = new int[2]
      {
        Dice.Roll1D6(),
        Dice.Roll1D6()
      };
    }

    public virtual void Generate(Attribute.DieRollType rollType)
    {
    }

    public static int GetCanonicalOrdinalForStat(string statName)
    {
      int canonicalOrdinalForStat = -1;
      for (int index = 0; index < Attribute.CanonicalStats.Length && canonicalOrdinalForStat == -1; ++index)
      {
        if (Attribute.CanonicalStats[index].Equals(statName, StringComparison.InvariantCultureIgnoreCase))
          canonicalOrdinalForStat = index;
      }
      return canonicalOrdinalForStat;
    }

    public int CompareTo(Attribute other)
    {
      return string.Compare(this.AsSerialized(), other.AsSerialized(), StringComparison.Ordinal);
    }

    public override string ToString() => this.Name + ": " + this._value.ToString();

    public enum DieRollType
    {
      _2d6,
      _1d6,
      _3d6,
      _2d3,
      _1d3,
    }
  }
}
