using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CroweCodingTest.API.Controllers
{
    public class APIHomeController : Controller
    {
        // GET: APIHome
        public ActionResult Index()
        {
            return View();
        }
    }
}