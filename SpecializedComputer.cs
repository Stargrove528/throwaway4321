
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.SpecializedComputer




using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class SpecializedComputer : ComputerOption
  {
    [JsonProperty]
    private SpecializedComputer.SoftwareType softwareType;
    [JsonProperty]
    private Software _specializedFor;

    [JsonIgnore]
    public Software SpecializedFor
    {
      get => this._specializedFor;
      set
      {
        this._specializedFor = value;
        this.softwareType = SpecializedComputer.GetSoftwareType(this._specializedFor);
      }
    }

    public SpecializedComputer(ComputerOption copyMe)
      : base(copyMe)
    {
    }

    [JsonConstructor]
    public SpecializedComputer()
    {
    }

    public static SpecializedComputer.SoftwareType GetSoftwareType(Software typeMe)
    {
      SpecializedComputer.SoftwareType softwareType = SpecializedComputer.SoftwareType.OTHER;
      if (typeMe is AgentProgram)
        softwareType = SpecializedComputer.SoftwareType.AGENT;
      else if (typeMe is ExpertProgram)
        softwareType = SpecializedComputer.SoftwareType.EXPERT;
      else if (typeMe.OriginalName.ToLower().Contains("database"))
        softwareType = SpecializedComputer.SoftwareType.DATABASE;
      else if (typeMe.OriginalName.ToLower().Contains("decryptor"))
        softwareType = SpecializedComputer.SoftwareType.DECRYPTOR;
      else if (typeMe.OriginalName.ToLower().Contains("friend"))
        softwareType = SpecializedComputer.SoftwareType.DIGITAL_FRIEND;
      else if (typeMe.OriginalName.ToLower().Contains("intellect"))
        softwareType = SpecializedComputer.SoftwareType.INTELLECT;
      else if (typeMe.OriginalName.ToLower().Contains("intelligent"))
        softwareType = SpecializedComputer.SoftwareType.INTELLIGENT_INTERFACE;
      else if (typeMe.OriginalName.ToLower().Contains("intrusion"))
        softwareType = SpecializedComputer.SoftwareType.INTRUSION;
      else if (typeMe.OriginalName.ToLower().Contains("security"))
        softwareType = SpecializedComputer.SoftwareType.SECURITY;
      return softwareType;
    }

    public int BonusProvided(Software doIgetABonus)
    {
      int num = 0;
      if (this.softwareType != SpecializedComputer.SoftwareType.OTHER && this.softwareType == SpecializedComputer.GetSoftwareType(doIgetABonus) || this.SpecializedFor != null && doIgetABonus.Id == this.SpecializedFor.Id)
        num = Math.Min(this.Rating, doIgetABonus.CurrentRating);
      return num;
    }

    public override int CalculatePrice(IEquipment equipmentToBeAddedTo)
    {
      return (int) Math.Ceiling((double) this.Rating * 0.25 * (double) equipmentToBeAddedTo.Cost);
    }

    public enum SoftwareType
    {
      DATABASE,
      EXPERT,
      AGENT,
      INTELLIGENT_INTERFACE,
      INTELLECT,
      INTRUSION,
      SECURITY,
      TRANSLATOR,
      DECRYPTOR,
      DIGITAL_FRIEND,
      PERSONAL_TRAINER,
      OTHER,
    }
  }
}
