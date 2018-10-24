using CroweCodingTest.Services;
using CroweCodingTest.Services.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.CroweCodingTest
{
  /// <summary>
  /// Tests for testing the "Services" methods
  /// </summary>
  [TestClass]
  public class ServicesTests
  {
    [TestMethod]
    public void CorpMessaging_GetMessage_Method_Test()
    {
      ICorpMessaging messaging = new CorpMessaging();

      var messageRetrieved = messaging.GetMessage(MessageConstants.HELLO_WORLD);
      Assert.AreEqual(MessageConstants.HELLO_WORLD, messageRetrieved, 
        @"Expected messages to be equal"
      );
    }
  }
}
