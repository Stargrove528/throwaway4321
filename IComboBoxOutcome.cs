
// Type: com.digitalarcsystems.Traveller.IComboBoxOutcome




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IComboBoxOutcome
  {
    IList<string> Choices { get; }

    string Selected { set; }
  }
}
