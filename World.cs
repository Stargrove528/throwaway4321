
// Type: com.digitalarcsystems.Traveller.DataModel.World




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class World : ICategorizable, IDescribable
  {
    [JsonProperty]
    internal List<World.WorldType> worldTypes = new List<World.WorldType>();
    [JsonProperty]
    public int hexNumber = 0;
    [JsonProperty]
    public int techLevel = 0;
    [JsonProperty]
    public string UWP = (string) null;
    [JsonProperty]
    public string starport = (string) null;
    [JsonProperty]
    public string size = (string) null;

    [JsonProperty]
    public List<string> Categories
    {
      get
      {
        return this.worldTypes.Select<World.WorldType, string>((Func<World.WorldType, string>) (wt => wt.ToString())).ToList<string>();
      }
    }

    [JsonProperty]
    public string SectorName { get; set; }

    [JsonProperty]
    public string SubsectorName { get; set; }

    [JsonProperty]
    public char atmosphere { get; private set; }

    [JsonProperty]
    public char hydrographic { get; private set; }

    [JsonProperty]
    public char governmentType { get; private set; }

    [JsonProperty]
    public char lawLevel { get; private set; }

    [JsonProperty]
    public bool isCustomWorld { get; set; } = false;

    [JsonIgnore]
    public string HistoricalEra
    {
      get
      {
        return new string[22]
        {
          "Stone Age",
          "Medieval Age",
          "Age of Sail",
          "Industrial Revolution",
          "Mechanized Age",
          "Broadcast Age",
          "Nuclear Age",
          "Space Age",
          "Digital Age",
          "Early Stellar Age / Fusion Age",
          "Early Stellar Age / Gravitic Age",
          "Average Imperial",
          "Average Imperial",
          "Average Stellar / Geneering Age",
          "High Stellar",
          "Imperial Maximum",
          "Artificial Persons Age",
          "Personality Transfer Age",
          "Exotics Age",
          "Matter Transport / Antimatter Age",
          "Skipdrive Age",
          "Stasis Age"
        }[this.techLevel];
      }
    }

    [JsonIgnore]
    public string LawLevelDescription
    {
      get
      {
        return new string[22]
        {
          "No Prohibitions (Nuclear Weapons)",
          "Body Pistols, Explosives, Poison Gas, etc.",
          "Portable Energy Weapons",
          "Machine Guns, Automatic Weapons, etc.",
          "Light Assault Weapons",
          "Personal Concealable Firearms",
          "All firearms except Shotguns",
          "Shotguns",
          "Blade Weapons Controlled",
          "No weapons outside home",
          "Weapon possession",
          "Rigid control of civilian movement",
          "Unrestricted invasion of privacy",
          "Paramilitary law enforcement",
          "Full-fledged police state",
          "All facets of daily life rigidly controlled",
          "Severe punishment for petty infractions",
          "Legalized oppressive practices",
          "Routinely oppressive and restrictive",
          "Excessively oppressive and restrictive",
          "Totally oppressive and restrictive",
          "The government is not afraid to use mind control technologies or slavery."
        }[Utility.ConvertTravellerCodeToNumber(this.lawLevel)];
      }
    }

    [JsonIgnore]
    public string GovernmentDescription
    {
      get
      {
        string[] strArray = new string[14]
        {
          "None",
          "Company / Corporation",
          "Participating Democracy",
          "Self-Perpetuating Oligarchy",
          "Representative Democracy",
          "Feudal Technocracy",
          "Captive Government",
          "Balkanisation",
          "Civil Service Bureaucracy",
          "Impersonal Bureaucracy",
          "Charismatic Dictator",
          "Non-Charismatic Dictator",
          "Charismatic Oligarchy",
          "Religious Dictatorship"
        };
        int number = Utility.ConvertTravellerCodeToNumber(this.governmentType);
        return number > 13 ? strArray[13] : strArray[number];
      }
    }

    [JsonIgnore]
    public string PopulationDescription
    {
      get
      {
        return new string[13]
        {
          "None",
          "Few",
          "Hundreds",
          "Thousands",
          "Tens of thousands",
          "Hundreds of thousands",
          "Millions",
          "Tens of millions",
          "Hundreds of millions",
          "Billions",
          "Tens of billions",
          "Hundreds of billions",
          "Trillions"
        }[this.Population];
      }
    }

    [JsonIgnore]
    public string HydrographicPercentage
    {
      get => (Utility.ConvertTravellerCodeToNumber(this.hydrographic) * 10).ToString() + "%";
    }

    [JsonIgnore]
    public string AtmosphereDescription
    {
      get
      {
        return new string[16]
        {
          "None",
          "Trace",
          "Very Thin, Tainted",
          "Very Thin",
          "Thin, Tainted",
          "Thin",
          "Standard",
          "Standard, Tainted",
          "Dense",
          "Dense, Tainted",
          "Exotic",
          "Corrosive",
          "Insidious",
          "Dense, High",
          "Thin, Low",
          "Unusual"
        }[Utility.ConvertTravellerCodeToNumber(this.atmosphere)];
      }
    }

    [JsonIgnore]
    public string StarportQualityDescription
    {
      get
      {
        switch (this.starport.ToLower())
        {
          case "a":
            return "Starship yards and fuel";
          case "b":
            return "Repair facilities and fuel";
          case "c":
            return "Maintenance facilities and fuel";
          case "d":
            return "Refined fuel";
          case "e":
            return "Unrefined fuel only";
          case "x":
            return "None";
          default:
            return "Undefined";
        }
      }
    }

    public string PlanetSizeInKm => this.DiameterInKm.ToString() + " km";

    public World(string sectorTextLine)
    {
      this.Population = 0;
      this.Name = sectorTextLine.Substring(0, 14).Trim();
      this.hexNumber = int.Parse(sectorTextLine.Substring(14, 4).Trim());
      this.UWP = sectorTextLine.Substring(19, 9).Trim();
      this.processUWP();
      this.processCodes(sectorTextLine.Substring(32, 15));
      if (sectorTextLine[48] == ' ')
        return;
      if (sectorTextLine[48] == 'R' || sectorTextLine[48] == 'r')
        this.worldTypes.Add(World.WorldType.Red_Zone);
      else if (sectorTextLine[48] == 'A' || sectorTextLine[48] == 'a')
        this.worldTypes.Add(World.WorldType.Amber_zone);
      else
        Console.WriteLine("Unknown TAS Designation: " + sectorTextLine[48].ToString());
    }

    [JsonConstructor]
    public World()
    {
      this.Population = 0;
      this.Name = "";
      this.hexNumber = 0;
      this.UWP = "0000000-0";
      this.processUWP();
      this.processCodes("               ");
    }

    private void processCodes(string codes)
    {
      string str = codes;
      char[] chArray = new char[1]{ ' ' };
      foreach (string classification in str.Split(chArray))
        this.addClassification(classification);
    }

    private void processUWP()
    {
      char ch = this.UWP[0];
      this.starport = ch.ToString() ?? "";
      ch = this.UWP[1];
      this.size = ch.ToString() ?? "";
      this.atmosphere = this.UWP[2];
      this.hydrographic = this.UWP[3];
      ch = this.UWP[4];
      this.PopulationInHex = ch.ToString() ?? "";
      this.governmentType = this.UWP[5];
      this.lawLevel = this.UWP[6];
      ch = this.UWP[8];
      this.TechLevelInHex = ch.ToString() ?? "";
      if (this.TechLevel >= 12)
      {
        this.addClassification(World.WorldType.High_Technology);
      }
      else
      {
        if (this.TechLevel > 5)
          return;
        this.addClassification(World.WorldType.Low_Technology);
      }
    }

    public virtual string PopulationInHex
    {
      get => this.Population.ToString("x");
      set => this.Population = Convert.ToInt32(value, 16);
    }

    [JsonProperty]
    public virtual int Population { get; set; }

    public virtual string Name { get; set; }

    public virtual void addClassification(string classification)
    {
      try
      {
        this.addClassification(World.WorldType.fromAbreviation(classification));
      }
      catch (Exception ex)
      {
        Console.WriteLine("Unknown Classification: " + classification);
      }
    }

    private void addClassification(World.WorldType addMe)
    {
      if (this.worldTypes.Contains(addMe))
        return;
      this.worldTypes.Add(addMe);
    }

    [JsonProperty]
    public IList<World.WorldType> DominantTypes
    {
      get
      {
        return (IList<World.WorldType>) new List<World.WorldType>((IEnumerable<World.WorldType>) this.worldTypes);
      }
    }

    public virtual bool containsClassification(World.WorldType containsMe)
    {
      return this.worldTypes.Contains(containsMe);
    }

    public virtual int TechLevel => this.techLevel;

    public virtual string TechLevelInHex
    {
      set
      {
        if (!value.Equals("G"))
          this.techLevel = Convert.ToInt32(value, 16);
        else
          this.techLevel = 16;
      }
    }

    public virtual int DiameterInKm
    {
      get
      {
        int diameterInKm = 0;
        switch (Convert.ToInt64(this.size, 16))
        {
          case 0:
            diameterInKm = 800;
            break;
          case 1:
            diameterInKm = 1600;
            break;
          case 2:
            diameterInKm = 3200;
            break;
          case 3:
            diameterInKm = 4800;
            break;
          case 4:
            diameterInKm = 6400;
            break;
          case 5:
            diameterInKm = 8000;
            break;
          case 6:
            diameterInKm = 9600;
            break;
          case 7:
            diameterInKm = 11200;
            break;
          case 8:
            diameterInKm = 12800;
            break;
          case 9:
            diameterInKm = 14400;
            break;
          case 10:
            diameterInKm = 16000;
            break;
        }
        return diameterInKm;
      }
    }

    public virtual float SufaceGravityInGs
    {
      get
      {
        float sufaceGravityInGs = 0.0f;
        switch (Convert.ToInt32(this.size, 16))
        {
          case 0:
            sufaceGravityInGs = 0.0f;
            break;
          case 1:
            sufaceGravityInGs = 0.05f;
            break;
          case 2:
            sufaceGravityInGs = 0.15f;
            break;
          case 3:
            sufaceGravityInGs = 0.25f;
            break;
          case 4:
            sufaceGravityInGs = 0.35f;
            break;
          case 5:
            sufaceGravityInGs = 0.45f;
            break;
          case 6:
            sufaceGravityInGs = 0.7f;
            break;
          case 7:
            sufaceGravityInGs = 0.9f;
            break;
          case 8:
            sufaceGravityInGs = 1f;
            break;
          case 9:
            sufaceGravityInGs = 1.25f;
            break;
          case 10:
            sufaceGravityInGs = 1.4f;
            break;
        }
        return sufaceGravityInGs;
      }
    }

    [JsonProperty]
    public virtual string AtmospericTypeDescription
    {
      get
      {
        return new string[16]
        {
          "None: Vacc Suit Required. (0.00 Atmospheres)",
          "Trace: Vacc Suit Required.  (0.001-0.009 Atmospheres)",
          "Very Thin, Tainted: Respirator & Filter Required (0.1-0.42 Atmospheres) ",
          "Very Thin: Respirator Required (0.1-0.42 Atmospheres)",
          "Thin, Tainted: Filter Required (0.43-0.7 Atmospheres)",
          "Thin: No special Gear Required (0.43-0.7 Atmospheres)",
          "Standard: No special Gear Required (0.71-1.49 Atmospheres)",
          "Standard, Tained: Filter Required (0.71-1.49 Atmospheres)",
          "Dense: No special Gear Required (1.5-2.49 Atmospheres)",
          "Dense Tainted: Filter Required (1.5-2.49 Atmospheres)",
          "Exotic: Air Supply required. (Any Atmospheres)",
          "Corrosive: Vacc Suit Required (HEV Ideal) (Any Atmospheres)",
          "Insidious: Vacc Suit Required (HEV Ideal) (Any Pressure)",
          "Dense, High: HEV Suit (2.5+ Atmospheres)",
          "Thin, Low (0.5 or fewer Atmospheres)",
          "Unusual Atmosphere."
        }[Convert.ToInt32(this.atmosphere.ToString(), 16)];
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Name: " + this.Name + " Starport: " + this.starport + " Size: " + this.DiameterInKm.ToString() + " km Gravity: " + this.SufaceGravityInGs.ToString() + "g ");
      stringBuilder.Append(" TechLevel: " + this.TechLevel.ToString());
      stringBuilder.Append(" Population: " + this.Population.ToString() + " ");
      stringBuilder.Append(" Law: " + this.lawLevel.ToString());
      stringBuilder.Append(" Hydro: " + this.hydrographic.ToString());
      stringBuilder.Append(" Atmo: " + this.AtmospericTypeDescription + " Codes: ");
      foreach (World.WorldType worldType in this.worldTypes)
        stringBuilder.Append(worldType?.ToString() + " ");
      return stringBuilder.ToString();
    }

    [JsonProperty]
    public string Description
    {
      get => this.ToString();
      set
      {
      }
    }

    public static List<World> LoadFromString(string contents)
    {
      List<World> worldList = new List<World>();
      int index1 = 0;
      string str = (string) null;
      string[] array = ((IEnumerable<string>) contents.Split('\n', '\r')).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x) || x == "\r")).ToArray<string>();
      while (!array[index1].StartsWith("...", StringComparison.Ordinal))
        ++index1;
      int index2 = index1 + 1;
      while (index2 < array.Length)
      {
        try
        {
          str = array[index2];
          ++index2;
          if (str != null && str.Count<char>() > 10)
            worldList.Add(new World(str));
        }
        catch (Exception ex)
        {
          Console.WriteLine("Exception occured at line: " + index2.ToString());
          Console.WriteLine("Line: " + str);
          Console.WriteLine("Exception was: " + ex?.ToString());
          Console.WriteLine(ex.ToString());
          Console.Write(ex.StackTrace);
        }
      }
      return worldList;
    }

    public static List<World> loadWorldsFromInputStream(Stream stream)
    {
      List<World> worldList = new List<World>();
      int num1 = 1;
      string str = (string) null;
      try
      {
        using (StreamReader streamReader = new StreamReader(stream))
        {
          Console.WriteLine("LoadingWorlds (STREAM) [" + streamReader?.ToString() + "]");
          while (!streamReader.ReadLine().StartsWith("...", StringComparison.Ordinal))
            ++num1;
          int num2 = num1 + 1;
          streamReader.ReadLine();
          while (!streamReader.EndOfStream)
          {
            try
            {
              str = streamReader.ReadLine();
              ++num2;
              if (str != null && str.Count<char>() > 10)
                worldList.Add(new World(str));
            }
            catch (Exception ex)
            {
              Console.WriteLine("Exception occured at line: " + num2.ToString());
              Console.WriteLine("Line: " + str);
              Console.WriteLine("Exception was: " + ex?.ToString());
              Console.WriteLine(ex.ToString());
              Console.Write(ex.StackTrace);
            }
          }
        }
      }
      catch (AccessViolationException ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
      catch (UnauthorizedAccessException ex)
      {
        Console.WriteLine(ex.ToString());
        Console.Write(ex.StackTrace);
      }
      Console.WriteLine("Read " + worldList.Count.ToString() + " worlds in");
      return worldList;
    }

    public virtual int distanceWithinSameSector(World destination)
    {
      int num1 = (int) Math.Floor((double) (this.hexNumber / 100));
      int num2 = (int) Math.Floor((double) (destination.hexNumber / 100));
      int num3 = this.hexNumber - 100 * num1;
      int num4 = destination.hexNumber - 100 * num2;
      int num5 = Math.Abs(num1 - num2);
      int num6 = num5 / 2;
      int num7 = num1 % 2;
      int num8 = num2 % 2;
      if (num7 == 1 && num8 == 0)
        ++num6;
      int num9 = num3 - num6;
      int num10 = num9 + num5;
      int num11 = 0;
      if (num4 < num9)
        num11 = num9 - num4;
      if (num4 > num10)
        num11 = num4 - num10;
      return num5 + num11;
    }

    public static void Main(string[] args)
    {
    }

    public sealed class WorldType
    {
      public static readonly World.WorldType All = new World.WorldType(nameof (All), World.WorldType.InnerEnum.All, nameof (All));
      public static readonly World.WorldType Agricultural = new World.WorldType(nameof (Agricultural), World.WorldType.InnerEnum.Agricultural, "AG");
      public static readonly World.WorldType Asteroid = new World.WorldType(nameof (Asteroid), World.WorldType.InnerEnum.Asteroid, "As");
      public static readonly World.WorldType Barren = new World.WorldType(nameof (Barren), World.WorldType.InnerEnum.Barren, "Ba");
      public static readonly World.WorldType Desert = new World.WorldType(nameof (Desert), World.WorldType.InnerEnum.Desert, "De");
      public static readonly World.WorldType Dangerous = new World.WorldType(nameof (Dangerous), World.WorldType.InnerEnum.Dangerous, "Da");
      public static readonly World.WorldType Fluid_Oceans = new World.WorldType(nameof (Fluid_Oceans), World.WorldType.InnerEnum.Fluid_Oceans, "Fl");
      public static readonly World.WorldType Forbidden = new World.WorldType(nameof (Forbidden), World.WorldType.InnerEnum.Forbidden, "Fo");
      public static readonly World.WorldType Garden = new World.WorldType(nameof (Garden), World.WorldType.InnerEnum.Garden, "Ga");
      public static readonly World.WorldType Hell_World = new World.WorldType("Hell World", World.WorldType.InnerEnum.HellWorld, "He");
      public static readonly World.WorldType High_Population = new World.WorldType(nameof (High_Population), World.WorldType.InnerEnum.High_Population, "Hi");
      public static readonly World.WorldType High_Technology = new World.WorldType(nameof (High_Technology), World.WorldType.InnerEnum.High_Technology, "Ht");
      public static readonly World.WorldType Ice_Capped = new World.WorldType(nameof (Ice_Capped), World.WorldType.InnerEnum.Ice_Capped, "IC");
      public static readonly World.WorldType Industrial = new World.WorldType(nameof (Industrial), World.WorldType.InnerEnum.Industrial, "In");
      public static readonly World.WorldType Low_Population = new World.WorldType(nameof (Low_Population), World.WorldType.InnerEnum.Low_Population, "Lo");
      public static readonly World.WorldType Low_Technology = new World.WorldType(nameof (Low_Technology), World.WorldType.InnerEnum.Low_Technology, "Lt");
      public static readonly World.WorldType Non_Agricultural = new World.WorldType(nameof (Non_Agricultural), World.WorldType.InnerEnum.Non_Agricultural, "Na");
      public static readonly World.WorldType Non_Industrial = new World.WorldType(nameof (Non_Industrial), World.WorldType.InnerEnum.Non_Industrial, "Ni");
      public static readonly World.WorldType Poor = new World.WorldType(nameof (Poor), World.WorldType.InnerEnum.Poor, "Po");
      public static readonly World.WorldType PreAgricultural = new World.WorldType("Preagricultural", World.WorldType.InnerEnum.PreAgriculture, "Pa");
      public static readonly World.WorldType PreIndustrial = new World.WorldType("Preindustrial", World.WorldType.InnerEnum.PreIndustrial, "Pi");
      public static readonly World.WorldType Puzzle = new World.WorldType(nameof (Puzzle), World.WorldType.InnerEnum.Puzzle, "Pz");
      public static readonly World.WorldType Rich = new World.WorldType(nameof (Rich), World.WorldType.InnerEnum.Rich, "Ri");
      public static readonly World.WorldType PreHigh = new World.WorldType("Prehigh", World.WorldType.InnerEnum.PreHigh, "Ph");
      public static readonly World.WorldType PreRich = new World.WorldType(nameof (PreRich), World.WorldType.InnerEnum.PreRich, "Pr");
      public static readonly World.WorldType Satellite = new World.WorldType(nameof (Satellite), World.WorldType.InnerEnum.Satellite, "Sa");
      public static readonly World.WorldType Vacuum = new World.WorldType(nameof (Vacuum), World.WorldType.InnerEnum.Vacuum, "Va");
      public static readonly World.WorldType Ocean_World = new World.WorldType("Ocean World", World.WorldType.InnerEnum.OceanWorld, "Oc");
      public static readonly World.WorldType Water_World = new World.WorldType(nameof (Water_World), World.WorldType.InnerEnum.Water_World, "Wa");
      public static readonly World.WorldType Amber_zone = new World.WorldType(nameof (Amber_zone), World.WorldType.InnerEnum.Amber_zone, "A");
      public static readonly World.WorldType Red_Zone = new World.WorldType(nameof (Red_Zone), World.WorldType.InnerEnum.Red_Zone, "R");
      public static readonly World.WorldType Restricted_Access = new World.WorldType("Restricted Access", World.WorldType.InnerEnum.RestrictedAccess, "Re");
      public static readonly World.WorldType No_Classification = new World.WorldType(nameof (No_Classification), World.WorldType.InnerEnum.No_Classification, "None");
      public static readonly World.WorldType Ancient_Site = new World.WorldType(nameof (Ancient_Site), World.WorldType.InnerEnum.Ancient_Site, "An");
      public static readonly World.WorldType Subsector_Capital = new World.WorldType(nameof (Subsector_Capital), World.WorldType.InnerEnum.Subsector_Capital, "Cp");
      public static readonly World.WorldType Sector_Capital = new World.WorldType(nameof (Sector_Capital), World.WorldType.InnerEnum.ImperialCapital, "Cs");
      public static readonly World.WorldType Imperial_Capital = new World.WorldType(nameof (Imperial_Capital), World.WorldType.InnerEnum.ImperialCapital, "Cx");
      public static readonly World.WorldType Exile_Camp = new World.WorldType(nameof (Exile_Camp), World.WorldType.InnerEnum.Exile_Camp, "Ex");
      public static readonly World.WorldType Millitary_Rule = new World.WorldType(nameof (Millitary_Rule), World.WorldType.InnerEnum.Millitary_Rule, "Mr");
      public static readonly World.WorldType Prison_Camp = new World.WorldType(nameof (Prison_Camp), World.WorldType.InnerEnum.Prison_Camp, "Px");
      public static readonly World.WorldType Research_Station = new World.WorldType(nameof (Research_Station), World.WorldType.InnerEnum.Research_Station, "Rs");
      public static readonly World.WorldType XBoat_Station = new World.WorldType(nameof (XBoat_Station), World.WorldType.InnerEnum.XBoat_Station, "Xb");
      public static readonly World.WorldType Chirper_World = new World.WorldType(nameof (Chirper_World), World.WorldType.InnerEnum.Chirper_World, "Cw");
      public static readonly World.WorldType Chirper_Population_Small = new World.WorldType(nameof (Chirper_Population_Small), World.WorldType.InnerEnum.Chirper_Population_Small, "C0");
      public static readonly World.WorldType Chirper_Population_10_Percent = new World.WorldType(nameof (Chirper_Population_10_Percent), World.WorldType.InnerEnum.Chirper_Population_10_Percent, "C1");
      public static readonly World.WorldType Chirper_Population_20_Percent = new World.WorldType(nameof (Chirper_Population_20_Percent), World.WorldType.InnerEnum.Chirper_Population_20_Percent, "C2");
      public static readonly World.WorldType Chirper_Population_30_Percent = new World.WorldType(nameof (Chirper_Population_30_Percent), World.WorldType.InnerEnum.Chirper_Population_30_Percent, "C3");
      public static readonly World.WorldType Chirper_Population_40_Percent = new World.WorldType(nameof (Chirper_Population_40_Percent), World.WorldType.InnerEnum.Chirper_Population_40_Percent, "C4");
      public static readonly World.WorldType Chirper_Population_50_Percent = new World.WorldType(nameof (Chirper_Population_50_Percent), World.WorldType.InnerEnum.Chirper_Population_50_Percent, "C5");
      public static readonly World.WorldType Chirper_Population_60_Percent = new World.WorldType(nameof (Chirper_Population_60_Percent), World.WorldType.InnerEnum.Chirper_Population_60_Percent, "C6");
      public static readonly World.WorldType Chirper_Population_70_Percent = new World.WorldType(nameof (Chirper_Population_70_Percent), World.WorldType.InnerEnum.Chirper_Population_70_Percent, "C7");
      public static readonly World.WorldType Chirper_Population_80_Percent = new World.WorldType(nameof (Chirper_Population_80_Percent), World.WorldType.InnerEnum.Chirper_Population_80_Percent, "C8");
      public static readonly World.WorldType Chirper_Population_90_Percent = new World.WorldType(nameof (Chirper_Population_90_Percent), World.WorldType.InnerEnum.Chirper_Population_90_Percent, "C9");
      public static readonly World.WorldType Droyne_Population_Small = new World.WorldType(nameof (Droyne_Population_Small), World.WorldType.InnerEnum.Droyne_Population_Small, "D0");
      public static readonly World.WorldType Droyne_Population_10_Percent = new World.WorldType(nameof (Droyne_Population_10_Percent), World.WorldType.InnerEnum.Droyne_Population_10_Percent, "D1");
      public static readonly World.WorldType Droyne_Population_20_Percent = new World.WorldType(nameof (Droyne_Population_20_Percent), World.WorldType.InnerEnum.Droyne_Population_20_Percent, "D2");
      public static readonly World.WorldType Droyne_Population_30_Percent = new World.WorldType(nameof (Droyne_Population_30_Percent), World.WorldType.InnerEnum.Droyne_Population_30_Percent, "D3");
      public static readonly World.WorldType Droyne_Population_40_Percent = new World.WorldType(nameof (Droyne_Population_40_Percent), World.WorldType.InnerEnum.Droyne_Population_40_Percent, "D4");
      public static readonly World.WorldType Droyne_Population_50_Percent = new World.WorldType(nameof (Droyne_Population_50_Percent), World.WorldType.InnerEnum.Droyne_Population_50_Percent, "D5");
      public static readonly World.WorldType Droyne_Population_60_Percent = new World.WorldType(nameof (Droyne_Population_60_Percent), World.WorldType.InnerEnum.Droyne_Population_60_Percent, "D6");
      public static readonly World.WorldType Droyne_Population_70_Percent = new World.WorldType(nameof (Droyne_Population_70_Percent), World.WorldType.InnerEnum.Droyne_Population_70_Percent, "D7");
      public static readonly World.WorldType Droyne_Population_80_Percent = new World.WorldType(nameof (Droyne_Population_80_Percent), World.WorldType.InnerEnum.Droyne_Population_80_Percent, "D8");
      public static readonly World.WorldType Droyne_Population_90_Percent = new World.WorldType(nameof (Droyne_Population_90_Percent), World.WorldType.InnerEnum.Droyne_Population_90_Percent, "D9");
      public static readonly World.WorldType Droyne_World = new World.WorldType(nameof (Droyne_World), World.WorldType.InnerEnum.Droyne_World, "Dw");
      public static readonly World.WorldType Non_Hiver_Population = new World.WorldType(nameof (Non_Hiver_Population), World.WorldType.InnerEnum.Non_Hiver_Population, "Nh");
      public static readonly World.WorldType Non_KKree_Population = new World.WorldType(nameof (Non_KKree_Population), World.WorldType.InnerEnum.Non_KKree_Population, "Nk");
      private static readonly IList<World.WorldType> valueList = (IList<World.WorldType>) new List<World.WorldType>();
      private readonly string nameValue;
      private readonly int ordinalValue;
      private readonly World.WorldType.InnerEnum innerEnumValue;
      private static int nextOrdinal = 0;
      public string abbreviation;

      [JsonConstructor]
      static WorldType()
      {
        World.WorldType.valueList.Add(World.WorldType.All);
        World.WorldType.valueList.Add(World.WorldType.Agricultural);
        World.WorldType.valueList.Add(World.WorldType.Asteroid);
        World.WorldType.valueList.Add(World.WorldType.Barren);
        World.WorldType.valueList.Add(World.WorldType.Dangerous);
        World.WorldType.valueList.Add(World.WorldType.Desert);
        World.WorldType.valueList.Add(World.WorldType.Fluid_Oceans);
        World.WorldType.valueList.Add(World.WorldType.Forbidden);
        World.WorldType.valueList.Add(World.WorldType.Garden);
        World.WorldType.valueList.Add(World.WorldType.Hell_World);
        World.WorldType.valueList.Add(World.WorldType.High_Population);
        World.WorldType.valueList.Add(World.WorldType.High_Technology);
        World.WorldType.valueList.Add(World.WorldType.Ice_Capped);
        World.WorldType.valueList.Add(World.WorldType.Industrial);
        World.WorldType.valueList.Add(World.WorldType.Low_Population);
        World.WorldType.valueList.Add(World.WorldType.Low_Technology);
        World.WorldType.valueList.Add(World.WorldType.Non_Agricultural);
        World.WorldType.valueList.Add(World.WorldType.Non_Industrial);
        World.WorldType.valueList.Add(World.WorldType.Ocean_World);
        World.WorldType.valueList.Add(World.WorldType.Poor);
        World.WorldType.valueList.Add(World.WorldType.PreAgricultural);
        World.WorldType.valueList.Add(World.WorldType.PreIndustrial);
        World.WorldType.valueList.Add(World.WorldType.PreHigh);
        World.WorldType.valueList.Add(World.WorldType.PreRich);
        World.WorldType.valueList.Add(World.WorldType.Prison_Camp);
        World.WorldType.valueList.Add(World.WorldType.Puzzle);
        World.WorldType.valueList.Add(World.WorldType.Rich);
        World.WorldType.valueList.Add(World.WorldType.Vacuum);
        World.WorldType.valueList.Add(World.WorldType.Water_World);
        World.WorldType.valueList.Add(World.WorldType.Amber_zone);
        World.WorldType.valueList.Add(World.WorldType.Red_Zone);
        World.WorldType.valueList.Add(World.WorldType.No_Classification);
        World.WorldType.valueList.Add(World.WorldType.Ancient_Site);
        World.WorldType.valueList.Add(World.WorldType.Satellite);
        World.WorldType.valueList.Add(World.WorldType.Subsector_Capital);
        World.WorldType.valueList.Add(World.WorldType.Sector_Capital);
        World.WorldType.valueList.Add(World.WorldType.Imperial_Capital);
        World.WorldType.valueList.Add(World.WorldType.Exile_Camp);
        World.WorldType.valueList.Add(World.WorldType.Millitary_Rule);
        World.WorldType.valueList.Add(World.WorldType.Prison_Camp);
        World.WorldType.valueList.Add(World.WorldType.Research_Station);
        World.WorldType.valueList.Add(World.WorldType.Restricted_Access);
        World.WorldType.valueList.Add(World.WorldType.XBoat_Station);
        World.WorldType.valueList.Add(World.WorldType.Chirper_World);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_Small);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_10_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_20_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_30_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_40_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_50_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_60_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_70_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_80_Percent);
        World.WorldType.valueList.Add(World.WorldType.Chirper_Population_90_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_Small);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_10_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_20_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_30_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_40_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_50_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_60_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_70_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_80_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_Population_90_Percent);
        World.WorldType.valueList.Add(World.WorldType.Droyne_World);
        World.WorldType.valueList.Add(World.WorldType.Non_Hiver_Population);
        World.WorldType.valueList.Add(World.WorldType.Non_KKree_Population);
      }

      public WorldType(string name, World.WorldType.InnerEnum innerEnum, string abbreviation)
      {
        this.abbreviation = abbreviation;
        this.nameValue = name;
        this.ordinalValue = World.WorldType.nextOrdinal++;
        this.innerEnumValue = innerEnum;
      }

      public static World.WorldType fromAbreviation(string subject)
      {
        World.WorldType worldType1 = World.WorldType.No_Classification;
        foreach (World.WorldType worldType2 in (IEnumerable<World.WorldType>) World.WorldType.values())
        {
          if (worldType2.abbreviation.Equals(subject.Trim(), StringComparison.CurrentCultureIgnoreCase))
          {
            worldType1 = worldType2;
            break;
          }
        }
        if (worldType1 == World.WorldType.No_Classification && subject != null && subject.Trim().Length > 0)
          Console.WriteLine("Unknown Abbreviation: " + subject);
        return worldType1;
      }

      public static IList<World.WorldType> values() => World.WorldType.valueList;

      public World.WorldType.InnerEnum InnerEnumValue() => this.innerEnumValue;

      public int ordinal() => this.ordinalValue;

      public override string ToString() => this.nameValue;

      public static World.WorldType valueOf(string name)
      {
        foreach (World.WorldType worldType in (IEnumerable<World.WorldType>) World.WorldType.values())
        {
          if (worldType.nameValue == name)
            return worldType;
        }
        throw new ArgumentException(name);
      }

      public enum InnerEnum
      {
        All,
        Agricultural,
        Asteroid,
        Barren,
        ColdWorld,
        Colony,
        Dangerous,
        DataRepository,
        Desert,
        DieBack,
        Farming,
        Fluid_Oceans,
        Forbidden,
        Frozen,
        Garden,
        HellWorld,
        High_Population,
        High_Technology,
        HotWorld,
        Ice_Capped,
        ImperialCapital,
        Industrial,
        Locked,
        Low_Population,
        Low_Technology,
        Mining,
        Non_Agricultural,
        Non_Industrial,
        OceanWorld,
        PenalColony,
        Poor,
        PreAgriculture,
        PreHigh,
        PreIndustrial,
        PreRich,
        Puzzle,
        Reserve,
        Rich,
        Satellite,
        Tropic,
        Tundra,
        Vacuum,
        Water_World,
        Amber_zone,
        Red_Zone,
        RestrictedAccess,
        No_Classification,
        Ancient_Site,
        Subsector_Capital,
        Sector_Capital,
        Exile_Camp,
        Millitary_Rule,
        Prison_Camp,
        Research_Station,
        XBoat_Station,
        Chirper_World,
        Chirper_Population_Small,
        Chirper_Population_10_Percent,
        Chirper_Population_20_Percent,
        Chirper_Population_30_Percent,
        Chirper_Population_40_Percent,
        Chirper_Population_50_Percent,
        Chirper_Population_60_Percent,
        Chirper_Population_70_Percent,
        Chirper_Population_80_Percent,
        Chirper_Population_90_Percent,
        Droyne_Population_Small,
        Droyne_Population_10_Percent,
        Droyne_Population_20_Percent,
        Droyne_Population_30_Percent,
        Droyne_Population_40_Percent,
        Droyne_Population_50_Percent,
        Droyne_Population_60_Percent,
        Droyne_Population_70_Percent,
        Droyne_Population_80_Percent,
        Droyne_Population_90_Percent,
        Droyne_World,
        Non_Hiver_Population,
        Non_KKree_Population,
      }
    }
  }
}
