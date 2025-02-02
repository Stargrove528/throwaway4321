
// Type: com.digitalarcsystems.Traveller.utility.FormsExtensions




using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility
{
  internal static class FormsExtensions
  {
    public static void AddAll<T>(this ICollection<T> t, IEnumerable<T> newItems)
    {
      foreach (T newItem in newItems)
        t.Add(newItem);
    }

    public static void AddAll<TKey, TValue>(
      this IDictionary<TKey, TValue> c,
      IDictionary<TKey, TValue> collectionToAdd)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) collectionToAdd)
        c[keyValuePair.Key] = keyValuePair.Value;
    }

    public static string JSubstring(this string str, int beginIndex, int endIndex)
    {
      return str.Substring(beginIndex, endIndex - beginIndex);
    }

    public static byte[] GetBytes(this string str) => Encoding.UTF8.GetBytes(str);

    public static byte[] GetBytes(this string str, Encoding encoding) => encoding.GetBytes(str);

    public static T[] ToArray<T>(this ICollection<T> col, T[] toArray)
    {
      int count = col.Count;
      T[] array;
      if (count <= toArray.Length)
      {
        col.CopyTo(toArray, 0);
        if (count != toArray.Length)
          toArray[count] = default (T);
        array = toArray;
      }
      else
      {
        array = new T[count];
        col.CopyTo(array, 0);
      }
      return array;
    }

    public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> col, TKey key)
    {
      TValue obj = default (TValue);
      if ((object) key != null)
        col.TryGetValue(key, out obj);
      return obj;
    }

    public static TValue Put<TKey, TValue>(
      this IDictionary<TKey, TValue> col,
      TKey key,
      TValue value)
    {
      TValue obj = col.Get<TKey, TValue>(key);
      col[key] = value;
      return obj;
    }

    public static TValue JRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
      TValue obj;
      dictionary.TryGetValue(key, out obj);
      dictionary.Remove(key);
      return obj;
    }

    public static T JRemoveAt<T>(this IList<T> list, int index)
    {
      T obj = list[index];
      list.RemoveAt(index);
      return obj;
    }

    public static void Write(this Stream stream, byte[] buffer)
    {
      stream.Write(buffer, 0, buffer.Length);
    }

    public static Assembly GetAssembly(this System.Type type) => type.Assembly;

    public static void DeleteCharAt(this StringBuilder builder, int index)
    {
      builder.Remove(index, 1);
    }

    public static bool EqualsIgnoreCase(this string str, string anotherString)
    {
      return string.Equals(str, anotherString, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> c) => c == null || c.Count == 0;
  }
}
