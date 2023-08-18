using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitMatch_BackEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        //彥儀試連 需要可取用
        //跟教練資料連結
        public IActionResult Trainer()
        {
            //資料庫連接
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Trainer> datas = from p in db.Trainers select p;
            return View(datas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}