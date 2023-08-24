using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitMatch_BackEnd.Controllers
{
    public class RobotController : SuperController
    {
        public IActionResult Index()
        {
            return View();
        }



        private IWebHostEnvironment _enviro = null;
        public RobotController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public IActionResult Robot()
        {

            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Robot> datas = from d in db.Robots select d;
            return View(datas);
        }


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
