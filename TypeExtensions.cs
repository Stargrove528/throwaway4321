
// Type: com.digitalarcsystems.Traveller.TypeExtensions




using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public static class TypeExtensions
  {
    public static IEnumerable<System.Type> GetInterfacesAndSelf(this System.Type type)
    {
      if (type == (System.Type) null)
        throw new ArgumentNullException();
      if (!type.IsInterface)
        return (IEnumerable<System.Type>) type.GetInterfaces();
      return ((IEnumerable<System.Type>) new System.Type[1]{ type }).Concat<System.Type>((IEnumerable<System.Type>) type.GetInterfaces());
    }

    public static IEnumerable<System.Type[]> GetDictionaryKeyValueTypes(this System.Type type)
    {
      foreach (System.Type intType in type.GetInterfacesAndSelf())
      {
        if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof (IDictionary<,>))
          yield return intType.GetGenericArguments();
      }
    }
  }
}
