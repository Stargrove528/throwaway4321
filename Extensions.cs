
// Type: com.digitalarcsystems.Traveller.Extensions




using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGSuiteCloud;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace com.digitalarcsystems.Traveller
{
  public static class Extensions
  {
    private static volatile 
    #nullable disable
    object _serializeLock = new object();
    private static readonly JsonSerializerSettings JsonSerializerSettings;

    public static void AddDistinct<T>(this List<T> list, T item)
    {
      if (!list.Contains(item))
        list.Add(item);
      IEnumerable<T> collection = list.Distinct<T>();
      list.Clear();
      list.AddRange(collection);
    }

    public static byte[] AsByteArray(this object instance)
    {
      return instance is LicensedBinaryAsset ? ((LicensedBinaryAsset) instance).AsByteArray() : instance.AsSerialized().AsBytes();
    }

    public static byte[] AsBytes(this string instance) => Encoding.UTF8.GetBytes(instance);

    public static async Task<T> AsDeserializedWithTimeout<T>(this string instance, int timeout)
    {
      T item = default (T);
      CancellationTokenSource cts = new CancellationTokenSource();
      cts.CancelAfter(timeout);
      try
      {
        T obj = await Task.Run<T>((Func<T>) (() => JsonConvert.DeserializeObject<T>(instance)), cts.Token);
        item = obj;
        obj = default (T);
      }
      catch (OperationCanceledException ex)
      {
        Console.WriteLine("Deserialization timed out.");
      }
      catch (Exception ex)
      {
        JObject result = JObject.Parse(instance);
        Console.WriteLine(string.Format("Caught exception of type {0} on deserialization: {1}{2}", (object) ex.GetType(), (object) ex.Message, ex.InnerException == null ? (object) "" : (object) ("\n" + ex.InnerException?.ToString())));
        result = (JObject) null;
      }
      T obj1 = item;
      item = default (T);
      cts = (CancellationTokenSource) null;
      return obj1;
    }

    public static byte[] AsReadyToUpload(this IAssetBase instance)
    {
      return instance.AsByteArray().Compress();
    }

    public static string AsSerialized(this object instance)
    {
      if (instance is ReadOnlySkillAdapter)
        return (instance as ReadOnlySkillAdapter).AsSerialized();
      lock (Extensions._serializeLock)
      {
        string str;
        try
        {
          str = JsonConvert.SerializeObject(instance, Formatting.None, Extensions.JsonSerializerSettings);
        }
        catch (Exception ex)
        {
          Extensions.Log(string.Format("caught exception of type {0} on deserialization: {1}{2}", (object) ex.GetType(), (object) ex.Message, ex.InnerException == null ? (object) "" : (object) ("\n" + ex.InnerException?.ToString())), true);
          return string.Empty;
        }
        if (string.IsNullOrEmpty(str))
          Extensions.Log("serialization returned an empty string.");
        return str;
      }
    }

    public static void AsSerializedAsync(this object instance, Action<string> onCompleted)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => onCompleted(instance.AsSerialized())));
    }

    public static T Clone<T>(this T instance)
    {
      if ((object) instance is IAsset)
      {
        Guid id = ((IAssetBase) (object) instance).Id;
      }
      return ((object) instance).AsSerialized().AsDeserialized<T>();
    }

    public static IEnumerable<T> Clone<T>(this IEnumerable<T> list)
    {
      return list.Select<T, T>((Func<T, T>) (x => x.Clone<T>()));
    }

    public static T FromStream<T>(this Stream stream)
    {
      JsonSerializer jsonSerializer = JsonSerializer.Create(Extensions.JsonSerializerSettings);
      using (StreamReader reader1 = new StreamReader(stream))
      {
        using (JsonTextReader reader2 = new JsonTextReader((TextReader) reader1))
        {
          T obj = default (T);
          try
          {
            return jsonSerializer.Deserialize<T>((JsonReader) reader2);
          }
          catch (Exception ex)
          {
            Extensions.Log("Getting object from stream failed: " + ex.Message);
          }
          return obj;
        }
      }
    }

    public static string GetLineStartingAtIndex(this string text, int index)
    {
      string lineStartingAtIndex = "";
      int num = text.IndexOf("\n", index, StringComparison.Ordinal);
      if (num > 0)
        lineStartingAtIndex = text.Substring(index, num - index);
      return lineStartingAtIndex;
    }

    public static string SHA1(this byte[] obj)
    {
      using (SHA1Managed shA1Managed = new SHA1Managed())
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte num in shA1Managed.ComputeHash(obj))
          stringBuilder.Append(num.ToString("x2"));
        return stringBuilder.ToString().ToLowerInvariant();
      }
    }

    public static string SHA1<T>(this T instance) where T : IAssetBase
    {
      return instance.AsByteArray().SHA1();
    }

    public static string SHA1(this string inputString) => inputString.AsByteArray().SHA1();

    private static void Log(string msg, bool isThisWrong = false)
    {
      if (isThisWrong)
        EngineLog.Warning(msg);
      else
        EngineLog.Print(msg);
      Debug.WriteLine(msg);
      Console.WriteLine(msg);
    }

    static Extensions()
    {
      JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
      serializerSettings.NullValueHandling = NullValueHandling.Ignore;
      serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      serializerSettings.TypeNameHandling = TypeNameHandling.All;
      serializerSettings.CheckAdditionalContent = false;
      serializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
      serializerSettings.MaxDepth = new int?(32);
      serializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
      serializerSettings.Error = (EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>) ((sender, args) =>
      {
        Console.WriteLine("Error during deserialization: " + args.ErrorContext.Error.Message);
        args.ErrorContext.Handled = true;
      });
      serializerSettings.Converters.Add((JsonConverter) new JsonGuidConverter());
      Extensions.JsonSerializerSettings = serializerSettings;
    }
  }
}
