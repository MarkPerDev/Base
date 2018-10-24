namespace CroweCodingTest.Services.Interface
{
  public interface ICorpMessaging
   {
    /// <summary>
    /// Used for retrieving messages. Accepts param
    /// <paramref name="messageToSend"/>.
    /// </summary>
    string GetMessage(string messageToSend);
  }
}
