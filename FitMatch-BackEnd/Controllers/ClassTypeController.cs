using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class ClassTypeController : Controller
    {
        public IWebHostEnvironment _enviro = null;
        public ClassTypeController(IWebHostEnvironment p)
        {
            _enviro = p;
        }


        public IActionResult List(CKeywordViewModel vm)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            IQueryable<ClassType> datas = db.ClassTypes;


            if (vm.txtKeyword != null || vm.StatusFilter != null)
            {
                if (!string.IsNullOrEmpty(vm?.StatusFilter))
                {
                    switch (vm.StatusFilter)
                    {
                        case "上架":
                            datas = db.ClassTypes.Where(t => t.Status == true);
                            break;
                        case "下架":
                            datas = db.ClassTypes.Where(t => t.Status == false);
                            break;
                        case "待審核":
                            datas = db.ClassTypes.Where(t => t.Status == null);
                            break;
                    }
                    if (!string.IsNullOrEmpty(vm?.txtKeyword))
                    {
                        datas = datas.Where(t => t.ClassName.Contains(vm.txtKeyword));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(vm?.txtKeyword))
                    {
                        datas = datas.Where(t => t.ClassName.Contains(vm.txtKeyword));
                    }
                }
            }
            else
            {
                datas = from p in db.ClassTypes
                        select p;
            }

            return View(datas);
        }
        public IActionResult Create()
        {
            CClassTypeWrap prodWp = new CClassTypeWrap();
            return View(prodWp.classtype);
            //return View();
        }
        [HttpPost]
        public IActionResult Create(CClassTypeWrap p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType custDb = new ClassType();

            if (p != null)
            {
                if (p.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";

                    //string path = Path.Combine(_enviro.WebRootPath, "/img/健康餐/", photoName);
                    //using (var stream = new FileStream(path, mode: FileMode.Create))
                    //{
                    //    custDb.Photo = photoName;
                    //}
                    string path = _enviro.WebRootPath + "/img/課程/" + photoName;
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    custDb.Photo = photoName;
                }
                custDb.ClassName = p.FName;
                custDb.Introduction = p.FIntroduction;
                custDb.CreateAt = p.CreateAt;
                custDb.Status = p.Status;

                db.ClassTypes.Add(custDb);
                db.SaveChanges();
            }

            //db.Restaurants.Add(p);
            //db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType cust = db.ClassTypes.FirstOrDefault(t => t.ClassTypeId == id);
            if (cust != null)
            {
                db.ClassTypes.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType prod = db.ClassTypes.FirstOrDefault(t => t.ClassTypeId == id);
            if (prod == null)
                return RedirectToAction("List");

            CClassTypeWrap prodWp = new CClassTypeWrap();
            prodWp.classtype = prod;
            return View(prodWp.classtype);
        }
        [HttpPost]
        public IActionResult Edit(CClassTypeWrap prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType custDb = db.ClassTypes.FirstOrDefault(t => t.ClassTypeId == prodIn.FId);


            if (prodIn != null)
            {
                if (prodIn.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";

                    //string path = Path.Combine(_enviro.WebRootPath , "/images/" , photoName);
                    //using (var stream = new FileStream(path, mode: FileMode.Create))
                    //{
                    //    custIn.photo = photoName;
                    //}
                    string path = _enviro.WebRootPath + "/img/課程/" + photoName;
                    prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
                    custDb.Photo = photoName;
                }
                custDb.ClassName = prodIn.FName;
                custDb.Introduction = prodIn.FIntroduction;
                custDb.CreateAt = prodIn.CreateAt;
                custDb.Status = prodIn.Status;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }


        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }



    }
}
