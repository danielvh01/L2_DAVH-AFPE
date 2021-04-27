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
        public string pathito = "";
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

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(IFormCollection collection)
        {
            var c = new Drug
            {
                name = collection["Name"]
            };
            var x = Singleton.Instance.guide.Find(c);
            if (x != null)
            {
                return RedirectToAction(nameof(DrugOrder),x) ;
            }
            TempData["testmsg"] = "The drug that you were trying to find does not exist or got out of stock!" + "\n" + "Try to resuply inventory.";
            return RedirectToAction(nameof(Index));
        }

        public ActionResult DrugOrder(Drug drug)
        {
            Drug result = Singleton.Instance.guide.Find(drug);
            if ( result != null)
            {
                var pharma2 = Singleton.Instance.inventory.Get(result.numberline);
                pharma2.Quantity = 1;
                return View(pharma2);
            }
            else
            {
                TempData["testmsg"] = "The drug that you were trying to find does not exist";
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DrugOrder(IFormCollection collection)
        {
            bool verif = false;
            
                var newOrder = new PharmacyModel
                {
                    Id = Singleton.Instance.contOrder++,
                    Name = collection["Name"],
                    Description = collection["Description"],
                    Production_Factory = collection["Production_Factory"],
                    Price = double.Parse(collection["Price"].ToString().Replace('$', ' ').Replace(')', ' ').Trim()),
                    Quantity = int.Parse(collection["Quantity"])
                };
                Drug obj = new Drug { name = newOrder.Name, numberline = 0 };
                int idx = Singleton.Instance.guide.Find(obj).numberline;
                PharmacyModel x = Singleton.Instance.inventory.Get(idx);
                if(x.Stock >= newOrder.Quantity)
                {
                    for (int i = 0; i < Singleton.Instance.orders.Length; i++)
                    {                    
                        if (newOrder.Name == Singleton.Instance.orders.Get(i).Name)
                        {
                            verif = true;
                        }
                    }                
                    Singleton.Instance.orders.InsertAtEnd(newOrder);
                    x.Stock = x.Stock - newOrder.Quantity;
                    Singleton.Instance.inventory.Delete(idx);
                    Singleton.Instance.inventory.Insert(x, idx);
                    if (x.Stock == 0)
                    {
                        Singleton.Instance.guide.Delete(Singleton.Instance.guide.Root, obj);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["testmsg"] = "Drug(s) selected out of stock";
                    return View(newOrder);
                }
            
            //catch(EventArgs e)
            //{
            //    TempData["testmsg"] = "The drug that you were trying to find does not exist";
            //    return RedirectToAction(nameof(Index));
            //}
        }

        // GET: PharmacyController/Details/5
        public ActionResult Details(int ID)
        {
            PharmacyModel drug = Singleton.Instance.orders.Get(ID);
            return View(drug);
        }

        // GET: PharmacyController/Create
        public ActionResult Create()
        {    
            while (Singleton.Instance.options.Length > 0)
            {
                Singleton.Instance.options.Delete(0);
            }
            Singleton.Instance.Traverse(Singleton.Instance.guide.Root);
            return View();
        }

        // POST: PharmacyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {

            try
            {
                var newOrder = new PharmacyModel
                {
                    Id = Singleton.Instance.contOrder++,
                    Name = collection["Name"],
                    Description = collection["Description"],
                    Production_Factory = collection["Production_Factory"],
                    Price = double.Parse(collection["Price"].ToString().Replace('$', ' ').Replace(')', ' ').Trim()),
                    Stock = int.Parse(collection["Stock"])
                };
                string name = newOrder.Name;
                Singleton.Instance.orders.InsertAtEnd(newOrder);
                Drug obj = new Drug { name = name, numberline = 0 };
                int idx = Singleton.Instance.guide.Find(obj).numberline;
                PharmacyModel x = Singleton.Instance.inventory.Get(idx);
                x.Stock--;
                if (x.Stock == 0)
                {   
                    Singleton.Instance.guide.Delete(Singleton.Instance.guide.Root, obj);
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
        public ActionResult Delete(int ID)
        {
            PharmacyModel drug = Singleton.Instance.orders.Get(ID);
            return View(drug);
        }

        // POST: PlayerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ID, IFormCollection collection)
        {
            try
            {
                Singleton.Instance.orders.Delete(ID);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Import()
        {
            if (!Singleton.Instance.fileUpload)
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
            Singleton.Instance.tree = "";
            Singleton.Instance.PrintTree(Singleton.Instance.guide.Root);
            return View();
        }
        public ActionResult DownloadFile()
        {
            StreamWriter file = new StreamWriter("Guide.txt", true);
            file.Write(Singleton.Instance.PrintTree(Singleton.Instance.guide.Root));
            file.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes("Guide.txt");
            string fileName = "Guide.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult Download()
        {
            return File(pathito, Singleton.Instance.PrintTree(Singleton.Instance.guide.Root), "report.txt");
        }
        public ActionResult Resuply()
        {
            string resuplied = "";
            bool verif = false;
            for (int i = 0; i < Models.Data.Singleton.Instance.inventory.Length; i++)
            {
                PharmacyModel item = Models.Data.Singleton.Instance.inventory.Get(i);
                if (item.Stock == 0)
                {
                    verif = true;
                    Random r = new Random();
                    int ra = r.Next(1, 15);
                    item.Stock = ra;
                    Singleton.Instance.guide.Insert(new Drug { name = item.Name, numberline = i, }, Singleton.Instance.guide.Root);                    
                    resuplied += "Drug resuplied: " + item.Name + "\n";
                }
                
            }
            if (verif)
            {
                TempData["testmsg"] = resuplied;
            }
            else {
                TempData["testmsg"] = "Drug inventory was not out of stock.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Import(FileModel model)
        {
            int contador = 0;
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                string filePath = null;
                if (model.File != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Uploads");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    pathito = filePath;
                    model.File.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                TextReader txtrdr = new StreamReader(model.File.OpenReadStream());
                TextFieldParser txtfldprsr = new TextFieldParser(txtrdr);
                txtfldprsr.SetDelimiters(new string[] { "," });
                txtfldprsr.HasFieldsEnclosedInQuotes = true;

                string[] Drugss;
                while (!txtfldprsr.EndOfData)
                {
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
                            Stock = int.Parse(Drugss[5])
                        };
                        int cant = 0;
                        while (Singleton.Instance.inventory.Find(newDrug) != null)
                        {
                            newDrug.Name += "#" + ++cant;
                        }
                        Singleton.Instance.inventory.InsertAtEnd(newDrug);
                        contador++;
                        if (newDrug.Stock > 0)
                        {
                            int cont = 0;
                            while (Singleton.Instance.guide.Find(new Drug { name = Drugss[1], numberline = contador }) != null)
                            {
                                Drugss[1] += "#" + ++cont;
                            }
                            Singleton.Instance.guide.Insert(new Drug { name = Drugss[1], numberline = contador }, Singleton.Instance.guide.Root);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

            }
            return RedirectToAction(nameof(Index));
        }


    }
}
