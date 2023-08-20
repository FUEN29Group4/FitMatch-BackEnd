using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class ClassTypeController : Controller
    {
        public IActionResult List(CKeywordViewModel vm)
        {

            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<ClassType> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.ClassTypes
                        select p;
            else
                datas = db.ClassTypes.Where(t => t.ClassName.Contains(vm.txtKeyword));
            return View(datas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ClassType p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.ClassTypes.Add(p);
            db.SaveChanges();
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
            return View(prodWp);
        }
        [HttpPost]
        public IActionResult Edit(CClassTypeWrap custIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            ClassType custDb = db.ClassTypes.FirstOrDefault(t => t.ClassTypeId == custIn.FId);

            if (custDb != null)
            {
                custDb.ClassName = custIn.FName;
                custDb.Introduction = custIn.FIntroduction;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

    }
}
