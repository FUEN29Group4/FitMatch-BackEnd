using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Gym> datas = from d in db.Gyms select d; //

            return View(datas);
        }

        //public IActionResult GymEdit()
        //{

        //    IEnumerable<Gym> datas = from p in _context.Gyms select p;
        //    return View(datas);
        //}

        public IActionResult GymCreate()
        {
            return View();
        }


        //刪除功能
        public IActionResult GymDelete(int? id)
        {
            if (id == null)
                return RedirectToAction("Gym");
            FitMatchDbContext db = new FitMatchDbContext();
            Gym cust = db.Gyms.FirstOrDefault(t => t.GymId == id);
            if (cust != null)
            {
                db.Gyms.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("Gym");
        }

        public IActionResult GymEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("Gym");
            FitMatchDbContext db = new FitMatchDbContext();
            Gym cust = db.Gyms.FirstOrDefault(t => t.GymId == id);
            if (cust == null)
                return RedirectToAction("Gym");
            return View(cust);
        }



    }
}
