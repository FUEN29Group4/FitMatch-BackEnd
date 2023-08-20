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
            CRestaurantWrap prodWp = new CRestaurantWrap();
            prodWp.restaurant = prod;
            return View(prodWp);
        }
        [HttpPost]
        public IActionResult Edit(CRestaurantWrap custIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Restaurant custDb = db.Restaurants.FirstOrDefault(t => t.RestaurantsId == custIn.FId);

            if (custDb != null)
            {
                custDb.RestaurantsName = custIn.FName;
                custDb.Phone = custIn.FPhone;
                custDb.Address = custIn.FAddress;
                custDb.RestaurantsDescription = custIn.FRestaurantsDescription;
                custDb.Approved = custIn.FApproved;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
