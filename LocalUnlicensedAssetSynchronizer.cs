
// Type: com.digitalarcsystems.Traveller.LocalUnlicensedAssetSynchronizer




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class LocalUnlicensedAssetSynchronizer
  {
    public static void synchronize(
      string data_cache_location,
      Dictionary<Guid, AssetMetadata> cache,
      DataManager dataManager)
    {
      try
      {
        if (!Directory.Exists(data_cache_location))
          return;
        List<IAsset> list1 = dataManager.Characters.Select<Character, IAsset>((Func<Character, IAsset>) (c => (IAsset) c)).ToList<IAsset>();
        List<IAsset> list2 = dataManager.GenerationStates.Select<GenerationState, IAsset>((Func<GenerationState, IAsset>) (gs => (IAsset) gs)).ToList<IAsset>();
        List<AssetMetadata> assetMetadataList = new List<AssetMetadata>();
        assetMetadataList.AddRange((IEnumerable<AssetMetadata>) LocalUnlicensedAssetSynchronizer.ObtainAssetsFromDirectory<Character>(".tchjson", list1, data_cache_location, dataManager));
        assetMetadataList.AddRange((IEnumerable<AssetMetadata>) LocalUnlicensedAssetSynchronizer.ObtainAssetsFromDirectory<GenerationState>(".tgsjson", list2, data_cache_location, dataManager));
        foreach (AssetMetadata assetMetadata in assetMetadataList)
        {
          cache.Add(assetMetadata.TheAsset.Id, assetMetadata);
          EngineLog.Warning("Restored Lost Asset [" + assetMetadata.TheAsset.Id.ToString() + "]");
          Console.WriteLine("Restored Lost Asset [" + assetMetadata.TheAsset.Id.ToString() + "]");
        }
      }
      catch (Exception ex)
      {
        EngineLog.Error("While trying to restore characters received this error: " + ex.Message);
      }
    }

    private static List<AssetMetadata> ObtainAssetsFromDirectory<T>(
      string defaultTargetExtension,
      List<IAsset> existingAssets,
      string data_cache_location,
      DataManager dataManager)
      where T : IAsset
    {
      List<string> list = ((IEnumerable<string>) Directory.GetFiles(data_cache_location)).Where<string>((Func<string, bool>) (fn => fn.EndsWith(defaultTargetExtension) && LocalUnlicensedAssetSynchronizer.FileNameParsesOutValidGuid(fn))).ToList<string>().Where<string>((Func<string, bool>) (tfn => !existingAssets.Any<IAsset>((Func<IAsset, bool>) (cc => cc.Id == LocalUnlicensedAssetSynchronizer.ParseGuidFromFileName(tfn))))).ToList<string>();
      List<AssetMetadata> assetsFromDirectory = new List<AssetMetadata>();
      if (list != null && list.Any<string>())
      {
        foreach (string str in list)
        {
          try
          {
            IAsset instance = (IAsset) dataManager.Load<T>(str);
            AssetMetadata assetMetadata = new AssetMetadata()
            {
              TheAsset = (IAssetBase) instance,
              LastModified = File.GetLastAccessTime(str),
              Location = str,
              StoredHash = instance.SHA1<IAsset>()
            };
            assetsFromDirectory.Add(assetMetadata);
          }
          catch (Exception ex)
          {
            EngineLog.Error("Unable to recover NLA: " + str);
          }
        }
      }
      return assetsFromDirectory;
    }

    private static Guid ParseGuidFromFileName(string fileName)
    {
      Guid guid = new Guid();
      string fileName1 = Path.GetFileName(fileName);
      Guid guidFromFileName = new Guid(fileName1.Contains(".") ? fileName1.Substring(0, fileName1.LastIndexOf(".")) : fileName1);
      if (guidFromFileName.Equals((object) null))
        guidFromFileName = guid;
      return guidFromFileName;
    }

    private static bool FileNameParsesOutValidGuid(string fileName)
    {
      return LocalUnlicensedAssetSynchronizer.ParseGuidFromFileName(fileName) != Guid.Empty;
    }
  }
}
