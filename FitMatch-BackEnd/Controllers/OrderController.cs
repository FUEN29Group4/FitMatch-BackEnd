using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class OrderController : Controller
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public OrderController(FitMatchDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Order()
        {
            IEnumerable<Order> datas = from p in _context.Orders select p;
            return View(datas);
        }
    }
}
