
// Type: com.digitalarcsystems.Traveller.IDataManager




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IDataManager
  {
    IList<Career> Careers { get; }

    IList<Character> Characters { get; }

    IList<Race> Races { get; }

    IList<IEquipment> Equipment { get; }

    IList<Contact> EntitiesToMeet { get; }

    IList<ISkill> Skills { get; }

    IList<Talent> Talents { get; }

    IList<World> Worlds { get; }

    IEnumerable<T> GetAsset<T>(Func<T, bool> func, bool isClone) where T : IAsset;
  }
}
