using Microsoft.AspNetCore.Mvc;
using FitMatch_BackEnd.Models;
using System.Linq;
using FitMatch_BackEnd.ViewModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using X.PagedList.Mvc.Core;
using System.Drawing.Printing;

namespace FitMatch_BackEnd.Controllers
{
    public class ProductController : SuperController
    {
        public IWebHostEnvironment _enviro = null;


        //連結DB
        private readonly FitMatchDbContext _context;


        public ProductController(FitMatchDbContext context, IWebHostEnvironment p)
        {
            _context = context;
            _enviro = p;

        }




        public IActionResult List(string searchField, string searchKeyword, CKeywordViewModel vm, int currentPage = 1)
        {
            // 應該使用依賴注入，而不是每次都新建一個實例
            FitMatchDbContext db = new FitMatchDbContext();

            var datas = db.Products.AsQueryable(); // 使用IQueryable以便後續操作

            if (!string.IsNullOrEmpty(vm?.StatusFilter))
            {
                bool status = vm.StatusFilter == "上架";
                datas = datas.Where(t => t.Status == status);
            }

            if (!string.IsNullOrEmpty(vm?.ProductFilter))
            {
                datas = datas.Where(t => t.TypeId.ToString().Contains(vm.ProductFilter));
            }

            if (!string.IsNullOrEmpty(vm?.txtKeyword))
            {
                datas = datas.Where(t => t.ProductName.Contains(vm.txtKeyword));
            }

            //計算總數量在過濾後，不是在所有產品上。
            int totalDataCount = datas.Count();

            int itemsPerPage = 5;
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;

            return View(datas);
        }




        //新增商品  

        public IActionResult Create()
        {
            CProductWrap prodWp = new CProductWrap();
            return View(prodWp.product);
            //return View();
        }
        [HttpPost]
        public IActionResult Create(CProductWrap p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Product custDb = new Product();

            if (p != null)
            {
                if (p.photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        p.photo.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        // Convert image to Base64
                        string base64Image = Convert.ToBase64String(imageBytes);
                        custDb.Photo = base64Image;
                    }
                }

                // Update other properties
                custDb.Status = p.Status;
                custDb.ProductName = p.ProductName;
                custDb.ProductDescription = p.ProductDescription;
                custDb.Price = p.Price;
                custDb.ProductInventory = p.ProductInventory;
                custDb.TypeId = p.TypeId;

                db.Products.Add(custDb);
                db.SaveChanges();
            }

            return RedirectToAction("List");
        }




        //刪除商品 
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            using (var db = new FitMatchDbContext())
            {
                Product cust = db.Products.FirstOrDefault(t => t.ProductId == id);
                if (cust != null)
                {
                    db.Products.Remove(cust);
                    db.SaveChanges();
                }
                else
                {
                    return NotFound("The product was not found.");
                }
            }

            return RedirectToAction("List");
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            using (var db = new FitMatchDbContext()) // 使用 using 語句
            {
                Product prod = db.Products.FirstOrDefault(t => t.ProductId == id);
                if (prod == null)
                    return RedirectToAction("List");
                CProductWrap prodWp = new CProductWrap();
                prodWp.product = prod;
                return View(prodWp.product);
            }
        }
        [HttpPost]
        public IActionResult Edit(CProductWrap prodIn)
        {
            if (prodIn == null || prodIn.ProductId <= 0)
                return BadRequest("Invalid product data.");

            using (var db = new FitMatchDbContext())
            {
                Product custDb = db.Products.FirstOrDefault(t => t.ProductId == prodIn.ProductId);

                if (custDb == null)
                    return NotFound("Product not found.");

                if (prodIn.photo != null)
                {
                    string fileExtension = Path.GetExtension(prodIn.photo.FileName);
                    if (fileExtension != ".jpg") // 確保正確的圖片格式
                        return BadRequest("Invalid image format.");

                    // 將上傳的圖片轉換為 Base64 格式
                    byte[] photoBytes;
                    using (var ms = new MemoryStream())
                    {
                        prodIn.photo.CopyTo(ms);
                        photoBytes = ms.ToArray();
                    }
                    string base64Photo = Convert.ToBase64String(photoBytes);
                    custDb.Photo = base64Photo;
                }

                // 更新其他屬性
                custDb.ProductName = prodIn.ProductName;
                custDb.ProductDescription = prodIn.ProductDescription;
                custDb.Price = prodIn.Price;
                custDb.ProductInventory = prodIn.ProductInventory;
                custDb.Status = prodIn.Status;
                custDb.TypeId = prodIn.TypeId;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        //[HttpPost]
        //public IActionResult Edit(CProductWrap prodIn)
        //{
        //    if (prodIn == null || prodIn.ProductId <= 0)
        //        return BadRequest("Invalid product data."); // 更好的錯誤處理

        //    using (var db = new FitMatchDbContext()) // 使用 using 語句
        //    {
        //        Product custDb = db.Products.FirstOrDefault(t => t.ProductId == prodIn.ProductId);

        //        if (custDb == null)
        //            return NotFound("Product not found.");

        //        if (prodIn.photo != null)
        //        {
        //            string fileExtension = Path.GetExtension(prodIn.photo.FileName);
        //            if (fileExtension != ".jpg") // 確保正確的圖片格式
        //                return BadRequest("Invalid image format.");

        //            string photoName = Guid.NewGuid().ToString() + fileExtension;

        //            string path = Path.Combine(_enviro.WebRootPath, "img/商城", photoName);

        //            using (var fileStream = new FileStream(path, FileMode.Create)) // 使用 using 語句
        //            {
        //                prodIn.photo.CopyTo(fileStream);
        //            }
        //            custDb.Photo = photoName;
        //        }

        //        // 更新其他屬性
        //        custDb.ProductName = prodIn.ProductName;
        //        custDb.ProductDescription = prodIn.ProductDescription;
        //        custDb.Price = prodIn.Price;
        //        custDb.ProductInventory = prodIn.ProductInventory;
        //        custDb.Status = prodIn.Status;
        //        custDb.TypeId = prodIn.TypeId;

        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}

        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string ProductFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }
    }
}