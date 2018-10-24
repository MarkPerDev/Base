using CroweCodingTest.Services.Interface;

namespace CroweCodingTest.Services
{
	public class CorpMessaging : ICorpMessaging
	{
    /// <summary>
    /// Used for retrieving messages. Accepts param
    /// <paramref name="messageToSend"/>.
    /// </summary>
    public string GetMessage(string messageToSend)
    {
      return messageToSend;
    }
  }
}
