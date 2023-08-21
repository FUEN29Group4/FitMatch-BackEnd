using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class RestaurantController : Controller
    {
        public IWebHostEnvironment _enviro = null;
        public RestaurantController(IWebHostEnvironment p)
        {
            _enviro = p;
        }


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
            return View(prodWp.restaurant);
        }
        [HttpPost]
        public IActionResult Edit(CRestaurantWrap prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Restaurant custDb = db.Restaurants.FirstOrDefault(t => t.RestaurantsId == prodIn.RestaurantsId);

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
                    string path = _enviro.WebRootPath + "/img/健康餐/" + photoName;
                    prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
                    custDb.Photo = photoName;
                }
                custDb.RestaurantsName = prodIn.RestaurantsName;
                custDb.Phone = prodIn.Phone;
                custDb.Address = prodIn.Address;
                custDb.RestaurantsDescription = prodIn.RestaurantsDescription;
                custDb.CreateAt = prodIn.CreateAt;
                custDb.Status = prodIn.Status;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
