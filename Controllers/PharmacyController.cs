using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L2_DAVH_AFPE.Controllers
{
    public class PharmacyController : Controller
    {
        // GET: PharmacyController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PharmacyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PharmacyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PharmacyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PharmacyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PharmacyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PharmacyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PharmacyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
