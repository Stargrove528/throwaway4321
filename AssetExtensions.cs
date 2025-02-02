
// Type: com.digitalarcsystems.Traveller.DataModel.AssetExtensions




using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public static class AssetExtensions
  {
    public static void AddChild(this IAsset asset, IAsset childAsset)
    {
      if (asset.ChildAssets == null)
        asset.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      AssetMetadata assetMetadata = new AssetMetadata()
      {
        LastModified = DateTime.UtcNow,
        StoredHash = childAsset.SHA1<IAsset>(),
        TheAsset = (IAssetBase) childAsset
      };
      if (asset.ChildAssets == null)
        asset.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      asset.ChildAssets.Add(childAsset.Id, assetMetadata);
    }

    public static void AddNonLicensedBinaryAsset(
      this IAsset asset,
      byte[] contents,
      params AssetTag[] tags)
    {
      asset.AddChild((IAsset) NonLicensedBinaryAsset.CreateFrom(contents, DataManager.UserID, tags));
    }

    public static IEnumerable<T> GetChildren<T>(this IAsset asset, Func<T, bool> func = null) where T : IAsset
    {
      if (asset.ChildAssets == null || !asset.ChildAssets.Any<KeyValuePair<Guid, AssetMetadata>>())
        return Enumerable.Empty<T>();
      return func != null ? asset.ChildAssets.Values.Where<AssetMetadata>((Func<AssetMetadata, bool>) (child => child.TheAsset is T && func((T) child.TheAsset))).Select<AssetMetadata, T>((Func<AssetMetadata, T>) (child => (T) child.TheAsset)) : asset.ChildAssets.Values.Select<AssetMetadata, T>((Func<AssetMetadata, T>) (c => (T) c.TheAsset));
    }

    public static IEnumerable<T> GetChildren<T>(this IAsset asset, params AssetTag[] tags) where T : IAsset
    {
      if (asset.ChildAssets == null || !asset.ChildAssets.Any<KeyValuePair<Guid, AssetMetadata>>())
        return Enumerable.Empty<T>();
      List<T> list = asset.ChildAssets.Values.Where<AssetMetadata>((Func<AssetMetadata, bool>) (a => a.TheAsset is T)).Select<AssetMetadata, T>((Func<AssetMetadata, T>) (a => (T) a.TheAsset)).ToList<T>();
      for (int index = 0; index < list.Count; ++index)
      {
        for (int y = 0; y < tags.Length; y++)
        {
          T obj = list[index];
          if (!obj.Tags.Any<AssetTag>((Func<AssetTag, bool>) (t => t.Equals((object) tags[y]))))
          {
            list.Remove(obj);
            --index;
            break;
          }
        }
      }
      return (IEnumerable<T>) list;
    }

    public static void RemoveChild(this IAsset asset, IAsset toRemove)
    {
      if (asset.ChildAssets == null || !asset.ChildAssets.Any<KeyValuePair<Guid, AssetMetadata>>() || !asset.ChildAssets.ContainsKey(toRemove.Id))
        return;
      asset.ChildAssets.Remove(toRemove.Id);
    }

    public static void RemoveChildren<T>(this IAsset asset, Func<T, bool> func) where T : IAsset
    {
      if (asset.ChildAssets == null || !asset.ChildAssets.Any<KeyValuePair<Guid, AssetMetadata>>())
        return;
      foreach (T obj in asset.ChildAssets.Values.Where<AssetMetadata>((Func<AssetMetadata, bool>) (c => func((T) c.TheAsset))).Select<AssetMetadata, T>((Func<AssetMetadata, T>) (c => (T) c.TheAsset)).ToList<T>())
        asset.ChildAssets.Remove(obj.Id);
    }

    public static void UpdateChild(this IAsset asset, IAsset toUpdate)
    {
      if (asset.ChildAssets == null || !asset.ChildAssets.Any<KeyValuePair<Guid, AssetMetadata>>() || !asset.ChildAssets.ContainsKey(toUpdate.Id))
        return;
      asset.ChildAssets[toUpdate.Id] = new AssetMetadata()
      {
        LastModified = DateTime.UtcNow,
        StoredHash = toUpdate.SHA1<IAsset>(),
        TheAsset = (IAssetBase) toUpdate
      };
    }

    public static void AddTag(this IAsset asset, AssetTag newTag)
    {
      if (asset.Tags == null)
        asset.Tags = new List<AssetTag>();
      asset.Tags.AddDistinct<AssetTag>(newTag);
    }

    public static void RemoveTag(this IAsset asset, AssetTag oldTag)
    {
      if (asset.Tags == null)
        return;
      asset.Tags.Remove(oldTag);
    }
  }
}
