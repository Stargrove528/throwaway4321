
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  [Serializable]
  public class Equipment : 
    Outcome,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    ILicensedAsset
  {
    [JsonProperty]
    private Dictionary<string, SkillModifiers> _tasks = new Dictionary<string, SkillModifiers>();
    [JsonProperty]
    private Dictionary<string, int> _stats = new Dictionary<string, int>();
    protected internal bool MusteringOutBenefit = false;

    [JsonProperty]
    public Guid Id { get; set; }

    [JsonProperty]
    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    [JsonProperty]
    public List<AssetTag> Tags { get; set; }

    [JsonProperty]
    public List<string> Categories { get; set; }

    [JsonProperty]
    public override string Description
    {
      get => this._description;
      set
      {
        if (this._originalDescription == null)
          this._originalDescription = value;
        this._description = value;
      }
    }

    [JsonProperty]
    public new virtual string Name
    {
      get => this._name;
      set
      {
        if (this._originalName == null)
          this._originalName = value;
        this._name = value;
      }
    }

    [JsonProperty]
    public virtual double Weight { get; set; }

    [JsonProperty]
    public virtual int TechLevel { get; set; }

    [JsonProperty]
    public virtual int Cost { get; set; }

    [JsonProperty]
    public string Skill { get; set; }

    [JsonProperty]
    public string SubSkill { get; set; }

    [JsonProperty]
    public string PrimaryAttribute { get; set; }

    [JsonProperty]
    public int MinimumRank { get; set; }

    [JsonProperty]
    public string Traits { get; set; }

    [JsonProperty]
    public Guid InstanceID { get; set; }

    [JsonProperty]
    private string _originalName { get; set; }

    [JsonProperty]
    private string _originalDescription { get; set; }

    [JsonIgnore]
    public string OriginalName
    {
      get
      {
        if (this._originalName == null)
          this._originalName = this.Name;
        return this._originalName;
      }
    }

    [JsonIgnore]
    public string OriginalDescription
    {
      get
      {
        if (this._originalDescription == null)
          this._originalDescription = this.Description;
        return this._originalDescription;
      }
    }

    public virtual bool ModifiesSkillTask(string skillName, string statName)
    {
      return this.BonusProvided(skillName, 0, -3, new com.digitalarcsystems.Traveller.DataModel.Attribute(statName, com.digitalarcsystems.Traveller.DataModel.Attribute.GetCanonicalOrdinalForStat(statName))
      {
        Value = 7
      }) != 0;
    }

    public virtual int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      int num = 0;
      List<string> stringList = new List<string>()
      {
        skillName
      };
      ISkill skill = DataManager.Instance.GetAsset<ISkill>((Func<ISkill, bool>) (a => a.Name.ToLower().Equals(skillName.ToLower()))).FirstOrDefault<ISkill>();
      if (skill != null)
      {
        if (skill.IsSpecialization)
          stringList.Add(skill.Parent.Name);
        else if (skill.Cascade)
          stringList.AddRange(skill.SpecializationSkills.Select<ISkill, string>((Func<ISkill, string>) (s => s.Name)));
      }
      foreach (string str in stringList)
      {
        if (this._tasks.ContainsKey(str.ToLower()))
        {
          num += this._tasks[str.ToLower()].MaxDifficulty < difficulty ? this._tasks[str.ToLower()].Bonus : 0;
          if (num != 0)
            break;
        }
      }
      if (this._stats.ContainsKey(stat.Name.ToLower()))
        num += this._stats[stat.Name.ToLower()];
      return num;
    }

    public virtual List<string> SkillTasksModified() => this._tasks.Keys.ToList<string>();

    public virtual bool UseEquipmentSkill(
      string skillName,
      int difficulty,
      int characterSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      return false;
    }

    public void AddTaskToModify(string skillName, int maxDifficulty, int bonus)
    {
      this._tasks[skillName.ToLowerInvariant()] = new SkillModifiers()
      {
        Bonus = bonus,
        MaxDifficulty = maxDifficulty
      };
    }

    public void AddTaskToModify(string stat, int bonus)
    {
      if (((IEnumerable<string>) com.digitalarcsystems.Traveller.DataModel.Attribute.CanonicalStats).Any<string>((Func<string, bool>) (s => s.ToLower().Equals(stat.ToLower()))))
      {
        if (this._stats.ContainsKey(stat.ToLower()))
          bonus += this._stats[stat.ToLower()];
        this._stats[stat.ToLower()] = bonus;
      }
      else
        EngineLog.Warning("Arrtibute " + stat + " used in equipment to try and be modified.");
    }

    public Equipment()
    {
      this.Categories = new List<string>();
      this.Weight = 0.0;
      this.TechLevel = 12;
      this.Cost = 0;
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.Tags = new List<AssetTag>();
    }

    public Equipment(IEquipment equipment)
      : this()
    {
      this.Copy(equipment);
    }

    public void Copy(IEquipment equipment)
    {
      this.Id = equipment.Id;
      this.Name = equipment.Name;
      this.Weight = equipment.Weight;
      this.Description = equipment.Description;
      this.Skill = equipment.Skill;
      this.SubSkill = equipment.SubSkill;
      this.TechLevel = equipment.TechLevel;
      this.Cost = equipment.Cost;
      this.PrimaryAttribute = equipment.PrimaryAttribute;
      this.Categories = equipment.Categories != null ? new List<string>((IEnumerable<string>) equipment.Categories) : new List<string>();
      this.Traits = equipment.Traits;
      this.ChildAssets = equipment.ChildAssets == null ? new Dictionary<Guid, AssetMetadata>() : new Dictionary<Guid, AssetMetadata>((IDictionary<Guid, AssetMetadata>) equipment.ChildAssets);
      if (equipment.Tags != null)
        this.Tags = new List<AssetTag>((IEnumerable<AssetTag>) equipment.Tags);
      else
        this.Tags = new List<AssetTag>();
    }

    public Equipment(string name, string description, int tl, int cost, int weight)
      : this()
    {
      this.Categories = new List<string>();
      this.Name = name;
      this.Description = description;
      this.TechLevel = tl;
      this.Cost = cost;
      this.Weight = (double) weight;
    }

    [JsonConstructor]
    public Equipment(string name)
      : this()
    {
      this.Categories = new List<string>();
      this.Name = name;
    }

    public virtual com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment setTechLevel(int tl)
    {
      this.TechLevel = tl;
      return this;
    }

    public override string ToString() => this.Name ?? this.GetType().Name;

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.character.addEquipment((IEquipment) this, true);
      currentState.recorder.RecordBenefit((Outcome) this, currentState);
    }

    public class TasMembership : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      [JsonConstructor]
      public TasMembership(Guid instanceID)
      {
        this.InstanceID = instanceID;
        if (!(instanceID == Guid.Empty))
          return;
        this.InstanceID = Guid.NewGuid();
      }

      public TasMembership()
      {
        this.Name = "TAS Membership";
        this.Description = "Nontransferable, lifetime membership in the exclusive Traveller’s Aid Society (TAS), a private organisation that maintains hostels and facilities at most class A and B starports. Facilities are available (at reasonable cost) to members and their guests. Membership is a reward for heroism or extraordinary service to the Society rather than  an official benefit from a career. The Traveller’s Aid Society is an exclusive organisation, made up of those who are truly citizens of the galaxy, not just a single world.  Every two months, members are provided one high passage to each member. This passage may be used, retained or sold.";
        this.Id = new Guid("{A30ED0B2-D5C1-490A-88EC-14385F1B242D}");
        this.InstanceID = Guid.NewGuid();
        this.Categories = new List<string>() { "General" };
        this.Categories.AddDistinct<string>("Organization");
      }

      public override string ToString() => this.Description;

      public override void handleOutcome(GenerationState currentState)
      {
        if (currentState.character.Equipment.Contains((IEquipment) this))
        {
          com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment equipment = (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment) new com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares(2);
          FeedbackStream.Send("Already have TAS Membership.  Converting to Ship Shares[2]");
          currentState.decisionMaker.present(new Presentation("Converting Duplicate TAS to 2 Ship Shares", "Since you alrady have a TAS Membership, you'll recieve 2 Ship Shares instead."));
          equipment.handleOutcome(currentState);
        }
        else
          base.handleOutcome(currentState);
      }

      public override bool Equals(object amIEqual) => amIEqual is com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.TasMembership;

      public override int GetHashCode() => "TAS Membership".GetHashCode();
    }

    public class Nenj : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.TasMembership
    {
      public Nenj()
      {
        this.Name = nameof (Nenj);
        this.Description = "The Nenjchinzhe’driante (the Consular Legion of Merit) is a post-career recognition of valuable service to all Zhodani. Membership in the Consular Legion of Merit is egalitarian – Nobles, Intendants and Proles are all eligible for the award. Those who have won enrolment are entitled to wear the distinctive gold sash of honour that marks them as recognised elite of the Consulate. Although it is largely honorary, members do receive concrete benefits. Zhodani citizens almost invariably grant members a 10% discount on just about everything, from meals to equipment purchases to starship passages. Sums of more than MCr1 are rarely so discounted but bank loans in these amounts are made without interest. Membership is for life and is not transferable but companions of a member may share in the benefits when they do the buying.";
        this.Id = new Guid("FF563F37-B5C4-4BB6-A5C9-45D0251E4C56");
        this.InstanceID = Guid.NewGuid();
        this.Categories = new List<string>()
        {
          "General, Zhodani, Membership"
        };
        this.Categories.AddDistinct<string>("Organization");
      }
    }

    public class ShipShares : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      public const string SHIP_SHARES_ID = "0a485bb4-83c2-4b5f-8e7f-e88115a85f96";
      public int Shares;

      public int Num
      {
        set
        {
          this.Shares = value;
          this.Name = "Ship Shares [" + value.ToString() + "]";
          this.Description = value.ToString() + " x 1 MCr towards a ship";
        }
        get => this.Shares;
      }

      [JsonConstructor]
      public ShipShares(Guid instanceID)
      {
        this.InstanceID = instanceID;
        if (!(instanceID == Guid.Empty))
          return;
        this.InstanceID = Guid.NewGuid();
      }

      public ShipShares()
      {
        this.Id = new Guid("0a485bb4-83c2-4b5f-8e7f-e88115a85f96");
        this.InstanceID = Guid.NewGuid();
        this.Name = "Ship Shares";
        this.Num = 1;
        this.Description = " MCr towards a ship";
        this.Categories = new List<string>() { "General" };
      }

      public ShipShares(int num)
        : this()
      {
        this.Num = num;
      }

      public override void handleOutcome(GenerationState currentState)
      {
        base.handleOutcome(currentState);
      }
    }

    public class Ship : Vehicle
    {
      public float MonthlyPayment => (float) this.PurchaseCost / 240f;

      public int PurchaseCost { get; set; }

      public override int Cost => this.PurchaseCost;

      public int MonthlyMaintenanceCost { get; set; }

      public int PercentageOwned { get; set; }

      public List<string> SpacecraftQuirks { get; set; }

      public com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship.ShipClass ShipType { get; set; }

      public int Tons { get; set; }

      public int Thrust { get; set; }

      public int Power { get; set; }

      public int Displacement { get; set; }

      public int StateRooms { get; set; }

      public int LowBerths { get; set; }

      public int Fuel { get; set; }

      public string Sensors { get; set; }

      public int Jump { get; set; }

      public string Weapons { get; set; }

      public string MainComputer { get; set; }

      [JsonConstructor]
      public Ship() => this.Categories.AddDistinct<string>("Vehicle");

      public Ship(string name) => this.Name = name;

      public enum ShipClass
      {
        Trader,
        Military,
        SmallCraft,
        Other,
      }
    }

    public class Survival : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      public Survival()
      {
      }

      public Survival(IEquipment copyMe)
        : base(copyMe)
      {
      }
    }

    public class Toolkit : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      public Toolkit()
      {
      }

      public Toolkit(IEquipment copyMe)
        : base(copyMe)
      {
      }

      public override bool ModifiesSkillTask(string skillName, string statName)
      {
        return Utility.equalToOrSubSkillOf(this.Skill, skillName) || Utility.equalToOrSubSkillOf(this.SubSkill, skillName);
      }

      public override int BonusProvided(
        string skillName,
        int difficulty,
        int charactersSkillLevel,
        com.digitalarcsystems.Traveller.DataModel.Attribute stat)
      {
        int num = 0;
        if (this.ModifiesSkillTask(skillName, stat.Name))
          num = 2;
        return num;
      }

      public override List<string> SkillTasksModified()
      {
        List<string> stringList = new List<string>();
        if (this.Skill != null && this.Skill.Any<char>())
          stringList.Add(this.Skill.ToLowerInvariant());
        if (this.SubSkill != null && this.SubSkill.Any<char>())
          stringList.Add(this.Skill.ToLowerInvariant());
        return stringList;
      }
    }

    public class Sensors : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      public Sensors()
      {
      }

      public Sensors(IEquipment copyMe)
        : base(copyMe)
      {
      }
    }

    public class ComputingSensor : 
      com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Sensors,
      IComputer,
      IEquipment,
      ICustomDescribable,
      IDescribable,
      IAsset,
      IAssetBase,
      ICategorizable,
      IUpgradable
    {
      protected Computer Computer = new Computer();

      public ComputingSensor()
      {
      }

      public ComputingSensor(com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Sensors copyMe)
        : base((IEquipment) copyMe)
      {
      }

      public int AvailableRating => this.Computer.AvailableRating;

      public int Rating
      {
        get => this.Computer.Rating;
        set => this.Computer.Rating = value;
      }

      public List<Software> RunningSoftware => this.Computer.RunningSoftware;

      public List<string> AllowedUpgradeCategories => this.Computer.AllowedUpgradeCategories;

      public List<IEquipmentOption> Options => this.Computer.Options;

      public List<IWeapon> WeaponSubcomponents => this.Computer.WeaponSubcomponents;

      public List<IComputer> ComputerSubcomponents => this.Computer.ComputerSubcomponents;

      public bool CanRun(Software canIRun) => this.Computer.CanRun(canIRun);

      public bool LoadSoftware(Software runMe) => this.Computer.LoadSoftware(runMe);

      public void UnloadSoftware(Software unloadMe) => this.Computer.UnloadSoftware(unloadMe);

      public bool AddOption(IEquipmentOption addMe) => this.Computer.AddOption(addMe);

      public void RemoveOption(IEquipmentOption removeMe) => this.Computer.RemoveOption(removeMe);

      public int MaxRunnableRating(Software whatRatingCanIRunAt)
      {
        return this.Computer.MaxRunnableRating(whatRatingCanIRunAt);
      }
    }

    public class MedicalSupplies : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      [JsonProperty]
      private List<ManualHealthState> _conditionsRelieved = new List<ManualHealthState>();

      public MedicalSupplies()
      {
      }

      public MedicalSupplies(IEquipment copyMe)
        : base(copyMe)
      {
      }
    }

    public class Communication : 
      com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment,
      IRangedEquipment,
      IEquipment,
      ICustomDescribable,
      IDescribable,
      IAsset,
      IAssetBase,
      ICategorizable
    {
      public Communication()
      {
      }

      public Communication(IEquipment copyMe)
        : base(copyMe)
      {
      }

      [JsonProperty]
      public int RangeInMeters { get; set; }
    }

    public class ComputerCommunicator : 
      com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Communication,
      IComputer,
      IEquipment,
      ICustomDescribable,
      IDescribable,
      IAsset,
      IAssetBase,
      ICategorizable,
      IUpgradable
    {
      [JsonProperty]
      private IComputer _computer;

      [JsonConstructor]
      public ComputerCommunicator()
      {
      }

      public ComputerCommunicator(IComputer basedOnMe)
        : base((IEquipment) basedOnMe)
      {
        this._computer = basedOnMe;
      }

      [JsonIgnore]
      public int Rating
      {
        get => this._computer.Rating;
        set => this._computer.Rating = value;
      }

      [JsonIgnore]
      public int AvailableRating => this._computer.AvailableRating;

      [JsonIgnore]
      public List<Software> RunningSoftware => this._computer.RunningSoftware;

      [JsonIgnore]
      public List<string> AllowedUpgradeCategories => this._computer.AllowedUpgradeCategories;

      [JsonIgnore]
      public List<IEquipmentOption> Options => this._computer.Options;

      [JsonIgnore]
      public List<IWeapon> WeaponSubcomponents => this._computer.WeaponSubcomponents;

      [JsonIgnore]
      public List<IComputer> ComputerSubcomponents => this._computer.ComputerSubcomponents;

      public bool AddOption(IEquipmentOption addMe) => this._computer.AddOption(addMe);

      public bool CanRun(Software canIRun) => this._computer.CanRun(canIRun);

      public bool LoadSoftware(Software runMe) => this._computer.LoadSoftware(runMe);

      public int MaxRunnableRating(Software whatRatingCanIRunAt)
      {
        return this._computer.MaxRunnableRating(whatRatingCanIRunAt);
      }

      public void RemoveOption(IEquipmentOption removeMe) => this._computer.RemoveOption(removeMe);

      public void UnloadSoftware(Software unloadMe) => this._computer.UnloadSoftware(unloadMe);

      public override int BonusProvided(
        string skillName,
        int difficulty,
        int charactersSkillLevel,
        com.digitalarcsystems.Traveller.DataModel.Attribute stat)
      {
        return base.BonusProvided(skillName, difficulty, charactersSkillLevel, stat) + this._computer.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
      }

      public override bool ModifiesSkillTask(string skillName, string statName)
      {
        return this._computer.ModifiesSkillTask(skillName, statName) || base.ModifiesSkillTask(skillName, statName);
      }
    }

    public class CombatImplant : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      [JsonConstructor]
      public CombatImplant()
        : base("Combat Implant", "This is a combat implant", 12, 0, 0)
      {
        this.Categories.AddDistinct<string>("Cybernetics");
        this.Id = new Guid("{8F1898EC-FB45-455F-9DA4-DAA73216F3F2}");
      }
    }

    public class ScientificEquipment : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
    {
      [JsonConstructor]
      public ScientificEquipment()
        : base("Scientific Equipment", "This is a scientific gadget of some kind", 12, 0, 0)
      {
        this.Categories.AddDistinct<string>("Scientific");
        this.Id = new Guid("{22C0C313-0E2B-414D-8368-54E45B8C3081}");
      }
    }
  }
}
