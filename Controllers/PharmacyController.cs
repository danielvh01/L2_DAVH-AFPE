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
            return View(Singleton.Instance.orders);
        }

        // GET: PharmacyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PharmacyController/Create
        public ActionResult Create()
        {
            if (Singleton.Instance.options.Length == 0)
            {
                Singleton.Instance.Traverse(Singleton.Instance.guide.Root);
            }
            return View();
        }

        // POST: PharmacyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {

            try
            {
                var newOrder = new Cart
                {
                    clientName = collection["clientName"],
                    NIT = collection["NIT"],
                    address = collection["address"],
                    amount = double.Parse(collection["amount"]),
                    product = collection["product"]
                };
                int idx = Singleton.Instance.guide.Find(new Drug { name = collection["product"], numberline = 0 }, Singleton.Instance.guide.Root).value.numberline;
                Singleton.Instance.inventory.Get(idx).Quantity--;
                if (Singleton.Instance.inventory.Get(idx).Quantity == 0)
                {
                    Singleton.Instance.guide.Delete(Singleton.Instance.guide.Root, x => x.name.CompareTo(collection["product"]));
                }
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
            if(!Singleton.Instance.fileUpload)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public ActionResult ViewTree()
        {
            Singleton.Instance.PrintTree(Singleton.Instance.guide.Root);
            return View();
        }

        public void Resuply()
        {
            for(int i = 0; i < Models.Data.Singleton.Instance.inventory.Length; i++)
            {
                PharmacyModel item = Models.Data.Singleton.Instance.inventory.Get(i);
                if (item.Quantity == 0)
                {
                    Random r = new Random();
                    item.Quantity = r.Next(1, 15);
                    Singleton.Instance.guide.Insert(new Drug { name = item.Name, numberline = i }, Singleton.Instance.guide.Root);
                }
            }
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
                    try
                    {
                        Drugss = txtfldprsr.ReadFields();
                        var newDrug = new PharmacyModel
                        {
                            Id = int.Parse(Drugss[0]),
                            Name = Drugss[1].ToString(),
                            Description = Drugss[2].ToString(),
                            Production_Factory = Drugss[3].ToString(),
                            Price = double.Parse(Drugss[4].Substring(1)),
                            Quantity = int.Parse(Drugss[5])
                        };
                        Singleton.Instance.inventory.InsertAtEnd(newDrug);
                        cont++;
                        if (newDrug.Quantity > 0)
                        {
                            Singleton.Instance.guide.Insert(new Drug { name = Drugss[1], numberline = cont }, Singleton.Instance.guide.Root);
                        }
                    }
                    catch(Exception e)
                    {
                        
                    }
                }

            }
            return RedirectToAction(nameof(Index));
        }
        
    }
}
