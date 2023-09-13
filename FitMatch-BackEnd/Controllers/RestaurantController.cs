using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace FitMatch_BackEnd.Controllers
{
    public class RestaurantController : SuperController
    {
        public IWebHostEnvironment _enviro = null;
        public RestaurantController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        CUserViewModel userViewModel = new CUserViewModel();

        public IActionResult List(int currentPage = 1, string txtKeyword = null, int? RestaurantStatus = null, string? city = "", int? Status = null)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            //預設一頁只能有8筆資料
            int itemsPerPage = 8;
            IEnumerable<Restaurant> datas = from p in db.Restaurants select p;

            // 如果有搜尋關鍵字
            if (!string.IsNullOrWhiteSpace(txtKeyword))
            {
                datas = datas.Where(p => p.RestaurantsName.Contains(txtKeyword));
            }

            // 如果City有值
            if (!string.IsNullOrEmpty(city))
            {
                datas = datas.Where(p => p.Address.StartsWith(city));
            }
            ViewBag.City = city;

            //審核篩選:如果Status有值
            if (Status.HasValue)
            {
                datas = datas.Where(p => p.Status == Status.Value);
            }

            // 根據當下頁碼獲取datas
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = db.Restaurants.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Keyword = txtKeyword;  // 將關鍵字存入ViewBag，以便在View中使用


            return View(datas);

        }

        private void 獲取頁碼(IQueryable<Restaurant> datas, int currentPage, int itemsPerPage = 8, string? txtKeyword = "")
        {
            FitMatchDbContext db = new FitMatchDbContext();
            // 根據當下頁碼獲取datas
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = db.Restaurants.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Keyword = txtKeyword;  // 將關鍵字存入ViewBag，以便在View中使用
        }

        public IActionResult Create()
        {
            CRestaurantWrap prodWp = new CRestaurantWrap();
            return View(prodWp);
        }
        [HttpPost]
        public IActionResult Create(CRestaurantWrap p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Restaurant custDb = new Restaurant();
            DateTime dateTime = DateTime.Now;

            if (p != null)
            {
                if (p.photo != null)
                {
                    //string photoName = Guid.NewGuid().ToString() + ".jpg";

                    //string path = _enviro.WebRootPath + "/img/健康餐/" + photoName;
                    //p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    //custDb.Photo = photoName;


                    // 使用 MemoryStream 讀取檔案
                    using (var memoryStream = new MemoryStream())
                    {
                        p.photo.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        // Convert image to Base64
                        string base64Image = Convert.ToBase64String(imageBytes);
                        custDb.Photo = base64Image;
                    }


                }
                // 更新 Approved 狀態
                custDb.Status = p.Status;
                custDb.RestaurantsName = p.RestaurantsName;
                custDb.Phone = p.Phone;
                custDb.Address = p.Address;
                custDb.RestaurantsDescription = p.RestaurantsDescription;
                custDb.CreateAt = dateTime;

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
            return View(prodWp);
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
                    //string photoName = Guid.NewGuid().ToString() + ".jpg";
                    //string path = _enviro.WebRootPath + "/img/健康餐/" + photoName;
                    //prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
                    //custDb.Photo = photoName;


                    // 使用 MemoryStream 讀取檔案
                    using (var memoryStream = new MemoryStream())
                    {
                        prodIn.photo.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        // Convert image to Base64
                        string base64Image = Convert.ToBase64String(imageBytes);
                        custDb.Photo = base64Image;
                    }

                }

                // 更新 Approved 狀態
                custDb.Status = prodIn.Status;
                custDb.RestaurantsName = prodIn.RestaurantsName;
                custDb.Phone = prodIn.Phone;
                custDb.Address = prodIn.Address;
                custDb.RestaurantsDescription = prodIn.RestaurantsDescription;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }


        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string RegionFilter { get; set; }
            public string StatusFilter { get; set; }
        }
    }
}





