using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class ProductController : Controller
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public ProductController(FitMatchDbContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            return View();
        }
        //商品管理資料連結然後呈現出views
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
    }
}
