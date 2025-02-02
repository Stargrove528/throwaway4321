
// Type: com.digitalarcsystems.Traveller.Dice




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class Dice
  {
    public const int NO_RAW_VALUE = 0;
    private static int seed = Dice.initializeSeed();
    private static Random randomGenerator = new Random(Dice.seed);

    public static IRollDelegate rollDelegate { get; set; }

    public static int initializeSeed()
    {
      RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider();
      byte[] data = new byte[4];
      cryptoServiceProvider.GetBytes(data);
      return BitConverter.ToInt32(data, 0);
    }

    public static int getSeed() => Dice.seed;

    public static int random(int min, int max) => Dice.randomGenerator.Next(1, max + 1);

    public static void setSeed(int newSeed)
    {
      Dice.seed = newSeed;
      Dice.randomGenerator = new Random(Dice.seed);
    }

    public static RollParam StatRoll(com.digitalarcsystems.Traveller.DataModel.Attribute stat, int targetNumber, string description)
    {
      return new RollParam()
      {
        attribute = stat,
        rawMinSuccessValue = targetNumber,
        description = description,
        totalModifier = stat.Modifier
      };
    }

    public static RollParam SkillRoll(
      com.digitalarcsystems.Traveller.DataModel.Attribute stat,
      ISkill skill,
      int targetNumber,
      string description)
    {
      RollParam rollParam = new RollParam()
      {
        attribute = stat,
        skill = skill,
        rawMinSuccessValue = targetNumber,
        description = description
      };
      rollParam.totalModifier = (stat != null ? stat.Modifier : 0) + (skill != null ? skill.Level : 0);
      return rollParam;
    }

    public static int TwoDiceRollEffect(
      int target,
      string description,
      string successMsg,
      string failureMsg)
    {
      RollParam settings = new RollParam();
      settings.description = description;
      settings.rawMinSuccessValue = target;
      settings.AddResultDescriptions(successMsg, failureMsg);
      return Dice.Roll(settings).effect;
    }

    public static RollEffect D6Roll(
      int num_of_dice,
      int modifier,
      string description,
      bool isToBeAnimated = true)
    {
      return Dice.Roll(new RollParam()
      {
        numOfDices = num_of_dice,
        totalModifier = modifier,
        description = description,
        isToBeAnimated = isToBeAnimated
      });
    }

    public static int Roll1D6() => Dice.randomGenerator.Next(6) + 1;

    public static int RawRollResult(int num_of_dice) => Dice.GenerateRandomDiceResult(num_of_dice);

    public static T RollRandomResult<T>(
      string description,
      IList<T> choices,
      ContextKeys choicesContextKey)
    {
      Context context = new Context();
      context.AddWithKey(choicesContextKey, (object) choices);
      RollParam settings = new RollParam();
      settings.possibleResultsAsContext = context;
      settings.numOfDices = choices.Count;
      settings.description = description;
      settings.rollType = RollType.CUSTOM;
      RollEffect rollEffect = Dice.Roll(settings);
      T obj = default (T);
      try
      {
        obj = choices[rollEffect.rawResult - 1];
      }
      catch (Exception ex)
      {
        EngineLog.Error("Dice (" + rollEffect.rawResult.ToString() + ") could not parse returned thing from rollRandomChoice: " + ex.Message);
      }
      if ((object) obj == null)
        EngineLog.Error("Dice can't... for zeus sake, don't roll for nulls. Roll for (" + settings.description + ")");
      return obj;
    }

    private static int GenerateRandomDiceResult(int nrOfDices, RollType interpretation = RollType.NORMAL)
    {
      int randomDiceResult = 0;
      int[] array = new int[nrOfDices];
      for (int index = 0; index < nrOfDices; ++index)
        array[index] = Dice.randomGenerator.Next(1, 7);
      switch (interpretation)
      {
        case RollType.NORMAL:
          for (int index = 0; index < nrOfDices; ++index)
            randomDiceResult += array[index];
          break;
        case RollType.CUSTOM:
          if (nrOfDices < 1)
          {
            EngineLog.Error("DICE:  Custom roll type.  Number of Dice Set to " + nrOfDices.ToString() + "SEED = " + Dice.seed.ToString());
            throw new Exception("DICE:  Custom roll type.  Number of Dice Set to " + nrOfDices.ToString() + " SEED = " + Dice.seed.ToString());
          }
          randomDiceResult = nrOfDices != 11 ? Dice.randomGenerator.Next(1, nrOfDices + 1) : array[0] + array[1] - 1;
          break;
        case RollType.BOON:
          Array.Sort<int>(array);
          randomDiceResult = array[0] + array[1];
          break;
        case RollType.BANE:
          Array.Sort<int>(array);
          randomDiceResult = array[nrOfDices - 2] + array[nrOfDices - 1];
          break;
        case RollType.D66:
          for (int y = 0; y < nrOfDices; ++y)
            randomDiceResult += array[y] * (int) Math.Pow(10.0, (double) y);
          break;
        default:
          EngineLog.Error("Dice RollType (" + interpretation.ToString() + ") NOT IMPLEMENTED");
          break;
      }
      return randomDiceResult;
    }

    public static RollEffect Roll(RollParam settings, int rawDiceValue = 0)
    {
      if (rawDiceValue == 0)
        rawDiceValue = Dice.GenerateRandomDiceResult(settings.numOfDices, settings.rollType);
      RollEffect setting = new RollEffect(settings, rawDiceValue);
      if (Dice.rollDelegate != null && settings.isToBeAnimated)
      {
        Dice.Log(string.Format("calling roll delegate to roll for : dices {0}, rawResult {1} and rollType {2} to: {3}", (object) settings.numOfDices, (object) rawDiceValue, (object) settings.rollType, (object) settings.description));
        setting = Dice.rollDelegate.AnimateRoll(setting);
      }
      return setting;
    }

    public static RollEffect Roll(RollParam setup, string successMessage, string failureMessage)
    {
      Context context = new Context();
      context.AddWithKey(ContextKeys.SUCCESS, (object) successMessage);
      context.AddWithKey(ContextKeys.FAILURE, (object) failureMessage);
      setup.possibleResultsAsContext = context;
      return Dice.Roll(setup);
    }

    public static int ProbabilityPercent(RollParam forRoll)
    {
      int minValueForSuccess = forRoll.rawMinSuccessValue - forRoll.totalModifier;
      return Dice.ProbabilityPercent(forRoll.numOfDices, minValueForSuccess);
    }

    public static int ProbabilityPercent(int nrOfDices, int minValueForSuccess)
    {
      List<int> intList = new List<int>();
      intList.Add(100);
      intList.Add(100);
      intList.Add(100);
      intList.Add(97);
      intList.Add(91);
      intList.Add(83);
      intList.Add(72);
      intList.Add(58);
      intList.Add(42);
      intList.Add(28);
      intList.Add(17);
      intList.Add(8);
      intList.Add(3);
      float a;
      if (nrOfDices > 2)
      {
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        int num1 = nrOfDices;
        int num2 = nrOfDices * 6;
        int length = num2 - num1 + 1;
        float[] numArray = new float[length];
        for (int key = 0; key < length; ++key)
          dictionary[key] = 0;
        for (int index = 0; index < 10000; ++index)
          ++dictionary[Dice.RawRollResult(nrOfDices) - num1];
        float num3 = 0.0f;
        for (int key = length - 1; key >= 0; --key)
        {
          numArray[key] = (float) dictionary[key] / 10f + num3;
          EngineLog.Print("Histogram[" + key.ToString() + "] == " + dictionary[key].ToString() + " (" + ((double) dictionary[key] * 1.0 / 100.0).ToString() + ")");
          num3 = numArray[key];
        }
        a = numArray[minValueForSuccess];
        int num4 = num1;
        foreach (float num5 in numArray)
          EngineLog.Print("Prob [" + num4++.ToString() + "/" + num2.ToString() + "]: " + num5.ToString());
      }
      else
        a = minValueForSuccess <= 12 ? (minValueForSuccess >= 2 ? (float) intList[minValueForSuccess] : 100f) : 0.0f;
      return (int) Math.Round((double) a);
    }

    public static void Main(string[] args)
    {
      Dice.Log("Testing d6 randomness...");
      int[] numArray = new int[8];
      for (int index = 0; index < 60000; ++index)
        ++numArray[Dice.D6Roll(1, 0, "").effect];
      Dice.Log("Distribution:");
      for (int index = 0; index < numArray.Length; ++index)
        Console.WriteLine(index.ToString() + ": " + numArray[index].ToString());
    }

    private static void Log(string msg, bool isSerious = false)
    {
      string message = "Dice: " + msg;
      if (isSerious)
        EngineLog.Warning(message);
      else
        EngineLog.Print(message);
    }
  }
}
