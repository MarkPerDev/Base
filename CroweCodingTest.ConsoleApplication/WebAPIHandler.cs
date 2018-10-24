using System;
using System.Configuration;
using System.Net.Http;

namespace CroweCodingTest.ConsoleApplication
{
  public class WebAPIHandler : IWebAPIHandler
  {
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

    private string _baseUri = null;
    public string BaseUri
    {
      get
      {
        if (_baseUri == null)
        {
          _baseUri = ConfigurationManager.AppSettings["webApiBaseUri"];
        }
        return _baseUri;
      }
    }

    public void GetWebAPIMessages()
    {
      using (var client = new HttpClient())
      {
        client.BaseAddress = new Uri(BaseUri);
        var response = client.GetAsync(ApiPath);
        response.Wait();

        var result = response.Result;
        if (result.IsSuccessStatusCode)
        {
          var readTask = result.Content.ReadAsStringAsync();
          readTask.Wait();
          Console.WriteLine(readTask.Result);
          Console.ReadKey();
        }
        else
        {
          Console.WriteLine("Error...");
          Console.ReadKey();
        }
      }
    }
  }
}
