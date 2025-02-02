
// Type: com.digitalarcsystems.Traveller.CustomSerializationBinder




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class CustomSerializationBinder : DefaultSerializationBinder
  {
    public override System.Type BindToType(string assemblyName, string typeName)
    {
      switch (typeName)
      {
        case "System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[com.digitalarcsystems.Traveller.DataModel.Skill, TCG_CS_Engine]]":
          return typeof (SkillDictionary);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Armor":
          return typeof (Armor);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+ArmorOption":
          return typeof (ArmorOption);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Augmentation":
          return typeof (Augmentation);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Cash":
          return typeof (Cash);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Communication":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Communication);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Computer":
          return typeof (Computer);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MedicalSupplies":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.MedicalSupplies);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_Armor":
          return typeof (MusteringOutBenefit_Armor);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_Blade":
          return typeof (MusteringOutBenefit_Blade);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_Gun":
          return typeof (MusteringOutBenefit_Gun);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_PersonalVehicle":
          return typeof (MusteringOutBenefit_PersonalVehicle);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_ScientificEquipment":
          return typeof (MusteringOutBenefit_ScientificEquipment);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_Ship":
          return typeof (MusteringOutBenefit_Ship);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_SmallCraft":
          return typeof (MusteringOutBenefit_SmallCraft);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutBenefit_Weapon":
          return typeof (MusteringOutBenefit_Weapon);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+MusteringOutcomeBenefit_CombatImplant":
          return typeof (MusteringOutcomeBenefit_CombatImplant);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Nenj":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Nenj);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Sensors":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Sensors);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Ship":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Ship);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+ShipShares":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.ShipShares);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Software":
          return typeof (Software);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Survival":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Survival);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+TasMembership":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.TasMembership);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Toolkit":
          return typeof (com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment.Toolkit);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Vehicle":
          return typeof (Vehicle);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+Weapon":
          return typeof (Weapon);
        case "com.digitalarcsystems.Traveller.DataModel.Equipment+WeaponOption":
          return typeof (WeaponOption);
        default:
          try
          {
            return base.BindToType(assemblyName, typeName);
          }
          catch (JsonSerializationException ex)
          {
            Console.WriteLine(ex.StackTrace);
            Debugger.Break();
            return (System.Type) null;
          }
      }
    }

    public IList<System.Type> KnownTypes
    {
      get
      {
        return (IList<System.Type>) new List<System.Type>()
        {
          typeof (Skill),
          typeof (Dictionary<string, Skill>),
          typeof (List<Skill>)
        };
      }
    }
  }
}
