using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;


namespace FitMatch_BackEnd.Controllers
{
    public class GymController : SuperController
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public GymController(FitMatchDbContext context)
        {
            _context = context;
        }

        //跟場館資料連結然後呈現出views
        public IActionResult Gym(CKeywordViewModel vm, int currentPage = 1 )
        {
            IQueryable<Gym> gyms = _context.Gyms;

            ViewBag.RegionFilter = vm?.RegionFilter;
            ViewBag.StatusFilter = vm?.StatusFilter;
            // Search and filter logic
            if (!string.IsNullOrEmpty(vm?.txtKeyword))
            {
                gyms = gyms.Where(t => EF.Functions.Like(t.Address, $"%{vm.txtKeyword}%") ||
                                        EF.Functions.Like(t.GymName, $"%{vm.txtKeyword}%"));
            }


            if (!string.IsNullOrEmpty(vm?.RegionFilter))
            {
                gyms = gyms.Where(t => t.Address.Contains(vm.RegionFilter));
            }
            if (!string.IsNullOrEmpty(vm?.StatusFilter))
            {
                switch (vm.StatusFilter)
                {
                    case "上架":
                        gyms = gyms.Where(t => t.Approved == true);
                        break;
                    case "下架":
                        gyms = gyms.Where(t => t.Approved == false);
                        break;
                    case "待審核":
                        gyms = gyms.Where(t => t.Approved == null);
                        break;
                }
            }

            // Pagination logic
            int itemsPerPage = 5;
            int totalDataCount = gyms.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            gyms = gyms.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;

            return View(gyms.ToList());
        }





        public IActionResult GymCreate()
        {
            return View(new Gym());
        }


        [HttpPost]
        public async Task<IActionResult> GymCreate(Gym p) // 注意這裡有 async 和 Task<IActionResult>
        {

            if (p.FileToUpload == null)
            {
                ModelState.Remove("FileToUpload");
            }

            if (ModelState.IsValid)
            {

                if (p.FileToUpload != null)
                {
                    // 使用 MemoryStream 讀取檔案
                    using (var memoryStream = new MemoryStream())
                    {
                        await p.FileToUpload.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        // 將圖片轉換為Base64
                        string base64String = Convert.ToBase64String(imageBytes);
                        // 將Base64儲存在DB中
                        p.Photo = base64String;
                    }
                }

                // 讀取表單中的 "Approved" 值
                string approvedValue = Request.Form["Approved"].ToString();

                // 如果Approved值未在表單中設置，則設置為 null
                if (string.IsNullOrEmpty(approvedValue))
                {
                    p.Approved = null;
                }
                else
                {
                    switch (approvedValue)
                    {
                        case "true":
                            p.Approved = true;
                            break;
                        case "false":
                            p.Approved = false;
                            break;
                        case "":  // 現在這個空字符串代表 "待審核"
                            p.Approved = null;
                            break;
                        default:
                            // 此處可以處理不符合預期的值或錯誤
                            break;
                    }
                }

                // 處理營業開始時間
                if (int.TryParse(Request.Form["startTime"], out int startTime))
                {
                    p.OpentimeStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startTime, 0, 0);
                }
                else
                {
                    p.OpentimeStart = null;
                }

                // 處理營業結束時間
                if (int.TryParse(Request.Form["endTime"], out int endTime))
                {
                    p.OpentimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endTime, 0, 0);
                }
                else
                {
                    p.OpentimeEnd = null;
                }

                // 將對象添加到數據庫
                _context.Gyms.Add(p);
                await _context.SaveChangesAsync();

                return RedirectToAction("Gym");
            }
            return View(p);
        }



        //刪除功能
        public IActionResult GymDelete(int? id)
        {
            if (id == null)
                return RedirectToAction("Gym");

            Gym cust = _context.Gyms.FirstOrDefault(t => t.GymId == id);
            if (cust != null)
            {
                _context.Gyms.Remove(cust);
                _context.SaveChanges();

                return RedirectToAction("Gym", new { deletedId = id });
            }

            return RedirectToAction("Gym");
        }



        public IActionResult GymEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("Gym");
            Gym cust = _context.Gyms.FirstOrDefault(t => t.GymId == id);
            if (cust == null)
                return RedirectToAction("Gym");
            return View(cust);
        }

        [HttpPost]
        public async Task<IActionResult> GymEdit(Gym custIn)
        {
            if (custIn == null ||
                string.IsNullOrEmpty(custIn.GymName) ||
                string.IsNullOrEmpty(custIn.Phone) ||
                string.IsNullOrEmpty(custIn.Address) ||
                string.IsNullOrEmpty(custIn.GymDescription))
            {
                ModelState.AddModelError("", "所有欄位都是必填的");
                return View(custIn);
            }

            Gym custDb = _context.Gyms.FirstOrDefault(t => t.GymId == custIn.GymId);

            if (custDb != null)
            {
                custDb.GymName = custIn.GymName;
                custDb.Phone = custIn.Phone;
                custDb.Address = custIn.Address;
                custDb.Approved = string.IsNullOrEmpty(Request.Form["Approved"].ToString()) ? (bool?)null : Convert.ToBoolean(Request.Form["Approved"]);
                custDb.GymDescription = custIn.GymDescription;

                if (int.TryParse(Request.Form["OpentimeStart"], out int opentimeStartHour))
                {
                    custDb.OpentimeStart = custDb.OpentimeStart.HasValue
                        ? new DateTime(custDb.OpentimeStart.Value.Year, custDb.OpentimeStart.Value.Month, custDb.OpentimeStart.Value.Day, opentimeStartHour, 0, 0)
                        : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, opentimeStartHour, 0, 0);
                }
                else
                {
                    custDb.OpentimeStart = null;
                }

                if (int.TryParse(Request.Form["OpentimeEnd"], out int opentimeEndHour))
                {
                    custDb.OpentimeEnd = custDb.OpentimeEnd.HasValue
                        ? new DateTime(custDb.OpentimeEnd.Value.Year, custDb.OpentimeEnd.Value.Month, custDb.OpentimeEnd.Value.Day, opentimeEndHour, 0, 0)
                        : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, opentimeEndHour, 0, 0);
                }
                else
                {
                    custDb.OpentimeEnd = null;
                }

                if (custIn.FileToUpload != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await custIn.FileToUpload.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        custDb.Photo = base64String;
                    }
                }

                // 在這裡保存所有更改，而不只是照片
                await _context.SaveChangesAsync();

                return RedirectToAction("Gym");
            }
            else
            {
                ModelState.AddModelError("", "找不到相對應的 Gym 資料");
                return View(custIn);
            }
        }






        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string RegionFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }



    }
}
