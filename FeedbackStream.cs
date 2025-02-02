
// Type: com.digitalarcsystems.Traveller.FeedbackStream




#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public static class FeedbackStream
  {
    public static event FeedbackStream.EngineFeedbackDelegate onFeedbackProvided;

    public static void Send(string message)
    {
      if (FeedbackStream.onFeedbackProvided != null)
      {
        FeedbackStream.onFeedbackProvided(message);
        EngineLog.Print("FEEDBACK from engine: " + message);
      }
      else
        EngineLog.Warning("Feedback from engine (no listeners): " + message);
    }

    public delegate void EngineFeedbackDelegate(string message);
  }
}
