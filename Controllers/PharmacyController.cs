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
            var x = Singleton.Instance.guide.Find(x => x.name.CompareTo(collection["Name"]), Singleton.Instance.guide.Root);
            if (x != null)
            {
                return RedirectToAction(nameof(DrugOrder),x) ;
            }
            else
            {
                TempData["testmsg"] = "The drug that you were trying to find does not exist or got out of stock!" + "\n" + "Try to resuply inventory.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult DrugOrder(Drug drug)
        {
            if ( drug != null)
            {
                var pharma2 = Singleton.Instance.inventory.Get(drug.numberline);
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
            
                var newOrder = new PharmacyModel
                {
                    Id = int.Parse(collection["Id"]),
                    Name = collection["Name"],
                    Description = collection["Description"],
                    Production_Factory = collection["Production_Factory"],
                    Price = double.Parse(collection["Price"].ToString().Replace('$', ' ').Replace(')', ' ').Trim()),
                    Quantity = int.Parse(collection["Quantity"])
                };
                int idx = Singleton.Instance.guide.Find(x => x.name.CompareTo(newOrder.Name), Singleton.Instance.guide.Root).numberline;
                PharmacyModel x = Singleton.Instance.inventory.Get(idx);
                if(x.Quantity >= newOrder.Quantity)
                {
                    if (Singleton.Instance.orders.Exists(a => a.Name == x.Name))
                    {
                        var order = Singleton.Instance.orders.Get(idx);
                        order.Quantity += newOrder.Quantity;
                    }
                    else
                    {
                        Singleton.Instance.orders.InsertAtEnd(newOrder);
                    }
                    x.Quantity -= newOrder.Quantity;
                    if (x.Quantity == 0)
                    {
                        Singleton.Instance.guide.Delete(Singleton.Instance.guide.Root, new Drug { name = x.Name });
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
            return View();
        }

        // POST: PharmacyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {

            try
            {
                //var newOrder = new PharmacyModel
                //{
                //    Id = Singleton.Instance.contOrder++,
                //    Name = collection["Name"],
                //    Description = collection["Description"],
                //    Production_Factory = collection["Production_Factory"],
                //    Price = double.Parse(collection["Price"].ToString().Replace('$', ' ').Replace(')', ' ').Trim()),
                //    Stock = int.Parse(collection["Stock"])
                //};
                //string name = newOrder.Name;
                //Singleton.Instance.orders.InsertAtEnd(newOrder);
                //Drug obj = new Drug { name = name, numberline = 0 };
                //int idx = Singleton.Instance.guide.Find(obj).numberline;
                //PharmacyModel x = Singleton.Instance.inventory.Get(idx);
                //x.Stock--;
                //if (x.Stock == 0)
                //{   
                //    Singleton.Instance.guide.Delete(Singleton.Instance.guide.Root, obj);
                //}
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
                PharmacyModel a = Singleton.Instance.orders.Get(ID);
                Singleton.Instance.inventory.Get(a.Id).Quantity += a.Quantity;
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
        public ActionResult ViewTreePO()
        {
            Singleton.Instance.tree = "";
            Singleton.Instance.PostOrder(Singleton.Instance.guide.Root);
            return View();
        }
        public ActionResult ViewTreeIO()
        {
            Singleton.Instance.tree = "";
            Singleton.Instance.InOrder(Singleton.Instance.guide.Root);
            return View();
        }
        public ActionResult ViewTreePEO()
        {
            Singleton.Instance.tree = "";
            Singleton.Instance.PreOrder(Singleton.Instance.guide.Root);
            return View();
        }
        public FileResult PreOrder()
        {
            StreamWriter file = new StreamWriter("Guide.txt", false);            
            file.Write(Singleton.Instance.PreOrder(Singleton.Instance.guide.Root));           
            file.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes("Guide.txt");
            string fileName = "Guide.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public FileResult PostOrder()
        {
            StreamWriter file = new StreamWriter("Guide.txt", false);
            file.Write(Singleton.Instance.PostOrder(Singleton.Instance.guide.Root));            
            file.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes("Guide.txt");
            string fileName = "Guide.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public FileResult InOrder()
        {
            StreamWriter file = new StreamWriter("Guide.txt", false);
            file.Write(Singleton.Instance.InOrder(Singleton.Instance.guide.Root));
            file.Close();
            byte[] fileBytes = System.IO.File.ReadAllBytes("Guide.txt");
            string fileName = "Guide.txt";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult Resuply()
        {
            string resuplied = "";
            bool verif = false;
            for (int i = 0; i < Models.Data.Singleton.Instance.inventory.Length; i++)
            {
                PharmacyModel item = Models.Data.Singleton.Instance.inventory.Get(i);
                if (item.Name == "Acyclovir") 
                { 
                }
                if (item.Quantity == 0)
                {
                    verif = true;
                    Random r = new Random();
                    int ra = r.Next(1, 15);
                    item.Quantity = ra;
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
                            Quantity = int.Parse(Drugss[5])
                        };
                        Singleton.Instance.inventory.InsertAtEnd(newDrug);
                        if (newDrug.Quantity > 0)
                        {
                            Singleton.Instance.guide.Insert(new Drug { name = Drugss[1], numberline = ++contador }, Singleton.Instance.guide.Root);
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
