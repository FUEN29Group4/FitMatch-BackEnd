using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class RestaurantController : Controller
    {
        public IActionResult List(CKeywordViewModel vm)
        {

            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Restaurant> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Restaurants
                        select p;
            else
                datas = db.Restaurants.Where(t => t.RestaurantsName.Contains(vm.txtKeyword));
            return View(datas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Restaurant p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.Restaurants.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Restaurant cust = db.Restaurants.FirstOrDefault(t => t.RestaurantsId == id);
            if (cust != null)
            {
                db.Restaurants.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Restaurant prod = db.Restaurants.FirstOrDefault(t => t.RestaurantsId == id);
            if (prod == null)
                return RedirectToAction("List");
            return View(prod);
        }
        [HttpPost]
        public IActionResult Edit(Restaurant custIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Restaurant custDb = db.Restaurants.FirstOrDefault(t => t.RestaurantsId == custIn.RestaurantsId);

            if (custDb != null)
            {
                custDb.RestaurantsName = custIn.RestaurantsName;
                custDb.Phone = custIn.Phone;
                custDb.Address = custIn.Address;
                custDb.RestaurantsDescription = custIn.RestaurantsDescription;


                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
