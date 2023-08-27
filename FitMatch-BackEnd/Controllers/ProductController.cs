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




        //***篩選功能***

        //取得商品管理頁面  ***圖片未抓取***
        public IActionResult List(string searchField, string searchKeyword, CKeywordViewModel vm, int currentPage = 1)
        {

            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Product> datas = db.Products;


            if (vm.txtKeyword != null || vm.StatusFilter != null || vm.ProductFilter != null)
            {
                if (!string.IsNullOrEmpty(vm?.StatusFilter))
                {
                    switch (vm.StatusFilter)
                    {
                        case "上架":
                            datas = db.Products.Where(t => t.Status == true);
                            break;
                        case "下架":
                            datas = db.Products.Where(t => t.Status == false);
                            break;
                    }
                    if (!string.IsNullOrEmpty(vm?.ProductFilter))
                    {
                        datas = datas.Where(t => t.TypeId.ToString().Contains(vm.ProductFilter));
                    }
                    if (!string.IsNullOrEmpty(vm?.txtKeyword))
                    {
                        datas = datas.Where(t => t.ProductName.Contains(vm.txtKeyword));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(vm?.ProductFilter))
                    {
                        datas = db.Products.Where(t => t.TypeId.ToString().Contains(vm.ProductFilter));
                        if (!string.IsNullOrEmpty(vm?.txtKeyword))
                        {
                            datas = datas.Where(t => t.ProductName.Contains(vm.txtKeyword));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(vm?.txtKeyword))
                        {
                            datas = db.Products.Where(t => t.ProductName.Contains(vm.txtKeyword));
                        }
                    }

                }
            }
            else
            {
                datas = from p in db.Products
                        select p;
            }

            //跨TABLE
            //var viewModelList = GetFilteredData(searchField, searchKeyword);


            int itemsPerPage = 8;

            // 根據當下頁碼獲取datas
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = _context.Products.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            return View(datas);

        }



        //新增商品  ***圖片未寫入，商品類別無法寫入***

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
                    string photoName = Guid.NewGuid().ToString() + ".jpg";

                    string path = _enviro.WebRootPath + "/img/商城/" + photoName;
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    custDb.Photo = photoName;
                }

                // 更新 Status 狀態
                custDb.Status = p.Status;
                custDb.ProductName = p.ProductName;
                custDb.ProductDescription = p.ProductDescription;
                custDb.Price = p.Price;
                custDb.ProductInventory = p.ProductInventory;
                custDb.TypeId = p.TypeId;

                db.Products.Add(custDb);
                db.SaveChanges();
            }

            //db.Restaurants.Add(p);
            //db.SaveChanges();
            return RedirectToAction("List");
        }




        //刪除商品 
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Product cust = db.Products.FirstOrDefault(t => t.ProductId == id);
            if (cust != null)
            {
                db.Products.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        //修改   ***圖片未寫入，商品類別無法寫入***
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Product prod = db.Products.FirstOrDefault(t => t.ProductId == id);
            if (prod == null)
                return RedirectToAction("List");

            //return View(prod);

            CProductWrap prodWp = new CProductWrap();
            prodWp.product = prod;
            return View(prodWp.product);
        }
        [HttpPost]
        public IActionResult Edit(CProductWrap prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Product prodDb = db.Products.FirstOrDefault(t => t.ProductId == prodIn.ProductId);

            if (prodIn != null)
            {
                if (prodIn.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";

                    //string path = Path.Combine(_enviro.WebRootPath , "/images/" , photoName);
                    //using (var stream = new FileStream(path, mode: FileMode.Create))
                    //{
                    //    custIn.photo = photoName;
                    //}
                    string path = _enviro.WebRootPath + "/img/商城/" + photoName;
                    prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
                    prodDb.Photo = photoName;
                }

                // 更新其他屬性
                prodDb.ProductName = prodIn.ProductName;
                prodDb.Photo = prodIn.Photo;
                prodDb.ProductDescription = prodIn.ProductDescription;
                prodDb.Price = prodIn.Price;
                prodDb.ProductInventory = prodIn.ProductInventory;
                prodDb.Status = prodIn.Status;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string ProductFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }
    }
}