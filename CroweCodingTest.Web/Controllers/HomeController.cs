using CroweCodingTest.Web.Models;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;

namespace CroweCodingTest.Web.Controllers
{
  public class HomeController : Controller
  {

    Message _message = new Message();

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
    /// <summary>
    /// Gets the data via web api call
    /// </summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      GetMessages();
      return View(_message);
    }

    public ActionResult Console()
    {
      var stringToAppend = @"CroweCodingTest.ConsoleApplication\bin\Debug\CroweCodingTest.ConsoleApplication.exe";
      var stringToReplace = @"CroweCodingTest.Web\";
      string curPath = AppDomain.CurrentDomain.BaseDirectory;
      curPath = curPath.Replace(stringToReplace, stringToAppend);
      Process.Start(curPath);

      return View();
    }

    private void GetMessages()
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
          _message.message = readTask.Result;
        }
        else
        {
          _message = null;
          ModelState.AddModelError(string.Empty, "Web API Error...");
        }
      }
    }
  }
}