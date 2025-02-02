
// Type: com.digitalarcsystems.Traveller.TradeCalculator




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class TradeCalculator
  {
    private static float[] modifiedPurchasePriceTable = new float[23]
    {
      400f,
      300f,
      200f,
      175f,
      150f,
      135f,
      125f,
      120f,
      115f,
      110f,
      105f,
      100f,
      95f,
      90f,
      85f,
      80f,
      75f,
      70f,
      65f,
      55f,
      50f,
      40f,
      25f
    };
    private static float[] modifiedSalePriceTable = new float[23]
    {
      25f,
      45f,
      50f,
      55f,
      60f,
      65f,
      75f,
      80f,
      85f,
      90f,
      95f,
      100f,
      105f,
      110f,
      115f,
      120f,
      125f,
      135f,
      150f,
      175f,
      200f,
      300f,
      400f
    };

    internal static int getPassangerTrafficModifier(World current, World destination)
    {
      int passangerTrafficModifier = current.Population;
      if (current.containsClassification(World.WorldType.Agricultural))
        passangerTrafficModifier = passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Asteroid))
        ++passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Barren))
        passangerTrafficModifier += -5;
      if (current.containsClassification(World.WorldType.Desert))
        passangerTrafficModifier += -1;
      if (current.containsClassification(World.WorldType.Fluid_Oceans))
        passangerTrafficModifier = passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Garden))
        passangerTrafficModifier += 2;
      if (current.containsClassification(World.WorldType.High_Population))
        passangerTrafficModifier = passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Ice_Capped))
        ++passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Industrial))
        passangerTrafficModifier += 2;
      if (current.containsClassification(World.WorldType.Low_Population))
        passangerTrafficModifier = passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Non_Agricultural))
        passangerTrafficModifier = passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Non_Industrial))
        passangerTrafficModifier = passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Poor))
        passangerTrafficModifier += -2;
      if (current.containsClassification(World.WorldType.Rich))
        passangerTrafficModifier += -1;
      if (current.containsClassification(World.WorldType.Water_World))
        passangerTrafficModifier = passangerTrafficModifier;
      if (current.containsClassification(World.WorldType.Amber_zone))
        passangerTrafficModifier += 2;
      if (current.containsClassification(World.WorldType.Red_Zone))
        passangerTrafficModifier += 4;
      if (destination.containsClassification(World.WorldType.Agricultural))
        passangerTrafficModifier = passangerTrafficModifier;
      if (destination.containsClassification(World.WorldType.Asteroid))
        passangerTrafficModifier += -1;
      if (destination.containsClassification(World.WorldType.Barren))
        passangerTrafficModifier += -5;
      if (destination.containsClassification(World.WorldType.Desert))
        passangerTrafficModifier += -1;
      if (destination.containsClassification(World.WorldType.Fluid_Oceans))
        passangerTrafficModifier = passangerTrafficModifier;
      if (destination.containsClassification(World.WorldType.Garden))
        passangerTrafficModifier += 2;
      if (destination.containsClassification(World.WorldType.High_Population))
        passangerTrafficModifier += 4;
      if (destination.containsClassification(World.WorldType.Ice_Capped))
        passangerTrafficModifier += -1;
      if (destination.containsClassification(World.WorldType.Industrial))
        ++passangerTrafficModifier;
      if (destination.containsClassification(World.WorldType.Low_Population))
        passangerTrafficModifier += -4;
      if (destination.containsClassification(World.WorldType.Non_Agricultural))
        passangerTrafficModifier = passangerTrafficModifier;
      if (destination.containsClassification(World.WorldType.Non_Industrial))
        passangerTrafficModifier += -1;
      if (destination.containsClassification(World.WorldType.Poor))
        passangerTrafficModifier += -1;
      if (destination.containsClassification(World.WorldType.Rich))
        passangerTrafficModifier += 2;
      if (destination.containsClassification(World.WorldType.Water_World))
        passangerTrafficModifier = passangerTrafficModifier;
      if (destination.containsClassification(World.WorldType.Amber_zone))
        passangerTrafficModifier += -2;
      if (destination.containsClassification(World.WorldType.Red_Zone))
        passangerTrafficModifier += -4;
      return passangerTrafficModifier;
    }

    public static int getFreightModifier(World current, World destination)
    {
      int num1 = destination.Population;
      if (current.containsClassification(World.WorldType.Agricultural))
        num1 += 2;
      if (current.containsClassification(World.WorldType.Asteroid))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Barren))
        num1 = num1;
      if (current.containsClassification(World.WorldType.Desert))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Fluid_Oceans))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Garden))
        num1 += 2;
      if (current.containsClassification(World.WorldType.High_Population))
        num1 += 2;
      if (current.containsClassification(World.WorldType.Ice_Capped))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Industrial))
        num1 += 3;
      if (current.containsClassification(World.WorldType.Low_Population))
        num1 += -5;
      if (current.containsClassification(World.WorldType.Non_Agricultural))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Non_Industrial))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Poor))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Rich))
        num1 += 2;
      if (current.containsClassification(World.WorldType.Water_World))
        num1 += -3;
      if (current.containsClassification(World.WorldType.Amber_zone))
        num1 += 5;
      if (current.containsClassification(World.WorldType.Red_Zone))
        num1 += -5;
      if (destination.containsClassification(World.WorldType.Agricultural))
        ++num1;
      if (destination.containsClassification(World.WorldType.Asteroid))
        ++num1;
      if (destination.containsClassification(World.WorldType.Barren))
        num1 += -5;
      if (destination.containsClassification(World.WorldType.Desert))
        num1 = num1;
      if (destination.containsClassification(World.WorldType.Fluid_Oceans))
        num1 = num1;
      if (destination.containsClassification(World.WorldType.Garden))
        ++num1;
      if (destination.containsClassification(World.WorldType.High_Population))
        num1 += 2;
      if (destination.containsClassification(World.WorldType.Ice_Capped))
        num1 = num1;
      if (destination.containsClassification(World.WorldType.Industrial))
        num1 += 2;
      if (destination.containsClassification(World.WorldType.Low_Population))
        num1 = num1;
      if (destination.containsClassification(World.WorldType.Non_Agricultural))
        ++num1;
      if (destination.containsClassification(World.WorldType.Non_Industrial))
        ++num1;
      if (destination.containsClassification(World.WorldType.Poor))
        num1 += -3;
      if (destination.containsClassification(World.WorldType.Rich))
        num1 += 2;
      if (destination.containsClassification(World.WorldType.Water_World))
        num1 = num1;
      if (destination.containsClassification(World.WorldType.Amber_zone))
        num1 += -5;
      if (destination.containsClassification(World.WorldType.Red_Zone))
        num1 += -20;
      int num2 = -1 * Math.Abs(current.TechLevel - destination.TechLevel);
      if (num2 < -5)
        num2 = -5;
      return num1 + num2;
    }

    public static int rollD6(int num_of_dice)
    {
      Random random = new Random(DateTime.Now.GetHashCode());
      int num = 0;
      for (int index = 0; index < num_of_dice; ++index)
        num += random.Next(1, 6);
      return num;
    }

    public static int rollD6() => TradeCalculator.rollD6(1);

    public static int rollD66() => TradeCalculator.rollD6() * 10 + TradeCalculator.rollD6();

    public static int calculateLowPassages(int passanger_modifier)
    {
      int lowPassages = 0;
      if (passanger_modifier > 0)
      {
        switch (passanger_modifier)
        {
          case 1:
            lowPassages = TradeCalculator.rollD6(2) - 6;
            break;
          case 2:
          case 3:
            lowPassages = TradeCalculator.rollD6(2);
            break;
          case 4:
          case 5:
            lowPassages = TradeCalculator.rollD6(3) - TradeCalculator.rollD6();
            break;
          case 6:
          case 7:
            lowPassages = TradeCalculator.rollD6(3);
            break;
          case 8:
          case 9:
            lowPassages = TradeCalculator.rollD6(4);
            break;
          case 10:
          case 11:
            lowPassages = TradeCalculator.rollD6(5);
            break;
          case 12:
          case 13:
            lowPassages = TradeCalculator.rollD6(6);
            break;
          case 14:
            lowPassages = TradeCalculator.rollD6(7);
            break;
          case 15:
            lowPassages = TradeCalculator.rollD6(8);
            break;
          default:
            lowPassages = TradeCalculator.rollD6(9);
            break;
        }
      }
      if (lowPassages < 0)
        lowPassages = 0;
      return lowPassages;
    }

    public static int calculateNumMiddlePassages(int passanger_modifier)
    {
      int numMiddlePassages = 0;
      if (passanger_modifier > 0)
      {
        switch (passanger_modifier)
        {
          case 1:
            numMiddlePassages = TradeCalculator.rollD6(1) - 2;
            break;
          case 2:
            numMiddlePassages = TradeCalculator.rollD6(1);
            break;
          case 3:
          case 4:
            numMiddlePassages = TradeCalculator.rollD6(2) - TradeCalculator.rollD6();
            break;
          case 5:
          case 6:
            numMiddlePassages = TradeCalculator.rollD6(3) - TradeCalculator.rollD6(2);
            break;
          case 7:
          case 8:
            numMiddlePassages = TradeCalculator.rollD6(3) - TradeCalculator.rollD6();
            break;
          case 9:
          case 10:
            numMiddlePassages = TradeCalculator.rollD6(3);
            break;
          case 11:
          case 12:
          case 13:
            numMiddlePassages = TradeCalculator.rollD6(4);
            break;
          case 14:
          case 15:
            numMiddlePassages = TradeCalculator.rollD6(5);
            break;
          default:
            numMiddlePassages = TradeCalculator.rollD6(6);
            break;
        }
      }
      if (numMiddlePassages < 0)
        numMiddlePassages = 0;
      return numMiddlePassages;
    }

    public static int calculateHighPassages(int passanger_modifier)
    {
      int highPassages = 0;
      if (passanger_modifier > 0)
      {
        switch (passanger_modifier)
        {
          case 1:
            highPassages = 0;
            break;
          case 2:
            highPassages = TradeCalculator.rollD6() - TradeCalculator.rollD6();
            break;
          case 3:
            highPassages = TradeCalculator.rollD6(2) - TradeCalculator.rollD6(2);
            break;
          case 4:
          case 5:
            highPassages = TradeCalculator.rollD6(2) - TradeCalculator.rollD6();
            break;
          case 6:
          case 7:
            highPassages = TradeCalculator.rollD6(3) - TradeCalculator.rollD6(2);
            break;
          case 8:
          case 9:
          case 10:
            highPassages = TradeCalculator.rollD6(3) - TradeCalculator.rollD6();
            break;
          case 11:
          case 12:
            highPassages = TradeCalculator.rollD6(3);
            break;
          case 13:
          case 14:
          case 15:
            highPassages = TradeCalculator.rollD6(4);
            break;
          default:
            highPassages = TradeCalculator.rollD6(5);
            break;
        }
      }
      if (highPassages < 0)
        highPassages = 0;
      return highPassages;
    }

    public static int calculateIncidentalFreightLots(int freight_modifier, World destination)
    {
      int incidentalFreightLots = 0;
      int num = freight_modifier + destination.Population;
      if (num > 8)
      {
        if (num > 16)
          freight_modifier = 16;
        switch (num)
        {
          case 9:
            incidentalFreightLots = TradeCalculator.rollD6() - 2;
            break;
          default:
            incidentalFreightLots = TradeCalculator.rollD6() + freight_modifier - 10;
            break;
        }
      }
      if (incidentalFreightLots < 0)
        incidentalFreightLots = 0;
      return incidentalFreightLots;
    }

    public static int calculateMinorFreightLots(int freight_modifier, World destination)
    {
      int minorFreightLots = 0;
      int num1 = freight_modifier + destination.Population;
      if (num1 > 0)
      {
        if (num1 > 16)
          freight_modifier = 16;
        int num2;
        switch (num1)
        {
          case 1:
            minorFreightLots = TradeCalculator.rollD6() - 4;
            break;
          case 14:
            minorFreightLots = TradeCalculator.rollD6() + 12;
            break;
          case 15:
            num2 = TradeCalculator.rollD6() + 14;
            goto case 16;
          case 16:
            num2 = TradeCalculator.rollD6() + 16;
            goto default;
          default:
            minorFreightLots = TradeCalculator.rollD6() + (num1 - 3);
            break;
        }
      }
      if (minorFreightLots < 0)
        minorFreightLots = 0;
      return minorFreightLots;
    }

    public static int calculateMajorFreightLots(int freight_modifier, World destination)
    {
      int majorFreightLots = 0;
      int num = freight_modifier + destination.Population;
      if (num > 0)
      {
        if (num > 16)
          freight_modifier = 16;
        majorFreightLots = num == 1 ? TradeCalculator.rollD6() - 4 : TradeCalculator.rollD6() + (freight_modifier - 4);
      }
      if (majorFreightLots < 0)
        majorFreightLots = 0;
      return majorFreightLots;
    }

    public static List<int> calculateMajorFreightTons(int numOfLots)
    {
      List<int> majorFreightTons = new List<int>();
      for (int index = 0; index < numOfLots; ++index)
        majorFreightTons.Add(TradeCalculator.rollD6() * 10);
      return majorFreightTons;
    }

    public static List<int> calculateMinorFreightTons(int numOfLots)
    {
      List<int> minorFreightTons = new List<int>();
      for (int index = 0; index < numOfLots; ++index)
        minorFreightTons.Add(TradeCalculator.rollD6() * 5);
      return minorFreightTons;
    }

    public static List<int> calculateIncidentalFreightTons(int numOfLots)
    {
      List<int> incidentalFreightTons = new List<int>();
      for (int index = 0; index < numOfLots; ++index)
        incidentalFreightTons.Add(TradeCalculator.rollD6());
      return incidentalFreightTons;
    }

    public static int calculateFreightFee(int tonsOfFreight, int distanceInParsecs)
    {
      return tonsOfFreight * (1000 + 200 * (distanceInParsecs - 1));
    }

    public static int calculateLateFreightDeliveryPenaltyPercent()
    {
      return (TradeCalculator.rollD6() + 4) * 10;
    }

    public static int calculateMailModifier(
      int freight_modifier,
      int highest_naval_or_scout_rank,
      int social_dm,
      bool ship_is_armed,
      int lowest_tech_level)
    {
      int num = 0;
      if (freight_modifier <= -10)
        num += -2;
      else if (-9 <= freight_modifier && freight_modifier <= -5)
        num += -1;
      else if (-4 <= freight_modifier && freight_modifier <= 4)
        num = num;
      else if (5 <= freight_modifier && freight_modifier <= 9)
        ++num;
      else if (freight_modifier >= 10)
        num += 2;
      if (ship_is_armed)
        num += 2;
      int mailModifier = num + highest_naval_or_scout_rank + social_dm;
      if (lowest_tech_level <= 5)
        mailModifier += -4;
      return mailModifier;
    }

    public static IList<TradeGood> determineTradeGoodsAvailable(
      World source,
      IList<TradeGood> tradeGoods)
    {
      List<TradeGood> tradeGoodsAvailable = new List<TradeGood>();
      Dictionary<int, TradeGood> dictionary = new Dictionary<int, TradeGood>();
      foreach (TradeGood tradeGood in (IEnumerable<TradeGood>) tradeGoods)
      {
        dictionary[tradeGood.D66] = tradeGood;
        if (tradeGood.isAvailable(source))
          tradeGoodsAvailable.Add(tradeGood);
      }
      int num = TradeCalculator.rollD6(1);
      for (int index = 0; index < num; ++index)
      {
        TradeGood tradeGood = dictionary[TradeCalculator.rollD66()];
        while (tradeGoodsAvailable.Contains(tradeGood) && tradeGoodsAvailable.Count < dictionary.Count)
          tradeGood = dictionary[TradeCalculator.rollD66()];
        if (tradeGood != null)
          tradeGoodsAvailable.Add(tradeGood);
      }
      return (IList<TradeGood>) tradeGoodsAvailable;
    }

    public static float determinePurchasePrice(
      World source,
      TradeGood tradeGood,
      int broker_skill,
      int intellegence_or_social,
      int supplier_prominence_modifier,
      int dieRoll)
    {
      int purchaseDm = tradeGood.getPurchaseDM(source);
      int saleDm = tradeGood.getSaleDM(source);
      int priceModifierRoll = dieRoll - saleDm + purchaseDm + broker_skill + intellegence_or_social - supplier_prominence_modifier;
      return (float) ((double) tradeGood.BasePrice * (double) TradeCalculator.lookupPriceModifier(priceModifierRoll, TradeCalculator.modifiedPurchasePriceTable) / 100.0);
    }

    public static float determinePurchasePrice(
      World source,
      TradeGood tradeGood,
      int broker_skill,
      int intellegence_or_social,
      int supplier_prominence_modifier)
    {
      int dieRoll = TradeCalculator.rollD6(3);
      return TradeCalculator.determinePurchasePrice(source, tradeGood, broker_skill, intellegence_or_social, supplier_prominence_modifier, dieRoll);
    }

    public static float lookupPriceModifier(int priceModifierRoll, float[] priceModifierTable)
    {
      if (priceModifierRoll < 0)
        priceModifierRoll = 0;
      else if (priceModifierRoll > priceModifierTable.Length - 1)
        priceModifierRoll = priceModifierTable.Length - 1;
      return priceModifierTable[priceModifierRoll];
    }

    public static float determineSalePrice(
      World destination,
      TradeGood tradeGood,
      int broker_skill,
      int intellegence_or_social,
      int buyer_prominence_modifier)
    {
      int dieRoll = TradeCalculator.rollD6(3);
      return TradeCalculator.determineSalePrice(destination, tradeGood, broker_skill, intellegence_or_social, buyer_prominence_modifier, dieRoll);
    }

    public static float determineSalePrice(
      World destination,
      TradeGood tradeGood,
      int broker_skill,
      int intellegence_or_social,
      int buyer_prominence_modifier,
      int dieRoll)
    {
      int purchaseDm = tradeGood.getPurchaseDM(destination);
      int saleDm = tradeGood.getSaleDM(destination);
      int priceModifierRoll = dieRoll + saleDm - purchaseDm + broker_skill + intellegence_or_social - buyer_prominence_modifier;
      return (float) ((double) tradeGood.BasePrice * (double) TradeCalculator.lookupPriceModifier(priceModifierRoll, TradeCalculator.modifiedSalePriceTable) / 100.0);
    }

    public static void Main(string[] args)
    {
    }

    internal static void printLots(List<int> printMe, int distance)
    {
      Console.Write("          ");
      int num = 0;
      foreach (int tonsOfFreight in printMe)
      {
        ++num;
        string[] strArray = new string[6]
        {
          tonsOfFreight.ToString(),
          " [",
          null,
          null,
          null,
          null
        };
        int freightFee = TradeCalculator.calculateFreightFee(tonsOfFreight, distance);
        strArray[2] = freightFee.ToString("N");
        strArray[3] = "-";
        freightFee = TradeCalculator.calculateFreightFee(tonsOfFreight, distance + 1);
        strArray[4] = freightFee.ToString("0,0 Cr");
        strArray[5] = "] , ";
        Console.Write(string.Concat(strArray));
        if (num >= 5)
        {
          Console.WriteLine();
          Console.Write("          ");
          num = 0;
        }
      }
    }

    private static World determineWorldByName(List<World> worlds, string worldName)
    {
      World worldByName = (World) null;
      foreach (World world in worlds)
      {
        if (world.Name.Equals(worldName, StringComparison.CurrentCultureIgnoreCase))
        {
          worldByName = world;
          break;
        }
      }
      return worldByName;
    }

    private static World determineWorldByHex(List<World> worlds, string hexNumber)
    {
      World worldByHex = (World) null;
      foreach (World world in worlds)
      {
        if (world.hexNumber == int.Parse(hexNumber))
        {
          worldByHex = world;
          break;
        }
      }
      return worldByHex;
    }

    private static void printUsage()
    {
      Console.WriteLine("usage: java com.digitalarcsystems.Traveller.Tradegood <WorldFile> <TradeGoodFile> <inputs> <operations>");
      Console.WriteLine("       INPUTS:");
      Console.WriteLine("              -sw <name of sourceworld>       The world goods are coming from.");
      Console.WriteLine("              -dw <name of destination world> The world goods are going to.");
      Console.WriteLine("              -swh <hex of sourceworld>       The hex number of the source world system.");
      Console.WriteLine("              -dwh <hex of destination world> The hex number of the destination world system.");
      Console.WriteLine("              -b <broker skill>               The rating of the broker skill being used. Default is 0 if unset.");
      Console.WriteLine("              -s <stat modifier>              The modifier being provided by the pertinent stat. Default is 0 if unset.");
      Console.WriteLine("              -r <number on die>              The roll to use if applicable.  Otherwise auto generated.");
      Console.WriteLine("              -armed <true or false>          True if the ship is armed.  Useful for mail calculations. Default is false if unset.");
      Console.WriteLine("              -highest_rank <higest mil rank> The highest millitary rank achieved by crew.  Useful for mail. Default is 0 if unset.");
      Console.WriteLine("              -trade_good <name of good>      The text name of the tade good. use \\<sp> for spaces.");
      Console.WriteLine("              -tgD66 <D66 id of trade good>   The D66 id of the trade good from mainbook.");
      Console.WriteLine();
      Console.WriteLine("              -buyer_prominence_modifier <int>");
      Console.WriteLine("              -bpm <int>                      The buyer prominence modifier to use when selling goods to an empowered buyer. Default is 0 if unset.");
      Console.WriteLine("                                              The higher the value, the more in favor the transaction is for the buyer.");
      Console.WriteLine("                                              Used in determineSalePrice operations.");
      Console.WriteLine();
      Console.WriteLine("              -seller_prominence_modifier <int>");
      Console.WriteLine("              -spm <int>                      The seller prominence modifier to use when buying goods from an empowered seller.  Default is 0 if unset.");
      Console.WriteLine("                                              The higher the value, the more in favor the transaction is for the seller.");
      Console.WriteLine("                                              Used in determinePurchasePrice operations.");
      Console.WriteLine("       OPERATIONS:");
      Console.WriteLine("              -dpp:                           ");
      Console.WriteLine("              -determine_purchase_price:      Determines the purchase price for a given TradeGood");
      Console.WriteLine("                                              Requires: -trade_good or -tgD66, -sw or -swh");
      Console.WriteLine("                                              Uses:     -b, -s (soc or int), -r (3d6)");
      Console.WriteLine("");
      Console.WriteLine("              -dsp:                           ");
      Console.WriteLine("              -determine_sale_price:");
      Console.WriteLine("                                              Requires: -trade_good or -tgD66, -dw or -dwh");
      Console.WriteLine("                                              Uses:     -b, -s (soc or int), -r (3d6)");
      Console.WriteLine();
      Console.WriteLine("              -determine_freight:");
      Console.WriteLine("                                              Requires: -sw or -swh, -dw or -dwh");
      Console.WriteLine("              -determine_mail:");
      Console.WriteLine("                                              Requires: -sw or -swh, -dw or -dwh");
      Console.WriteLine("                                              Uses:     -s (soc), -highest_rank, -armed, -r");
      Console.WriteLine("              -determine_goods_available:");
      Console.WriteLine("                                              Requires: -sw or -swh");
      Console.WriteLine("\n\n\nExample:\n\nava com.digitalarcsystems.Traveller.TradeCalculator ~/Documents/Role\\ Playing/Traveler/spinward.txt ~/Documents/Role\\ Playing/Traveler/TradeGoods.csv -determine_goods_available -determine_freight -determine_mail -dpp -dsp -tgD66 12 -r 9 -s 2 -b 2 -spm 0 -bpm 0 -armed true -sw Lunion -dw Persephone");
    }
  }
}
