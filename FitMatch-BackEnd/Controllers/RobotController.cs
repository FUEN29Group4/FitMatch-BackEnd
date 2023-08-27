using System;
using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitMatch_BackEnd.Controllers
{
    public class RobotController : SuperController
    {
        //注入DB 可以在很多方法用他來連結資料庫
        private readonly FitMatchDbContext _context;

        private IWebHostEnvironment _enviro = null;
        public RobotController(IWebHostEnvironment enviro, FitMatchDbContext context)
        {
            _enviro = enviro;
            _context = context;
        }


        //-----Robot List 1------
        public IActionResult Robot(int currentPage = 1)
        {

            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Robot> datas = from d in db.Robots select d;

            int itemsPerPage = 8;
            // 根據當下頁碼獲取數據
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = db.Robots.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;


            return View(datas);
        }



        //-----Robot List 2------
        //跟資料連結然後呈現出views
        //public IActionResult Robot(int currentPage = 1, string txtKeyword = null)
        //{
        //    //預設一頁只能有8筆資料
        //    int itemsPerPage = 8;
        //    IEnumerable<Robot> datas = from p in _context.Robots select p;
        //    // 如果有搜尋關鍵字 預設問題或預設回答有包括關鍵字

        //    //Todo: 搜尋一直出錯啊啊啊ＱＱ
        //    //if (datas != null || txtKeyword != null)
        //    //{
        //    //    datas = datas.Where(p => p.DefaultQuestion.Contains(txtKeyword) || p.DefaultResponse.Contains(txtKeyword));
        //    //}

           

        //    // 根據當下頁碼獲取datas
        //    datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

        //    int totalDataCount = _context.Trainers.Count();
        //    int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

        //    ViewBag.TotalPages = totalPages;
        //    ViewBag.CurrentPage = currentPage;
        //    ViewBag.Keyword = txtKeyword;  // 將關鍵字存入ViewBag，以便在View中使用

        //    return View(datas);
        //}







        public IActionResult RobotCreate()
        {


            List<string> typeOptions = new List<string>
        {
            "教練", "媒合", "課程", "訂單", "其他"
        };
            ViewBag.TypeOptions = new SelectList(typeOptions);





            return View();
        }

        [HttpPost]
        public IActionResult RobotCreate(Robot p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.Robots.Add(p);
            db.SaveChanges();
            return RedirectToAction("Robot");
        }


        public IActionResult RobotDelete(int? id)
        {
            if (id == null)
                return RedirectToAction("Robot");
            FitMatchDbContext db = new FitMatchDbContext();
            Robot cust = db.Robots.FirstOrDefault(t => t.RobotId == id);
            if (cust != null)
            {
                db.Robots.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("Robot");
        }



        public IActionResult RobotEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("Robot");
            FitMatchDbContext db = new FitMatchDbContext();
            Robot RobotDb = db.Robots.FirstOrDefault(t => t.RobotId == id);


            if (RobotDb == null)
                return RedirectToAction("Robot");
            RobotWrap RWp = new RobotWrap();
            RWp.Robot = RobotDb;
            return View(RWp.Robot);
        }

        [HttpPost]
        public IActionResult RobotEdit(RobotWrap RoIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Robot RobotDb = db.Robots.FirstOrDefault(t => t.RobotId == RoIn.RobotId);

            if (RobotDb != null)
            {
                //if (RoIn.RobotId != null)
                //{
                //    string photoName = Guid.NewGuid().ToString() + ".jpg";
                //    string path = _enviro.WebRootPath + "/images/" + photoName;
                //    prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
                //    prodDb.FImagePath = photoName;
                //}


                RobotDb.DefaultResponse = RoIn.DefaultResponse;
                RobotDb.DefaultQuestion = RoIn.DefaultQuestion;
                RobotDb.Type = RoIn.Type;



                //抓值的功能代修改
                //if (int.TryParse(Request.Form["OpentimeStart"], out int opentimeStartHour))
                //{
                //    custDb.OpentimeStart = custDb.OpentimeStart.HasValue
                //        ? new DateTime(custDb.OpentimeStart.Value.Year, custDb.OpentimeStart.Value.Month, custDb.OpentimeStart.Value.Day, opentimeStartHour, 0, 0)
                //        : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, opentimeStartHour, 0, 0);
                //}
                //else
                //{
                //    custDb.OpentimeStart = null;
                //}

                //if (int.TryParse(Request.Form["OpentimeEnd"], out int opentimeEndHour))
                //{
                //    custDb.OpentimeEnd = custDb.OpentimeEnd.HasValue
                //        ? new DateTime(custDb.OpentimeEnd.Value.Year, custDb.OpentimeEnd.Value.Month, custDb.OpentimeEnd.Value.Day, opentimeEndHour, 0, 0)
                //        : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, opentimeEndHour, 0, 0);
                //}
                //else
                //{
                //    custDb.OpentimeEnd = null;
                //}
               
                db.SaveChanges();
            }
            return RedirectToAction("Robot");
        }



        //public IActionResult RobotEdit(int? id)
        //{
        //    if (id == null)
        //        return RedirectToAction("Robot");
        //    FitMatchDbContext db = new FitMatchDbContext();
        //    Robot cust = db.Robots.FirstOrDefault(t => t.RobotId == id);
        //    if (cust == null)
        //        return RedirectToAction("Robot");
        //    return View(cust);
        //}

        //[HttpPost]
        //public IActionResult RobotEdit(Robot custIn)
        //{
        //    FitMatchDbContext db = new FitMatchDbContext();
        //    Robot custDb = db.Robots.FirstOrDefault(t => t.RobotId == custIn.RobotId);

        //    if (custDb != null)
        //    {

        //        custDb.Type = custIn.Type;
        //        custDb.DefaultQuestion = custIn.DefaultQuestion;
        //        custDb.DefaultResponse = custIn.DefaultResponse;
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Robot");
        //}

    }
}
