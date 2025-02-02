
// Type: com.digitalarcsystems.Traveller.DataManagerErrorEventArgs




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class DataManagerErrorEventArgs : EventArgs
  {
    public string Error { get; set; }

    public DataManagerErrorEventArgs(string error) => this.Error = error;
  }
}
