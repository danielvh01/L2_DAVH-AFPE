using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2_DAVH_AFPE.Models;
using L2_DAVH_AFPE.Models.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;


namespace L2_DAVH_AFPE.Controllers
{
    public class PharmacyController : Controller
    {
        int cont = 0;
        private readonly IHostingEnvironment hostingEnvironment;
        public PharmacyController(IHostingEnvironment hostingEnvironment)
        {            
            this.hostingEnvironment = hostingEnvironment;
        }
        // GET: PharmacyController
        public ActionResult Index()
        {
            return View(Singleton.Instance.HandcraftedList);
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

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(FileModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                string filePath = null;
                if (model.File != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Uploads");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.File.CopyTo(new FileStream(filePath, FileMode.Create));
                }                                
                TextReader txtrdr = new StreamReader(model.File.OpenReadStream());
                TextFieldParser txtfldprsr = new TextFieldParser(txtrdr);
                txtfldprsr.SetDelimiters(new string[] {","});
                txtfldprsr.HasFieldsEnclosedInQuotes = true;

                string[] Drugss;

                while (!txtfldprsr.EndOfData) {
                    for (int i = 0; i < 5; i++) {
                        Drugss = txtfldprsr.ReadFields();
                        var newDrug = new Models.PharmacyModel
                        {
                            Id   = Convert.ToInt32(Drugss[1]),
                            Name = Drugss[2],
                            Description = Drugss[3],
                            Production_Factory = Drugss[4],
                            Price = Convert.ToDouble(Drugss[5]),
                            Quantity = Convert.ToInt32(Drugss[6])
                        };
                    }
                }


            }
            return RedirectToAction(nameof(Index));
        }

    }
}
