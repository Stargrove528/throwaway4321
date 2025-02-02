
// Type: com.digitalarcsystems.Traveller.DataModel.Race




using com.digitalarcsystems.Traveller.DataModel.Equipment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  [JsonObject(MemberSerialization.OptIn)]
  [Serializable]
  public sealed class Race : IAsset, IDescribable, IAssetBase, ILicensedAsset
  {
    [JsonProperty]
    private IWeapon _naturalAttack = (IWeapon) null;
    [JsonProperty]
    private Armor _naturalArmor = (Armor) null;
    [JsonProperty]
    private List<Skill> _racialSkills;
    [JsonProperty]
    private List<Outcome> _racialBenefits = new List<Outcome>();

    public static Race StandardHuman
    {
      get
      {
        return new Race("Human", "Modern humans (Homo sapiens) are the only extant members of the hominin clade, a branch of great apes characterized by erect posture and bipedal locomotion; manual dexterity and increased tool use; and a general trend toward larger, more complex brains and societies", "All stats are standard.", Race.CreateCanonicalStats(), new List<Skill>(), new List<Outcome>())
        {
          Genders = new List<Gender>()
          {
            new Gender("Male", Gender.PronounType.MALE),
            new Gender("Female", Gender.PronounType.FEMALE)
          },
          _naturalAttack = Race.MakeUnarmedAttack(),
          BaseMovementMeters = 6f
        };
      }
    }

    public static Race BlankRace
    {
      get
      {
        return new Race("", "", "", Race.CreateCanonicalStats(), new List<Skill>(), new List<Outcome>())
        {
          Genders = {
            new Gender("Other", Gender.PronounType.OTHER)
          },
          BaseMovementMeters = 6f,
          _naturalAttack = Race.MakeUnarmedAttack()
        };
      }
    }

    public Race(string raceName)
      : this(Race.BlankRace)
    {
      this.Name = raceName;
    }

    [JsonProperty]
    public string Name { get; set; }

    [JsonProperty]
    public string Description { get; set; }

    [JsonProperty]
    public string Notes { get; private set; }

    [JsonProperty]
    public List<Gender> Genders { get; internal set; }

    [JsonProperty]
    public float BaseMovementMeters { get; internal set; }

    public IWeapon naturalAttack
    {
      get
      {
        if (this._naturalAttack == null)
          this._naturalAttack = Race.MakeUnarmedAttack();
        return this._naturalAttack;
      }
      set => this._naturalAttack = value;
    }

    public Armor NaturalArmor
    {
      get => this._naturalArmor;
      set => this._naturalArmor = value;
    }

    private static IWeapon MakeUnarmedAttack()
    {
      Weapon weapon = new Weapon("Unarmed");
      weapon.Damage = 1;
      weapon.Skill = "Melee";
      weapon.SubSkill = "Unarmed";
      weapon.AllowedUpgradeCategories = new List<string>()
      {
        "unarmed, natural"
      };
      weapon.Id = new Guid("40A9DAF2-4635-4039-9100-3B3C6B1EA4C2");
      return (IWeapon) weapon;
    }

    public override string ToString() => this.AttribDebugText();

    [JsonConstructor]
    public Race()
    {
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      if (this.Genders != null && this.Genders.Count > 0)
        this.Genders = this.Genders.Distinct<Gender>().ToList<Gender>();
      if (this.Characteristics == null || this.Characteristics.Count <= 0)
        return;
      this.Characteristics = this.Characteristics.Distinct<Attribute>().ToList<Attribute>();
    }

    public Race(Race copyMe)
      : this()
    {
      this.Name = copyMe.Name;
      this.Description = copyMe.Description;
      this.Notes = copyMe.Notes;
      this._racialSkills = new List<Skill>((IEnumerable<Skill>) copyMe.RacialSkills);
      this._racialBenefits = new List<Outcome>((IEnumerable<Outcome>) copyMe.RacialBenefits);
      this.Genders = new List<Gender>((IEnumerable<Gender>) copyMe.Genders);
      this.Characteristics = copyMe.CharacteristicsCopy();
    }

    public Race(
      string name,
      string description,
      string notes,
      List<Attribute> characteristics,
      List<Skill> racialSkills)
      : this(name, description, notes, characteristics, racialSkills, (List<Outcome>) null)
    {
    }

    public Race(
      string name,
      string description,
      string notes,
      Gender gender,
      List<Attribute> characteristics,
      List<Skill> racialSkills,
      List<Outcome> racialBenefits)
      : this(name, description, notes, characteristics, racialSkills, racialBenefits)
    {
      this.Genders.AddDistinct<Gender>(gender);
    }

    public Race(
      string name,
      string description,
      string notes,
      List<Gender> genders,
      List<Attribute> characteristics,
      List<Skill> racialSkills,
      List<Outcome> racialBenefits)
      : this(name, description, notes, characteristics, racialSkills, racialBenefits)
    {
      foreach (Gender gender in genders)
        this.Genders.AddDistinct<Gender>(gender);
    }

    public Race(
      string name,
      string description,
      string notes,
      List<Attribute> characteristics,
      List<Skill> racialSkills,
      List<Outcome> racialBenefits)
      : this()
    {
      this.Name = name;
      this.Description = description;
      this.Notes = notes;
      this.Characteristics = characteristics.Count == 0 ? Race.CreateCanonicalStats() : characteristics;
      this.Characteristics = this.Characteristics.Distinct<Attribute>().ToList<Attribute>();
      this._racialSkills = racialSkills;
      this._racialBenefits = racialBenefits;
      this.Genders = new List<Gender>()
      {
        new Gender("Male", Gender.PronounType.MALE),
        new Gender("Female", Gender.PronounType.FEMALE)
      };
    }

    private static List<Attribute> CreateCanonicalStats()
    {
      int index = 0;
      return ((IEnumerable<string>) Attribute.CanonicalStats).Select<string, Attribute>((Func<string, Attribute>) (stat => new Attribute(stat, index++))).ToList<Attribute>().Distinct<Attribute>().ToList<Attribute>();
    }

    [JsonProperty]
    public List<Attribute> Characteristics { get; set; }

    public List<Attribute> CharacteristicsCopy()
    {
      return this.Characteristics.Select<Attribute, Attribute>((Func<Attribute, Attribute>) (characteristic => new Attribute(characteristic))).ToList<Attribute>().Distinct<Attribute>().ToList<Attribute>();
    }

    public string BonusesDebugText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Outcome racialBenefit in (IEnumerable<Outcome>) this.RacialBenefits)
        stringBuilder.Append(string.Format("+[{0}:{1}]\n", (object) racialBenefit.Name, (object) racialBenefit.Description));
      return stringBuilder.ToString();
    }

    public string AttribDebugText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Name + ":");
      foreach (Attribute characteristic in this.Characteristics)
        stringBuilder.Append(string.Format("[{0}:{1}] (+/- {2})  ", (object) characteristic.Name, (object) characteristic.Value, (object) characteristic.RacialBonus));
      return stringBuilder.ToString();
    }

    public IList<Skill> RacialSkills => (IList<Skill>) (this._racialSkills ?? new List<Skill>());

    public IList<Outcome> RacialBenefits
    {
      get => (IList<Outcome>) (this._racialBenefits ?? new List<Outcome>());
    }

    [JsonProperty]
    public Guid Id { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }
  }
}
