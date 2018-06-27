using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Pazzino.Database;

namespace Pazzino.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult UploadResumePostAdView()
        //{
        //    return View();
        //}

        public ActionResult RedirectTo(string Path)
        {
            return View(model:Path);
        }

        public ActionResult Index()
        {
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