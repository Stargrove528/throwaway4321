
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IComputer




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IComputer : 
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IUpgradable
  {
    int Rating { get; set; }

    int AvailableRating { get; }

    List<Software> RunningSoftware { get; }

    bool CanRun(Software canIRun);

    int MaxRunnableRating(Software whatRatingCanIRunAt);

    bool LoadSoftware(Software runMe);

    void UnloadSoftware(Software unloadMe);
  }
}
