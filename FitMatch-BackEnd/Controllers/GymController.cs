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
        public IActionResult Gym(CKeywordViewModel vm, int currentPage = 1)
        {
            IQueryable<Gym> gyms = _context.Gyms;

            // Search and filter logic
            if (!string.IsNullOrEmpty(vm?.txtKeyword))
            {
                gyms = gyms.Where(t => EF.Functions.Like(t.Address, $"%{vm.txtKeyword}%"));
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
                    // 檢查 MIME 類型
                    var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                    if (!allowedMimeTypes.Contains(p.FileToUpload.ContentType))
                    {
                        ModelState.AddModelError("FileToUpload", "只允許 JPEG, PNG 或 GIF 格式的文件");
                        return View(p);
                    }

                    // 檢查文件大小
                    if (p.FileToUpload.Length > 10 * 1024 * 1024)  // 10 MB
                    {
                        ModelState.AddModelError("FileToUpload", "文件大小不能超過 10 MB");
                        return View(p);
                    }


                    // 生成一個唯一的文件名（這裡使用了 Guid，您也可以使用其他方式）
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + p.FileToUpload.FileName;

                    // 確定保存位置
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/gym", uniqueFileName);

                    // 保存文件
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await p.FileToUpload.CopyToAsync(stream);
                    }

                    // 保存新文件名到數據庫
                    p.Photo = uniqueFileName;
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

                // 更新 Approved 狀態
                custDb.Approved = string.IsNullOrEmpty(Request.Form["Approved"].ToString()) ? (bool?)null : Convert.ToBoolean(Request.Form["Approved"]);

                // 更新 GymDescription
                custDb.GymDescription = custIn.GymDescription;

                // 更新 OpentimeStart 和 OpentimeEnd
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
                    // 檢查文件大小（以字節為單位，這裡限制為 5MB）
                    if (custIn.FileToUpload.Length > 10 * 1024 * 1024)
                    {
                        // 文件過大
                        ModelState.AddModelError("FileToUpload", "文件大小不能超過 10 MB.");
                        return View(custIn);
                    }

                    // 檢查MIME類型
                    string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    string fileExtension = Path.GetExtension(custIn.FileToUpload.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(fileExtension) || !permittedExtensions.Contains(fileExtension))
                    {
                        // 非法文件類型
                        ModelState.AddModelError("FileToUpload", "只允許 JPEG, PNG 或 GIF 格式的文件");
                        return View(custIn);
                    }

                    // 生成一個唯一的檔名
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/gym", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await custIn.FileToUpload.CopyToAsync(stream);
                    }

                    // 刪除舊照片（如果需要）
                    if (!string.IsNullOrEmpty(custDb.Photo))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/gym", custDb.Photo);
                        System.IO.File.Delete(oldPath);
                    }

                    custDb.Photo = uniqueFileName;
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Gym");
        }





        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string RegionFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }



    }
}
