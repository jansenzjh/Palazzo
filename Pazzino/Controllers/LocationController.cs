using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pazzino.Models;

namespace Pazzino.Controllers
{
    public class LocationController : Controller
    {
        public ActionResult GetLocation(string text)
        {
            return Json(new Location().GetLocation(text), JsonRequestBehavior.AllowGet);
        }

        // GET: State/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: State/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: State/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: State/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: State/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: State/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: State/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
