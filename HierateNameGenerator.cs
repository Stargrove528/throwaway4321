
// Type: com.digitalarcsystems.Traveller.utility.HierateNameGenerator




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility
{
  public class HierateNameGenerator
  {
    private Random random;
    private Dictionary<string, HierateNameGenerator.SoundStruct> vowelExplanations;
    private List<string> vowels;
    private Dictionary<string, HierateNameGenerator.SoundStruct> icExplanations;
    private List<string> initialConsonants;
    private Dictionary<string, HierateNameGenerator.SoundStruct> fcExplanations;
    private List<string> finalConstants;

    public string NewName(object p) => throw new NotImplementedException();

    public HierateNameGenerator()
    {
      DateTime dateTime = DateTime.Now;
      long ticks1 = dateTime.Ticks;
      dateTime = new DateTime(2010, 5, 27);
      long ticks2 = dateTime.Ticks;
      this.random = new Random((int) (ticks1 - ticks2));
      this.vowelExplanations = new Dictionary<string, HierateNameGenerator.SoundStruct>();
      this.vowels = new List<string>();
      this.icExplanations = new Dictionary<string, HierateNameGenerator.SoundStruct>();
      this.initialConsonants = new List<string>();
      this.fcExplanations = new Dictionary<string, HierateNameGenerator.SoundStruct>();
      this.finalConstants = new List<string>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.vowelExplanations.Add("a", new HierateNameGenerator.SoundStruct("a", "as in l[o]ck", 10));
      this.vowelExplanations.Add("ai", new HierateNameGenerator.SoundStruct("ai", "as in k[i]te", 3));
      this.vowelExplanations.Add("ao", new HierateNameGenerator.SoundStruct("ao", "as in M[ao] Chinese", 2));
      this.vowelExplanations.Add("au", new HierateNameGenerator.SoundStruct("au", "as in h[ou]se", 1));
      this.vowelExplanations.Add("e", new HierateNameGenerator.SoundStruct("e", "as in g[e]t", 6));
      this.vowelExplanations.Add("ea", new HierateNameGenerator.SoundStruct("ea", "as in E A", 6));
      this.vowelExplanations.Add("ei", new HierateNameGenerator.SoundStruct("ei", "as in b[a]y", 2));
      this.vowelExplanations.Add("i", new HierateNameGenerator.SoundStruct("i", "as in k[i]t", 4));
      this.vowelExplanations.Add("iy", new HierateNameGenerator.SoundStruct("iy", "as in f[ee]t", 3));
      this.vowelExplanations.Add("o", new HierateNameGenerator.SoundStruct("o", "as in g[o]ne", 2));
      this.vowelExplanations.Add("oa", new HierateNameGenerator.SoundStruct("oa", "as in O A", 1));
      this.vowelExplanations.Add("oi", new HierateNameGenerator.SoundStruct("oi", "as in n[oi]se", 2));
      this.vowelExplanations.Add("ou", new HierateNameGenerator.SoundStruct("ou", "as in O U", 1));
      this.vowelExplanations.Add("u", new HierateNameGenerator.SoundStruct("u", "as in l[u]te", 1));
      this.vowelExplanations.Add("ua", new HierateNameGenerator.SoundStruct("ua", "as in U A", 1));
      this.vowelExplanations.Add("ui", new HierateNameGenerator.SoundStruct("ui", "as in U I", 1));
      this.vowelExplanations.Add("ya", new HierateNameGenerator.SoundStruct("ya", "as in [ya]rd", 2));
      this.vowelExplanations.Add("yu", new HierateNameGenerator.SoundStruct("yu", "as in f[eu]d", 1));
      foreach (HierateNameGenerator.SoundStruct soundStruct in this.vowelExplanations.Values)
      {
        for (int index = 0; index < soundStruct.frequency; ++index)
          this.vowels.Add(soundStruct.sound);
      }
      this.icExplanations.Add("f", new HierateNameGenerator.SoundStruct("f", "as in [whew]", 5));
      this.icExplanations.Add("ft", new HierateNameGenerator.SoundStruct("ft", "as in ri[ft]", 4));
      this.icExplanations.Add("h", new HierateNameGenerator.SoundStruct("h", "as in [h]it", 7));
      this.icExplanations.Add("hf", new HierateNameGenerator.SoundStruct("hf", "as in [hf]ang", 2));
      this.icExplanations.Add("hk", new HierateNameGenerator.SoundStruct("hk", "as in [hk]ang", 5));
      this.icExplanations.Add("hl", new HierateNameGenerator.SoundStruct("hl", "as in [hl]ang", 3));
      this.icExplanations.Add("hr", new HierateNameGenerator.SoundStruct("hr", "as in [hr]ang", 3));
      this.icExplanations.Add("ht", new HierateNameGenerator.SoundStruct("ht", "as in heig[ht]", 5));
      this.icExplanations.Add("hw", new HierateNameGenerator.SoundStruct("hw", "as in [wh]at", 2));
      this.icExplanations.Add("k", new HierateNameGenerator.SoundStruct("k", "as in [k]ite", 7));
      this.icExplanations.Add("kh", new HierateNameGenerator.SoundStruct("kh", "as in lo[ch] Scottish", 6));
      this.icExplanations.Add("kht", new HierateNameGenerator.SoundStruct("kht", "as in Na[kht] German", 4));
      this.icExplanations.Add("kt", new HierateNameGenerator.SoundStruct("kt", "as in ba[cked]", 4));
      this.icExplanations.Add("l", new HierateNameGenerator.SoundStruct("l", "as in [l]ike", 2));
      this.icExplanations.Add("r", new HierateNameGenerator.SoundStruct("r", "as in [r]un", 3));
      this.icExplanations.Add("s", new HierateNameGenerator.SoundStruct("s", "as in [s]un", 4));
      this.icExplanations.Add("st", new HierateNameGenerator.SoundStruct("st", "as in [st]op", 3));
      this.icExplanations.Add("t", new HierateNameGenerator.SoundStruct("t", "as in [t]on", 8));
      this.icExplanations.Add("tl", new HierateNameGenerator.SoundStruct("tl", "as in [tl]aloc Aztech", 2));
      this.icExplanations.Add("tr", new HierateNameGenerator.SoundStruct("tr", "as in [tr]ip", 2));
      this.icExplanations.Add("w", new HierateNameGenerator.SoundStruct("w", "as in [w]in", 6));
      foreach (HierateNameGenerator.SoundStruct soundStruct in this.icExplanations.Values)
      {
        for (int index = 0; index < soundStruct.frequency; ++index)
          this.initialConsonants.Add(soundStruct.sound);
      }
      this.fcExplanations.Add("h", new HierateNameGenerator.SoundStruct("h", "as in [h]ow", 10));
      this.fcExplanations.Add("kh", new HierateNameGenerator.SoundStruct("kh", "as in lo[ch] Scottish", 4));
      this.fcExplanations.Add("l", new HierateNameGenerator.SoundStruct("l", "as in a[ll]", 7));
      this.fcExplanations.Add("lr", new HierateNameGenerator.SoundStruct("lr", "as in a[ll r]ight", 3));
      this.fcExplanations.Add("r", new HierateNameGenerator.SoundStruct("r", "as in fa[r]", 5));
      this.fcExplanations.Add("rl", new HierateNameGenerator.SoundStruct("rl", "as in ea[rl]", 4));
      this.fcExplanations.Add("ss", new HierateNameGenerator.SoundStruct("ss", "as in hi[ss]", 5));
      this.fcExplanations.Add("w", new HierateNameGenerator.SoundStruct("w", "as in wo[w]", 6));
      this.fcExplanations.Add("`", new HierateNameGenerator.SoundStruct("`", "glottal stop", 3));
      foreach (HierateNameGenerator.SoundStruct soundStruct in this.fcExplanations.Values)
      {
        for (int index = 0; index < soundStruct.frequency; ++index)
          this.finalConstants.Add(soundStruct.sound);
      }
    }

    public string getVowel() => this.vowels[this.random.Next(this.vowels.Count)];

    public string getInitialConsonant()
    {
      return this.initialConsonants[this.random.Next(this.initialConsonants.Count)];
    }

    public string getFinalConsonant()
    {
      return this.finalConstants[this.random.Next(this.finalConstants.Count)];
    }

    private string GetPronounciationGuid(string name) => "";

    public string NewName(Character aslanToName)
    {
      string str1 = "";
      for (int index = 0; index < 2; ++index)
      {
        int num = this.random.Next(10);
        string str2;
        if (num < 3)
        {
          str2 = this.getVowel();
        }
        else
        {
          switch (num)
          {
            case 3:
            case 4:
            case 5:
              str2 = this.getInitialConsonant() + this.getFinalConsonant();
              break;
            default:
              switch (num)
              {
                case 6:
                case 7:
                  str2 = this.getVowel() + this.getFinalConsonant();
                  break;
                default:
                  switch (num)
                  {
                    case 8:
                    case 9:
                      str2 = this.getInitialConsonant() + this.getVowel() + this.getFinalConsonant();
                      break;
                    default:
                      str2 = "lion-owe";
                      break;
                  }
                  break;
              }
              break;
          }
        }
        str1 += str2;
      }
      return char.ToUpper(str1[0]).ToString() + str1.Substring(1);
    }

    public static void Main(string[] args)
    {
      HierateNameGenerator hierateNameGenerator = new HierateNameGenerator();
      int num = 1;
      if (args.Length != 0)
        num = int.Parse(args[0]);
      for (int index = 0; index < num; ++index)
        Console.WriteLine(hierateNameGenerator.NewName((Character) null));
    }

    private class SoundStruct
    {
      public string sound;
      public int frequency;
      public string explanation;

      public SoundStruct(string sound, string explanation, int frequency)
      {
        this.sound = sound;
        this.explanation = explanation;
        this.frequency = frequency;
      }
    }
  }
}
