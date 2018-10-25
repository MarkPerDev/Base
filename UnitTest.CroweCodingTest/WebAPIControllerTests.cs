using CroweCodingTest.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UnitTest.CroweCodingTest
{
  [TestClass]
  public class WebAPIControllerTests
  {
   
    [TestMethod]
    public void API_Get_NoIdParam_Test()
    {
      var controller = new MessagingController();
      var response = controller.Get();
      Assert.IsTrue(response != null && response.Contains(MessageConstants.HELLO_WORLD_MSG_ENG));
    }

    [TestMethod]
    public void API_Get_IdParam_English_Test()
    {
      var controller = new MessagingController();
      var response = controller.Get(1);
      Assert.IsTrue(response != null && response.Contains(MessageConstants.HELLO_WORLD_MSG_ENG));
    }

    [TestMethod]
    public void API_Get_IdParam_Spanish_Test()
    {
      var controller = new MessagingController();
      var response = controller.Get(2);
      Assert.IsTrue(response != null && response.Contains(MessageConstants.HELLO_WORLD_MSG_SPANISH));
    }

    [TestMethod]
    public void API_Get_IdParam_French_Test()
    {
      var controller = new MessagingController();
      var response = controller.Get(3);
      Assert.IsTrue(response != null && response.Contains(MessageConstants.HELLO_WORLD_MSG_FRENCH));
    }

    [TestMethod]
    public void API_Get_IdParam_NotFound_Test()
    {
      var controller = new MessagingController();
      var response = controller.Get(0);
      Assert.IsTrue(response != null && response.Contains(MessageConstants.NOT_FOUND_MSG));
    }
  }
}
