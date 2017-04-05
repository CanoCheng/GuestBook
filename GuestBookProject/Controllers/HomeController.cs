using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuestBookProject.Service;

namespace GuestBookProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TestConnectionData test = new TestConnectionData();
            test.Connecting();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}