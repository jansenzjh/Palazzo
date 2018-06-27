using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pazzino.Models;
using Pazzino.Database;

namespace Pazzino.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult EditCompany()
        {
            if(SessionItems.CurrentUser.CompanyID != Guid.Empty)
            {
                return View(new CompanyModel().Read(SessionItems.CurrentUser.CompanyID));
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditCompany(Company model)
        {
            try
            {
                //CompanyModel model = new CompanyModel();
                //TryUpdateModel(model);
                new CompanyModel().SaveOrUpdate(SessionItems.CurrentUser.Id, model);
                return RedirectToAction("index", "Manage");
            }
            catch
            {
                return View();
            }
        }
    }
}