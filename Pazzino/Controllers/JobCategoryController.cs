using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pazzino.Models;

namespace Pazzino.Controllers
{
    public class JobCategoryController : Controller
    {
        // GET: JobCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetJobCategories(string text)
        {
            return Json(new JobCategory().GetCategories(text), JsonRequestBehavior.AllowGet);
        }
    }
}