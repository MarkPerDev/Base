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
    #region Fields
    protected static HttpClient _client = null;
    protected static string _resource = string.Empty;

    #endregion Fields

    #region Properties

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext { get; set; }

    private string _apiPath = null;
    public string ApiPath
    {
      get
      {
        if (_apiPath == null)
        {
          _apiPath = ConfigurationManager.AppSettings["apiPath"];
        }
        return _apiPath;
      }
    }

    #endregion Properties

    #region Additional test attributes

    [ClassInitialize()]
    public static void ClassInitialize(TestContext testContext)
    {
      var handler = new HttpClientHandler()
      {
        UseDefaultCredentials = true
      };

      _client = new HttpClient(handler)
      {
        BaseAddress = new Uri(ConfigurationManager.AppSettings["webApiClientUri"])
      };

      // Add an Accept header for JSON or XML format.
      _client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue(ConfigurationManager.AppSettings["webApiMediaType"])
       );

      _client.DefaultRequestHeaders.From = @"webApiUnitTest@MarkPerDev.com"; // Fake email...
    }

    [ClassCleanup()]
    public static void ClassCleanup()
    {
      if (_client != null)
        _client.Dispose();
    }
    #endregion Additional Test Attributes


    [TestMethod]
    public void Messaging_API_Get_Test()
    {
      // Get the Web API "Get" result
      var response = _client.GetAsync(ApiPath);
      response.Wait();
      response.Result.EnsureSuccessStatusCode();

      var list = response.Result.Content.ReadAsStringAsync().Result;
      Assert.IsTrue(list != null && list.Contains(MessageConstants.HELLO_WORLD));
    }
  }
}
