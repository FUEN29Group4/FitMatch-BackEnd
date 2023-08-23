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
            IQueryable<Restaurant> datas = db.Restaurants;


            if (vm.txtKeyword != null || vm.StatusFilter != null || vm.RegionFilter != null)
            {
                if (!string.IsNullOrEmpty(vm?.StatusFilter))
                {
                    switch (vm.StatusFilter)
                    {
                        case "上架":
                            datas = db.Restaurants.Where(t => t.Status == true);
                            break;
                        case "下架":
                            datas = db.Restaurants.Where(t => t.Status == false);
                            break;
                        case "待審核":
                            datas = db.Restaurants.Where(t => t.Status == null);
                            break;
                    }
                    if (!string.IsNullOrEmpty(vm?.RegionFilter))
                    {
                        datas = datas.Where(t => t.Address.Contains(vm.RegionFilter));
                    }
                    if (!string.IsNullOrEmpty(vm?.txtKeyword))
                    {
                        datas = datas.Where(t => t.RestaurantsName.Contains(vm.txtKeyword));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(vm?.RegionFilter))
                    {
                        datas = db.Restaurants.Where(t => t.Address.Contains(vm.RegionFilter));
                        if (!string.IsNullOrEmpty(vm?.txtKeyword))
                        {
                            datas = datas.Where(t => t.RestaurantsName.Contains(vm.txtKeyword));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(vm?.txtKeyword))
                        {
                            datas = db.Restaurants.Where(t => t.RestaurantsName.Contains(vm.txtKeyword));
                        }
                    }

                }
            }
            else
            {
                datas = from p in db.Restaurants
                        select p;
            }


            return View(datas);

        }
        public IActionResult Create()
        {
            CRestaurantWrap prodWp = new CRestaurantWrap();
            return View(prodWp.restaurant);
            //return View();
        }
        [HttpPost]
        public IActionResult Create(CRestaurantWrap p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Restaurant custDb = new Restaurant();

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
                    string path = _enviro.WebRootPath + "/img/健康餐/" + photoName;
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    custDb.Photo = photoName;
                }
                custDb.RestaurantsName = p.RestaurantsName;
                custDb.Phone = p.Phone;
                custDb.Address = p.Address;
                custDb.RestaurantsDescription = p.RestaurantsDescription;
                custDb.CreateAt = p.CreateAt;
                custDb.Status = p.Status;

                db.Restaurants.Add(custDb);
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


        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string RegionFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }




    }
}





