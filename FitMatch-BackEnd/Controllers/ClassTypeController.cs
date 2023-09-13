using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace FitMatch_BackEnd.Controllers
{
    public class ClassTypeController : SuperController
    {
        public IWebHostEnvironment _enviro = null;
        public ClassTypeController(IWebHostEnvironment p)
        {
            _enviro = p;
        }


        public IActionResult List(int currentPage = 1, string txtKeyword = null, int? ClassTypeStatus = null, int? Status = null)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            //預設一頁只能有8筆資料
            int itemsPerPage = 8;
            IEnumerable<ClassType> datas = from p in db.ClassTypes select p;

            // 如果有搜尋關鍵字
            if (!string.IsNullOrWhiteSpace(txtKeyword))
            {
                datas = datas.Where(p => p.ClassName.Contains(txtKeyword));
            }

            //審核篩選:如果Status有值
            if (Status.HasValue)
            {
                datas = datas.Where(p => p.Status == Status.Value);
            }

            // 根據當下頁碼獲取datas
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = db.Restaurants.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Keyword = txtKeyword;  // 將關鍵字存入ViewBag，以便在View中使用

            return View(datas);
        }
        public IActionResult Create()
        {
            CClassTypeWrap prodWp = new CClassTypeWrap();
            return View(prodWp.classtype);
            //return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CClassTypeWrap p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType custDb = new ClassType();

            if (p != null)
            {
                if (p.photo != null)
                {
                    //string photoName = Guid.NewGuid().ToString() + ".jpg";

                    //string path = _enviro.WebRootPath + "/img/課程/" + photoName;
                    //p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    //custDb.Photo = photoName;



                    // 使用 MemoryStream 讀取檔案
                    using (var memoryStream = new MemoryStream())
                    {
                        p.photo.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        // Convert image to Base64
                        string base64Image = Convert.ToBase64String(imageBytes);
                        custDb.Photo = base64Image;
                    }

                }
                custDb.Status = p.Status;
                custDb.ClassName = p.ClassName;
                custDb.Introduction = p.Introduction;


                db.ClassTypes.Add(custDb);
                db.SaveChanges();
            }

            //db.Restaurants.Add(p);
            //db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType cust = db.ClassTypes.FirstOrDefault(t => t.ClassTypeId == id);
            if (cust != null)
            {
                db.ClassTypes.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType prod = db.ClassTypes.FirstOrDefault(t => t.ClassTypeId == id);
            if (prod == null)
                return RedirectToAction("List");

            CClassTypeWrap prodWp = new CClassTypeWrap();
            prodWp.classtype = prod;
            return View(prodWp.classtype);
        }
        [HttpPost]
        public IActionResult Edit(CClassTypeWrap prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType custDb = db.ClassTypes.FirstOrDefault(t => t.ClassTypeId == prodIn.ClassTypeId);


            if (prodIn != null)
            {
                if (prodIn.photo != null)
                {
                    //string photoName = Guid.NewGuid().ToString() + ".jpg";

                    //string path = _enviro.WebRootPath + "/img/課程/" + photoName;
                    //prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
                    //custDb.Photo = photoName;



                    // 使用 MemoryStream 讀取檔案
                    using (var memoryStream = new MemoryStream())
                    {
                        prodIn.photo.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        // Convert image to Base64
                        string base64Image = Convert.ToBase64String(imageBytes);
                        custDb.Photo = base64Image;
                    }
                }
                custDb.Status = prodIn.Status;
                custDb.ClassName = prodIn.ClassName;
                custDb.Introduction = prodIn.Introduction;


                db.SaveChanges();
            }
            return RedirectToAction("List");
        }


        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }



    }
}
