
// Type: com.digitalarcsystems.Traveller.DataModel.TradeGood




using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class TradeGood
  {
    private bool isCommon = false;
    private int d66 = 11;
    private string name = (string) null;
    private List<World.WorldType> available = new List<World.WorldType>();
    private int LotTonsMultiplier_Renamed = 1;
    private float base_price = 10000f;
    private List<TradeGood.TransactionModifier> saleDM = new List<TradeGood.TransactionModifier>();
    private List<TradeGood.TransactionModifier> purchaseDM = new List<TradeGood.TransactionModifier>();
    internal string examples = (string) null;

    public virtual int LotTonsMultiplier => this.LotTonsMultiplier_Renamed;

    public virtual bool Common => this.isCommon;

    public virtual bool isAvailable(World source)
    {
      bool flag = false;
      if (this.Common)
      {
        flag = true;
      }
      else
      {
        for (int index = 0; index < this.available.Count && !flag; ++index)
          flag = source.containsClassification(this.available[index]);
      }
      return flag;
    }

    public virtual int getPurchaseDM(World source)
    {
      return this.calculateModifier(source, (IList<TradeGood.TransactionModifier>) this.purchaseDM);
    }

    private int calculateModifier(World world, IList<TradeGood.TransactionModifier> modifierList)
    {
      int modifier1 = 0;
      foreach (TradeGood.TransactionModifier modifier2 in (IEnumerable<TradeGood.TransactionModifier>) modifierList)
      {
        if (world.worldTypes.Contains(modifier2.worldType) && modifier2.diceModifier > modifier1)
          modifier1 = modifier2.diceModifier;
      }
      return modifier1;
    }

    public virtual int getSaleDM(World destination)
    {
      return this.calculateModifier(destination, (IList<TradeGood.TransactionModifier>) this.saleDM);
    }

    public virtual int D66 => this.d66;

    public virtual float BasePrice => this.base_price;

    public virtual void addPurchaseTransactionModifier(World.WorldType worldType, int diceModifier)
    {
      this.purchaseDM.Add(new TradeGood.TransactionModifier(this, worldType, diceModifier));
    }

    public virtual void addSaleTransactionModifier(World.WorldType worldType, int diceModifier)
    {
      this.saleDM.Add(new TradeGood.TransactionModifier(this, worldType, diceModifier));
    }

    public static List<TradeGood> loadTradeGoodFromFile(string fileName)
    {
      return TradeGood.loadTradeGoodFromFile(fileName, ":", ",");
    }

    public static List<TradeGood> loadTradeGoodFromFile(
      string fileName,
      string delimiter,
      string withinFieldDelimiter)
    {
      string[] separator1 = new string[1]{ delimiter };
      string[] separator2 = new string[1]
      {
        withinFieldDelimiter
      };
      StreamReader streamReader = new StreamReader(fileName);
      List<TradeGood> tradeGoodList = new List<TradeGood>();
      streamReader.ReadLine();
      string str1;
      while ((str1 = streamReader.ReadLine()) != null)
      {
        int num1 = 0;
        try
        {
          string[] strArray1 = str1.Split(separator1, StringSplitOptions.None);
          TradeGood tradeGood1 = new TradeGood();
          TradeGood tradeGood2 = tradeGood1;
          string[] strArray2 = strArray1;
          int index1 = num1;
          int num2 = index1 + 1;
          int integer1 = TradeGood.toInteger(strArray2[index1]);
          tradeGood2.d66 = integer1;
          TradeGood tradeGood3 = tradeGood1;
          string[] strArray3 = strArray1;
          int index2 = num2;
          int num3 = index2 + 1;
          string str2 = strArray3[index2];
          tradeGood3.name = str2;
          string[] strArray4 = strArray1;
          int index3 = num3;
          int num4 = index3 + 1;
          string[] strArray5 = strArray4[index3].Split(separator1, StringSplitOptions.None);
          int num5 = 0;
          while (strArray5.Length > num5)
            tradeGood1.available.Add(World.WorldType.fromAbreviation(strArray5[num5++].Trim()));
          if (tradeGood1.available.Contains(World.WorldType.All))
            tradeGood1.isCommon = true;
          int num6 = num4 + 1;
          TradeGood tradeGood4 = tradeGood1;
          string[] strArray6 = strArray1;
          int index4 = num6;
          int num7 = index4 + 1;
          int num8 = int.Parse(strArray6[index4]);
          tradeGood4.LotTonsMultiplier_Renamed = num8;
          TradeGood tradeGood5 = tradeGood1;
          string[] strArray7 = strArray1;
          int index5 = num7;
          int num9 = index5 + 1;
          double num10 = (double) float.Parse(strArray7[index5]);
          tradeGood5.base_price = (float) num10;
          string[] strArray8 = strArray1;
          int index6 = num9;
          int num11 = index6 + 1;
          string[] strArray9 = strArray8[index6].Split(separator2, StringSplitOptions.None);
          int num12 = 0;
          while (strArray9.Length > num12)
          {
            int num13 = 0;
            string[] strArray10 = strArray9[num12++].Split(' ');
            TradeGood tradeGood6 = tradeGood1;
            string[] strArray11 = strArray10;
            int index7 = num13;
            int num14 = index7 + 1;
            World.WorldType worldType = World.WorldType.fromAbreviation(strArray11[index7].Trim());
            string[] strArray12 = strArray10;
            int index8 = num14;
            int num15 = index8 + 1;
            int integer2 = TradeGood.toInteger(strArray12[index8]);
            tradeGood6.addPurchaseTransactionModifier(worldType, integer2);
          }
          string[] strArray13 = strArray1;
          int index9 = num11;
          int num16 = index9 + 1;
          string[] strArray14 = strArray13[index9].Split(separator2, StringSplitOptions.None);
          int num17 = 0;
          while (strArray14.Length > num17)
          {
            int num18 = 0;
            string[] strArray15 = strArray14[num17++].Split(' ');
            TradeGood tradeGood7 = tradeGood1;
            string[] strArray16 = strArray15;
            int index10 = num18;
            int num19 = index10 + 1;
            World.WorldType worldType = World.WorldType.fromAbreviation(strArray16[index10].Trim());
            string[] strArray17 = strArray15;
            int index11 = num19;
            int num20 = index11 + 1;
            int integer3 = TradeGood.toInteger(strArray17[index11]);
            tradeGood7.addSaleTransactionModifier(worldType, integer3);
          }
          TradeGood tradeGood8 = tradeGood1;
          string[] strArray18 = strArray1;
          int index12 = num16;
          int num21 = index12 + 1;
          string str3 = strArray18[index12];
          tradeGood8.examples = str3;
          tradeGoodList.Add(tradeGood1);
        }
        catch (Exception ex)
        {
          Console.WriteLine("On line: [" + str1 + "] encountered Exception " + ex?.ToString());
        }
      }
      return tradeGoodList;
    }

    public static TradeGood determineTradeGoodFromD66Value(int d66, List<TradeGood> goods)
    {
      foreach (TradeGood good in goods)
      {
        if (good.d66 == d66)
          return good;
      }
      return (TradeGood) null;
    }

    private static int toInteger(string nextToken)
    {
      if (nextToken[0] == '+')
        nextToken = nextToken.Substring(1);
      return int.Parse(nextToken);
    }

    public static void Main(string[] args)
    {
      List<TradeGood> tradeGoodList = (List<TradeGood>) null;
      try
      {
        tradeGoodList = TradeGood.loadTradeGoodFromFile(args[0]);
      }
      catch (MissingFieldException ex)
      {
        Console.WriteLine(ex.Message);
        Console.Write(ex.StackTrace);
      }
      foreach (object obj in tradeGoodList)
        Console.WriteLine(obj);
    }

    public virtual string Name => this.name;

    public override string ToString()
    {
      string str = "\t\t";
      if (this.Name.Length >= 24)
        str = "\t";
      else if (this.Name.Length <= 15 && this.Name.Length > 7)
        str = "\t\t\t";
      else if (this.Name.Length <= 7)
        str = "\t\t\t\t";
      StringBuilder stringBuilder = new StringBuilder(this.BasePrice.ToString("C"));
      while (stringBuilder.Length < 9)
        stringBuilder.Insert(0, " ");
      return this.Name + str + "BaseCost: " + stringBuilder?.ToString() + " Cr/t\tD66: " + this.D66.ToString();
    }

    public static TradeGood determineTradeGoodFromName(string tradeGood, List<TradeGood> goods)
    {
      foreach (TradeGood good in goods)
      {
        if (good.name.Equals(tradeGood))
          return good;
      }
      return (TradeGood) null;
    }

    internal class TransactionModifier
    {
      private readonly TradeGood outerInstance;
      public World.WorldType worldType;
      public int diceModifier = 0;

      public TransactionModifier(
        TradeGood outerInstance,
        World.WorldType worldType,
        int diceModifier)
      {
        this.outerInstance = outerInstance;
        this.worldType = worldType;
        this.diceModifier = diceModifier;
      }
    }
  }
}
