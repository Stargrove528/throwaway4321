
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit_Ship




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit_Ship : Outcome
  {
    [JsonProperty]
    public string ShipName { get; set; }

    [JsonProperty]
    public bool ShipOnLoan { get; set; }

    [JsonConstructor]
    public MusteringOutBenefit_Ship(string shipName, bool shipOnLoan)
      : this(shipName)
    {
      this.ShipOnLoan = shipOnLoan;
      if (!shipOnLoan)
        return;
      this.Description = " Congratulations!  A fully functional starship has been placed at your disposal.  You don't own it, but you have use of.    You'll never have to make a monthly mortgage payment, but your are legally, morally, or cirumstancially obligated to the owning entity (for instance the Scout service)  who may require your assistance from time to time.";
    }

    public MusteringOutBenefit_Ship(string shipName)
    {
      this.ShipName = shipName;
      this.Name = shipName;
      this.Description = "Congratulations!  You've now own part of a starship, specifically a " + this.ShipName + ". Each time this benefit is obtained, you will be granted 25% equity in the ship.  If you gain this benefit 4 times, the ship will be yours! Ships gained this way are never new, and yours may have a quirk or two.";
    }

    public override void handleOutcome(GenerationState currentState)
    {
      com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship ship = DataManager.Instance.GetAsset<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>((Func<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship, bool>) (s => s.Name.IndexOf(this.ShipName, StringComparison.InvariantCultureIgnoreCase) >= 0)).FirstOrDefault<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>();
      if (ship == null)
        throw new Exception("Unable to find Ship [" + this.ShipName + "] in Equipment");
      com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship ship1 = currentState.character.FindEquipment<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>((Func<IEquipment, bool>) (shp => shp.Id == ship.Id)).FirstOrDefault<com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship>();
      if (ship1 != null && ship1.Id.Equals(ship.Id) && !this.ShipOnLoan)
      {
        if (ship1.PercentageOwned < 100 && !this.ShipOnLoan)
        {
          ship1.PercentageOwned += 25;
          ship1.Name = ship1.OriginalName + " [" + ship1.PercentageOwned.ToString() + "%]";
          new Outcome.InformUser("More Ship Equity!", "You managed to earn another 25% of your ship.  You now own " + ship1.PercentageOwned.ToString() + "% of a " + ship1.OriginalName + ".").handleOutcome(currentState);
        }
        else
          new Event.ChoiceOutcome(1, "Learn About Your Ship", "Since you already own 100% of your ship, more focus on ships results in your learning about your ship.  Please select a ship based skill as your benefit from the following.", new Outcome[3]
          {
            (Outcome) new Outcome.GainSkill("Spacecraft"),
            (Outcome) new Outcome.GainSkill("Engineer"),
            (Outcome) new Outcome.GainSkill("Electronics")
          }).handleOutcome(currentState);
      }
      else if (!this.ShipOnLoan)
      {
        ship.PercentageOwned = 25;
        ship.Name = ship.OriginalName + " [" + ship.PercentageOwned.ToString() + "%]";
        string str1 = "Double maintenance costs";
        string str2 = "Severly Damaged: -1 Hull";
        List<string> stringList1 = new List<string>()
        {
          "Black-listed: Trader will be impounded in several systems.  DM -1 to all Broker checks.",
          "Well maintained:  Reduce all maintenance costs by 50%",
          "Vessel contains concealed smuggling compartments.",
          "Cargo bay is tainted by chemical spills and leaks.  Vulnerable cargos may be damaged in transit.",
          "Damaged Sensors: DM-1 to all Electronics(sensors) checks",
          "DM-1 to all repair attempts",
          str1,
          str2,
          "Damaged thrusters: DM -1 to all Pilot checks",
          "Ship is a famous and respected trader with a good reputation.",
          "Upgrade computer to next best type."
        };
        List<string> stringList2 = new List<string>()
        {
          str2,
          "Well maintained:  Reduce all maintenance costs by 50%",
          "Upgrade sensors to next best type.",
          "Vessel is equiped with an extra turret, if possible.",
          "Damaged Sensors: DM-1 to all Electronics(sensors) checks",
          "DM-1 to all repair attempts",
          str1,
          str2,
          "Damaged thrusters: DM -1 to all Pilot checks",
          "Ship served with distinction and has a good reputation with the Navy.",
          "Add a weapon costing up to MCr2"
        };
        List<string> choices = new List<string>()
        {
          "Leaky Reactor Core: Roll 2D when the ship jumps.  On a 12, all crew suffer 2D x 20 rads.",
          "Luxurious starship:  DM+1 to all Steward checks",
          "Library computer contains erroneous information.",
          "Vessel contains disturbing psionic echoes.",
          "Damaged Sensors: DM-1 to all Electronics(sensors) checks",
          "DM-1 to all repair attempts",
          str1,
          str2,
          "Damaged thrusters: DM -1 to all Pilot checks",
          "Library computer contains secret or unusual information.",
          "Upgrade sensors to the next best type."
        };
        com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship.ShipClass shipType = ship.ShipType;
        if (shipType.CompareTo((object) com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship.ShipClass.Trader) == 0)
        {
          choices = stringList1;
        }
        else
        {
          shipType = ship.ShipType;
          if (shipType.CompareTo((object) com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship.ShipClass.Military) == 0)
            choices = stringList2;
        }
        int num = (int) Math.Ceiling((double) Dice.D6Roll(1, 0, "You've managed to acquire 25% interest in a " + ship.Name + ".There's something quirky about it though.  Determine the number of quirks for your ship.\r1-2:  1 Quirk\r3-4:  2 Quirks\r5-6:  3 Quirks").rawResult / 2.0);
        for (int index = 1; index <= num; ++index)
        {
          string description = Dice.RollRandomResult<string>("Roll to determine quirk " + index.ToString() + "/" + num.ToString(), (IList<string>) choices, ContextKeys.STRING_CHOICES);
          if (description == str1)
            ship.MonthlyMaintenanceCost *= 2;
          else if (description == str2)
            --ship.Hull;
          currentState.decisionMaker.present(new Presentation("Quirk " + index.ToString(), description));
          if (ship.SpacecraftQuirks == null)
            ship.SpacecraftQuirks = new List<string>()
            {
              description
            };
          else
            ship.SpacecraftQuirks.Add(description);
        }
        new Outcome.AddEquipment((IEquipment) ship).handleOutcome(currentState);
      }
      else if (ship1 == null || !ship1.Id.Equals(ship.Id))
      {
        new Event("Ship on Loan [" + ship.Name + "]", "You have been given care over a ship which you don't own, but have use of.  You don't need to make payments, but the owning entity (for instance the Scout service) may require your assistance from  time to time.").handleOutcome(currentState);
        new Outcome.AddEquipment((IEquipment) ship).handleOutcome(currentState);
      }
      else
      {
        List<Outcome> choices = new List<Outcome>();
        choices.Add((Outcome) new Outcome.MusteringOutBenefitModifier("Reroll", "Choose this option to reroll.", 1));
        List<Outcome> outcomeList = choices;
        Outcome.GainSkill gainSkill = new Outcome.GainSkill("Spacecraft");
        gainSkill.Name = "Increase Pilot Spacecraft";
        gainSkill.Description = "Choose this option to increase your pilot spacecraft skill.";
        outcomeList.Add((Outcome) gainSkill);
        currentState.decisionMaker.ChooseOne<Outcome>("You've already got a ship, so what would you like to do?", (IList<Outcome>) choices).handleOutcome(currentState);
      }
    }
  }
}
