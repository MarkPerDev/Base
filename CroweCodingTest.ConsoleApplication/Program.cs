using CroweCodingTest.Services;
using CroweCodingTest.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CroweCodingTest.ConsoleApplication
{
  class Program
  {
    static void Main(string[] args)
    {
      IWebAPIHandler webAPIHandler = new WebAPIHandler();
      webAPIHandler.GetWebAPIMessages();
    }
  }
}
