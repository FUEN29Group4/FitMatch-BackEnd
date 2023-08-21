using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class ProductController : Controller
    {
        //連結DB
        private readonly FitMatchDbContext _context;

        
        public ProductController(FitMatchDbContext context)
        {
            _context = context;
        }
        
        public IActionResult ProductType()
        {
            IEnumerable<ProductType> datas = from p in _context.ProductTypes select p;
            return View(datas);
        }
        public IActionResult Product()
        {
            IEnumerable<Product> datas = from p in _context.Products select p;
            return View(datas);
        }

        //取得產品管理頁面
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
        //public IActionResult ProductTypeList()
        //{
        //    List<Product> datas = (new ProductType()).queryAll();
        //        return View(datas);
        //}
        //商品管理資料連結然後呈現出views
    }
}
