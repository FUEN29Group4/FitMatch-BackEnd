﻿using System;
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
        public IActionResult Gym(CKeywordViewModel vm)
        {
            IQueryable<Gym> gyms = _context.Gyms;

            if (!string.IsNullOrEmpty(vm?.txtKeyword))
            {
                gyms = gyms.Where(t => t.GymName.Contains(vm.txtKeyword));
            }

            if (!string.IsNullOrEmpty(vm?.RegionFilter))
            {
                gyms = gyms.Where(t => t.Address.Contains(vm.RegionFilter));
            }

            if (!string.IsNullOrEmpty(vm?.StatusFilter))
            {
                switch (vm.StatusFilter)
                {
                    case "上架":
                        gyms = gyms.Where(t => t.Approved == true);
                        break;
                    case "下架":
                        gyms = gyms.Where(t => t.Approved == false);
                        break;
                    case "待審核":
                        gyms = gyms.Where(t => t.Approved == null);
                        break;
                }
            }


            return View(gyms.ToList());
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

        [HttpPost]
        public IActionResult GymCreate(Gym p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.Gyms.Add(p);
            db.SaveChanges();
            return RedirectToAction("Gym");
        }


        //刪除功能
        public IActionResult GymDelete(int? id)
        {
            if (id == null)
                return RedirectToAction("Gym");
            Gym cust = _context.Gyms.FirstOrDefault(t => t.GymId == id);
            if (cust != null)
            {
                _context.Gyms.Remove(cust);
                _context.SaveChanges();
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

        [HttpPost]
        public IActionResult GymEdit(Gym custIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Gym custDb = db.Gyms.FirstOrDefault(t => t.GymId == custIn.GymId);

            if (custDb != null)
            {
                custDb.GymName = custIn.GymName;
                custDb.Phone = custIn.Phone;
                custDb.Address = custIn.Address;
                db.SaveChanges();
            }
            return RedirectToAction("Gym");
        }

        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string RegionFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }




    }
}
