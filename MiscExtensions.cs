
// Type: com.digitalarcsystems.Traveller.MiscExtensions




using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public static class MiscExtensions
  {
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
    {
      return source.Skip<T>(Math.Max(0, source.Count<T>() - N));
    }
  }
}
