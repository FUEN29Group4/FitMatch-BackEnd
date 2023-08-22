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
        public IActionResult Robot(CKeywordViewModel vm)
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
            return RedirectToAction("List");
        }



        public IActionResult RobotEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
           Robot Rob = db.Robots.FirstOrDefault(t => t.RobotId == id);
            if (Rob == null)
                return RedirectToAction("List");
            RobotWrap RWp = new RobotWrap();
            RWp.Robot = Rob;
            return View(RWp);
        }

        //[HttpPost]
        //public IActionResult RobotEdit(CProductWrap prodIn)
        //{
        //    FitMatchDbContext db = new FitMatchDbContext();
        //    Robot prodDb = db.Robots.FirstOrDefault(t => t.RobotId == prodIn.);

        //    if (prodDb != null)
        //    {
        //        if (prodIn.photo != null)
        //        {
        //            string photoName = Guid.NewGuid().ToString() + ".jpg";
        //            string path = _enviro.WebRootPath + "/images/" + photoName;
        //            prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
        //            prodDb.FImagePath = photoName;
        //        }

        //        prodDb.FName = prodIn.FName;
        //        prodDb.FQty = prodIn.FQty;
        //        prodDb.FCost = prodIn.FCost;
        //        prodDb.FPrice = prodIn.FPrice;

        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}
    }
}
