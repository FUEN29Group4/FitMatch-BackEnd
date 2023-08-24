using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class ProductController : SuperController
    {
        //連結DB
        private readonly FitMatchDbContext _context;


        public ProductController(FitMatchDbContext context, IWebHostEnvironment p)
        {
            _context = context;
            _enviro = p;
        }

        public IWebHostEnvironment _enviro = null;

        //***篩選功能***

        //取得商品管理頁面  ***上架狀態要修***圖片未抓取***
        public IActionResult List(CKeywordViewModel vm)
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
                        case "待審核":
                            datas = db.Products.Where(t => t.Status == null);
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


            //if (string.IsNullOrEmpty(vm.txtKeyword))
            //    datas = from p in db.Products
            //            select p;
            //else
            //    datas = db.Products.Where(t => t.ProductName.Contains(vm.txtKeyword));
            return View(datas);
        }

        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string ProductFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }

        //新增商品  ***圖片未寫入，商品類別無法寫入***

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.Products.Add(p);
            db.SaveChanges();
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
            
            return View(prod);
          
        }
        [HttpPost]
        public IActionResult Edit(Product prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Product prodDb = db.Products.FirstOrDefault(t => t.ProductId == prodIn.ProductId);

            if (prodDb != null)
            {
                // 更新其他屬性
                prodDb.ProductName = prodIn.ProductName;
                prodDb.Photo = prodIn.Photo;
                prodDb.ProductDescription = prodIn.ProductDescription;
                prodDb.Price = prodIn.Price;
                prodDb.ProductInventory = prodIn.ProductInventory;
                prodDb.Approved = prodIn.Approved;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}