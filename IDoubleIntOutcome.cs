
// Type: com.digitalarcsystems.Traveller.IDoubleIntOutcome




#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IDoubleIntOutcome : ISingleIntOutcome
  {
    int SecondInt { set; get; }

    string SecondIntLabel { get; }
  }
}
