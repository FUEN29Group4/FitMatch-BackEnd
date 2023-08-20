using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitMatch_BackEnd.Controllers
{
    public class GymController : Controller
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public GymController(FitMatchDbContext context)
        {
            _context = context;
        }

        //跟場館資料連結然後呈現出views
        public IActionResult Gym()
        {

            IEnumerable<Gym> datas = from p in _context.Gyms select p;
            return View(datas);
        }
    }
}
