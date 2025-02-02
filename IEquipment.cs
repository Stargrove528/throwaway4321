
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IEquipment




using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IEquipment : ICustomDescribable, IDescribable, IAsset, IAssetBase, ICategorizable
  {
    Guid InstanceID { get; set; }

    double Weight { get; set; }

    int TechLevel { get; set; }

    int Cost { get; set; }

    string Skill { get; set; }

    string SubSkill { get; set; }

    string PrimaryAttribute { get; set; }

    bool ModifiesSkillTask(string skillName, string statName);

    int BonusProvided(string skillName, int difficulty, int charactersSkillLevel, com.digitalarcsystems.Traveller.DataModel.Attribute stat);

    List<string> SkillTasksModified();

    bool UseEquipmentSkill(
      string skillName,
      int difficulty,
      int characterSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat);

    string Traits { get; set; }

    void AddTaskToModify(string skillName, int maxDifficulty, int bonus);

    void AddTaskToModify(string stat, int bonus);
  }
}
