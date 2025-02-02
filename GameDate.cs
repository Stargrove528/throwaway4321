
// Type: com.digitalarcsystems.Traveller.DataModel.GameDate




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class GameDate
  {
    private int _imperialDay;

    [JsonProperty]
    public int ImperialDay
    {
      get => this._imperialDay;
      set
      {
        if (value <= 0)
          this._imperialDay = 1;
        else
          this._imperialDay = value > 365 ? 365 : value;
      }
    }

    public void IncrementDay(int numDays)
    {
      if (this._imperialDay + numDays > 365)
      {
        this.ImperialYear += (this._imperialDay + numDays) / 365;
        this._imperialDay = (this._imperialDay + numDays) % 365;
      }
      else if (this._imperialDay + numDays < 1)
      {
        Decimal d = ((Decimal) this._imperialDay + (Decimal) numDays) / 365M;
        int num1 = (int) Decimal.Truncate(d);
        Decimal num2 = d - (Decimal) num1;
        this.ImperialYear += num1 - 1;
        this._imperialDay = 365 + (numDays + this._imperialDay) % 365;
      }
      else
        this._imperialDay += numDays;
    }

    public void NextDay() => this.IncrementDay(1);

    public void NextWeek() => this.IncrementDay(7);

    public void NextMonth() => this.IncrementDay(30);

    public GameDate.DayOfWeek GetDayOfWeek()
    {
      return this._imperialDay == 1 ? GameDate.DayOfWeek.Holiday : (GameDate.DayOfWeek) ((this._imperialDay - 2) % 7 + 1);
    }

    [JsonProperty]
    public int ImperialYear { get; set; }

    [JsonConstructor]
    public GameDate()
    {
    }

    public static void Main(string[] args)
    {
      GameDate gameDate = new GameDate()
      {
        ImperialDay = 365,
        ImperialYear = 1105
      };
      Console.WriteLine("Starting with: " + gameDate?.ToString());
      Console.WriteLine("Adding 1 day to it.");
      gameDate.IncrementDay(1);
      string str1 = gameDate.ImperialDay.ToString();
      int num1 = gameDate.ImperialYear;
      string str2 = num1.ToString();
      Console.WriteLine("Current GameDate: " + str1 + "-" + str2);
      gameDate.ImperialDay = 1;
      gameDate.ImperialYear = 1105;
      Console.WriteLine("Starting with " + gameDate?.ToString());
      Console.WriteLine("Adding -1 day to it.");
      gameDate.IncrementDay(-1);
      num1 = gameDate.ImperialDay;
      string str3 = num1.ToString();
      num1 = gameDate.ImperialYear;
      string str4 = num1.ToString();
      Console.WriteLine("Current GameDate: " + str3 + "-" + str4);
      gameDate.ImperialDay = 1;
      gameDate.ImperialYear = 1105;
      Console.WriteLine("Starting with " + gameDate?.ToString());
      num1 = 1587;
      Console.WriteLine("Adding " + num1.ToString() + " days to it. (4 years + 127 days)");
      gameDate.IncrementDay(1587);
      num1 = gameDate.ImperialDay;
      string str5 = num1.ToString();
      num1 = gameDate.ImperialYear;
      string str6 = num1.ToString();
      Console.WriteLine("Current GameDate: " + str5 + "-" + str6);
      gameDate.ImperialDay = 1;
      gameDate.ImperialYear = 1105;
      Console.WriteLine("Starting with " + gameDate?.ToString());
      num1 = -1470;
      Console.WriteLine("Adding " + num1.ToString() + " days to it. (-4 years - 10 days)");
      gameDate.IncrementDay(-1470);
      num1 = gameDate.ImperialDay;
      string str7 = num1.ToString();
      num1 = gameDate.ImperialYear;
      string str8 = num1.ToString();
      Console.WriteLine("Current GameDate: " + str7 + "-" + str8);
      gameDate.ImperialDay = 1;
      gameDate.ImperialYear = 1105;
      Console.WriteLine("Starting with " + gameDate?.ToString());
      Console.WriteLine("NextDay()");
      gameDate.NextDay();
      num1 = gameDate.ImperialDay;
      string str9 = num1.ToString();
      num1 = gameDate.ImperialYear;
      string str10 = num1.ToString();
      Console.WriteLine("Current GameDate: " + str9 + "-" + str10);
      gameDate.ImperialDay = 1;
      gameDate.ImperialYear = 1105;
      Console.WriteLine("Starting with " + gameDate?.ToString());
      Console.WriteLine("NextWeek()");
      gameDate.NextWeek();
      num1 = gameDate.ImperialDay;
      string str11 = num1.ToString();
      num1 = gameDate.ImperialYear;
      string str12 = num1.ToString();
      Console.WriteLine("Current GameDate: " + str11 + "-" + str12);
      gameDate.ImperialDay = 1;
      gameDate.ImperialYear = 1105;
      Console.WriteLine("Starting with " + gameDate?.ToString());
      Console.WriteLine("NextMonth()");
      gameDate.NextMonth();
      num1 = gameDate.ImperialDay;
      string str13 = num1.ToString();
      num1 = gameDate.ImperialYear;
      string str14 = num1.ToString();
      Console.WriteLine("Current GameDate: " + str13 + "-" + str14);
      gameDate.ImperialDay = 1;
      gameDate.ImperialYear = 1105;
      Console.WriteLine("Starting with " + gameDate?.ToString());
      Console.WriteLine("Day of Week Should be Holiday");
      Console.WriteLine("Day of Week: " + gameDate.GetDayOfWeek().ToString());
      Dictionary<GameDate.DayOfWeek, List<int>> dictionary = new Dictionary<GameDate.DayOfWeek, List<int>>();
      for (int index = 1; index < 366; ++index)
      {
        gameDate.ImperialDay = index;
        if (!dictionary.TryGetValue(gameDate.GetDayOfWeek(), out List<int> _))
          dictionary[gameDate.GetDayOfWeek()] = new List<int>();
        dictionary[gameDate.GetDayOfWeek()].Add(index);
      }
      foreach (GameDate.DayOfWeek key in (IEnumerable<GameDate.DayOfWeek>) Enum.GetValues(typeof (GameDate.DayOfWeek)))
      {
        Console.Write(key.ToString() + ": ");
        foreach (int num2 in dictionary[key])
          Console.Write(num2.ToString() + ",");
        Console.WriteLine("END");
      }
    }

    public override string ToString()
    {
      return string.Format("{0}-{1}", (object) this.ImperialDay, (object) this.ImperialYear);
    }

    public enum DayOfWeek
    {
      Holiday,
      Wonday,
      Tuday,
      Thirday,
      Forday,
      Fiday,
      Sexday,
      Senday,
    }
  }
}
