using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class ProductController : Controller
    {
        //連結DB
        private readonly FitMatchDbContext _context;


        public ProductController(FitMatchDbContext context, IWebHostEnvironment p)
        {
            _context = context;
            _enviro = p;
        }

        public IWebHostEnvironment _enviro = null;


        //取得商品管理頁面  ***上架狀態要修***圖片未抓取***全部的商品類別TypeId皆未出現，要修改***
        public IActionResult List(CKeywordViewModel vm)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Product> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Products
                        select p;
            else
                datas = db.Products.Where(t => t.ProductName.Contains(vm.txtKeyword));
            return View(datas);
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

        //修改  ***資料庫無法改寫***
        //***SqlException: Cannot insert explicit value for identity column in table 'Product' when IDENTITY_INSERT is set to OFF.***
        //***SqlException：當 IDENTITY_INSERT 設置為 OFF 時，無法在表“Product”中插入標識列的顯式值。***
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Product prod = db.Products.FirstOrDefault(t => t.ProductId == id);
            if (prod == null)
                return RedirectToAction("List");

            CProductWrap prodWp = new CProductWrap();
            prodWp.product = prod;
            return View(prodWp);
            //return View(prod);
        }
        [HttpPost]
        public IActionResult Edit(CProductWrap prodIn)
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
