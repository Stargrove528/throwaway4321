
// Type: com.digitalarcsystems.Traveller.DataModel.Character




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  [JsonObject]
  public class Character : IAsset, IDescribable, IAssetBase, INonLicensedAsset
  {
    [JsonProperty]
    private Dictionary<int, Armor> _armorSlots = new Dictionary<int, Armor>();
    [JsonProperty]
    protected List<Guid> _equippedIds = new List<Guid>();
    [JsonProperty]
    private List<Attribute> _attributes;
    [JsonProperty]
    private int _credits;
    private bool _dead;
    [JsonProperty]
    private int _debt;
    private bool _drafted;
    [JsonProperty]
    private List<IEquipment> _equipment = new List<IEquipment>();
    private bool _firstTermDrafted = true;
    [JsonProperty]
    private Race _race;
    [JsonProperty]
    private Dictionary<string, SkillTraining> _skillsBeingTrained;
    private List<string> connections = new List<string>();
    [JsonProperty]
    private Dictionary<Guid, int> equipment = new Dictionary<Guid, int>();
    [JsonProperty]
    [JsonConverter(typeof (IgnoreEnumerableTypeConverter))]
    private SkillDictionary skills = new SkillDictionary();
    [JsonProperty]
    private Dictionary<string, Talent> talents = new Dictionary<string, Talent>();
    [JsonProperty]
    private Dictionary<string, Trait> traits = new Dictionary<string, Trait>();

    [JsonIgnore]
    public static Character BlankCharacter
    {
      get
      {
        List<Attribute> characteristics = new List<Attribute>()
        {
          new Attribute("Str", 0),
          new Attribute("Dex", 1),
          new Attribute("End", 2),
          new Attribute("Int", 3),
          new Attribute("Edu", 4),
          new Attribute("Soc", 5)
        };
        Character blankCharacter = new Character(DataManager.UserID != 0 ? DataManager.UserID : 1)
        {
          Race = new Race("Human", "Blank Race", "", characteristics, new List<Skill>()),
          CurrentDate = new GameDate()
          {
            ImperialDay = 1,
            ImperialYear = 1105
          }
        };
        blankCharacter.Gender = blankCharacter.Race.Genders[0];
        blankCharacter.generateCharacteristics(new int?(7));
        return blankCharacter;
      }
    }

    [JsonProperty]
    public int Age { get; set; }

    [JsonProperty]
    public float Movement { get; set; }

    [JsonProperty]
    public IWeapon Unarmed { get; set; }

    [JsonProperty]
    public Armor NaturalArmor { get; set; }

    protected void equipArmor(Armor equipMe)
    {
      int strata = equipMe.Strata;
      if (this._armorSlots.ContainsKey(strata))
        this.UnEquip((IEquipment) this._armorSlots[strata]);
      this._armorSlots.Add(strata, equipMe);
    }

    protected void unEquipArmor(Armor removeMe) => this._armorSlots.Remove(removeMe.Strata);

    protected ReadOnlyCollection<Armor> equippedArmor()
    {
      List<Armor> armorList = new List<Armor>((IEnumerable<Armor>) this._armorSlots.Values);
      if (this.NaturalArmor != null)
        armorList.Add(this.NaturalArmor);
      return armorList.AsReadOnly();
    }

    public List<Armor> getArmor()
    {
      return this.equippedArmor().OrderByDescending<Armor, int>((Func<Armor, int>) (a => a.ProtectionKinetic)).ToList<Armor>();
    }

    public int getArmorProtection(DamageType damageType)
    {
      int armorProtection = 0;
      List<Armor> armor1 = this.getArmor();
      if (armor1.Count<Armor>() > 0)
      {
        foreach (Armor armor2 in armor1)
        {
          armor2.protectionByDamageType(damageType);
          armorProtection += armor2.protectionByDamageType(damageType);
        }
      }
      return armorProtection;
    }

    [JsonIgnore]
    public virtual List<Contact> Allies
    {
      get
      {
        return this.EntityIKnow == null ? new List<Contact>() : this.EntityIKnow.Where<Contact>((Func<Contact, bool>) (entity => entity.Type == Contact.ContactType.Ally)).ToList<Contact>();
      }
    }

    [JsonProperty]
    public AvatarInfo Avatar { get; set; }

    [JsonProperty]
    public virtual List<Term> CareerHistory { get; private set; }

    public virtual int CashRolls { get; private set; }

    [JsonProperty]
    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    [JsonIgnore]
    public virtual List<Contact> Contacts
    {
      get
      {
        return this.EntityIKnow == null ? new List<Contact>() : this.EntityIKnow.Where<Contact>((Func<Contact, bool>) (entity => entity.Type == Contact.ContactType.Contact)).ToList<Contact>();
      }
    }

    [JsonProperty]
    public int CreatingUser { get; set; }

    [JsonIgnore]
    public virtual int Credits
    {
      get => this._credits;
      set
      {
        if (value >= 0)
          this._credits = value;
        else
          this.Debt += value * -1;
      }
    }

    [JsonProperty]
    public int CumulativeRads { get; set; }

    [JsonProperty]
    public Combat currentCombat { get; set; }

    [JsonProperty]
    public GameDate CurrentDate { get; set; }

    [JsonIgnore]
    public virtual Term CurrentTerm
    {
      get
      {
        return this.CareerHistory.Count == 0 ? (Term) null : this.CareerHistory[this.CareerHistory.Count - 1];
      }
    }

    [JsonIgnore]
    public virtual bool CurrentTermNewCareer
    {
      get
      {
        return this.CareerHistory.Count < 2 || !this.CurrentTerm.careerName.Equals(this.CareerHistory[this.CareerHistory.Count - 2].careerName, StringComparison.CurrentCultureIgnoreCase);
      }
    }

    [JsonIgnore]
    public int Debt
    {
      get => this._debt;
      set
      {
        if (value < 0)
          this.Credits += value * -1;
        else
          this._debt = value;
      }
    }

    [JsonProperty]
    public string DefaultFileName => this.Id.ToString("D") + ".tchjson";

    public virtual string Description { get; set; }

    [JsonIgnore]
    public virtual List<Contact> Enemies
    {
      get
      {
        return this.EntityIKnow == null ? new List<Contact>() : this.EntityIKnow.Where<Contact>((Func<Contact, bool>) (entity => entity.Type == Contact.ContactType.Enemy)).ToList<Contact>();
      }
    }

    [JsonProperty]
    public List<Contact> EntityIKnow { get; private set; }

    [JsonIgnore]
    public virtual IList<IEquipment> Equipment
    {
      get
      {
        if (this._equipment.Any<IEquipment>((Func<IEquipment, bool>) (e => e.InstanceID == Guid.Empty)))
        {
          EngineLog.Warning("DM didn't convert character.  Converting Now: [" + this.Name + "] " + this.Id.ToString());
          this.convertToNewDataModel();
        }
        return !this._equipment.Any<IEquipment>() ? (IList<IEquipment>) new List<IEquipment>().AsReadOnly() : (IList<IEquipment>) this._equipment.AsReadOnly();
      }
    }

    [JsonIgnore]
    public IList<IEquipment> Equipped
    {
      get
      {
        List<IEquipment> equipped = new List<IEquipment>();
        if (this._equippedIds == null)
          this._equippedIds = new List<Guid>();
        if (this._equipment.Any<IEquipment>())
        {
          foreach (Guid equippedId in this._equippedIds)
          {
            Guid id = equippedId;
            IEquipment equipment = this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.InstanceID.Equals(id))).FirstOrDefault<IEquipment>();
            if (equipment == null)
              EngineLog.Error("Encountered Equipped items not in characters equipment: " + id.ToString());
            else
              equipped.Add(equipment);
          }
        }
        return (IList<IEquipment>) equipped;
      }
      private set
      {
        if (value == null || !value.Any<IEquipment>() || this._equippedIds != null && this._equippedIds.Any<Guid>())
          return;
        EngineLog.Warning("Character.Equipped.Set: Character [" + this.Name + " : " + this.Id.ToString() + "] is UPGRADING.");
        if (value != null && value.Any<IEquipment>())
          this._equippedIds = new List<Guid>();
        value.ToList<IEquipment>();
        if (value.Any<IEquipment>((Func<IEquipment, bool>) (e => e.InstanceID == Guid.Empty)))
        {
          List<IEquipment> list = value.Select<IEquipment, IEquipment>((Func<IEquipment, IEquipment>) (e =>
          {
            if (e.InstanceID.Equals(Guid.Empty))
              e.InstanceID = Guid.NewGuid();
            return e;
          })).ToList<IEquipment>();
          if (this._equippedIds == null)
            this._equippedIds = new List<Guid>();
          list.ForEach((Action<IEquipment>) (completedValue =>
          {
            if (!this.Equipment.Any<IEquipment>((Func<IEquipment, bool>) (eq => eq.InstanceID.Equals(completedValue.InstanceID))))
              this.addEquipment(completedValue);
            if (this._equippedIds.Contains(completedValue.InstanceID))
              return;
            this._equippedIds.Add(completedValue.InstanceID);
          }));
        }
      }
    }

    [JsonProperty]
    public double EquippedMass { get; private set; }

    [JsonProperty]
    public Gender Gender { get; set; }

    [JsonProperty]
    public bool HadPreCareer { get; private set; }

    [JsonProperty]
    public virtual World HomeWorld { get; set; }

    [JsonProperty]
    public Guid Id { get; set; }

    [JsonIgnore]
    public virtual bool InCrisis
    {
      get
      {
        return this._attributes != null && this._attributes.GetRange(0, 5).Any<Attribute>((Func<Attribute, bool>) (stat => stat.InCrisis()));
      }
    }

    [JsonIgnore]
    public virtual bool Injured
    {
      get
      {
        return this._attributes != null && this._attributes.Where<Attribute>((Func<Attribute, bool>) (a => a.Ordinal != 5 && !a.Name.Equals("ter", StringComparison.InvariantCultureIgnoreCase))).Any<Attribute>((Func<Attribute, bool>) (stat => stat.Injured));
      }
    }

    [JsonIgnore]
    public virtual IList<Attribute> InjuredStats
    {
      get
      {
        return this._attributes == null ? (IList<Attribute>) new List<Attribute>() : (IList<Attribute>) this._attributes.Where<Attribute>((Func<Attribute, bool>) (stat => stat.Injured)).ToList<Attribute>();
      }
    }

    [JsonProperty]
    public List<JournalEntry> Journal { get; set; }

    [JsonProperty]
    public string ModelCustomizationList { get; set; }

    [JsonProperty]
    public int Mortgage { get; set; }

    [JsonProperty]
    public virtual string Name { get; set; }

    [JsonProperty]
    public string Notes { get; set; }

    [JsonProperty]
    public virtual int NumberOfCareers { get; private set; }

    [JsonProperty]
    public int NumberOfTermsOnAnagathics { get; set; }

    [JsonProperty]
    public int NumberOfYearsOnAnagathics { get; set; }

    [JsonProperty]
    public bool OnAnagathics { get; set; }

    [JsonProperty]
    public int Pension { get; set; }

    [JsonIgnore]
    public virtual Race Race
    {
      get => this._race;
      set
      {
        if (value != null && this._attributes != null && this._race != null && !value.Name.Equals(this._race.Name, StringComparison.InvariantCultureIgnoreCase))
        {
          Dictionary<int, Attribute> dictionary = new Dictionary<int, Attribute>();
          foreach (Attribute attribute in this._attributes)
            dictionary[attribute.Ordinal] = attribute;
          foreach (Attribute attribute1 in value.CharacteristicsCopy())
          {
            if (dictionary.Keys.Contains<int>(attribute1.Ordinal))
            {
              Attribute attribute2 = dictionary[attribute1.Ordinal];
              int num1 = attribute2.UninjuredValue - attribute2.Value;
              int num2 = attribute2.UninjuredValue - attribute2.GetCalculatedRacialValue();
              attribute2.RacialBonus = attribute1.RacialBonus;
              attribute2.RollType = attribute1.RollType;
              attribute2.Name = attribute1.Name;
              attribute2.RawRollValues = attribute2.RawRollValues;
              attribute2.UninjuredValue += num2;
              attribute2.Value = attribute2.UninjuredValue - num1;
            }
            else
            {
              attribute1.RawRollValues = new int[2]
              {
                Dice.Roll1D6(),
                Dice.Roll1D6()
              };
              this._attributes.Add(attribute1);
            }
          }
          List<Attribute> source = value.CharacteristicsCopy();
          if (this._attributes.Count > source.Count)
          {
            for (int i = 0; i < this._attributes.Count; i++)
            {
              if (!source.Any<Attribute>((Func<Attribute, bool>) (s => s.Name.ToLower().Equals(this._attributes[i].Name.ToLower()))))
              {
                this._attributes.Remove(this._attributes[i]);
                i--;
              }
            }
          }
        }
        this.Movement = value.BaseMovementMeters;
        this.Unarmed = value.naturalAttack;
        this.NaturalArmor = value.NaturalArmor;
        this._race = value;
      }
    }

    [JsonIgnore]
    public virtual List<Contact> Rivals
    {
      get
      {
        return this.EntityIKnow == null ? new List<Contact>() : this.EntityIKnow.Where<Contact>((Func<Contact, bool>) (entity => entity.Type == Contact.ContactType.Rival)).ToList<Contact>();
      }
    }

    [JsonIgnore]
    public virtual IList<ISkill> Skills
    {
      get
      {
        return this.skills == null || this.skills.Count == 0 ? (IList<ISkill>) new List<ISkill>() : (IList<ISkill>) this.skills.Values.ToList<ISkill>();
      }
    }

    [JsonIgnore]
    public List<SkillTraining> SkillsBeingTrained
    {
      get
      {
        return this._skillsBeingTrained == null ? new List<SkillTraining>() : new List<SkillTraining>((IEnumerable<SkillTraining>) this._skillsBeingTrained.Values);
      }
    }

    [JsonProperty]
    public List<ManualHealthState> States { get; set; }

    [JsonIgnore]
    public virtual IList<Attribute> StatsInCrisis
    {
      get
      {
        return this._attributes == null ? (IList<Attribute>) new List<Attribute>() : (IList<Attribute>) this._attributes.Where<Attribute>((Func<Attribute, bool>) (stat => stat.InCrisis())).ToList<Attribute>();
      }
    }

    public List<AssetTag> Tags { get; set; }

    [JsonIgnore]
    public virtual IList<Talent> Talents
    {
      get
      {
        return this.talents == null || this.talents.Count <= 0 ? (IList<Talent>) new List<Talent>() : (IList<Talent>) new List<Talent>((IEnumerable<Talent>) this.talents.Values);
      }
    }

    [JsonIgnore]
    public virtual int TotalNumberOfTerms
    {
      get => this.CareerHistory != null ? this.CareerHistory.Count : 0;
    }

    public virtual void addAlly(string ally) => this.addEntityIKnow(ally, Contact.ContactType.Ally);

    public virtual void addContact(string contact)
    {
      this.addEntityIKnow(contact, Contact.ContactType.Contact);
    }

    public virtual void addEnemy(string enemy)
    {
      this.addEntityIKnow(enemy, Contact.ContactType.Enemy);
    }

    public virtual void addEntityIKnow(Contact addMe) => this.EntityIKnow.Add(addMe);

    public virtual void addEntityIKnow(string entityName, Contact.ContactType type)
    {
      this.EntityIKnow.Add(new Contact(entityName)
      {
        Type = type,
        Id = Guid.NewGuid()
      });
    }

    public IList<IComputer> getComputers()
    {
      return (IList<IComputer>) this.Equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e is IComputer)).ToList<IEquipment>().Select<IEquipment, IComputer>((Func<IEquipment, IComputer>) (r => (IComputer) r)).ToList<IComputer>().AsReadOnly();
    }

    [JsonIgnore]
    public virtual IList<IAugmentation> Augments
    {
      get
      {
        return (IList<IAugmentation>) this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e is IAugmentation)).Select<IEquipment, IAugmentation>((Func<IEquipment, IAugmentation>) (e => (IAugmentation) e)).ToList<IAugmentation>().AsReadOnly();
      }
    }

    public virtual IEquipment addEquipment(IEquipment addMe, bool cloneIfNoInstanceId = false)
    {
      if (addMe.InstanceID == Guid.Empty && addMe.InstanceID == Guid.Empty)
      {
        if (!cloneIfNoInstanceId)
          throw new ArgumentException("Equipment being equpped or added to character must be cloned from the datamanager equipment.");
        addMe = addMe.Clone<IEquipment>();
        addMe.InstanceID = Guid.NewGuid();
      }
      if (addMe.Id.ToString() == "0a485bb4-83c2-4b5f-8e7f-e88115a85f96")
      {
        if (this.equipmentQuantity(addMe) > 0)
        {
          com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares shipShares = (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares) this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString() == "0a485bb4-83c2-4b5f-8e7f-e88115a85f96")).First<IEquipment>();
          shipShares.Num += (addMe as com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares).Num;
          return (IEquipment) shipShares;
        }
        addMe = (IEquipment) new com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares((addMe as com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares).Num);
      }
      if (addMe.Id.ToString() == "16f7e066-579a-41b3-a5d3-792352cb3067")
      {
        if (this.equipmentQuantity(addMe) > 0)
        {
          ClanShares clanShares = (ClanShares) this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString() == "16f7e066-579a-41b3-a5d3-792352cb3067")).First<IEquipment>();
          clanShares.Number += (addMe as ClanShares).Number;
          return (IEquipment) clanShares;
        }
        addMe = (IEquipment) new ClanShares((addMe as ClanShares).Number);
      }
      this._equipment.Add(addMe);
      return addMe;
    }

    public virtual void addRival(string rival)
    {
      this.addEntityIKnow(rival, Contact.ContactType.Rival);
    }

    public virtual void addSkill(ISkill addMe)
    {
      if (addMe is ReadOnlySkillAdapter)
        addMe = (ISkill) ((ReadOnlySkillAdapter) addMe).Clone();
      this.addSkillToDictionary<ISkill>(addMe, (Dictionary<string, ISkill>) this.skills);
    }

    public virtual void addTalent(Talent addMe)
    {
      this.addSkillToDictionary<Talent>(addMe, this.talents);
    }

    public virtual void addTrait(Trait addMe)
    {
      if (this.traits.ContainsKey(addMe.Name))
        this.traits.Remove(addMe.Name);
      this.traits.Add(addMe.Name, addMe);
    }

    public virtual void applyChildhoodCareer(string planetName)
    {
      Career.Specialization withSpecialization = new Career.Specialization("Childhood", "raised on " + planetName, "Int", 0, "", 0, (Outcome[]) null, (Dictionary<int, Rank>) null);
      this.startNewTerm(new Career("Primary Education", "Everyone raised on " + planetName + " has learned a lot", 0, 0, 0, 0, "Background Skills", "None", 0, false, "None", 0, false, false, new Career.Specialization[1]
      {
        withSpecialization
      }, (Outcome[]) null, (Outcome[]) null, (Outcome[]) null, (Outcome[]) null, (Dictionary<int, Event>) null, (Dictionary<int, Event>) null, (Dictionary<int, Rank>) null, (Dictionary<int, Rank>) null, (Dictionary<int, MusteringOutBenefit>) null), withSpecialization);
    }

    public bool Equals(Character character)
    {
      return this.Id == character.Id && this.SHA1<Character>() == character.SHA1<Character>();
    }

    public IEquipment Equip(IEquipment toEquip, bool cloneIfNoInstanceId = false)
    {
      if (this.Equipped.Any<IEquipment>((Func<IEquipment, bool>) (e => e.InstanceID.Equals(toEquip.InstanceID))))
        return toEquip;
      if (toEquip.InstanceID == Guid.Empty)
      {
        if (!cloneIfNoInstanceId)
          throw new ArgumentException("Equipment being equpped or added to character must be cloned from the datamanager equipment.");
        toEquip = toEquip.Clone<IEquipment>();
        toEquip.InstanceID = Guid.NewGuid();
      }
      if (!this.Equipment.Any<IEquipment>((Func<IEquipment, bool>) (e => e.InstanceID.Equals(toEquip.InstanceID))))
        this.addEquipment(toEquip);
      this._equippedIds.Add(toEquip.InstanceID);
      if (!(toEquip is PoweredArmor))
        this.EquippedMass += toEquip is Armor ? toEquip.Weight * 0.25 : toEquip.Weight;
      if (toEquip is Armor)
        this.equipArmor((Armor) toEquip);
      if (toEquip is ICharacterModifier)
        ((IModifier<Character>) toEquip).ActionsOnAdd(this);
      return toEquip;
    }

    public virtual int equipmentQuantity(IEquipment of)
    {
      int num = 0;
      if (of.Id == Guid.Empty)
      {
        EngineLog.Error("Character.equipmentQuaitity called for [" + of.Name + "] with invalid ID of [" + of.Id.ToString() + "]");
      }
      else
      {
        Guid id = of.Id;
        if (id.ToString() == "16f7e066-579a-41b3-a5d3-792352cb3067")
        {
          ClanShares clanShares = (ClanShares) this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString() == "16f7e066-579a-41b3-a5d3-792352cb3067")).FirstOrDefault<IEquipment>();
          if (clanShares != null)
            num = clanShares.Number;
        }
        else
        {
          id = of.Id;
          if (id.ToString() == "0a485bb4-83c2-4b5f-8e7f-e88115a85f96")
          {
            com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares shipShares = (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares) this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString() == "0a485bb4-83c2-4b5f-8e7f-e88115a85f96")).FirstOrDefault<IEquipment>();
            if (shipShares != null)
              num = shipShares.Num;
          }
          else
            num = this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Id.Equals(of.Id))).Count<IEquipment>();
        }
      }
      return num;
    }

    public IEnumerable<T> FindEquipment<T>(Func<IEquipment, bool> query = null) where T : IEquipment
    {
      return query == null ? this.Equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e is T)).Select<IEquipment, T>((Func<IEquipment, T>) (e => (T) e)) : this.Equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e is T && query(e))).Select<IEquipment, T>((Func<IEquipment, T>) (e => (T) e));
    }

    public virtual IList<Attribute> generateCharacteristics(int? defaultValue = null)
    {
      if (this.Race == null)
      {
        EngineLog.Warning("Character.generateCharacteristics null race was passed to character.");
        return (IList<Attribute>) this._attributes;
      }
      this._attributes = this.Race.CharacteristicsCopy();
      if (this.traits.Count > 0)
      {
        foreach (Trait trait1 in this.traits.Values)
        {
          Trait trait = trait1;
          if (!this._attributes.Select<Attribute, string>((Func<Attribute, string>) (a => a.Name)).Any<string>((Func<string, bool>) (a => a.Contains(trait.associatedAttribute))))
            this._attributes.Add(new Attribute(trait.associatedAttribute, trait.ordinal, 2, 0));
        }
      }
      EngineLog.Print("Character.generateCharacteristics for race " + this.Race.Name + ": " + this._attributes.Count.ToString());
      foreach (Attribute attribute in this._attributes)
      {
        if (defaultValue.HasValue)
        {
          attribute.RawRollValues = new int[2]
          {
            defaultValue.Value / 6 * 6,
            defaultValue.Value % 6
          };
        }
        else
        {
          attribute.Generate();
          while (attribute.Value <= 0)
            attribute.Generate();
        }
      }
      return this.getAttributes();
    }

    public virtual int getAtrributeValue(int ordinal)
    {
      EngineLog.Warning("getting attribute by ordinal is highly discouraged as it can be dynamic array");
      if (ordinal < 0 || ordinal >= this._attributes.Count)
        throw new Exception("Trying to get characteristic #" + ordinal.ToString() + " but it's out of range.");
      return this._attributes[ordinal].Value;
    }

    public virtual Attribute getAttribute(string characteristic)
    {
      return this.getAttributeByName(characteristic);
    }

    public virtual Attribute getAttribute(int ordinal)
    {
      EngineLog.Warning("getting attribute by ordinal is highly discouraged as it can be dynamic array");
      return this._attributes[ordinal];
    }

    public virtual int getAttributeModifier(int ordinal)
    {
      EngineLog.Warning("getting attribute by ordinal is highly discouraged as it can be dynamic array");
      if (ordinal < 0 || ordinal >= this._attributes.Count)
        throw new Exception("Trying to get characteristic #" + ordinal.ToString() + " but it's out of range.");
      return this._attributes[ordinal].Modifier;
    }

    public virtual int getAttributeModifier(string characteristic)
    {
      return (this.getAttributeByName(characteristic) ?? throw new NullReferenceException("Characteristic [" + characteristic + "] could not be found")).Modifier;
    }

    public virtual IList<Attribute> getAttributes()
    {
      return (IList<Attribute>) (this._attributes ?? new List<Attribute>());
    }

    public virtual int getAttributeValue(string characteristic)
    {
      return this.getAttributeByName(characteristic).TotalValue;
    }

    public virtual int getNumMusteringOutBenefitsAlreadyAwaredInCareer(string careerName)
    {
      int alreadyAwaredInCareer = 0;
      foreach (Term term in this.CareerHistory)
      {
        if (term.careerName.Equals(careerName, StringComparison.CurrentCultureIgnoreCase))
          alreadyAwaredInCareer += term.num_mustering_out_benefits_awarded_this_term;
      }
      return alreadyAwaredInCareer;
    }

    public virtual IList<Attribute> getPhysicalAttributes()
    {
      return (IList<Attribute>) new List<Attribute>()
      {
        this.getAttribute("Str"),
        this.getAttribute("Dex"),
        this.getAttribute("End")
      };
    }

    public virtual Term getPreviousTermOfCareer(string careerName, int termsAgo = 0)
    {
      List<Term> termList = new List<Term>();
      Term previousTermOfCareer = (Term) null;
      foreach (Term term in this.CareerHistory.Where<Term>((Func<Term, bool>) (currentTerm => currentTerm.careerName.Equals(careerName, StringComparison.CurrentCultureIgnoreCase))))
        termList.Add(term);
      if (termsAgo <= termList.Count)
        previousTermOfCareer = termList[termList.Count - (termsAgo + 1)];
      return previousTermOfCareer;
    }

    public virtual ISkill getSkill(string skillname)
    {
      return this.getEntity<ISkill>(skillname, (Dictionary<string, ISkill>) this.skills);
    }

    public virtual ISkill getSkill(ISkill skill)
    {
      return this.getEntity<ISkill>(skill.Name, (Dictionary<string, ISkill>) this.skills);
    }

    public virtual Talent getTalent(string talentname)
    {
      return this.getEntity<Talent>(talentname, this.talents);
    }

    public virtual Talent getTalent(Talent talent)
    {
      return this.getEntity<Talent>(talent.Name, this.talents);
    }

    public virtual Term getTerm(int termNumber)
    {
      Term term = (Term) null;
      if (termNumber >= 0 && termNumber < this.CareerHistory.Count)
        term = this.CareerHistory[termNumber];
      return term;
    }

    public virtual bool hasAlreadyBeenDrafted() => this._drafted;

    public virtual bool hasBeenInCrisis()
    {
      return this._attributes.Any<Attribute>((Func<Attribute, bool>) (stat => stat.HasBeenInCrisis()));
    }

    public virtual bool hasConnection(Character character)
    {
      return this.connections.Contains(character.Name);
    }

    public virtual bool hasDied() => this._dead;

    public virtual bool hasSkill(string skillName)
    {
      return this.getEntity<ISkill>(skillName, (Dictionary<string, ISkill>) this.skills) != null;
    }

    public virtual bool hasSkill(ISkill skill)
    {
      return skill != null ? this.skills.ContainsValue(skill) : throw new NullReferenceException("Character.hasSkill(NULL) isn't a valid parameter");
    }

    public virtual bool hasTalent(string talentName)
    {
      return this.getEntity<Talent>(talentName, this.talents) != null;
    }

    public virtual bool hasTalent(Talent talent) => this.talents.ContainsValue(talent);

    public bool hasTrait(Trait trait)
    {
      EngineLog.Print("Character has the following Traits:");
      foreach (Trait trait1 in this.traits.Values)
        EngineLog.Print(trait1.Name ?? "");
      return this.traits.ContainsKey(trait.Name);
    }

    public bool hasTrait(string traitName) => this.traits.ContainsKey(traitName);

    public virtual bool haveHadCareer(Career career) => this.haveHadCareer(career.Name);

    public virtual bool haveHadCareer(string careerName)
    {
      return this.CareerHistory.Any<Term>((Func<Term, bool>) (term => term.careerName.Equals(careerName, StringComparison.CurrentCultureIgnoreCase)));
    }

    public virtual void incrementCashRolls() => ++this.CashRolls;

    public virtual Rank incrementRankAndGetTitle(Career career)
    {
      return this.incrementRankAndGetTitle(career, false);
    }

    public virtual Rank incrementRankAndGetTitle(Career career, bool newCommission)
    {
      Term currentTerm = this.CurrentTerm;
      if (newCommission)
      {
        currentTerm.rank = 1;
        currentTerm.officer = true;
      }
      else
        ++currentTerm.rank;
      ++currentTerm.total_ranks_in_career;
      Rank rank = career.getRank(currentTerm.officer, currentTerm.rank, currentTerm.specializationName);
      if (rank != null && rank.title.Length > 0)
        currentTerm.title = rank.title;
      return rank;
    }

    public bool incrementWeeksTrained(Skill skill)
    {
      bool flag = false;
      string lowerInvariant = skill.Name.ToLowerInvariant();
      if (this._skillsBeingTrained.ContainsKey(lowerInvariant))
        this.trainSkill(skill);
      SkillTraining skillTraining = this._skillsBeingTrained[lowerInvariant];
      if (skillTraining.increment())
      {
        Skill skillBeingTraining = skillTraining.SkillBeingTraining;
        skillBeingTraining.Level = 1;
        this.addSkill((ISkill) skillBeingTraining);
        this._skillsBeingTrained.Remove(lowerInvariant);
        flag = true;
      }
      return flag;
    }

    public virtual void killCharacter() => this._dead = true;

    public virtual void makeConnection(
      Character personToConnectTo,
      int term,
      string eventName,
      string eventDescription,
      string notes)
    {
      Term term1 = this.getTerm(term);
      if (term1 == null)
        throw new ArgumentOutOfRangeException(nameof (term), "The provided Term [" + term.ToString() + "] didn't resolve to an existing term in the characters career history");
      if (personToConnectTo == null)
        throw new ArgumentNullException(nameof (personToConnectTo), "Must have a non-null name");
      if (eventDescription == null)
        throw new ArgumentNullException(nameof (eventDescription), "Should resolve to a valid event.");
      if (eventName == null)
        throw new ArgumentNullException(nameof (eventName), "Should resolve to a valid event.");
      this.connections.Add(personToConnectTo.Name);
      term1.addConnection(personToConnectTo.Name);
      term1.connectionEvent = new Event(eventName, eventDescription);
      Term term2 = personToConnectTo.CareerHistory[personToConnectTo.CareerHistory.Count - 1];
      string str = "\nConnection Event [" + eventName + "] with " + personToConnectTo.Name + "(" + personToConnectTo.Race.Name + ", " + personToConnectTo.Age.ToString() + ", " + personToConnectTo.Gender.name + ", " + term2.careerName + "-" + term2.specializationName + "):" + notes + "\n";
      term1.Notes += str;
      this.Notes = this.Notes + str + "\n";
    }

    public virtual void removeAlly(string ally)
    {
      this.removeEntityIKnow(ally, Contact.ContactType.Ally);
    }

    public virtual void removeContact(string contact)
    {
      this.removeEntityIKnow(contact, Contact.ContactType.Contact);
    }

    public virtual void removeEnemy(string enemy)
    {
      this.removeEntityIKnow(enemy, Contact.ContactType.Enemy);
    }

    public virtual void removeEntityIKnow(Contact removeMe) => this.EntityIKnow.Remove(removeMe);

    public virtual void removeEntityIKnow(string name, Contact.ContactType type)
    {
      this.EntityIKnow.RemoveAll((Predicate<Contact>) (x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && x.Type == type));
    }

    public virtual void removeEquipment(IEquipment removeMe)
    {
      if (removeMe == null)
        EngineLog.Warning("Asked to remove a null piece of equipment.  Ignoring.");
      else if (removeMe.InstanceID == Guid.Empty)
      {
        EngineLog.Warning("Asked to remove a piece of equipment with no InstanceID.  Ignoring.");
      }
      else
      {
        Guid id = removeMe.Id;
        if (id.ToString() == "16f7e066-579a-41b3-a5d3-792352cb3067")
        {
          if (this.equipmentQuantity(removeMe) <= 0)
            return;
          ClanShares clanShares = (ClanShares) this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString() == "16f7e066-579a-41b3-a5d3-792352cb3067")).First<IEquipment>();
          clanShares.Number -= (removeMe as ClanShares).Number;
          if (clanShares.Number <= 0)
            this._equipment.Remove((IEquipment) clanShares);
        }
        else
        {
          id = removeMe.Id;
          if (id.ToString() == "0a485bb4-83c2-4b5f-8e7f-e88115a85f96")
          {
            if (this.equipmentQuantity(removeMe) <= 0)
              return;
            com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares shipShares = (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares) this._equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e.Id.ToString() == "0a485bb4-83c2-4b5f-8e7f-e88115a85f96")).First<IEquipment>();
            shipShares.Num -= (removeMe as com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares).Num;
            if (shipShares.Num <= 0)
              this._equipment.Remove((IEquipment) shipShares);
          }
          else
          {
            IEquipment dropMe = (IEquipment) null;
            EngineLog.Error("Character.removeEquipment(" + removeMe.Name + "]) - removing item with no number_of_items entry.  Defaulting to seraching for the item and removing it.");
            dropMe = this._equipment.FirstOrDefault<IEquipment>((Func<IEquipment, bool>) (item => item.Id.Equals(removeMe.Id) && item.InstanceID.Equals(removeMe.InstanceID)));
            if (dropMe == null)
            {
              string name = removeMe.Name;
              id = removeMe.Id;
              string str = id.ToString();
              EngineLog.Error(name + " GUID[" + str + "] didn't have a valid GUID or couldn't be found by GUID.  Removing by Name");
              dropMe = this._equipment.FirstOrDefault<IEquipment>((Func<IEquipment, bool>) (item => item.Name == removeMe.Name));
            }
            if (dropMe != null)
            {
              if (dropMe is Software)
              {
                foreach (IComputer computer in (IEnumerable<IComputer>) this.getComputers())
                  computer.UnloadSoftware((Software) dropMe);
              }
              if (dropMe is IEquipmentOption)
              {
                IList<IUpgradable> list = (IList<IUpgradable>) this.Equipment.Where<IEquipment>((Func<IEquipment, bool>) (e => e is IUpgradable && ((IUpgradable) e).Options.Contains((IEquipmentOption) dropMe))).Select<IEquipment, IUpgradable>((Func<IEquipment, IUpgradable>) (x => x as IUpgradable)).ToList<IUpgradable>();
                if (list != null && list.Any<IUpgradable>())
                {
                  foreach (IUpgradable upgradable in (IEnumerable<IUpgradable>) list)
                    upgradable.RemoveOption((IEquipmentOption) dropMe);
                }
              }
              else if (this._equippedIds.Contains(dropMe.InstanceID))
                this.UnEquip(dropMe);
              this._equipment.Remove(dropMe);
            }
            else
              EngineLog.Error("Unable to remove " + removeMe.Name);
          }
        }
      }
    }

    public virtual void removeRival(string rival)
    {
      this.removeEntityIKnow(rival, Contact.ContactType.Rival);
    }

    public virtual void removeSkill(ISkill removeMe)
    {
      this.skills.Remove(removeMe.Name.ToLower().Replace(" ", ""));
      if (!removeMe.Cascade)
        return;
      foreach (IDescribable specializationSkill in (List<ISkill>) removeMe.SpecializationSkills)
        this.skills.Remove(specializationSkill.Name.ToLower().Replace(" ", ""));
    }

    public virtual void setAttribute(string name, int newValue)
    {
      name = name.ToLowerInvariant();
      int ordinal = -1;
      for (int index = 0; index < this._attributes.Count<Attribute>(); ++index)
      {
        if (this._attributes[index].Name.ToLowerInvariant() == name)
        {
          ordinal = index;
          break;
        }
      }
      if (ordinal < 0)
        throw new ArgumentException(name + " couldn't be found in the set of the characters attribtues.");
      this.setAttribute(ordinal, newValue);
    }

    public virtual void setAttribute(int ordinal, int newValue)
    {
      EngineLog.Warning("setting attribute by ordinal is highly discouraged as it can be dynamic array");
      if (ordinal < 0 || ordinal >= this._attributes.Count)
        throw new Exception("Trying to get characteristic #" + ordinal.ToString() + " but it's out of range.");
      this._attributes[ordinal].Value = newValue;
    }

    public virtual void setAttributes(List<Attribute> newAttributes)
    {
      EngineLog.Print(">>> SETTING attirbutes for Character: " + newAttributes.Count.ToString());
      this._attributes = newAttributes;
    }

    public virtual void setDrafted() => this._drafted = true;

    public virtual void startNewTerm(Career newCareer, Career.Specialization withSpecialization = null)
    {
      Term term = new Term() { careerName = newCareer.Name };
      if (withSpecialization != null)
        term.specializationName = withSpecialization.Name;
      if (this._drafted && this._firstTermDrafted)
      {
        term.drafted = true;
        this._firstTermDrafted = false;
      }
      if (this.haveHadCareer(newCareer.Name))
      {
        Term previousTermOfCareer = this.getPreviousTermOfCareer(newCareer.Name);
        term.rank = previousTermOfCareer != null ? previousTermOfCareer.rank : throw new Exception("We were supposed to have had the career [" + newCareer.Name + "] before, but we couldn't find it.");
        term.officer = previousTermOfCareer.officer;
        term.total_ranks_in_career = previousTermOfCareer.total_ranks_in_career;
        term.title = previousTermOfCareer.title;
        term.numberOfTermsInCareerIncludingThisOne = previousTermOfCareer.numberOfTermsInCareerIncludingThisOne + 1;
        term.pension_paid_out_for_this_career = previousTermOfCareer.pension_paid_out_for_this_career;
        term.survivalModifier = previousTermOfCareer.survivalModifier;
      }
      else
      {
        if (!newCareer.Name.Equals("Primary Education", StringComparison.CurrentCultureIgnoreCase) && !(newCareer is PreCareer))
          ++this.NumberOfCareers;
        else if (newCareer is PreCareer)
          this.HadPreCareer = true;
        term.numberOfTermsInCareerIncludingThisOne = 1;
      }
      term.term = this.CurrentTerm != null ? this.CurrentTerm.term + 1 : 0;
      this.CareerHistory.Add(term);
    }

    public override string ToString() => new CharacterStringWriter().write(this);

    public bool trainSkill(Skill skill)
    {
      bool flag = false;
      string lowerInvariant = skill.Name.ToLowerInvariant();
      if (!lowerInvariant.Contains("jack") && !this._skillsBeingTrained.Keys.Contains<string>(lowerInvariant))
      {
        int desiredSkillLevel = this.hasSkill((ISkill) skill) ? this.getSkill((ISkill) skill).Level + 1 : 0;
        this._skillsBeingTrained[lowerInvariant] = new SkillTraining(skill, desiredSkillLevel, desiredSkillLevel - 1);
        flag = true;
      }
      return flag;
    }

    public void UnEquip(IEquipment toRemove)
    {
      if (!this._equippedIds.Contains(toRemove.InstanceID))
        return;
      this._equippedIds.Remove(toRemove.InstanceID);
      if (!(toRemove is PoweredArmor))
        this.EquippedMass -= toRemove is Armor ? toRemove.Weight * 0.25 : toRemove.Weight;
      if (this.EquippedMass < 0.0 || !this._equippedIds.Any<Guid>())
        this.EquippedMass = 0.0;
      if (toRemove is Armor)
        this.unEquipArmor((Armor) toRemove);
      if (toRemove is IUpgradable)
        ((IUpgradable) toRemove).WeaponSubcomponents.ForEach((Action<IWeapon>) (w => this.UnEquip((IEquipment) w)));
      if (toRemove is ICharacterModifier)
        ((IModifier<Character>) toRemove).ActionsOnRemoval(this);
    }

    public Character(int creatingUser)
    {
      this.EntityIKnow = new List<Contact>();
      this.Journal = new List<JournalEntry>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.CreatingUser = creatingUser;
      this.CareerHistory = new List<Term>();
      this.Equipped = (IList<IEquipment>) new List<IEquipment>();
      this.CashRolls = 0;
      this.NumberOfCareers = 0;
      this.Age = 18;
      this.Race = Race.StandardHuman;
      this.Gender = Race.StandardHuman.Genders[0];
      this.Name = "Nameless Traveller";
      this.States = new List<ManualHealthState>();
      this.CurrentDate = new GameDate()
      {
        ImperialDay = 1,
        ImperialYear = 1105
      };
      this.Id = Guid.NewGuid();
      this.Avatar = new AvatarInfo();
    }

    [JsonConstructor]
    public Character(Dictionary<string, Skill> oldskills, int creatingid)
      : this(creatingid)
    {
      this.skills = new SkillDictionary();
      if (oldskills == null)
        return;
      foreach (KeyValuePair<string, Skill> oldskill in oldskills)
        this.skills.Add(oldskill.Key.ToLower().Replace(" ", ""), (ISkill) oldskill.Value);
    }

    protected internal virtual void initiateCommission(Career career)
    {
      Term currentTerm = this.CurrentTerm;
      if (currentTerm.officer || !career.hasCommissions())
        return;
      currentTerm.officer = true;
      currentTerm.rank = 1;
    }

    private void addSkillToDictionary<T>(T addMe, Dictionary<string, T> dictionary) where T : IAddable<T>
    {
      string key = addMe.Name.ToLower().Replace(" ", "");
      if (dictionary.ContainsKey(key))
        dictionary[key].Add(addMe);
      else
        dictionary[key] = addMe;
    }

    private Attribute getAttributeByName(string ofName)
    {
      if (string.IsNullOrEmpty(ofName) || ofName == "")
      {
        EngineLog.Warning("Attribute reqested was " + ofName);
        return (Attribute) null;
      }
      Attribute attributeByName = (Attribute) null;
      if (this._attributes != null)
      {
        foreach (Attribute attribute in this._attributes)
        {
          if (attribute.Name.Equals(ofName, StringComparison.InvariantCultureIgnoreCase))
          {
            attributeByName = attribute;
            break;
          }
        }
        if (attributeByName == null)
        {
          string str = "Character.getAttributeByName(" + ofName + ") there is no such attribute. Available are: ";
          foreach (Attribute attribute in this._attributes)
            str = str + " " + attribute.Name + ", ";
          EngineLog.Warning(str + " Trying to figure out by ordinal");
          int canonicalOrdinalForStat = Attribute.GetCanonicalOrdinalForStat(ofName);
          if (canonicalOrdinalForStat >= 0)
          {
            attributeByName = this._attributes[canonicalOrdinalForStat];
            EngineLog.Warning("Stat found by Ordinal: [" + attributeByName?.ToString() + "]");
          }
        }
      }
      else
        EngineLog.Error("accessing attributes of character before they are instantiated");
      return attributeByName;
    }

    private T getEntity<T>(string name, Dictionary<string, T> dictionary)
    {
      T entity;
      if (!dictionary.TryGetValue(name, out entity))
      {
        name = name.ToLower().Replace(" ", "");
        foreach (KeyValuePair<string, T> keyValuePair in dictionary)
        {
          if (keyValuePair.Key.ToLower().Replace(" ", "") == name)
          {
            entity = keyValuePair.Value;
            break;
          }
        }
      }
      return entity;
    }

    public void convertToNewDataModel()
    {
      EngineLog.Print("Refreshing Equipment [" + this.Name + "] " + this.Id.ToString());
      if (this._equipment.Any<IEquipment>((Func<IEquipment, bool>) (e => e.InstanceID == Guid.Empty)))
      {
        for (int index = 0; index < this._equipment.Count<IEquipment>(); ++index)
        {
          IEquipment equipment = this._equipment[index];
          if (equipment.InstanceID == Guid.Empty)
          {
            IEquipment asset = DataManager.Instance.GetAsset<IEquipment>(equipment.Id, true);
            this._equipment.RemoveAt(index);
            if (asset != null)
            {
              this._equipment.Insert(index, asset);
              if (asset is IMultishotRangedWeapon && !((IConsumer) asset).UnlimitedAmmunition)
              {
                IMultishotRangedWeapon multishotRangedWeapon = asset as IMultishotRangedWeapon;
                Ammunition ammunition = new Ammunition();
                ammunition.Name = "STANDARD CLIP [" + multishotRangedWeapon.Name + "]";
                ammunition.Description = "The standard ammunition for a " + multishotRangedWeapon.Description;
                ammunition.MaxAmount = multishotRangedWeapon.Capacity;
                ammunition.CurrentAmount = multishotRangedWeapon.Capacity;
                ammunition.AmountName = multishotRangedWeapon.ConsumableAmountName;
                ammunition.Cost = multishotRangedWeapon.StandardConsumableCost;
              }
            }
            else
              EngineLog.Error("Unable to Upgrade Equipment: " + equipment.Id.ToString() + ": " + equipment.Name);
          }
        }
      }
      EngineLog.Print("Refreshing Talents [" + this.Name + "] " + this.Id.ToString());
      for (int index = 0; index < this.talents.Count; ++index)
      {
        foreach (string key in new List<string>((IEnumerable<string>) this.talents.Keys))
        {
          Talent asset = DataManager.Instance.GetAsset<Talent>(this.talents[key].Id);
          if (asset == null)
          {
            EngineLog.Error("DataManager is missing Talent " + key);
          }
          else
          {
            asset.Level = this.talents[key].Level;
            this.talents[key] = asset;
          }
        }
      }
    }
  }
}
