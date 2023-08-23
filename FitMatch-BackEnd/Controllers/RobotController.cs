using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class RobotController : Controller
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



        //public IActionResult RobotEdit(int? id)
        //{
        //    if (id == null)
        //        return RedirectToAction("Robot");
        //    FitMatchDbContext db = new FitMatchDbContext();
        //   Robot RoDb = db.Robots.FirstOrDefault(t => t.RobotId == id);


        //    if (RoDb == null)
        //        return RedirectToAction("Robot");
        //    RobotWrap RWp = new RobotWrap();
        //    RWp.Robot = RoDb;
        //    return View(RWp);
        //}

        //[HttpPost]
        //public IActionResult RobotEdit(RobotWrap RoIn)
        //{
        //    FitMatchDbContext db = new FitMatchDbContext();
        //    Robot RoDb = db.Robots.FirstOrDefault(t => t.RobotId == RoIn.RobotId);

        //    if (RoDb != null)
        //    {
        //        //if (RoIn.RobotId != null)
        //        //{
        //        //    string photoName = Guid.NewGuid().ToString() + ".jpg";
        //        //    string path = _enviro.WebRootPath + "/images/" + photoName;
        //        //    prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
        //        //    prodDb.FImagePath = photoName;
        //        //}


        //        RoDb.DefaultResponse = RoIn.DefaultResponse;
        //        RoDb.DefaultQuestion = RoIn.DefaultQuestion;
        //        RoDb.Type = RoIn.Type;

        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Robot");
        //}



        public IActionResult RobotEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("Robot");
            FitMatchDbContext db = new FitMatchDbContext();
            Robot cust = db.Robots.FirstOrDefault(t => t.RobotId == id);
            if (cust == null)
                return RedirectToAction("Robot");
            return View(cust);
        }

        [HttpPost]
        public IActionResult RobotEdit(Robot custIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Robot custDb = db.Robots.FirstOrDefault(t => t.RobotId == custIn.RobotId);

            if (custDb != null)
            {
               
                custDb.Type = custIn.Type;
                custDb.DefaultQuestion = custIn.DefaultQuestion;
                custDb.DefaultResponse = custIn.DefaultResponse;
                db.SaveChanges();
            }
            return RedirectToAction("Robot");
        }

    }
}
