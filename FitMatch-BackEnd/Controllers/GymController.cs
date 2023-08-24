using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Mvc;


namespace FitMatch_BackEnd.Controllers
{
    public class GymController : Controller
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public GymController(FitMatchDbContext context)
        {
            _context = context;
        }

        //跟場館資料連結然後呈現出views
        public IActionResult Gym(CKeywordViewModel vm)
        {
            IQueryable<Gym> gyms = _context.Gyms;

            if (!string.IsNullOrEmpty(vm?.txtKeyword))
            {
                gyms = gyms.Where(t => t.GymName.Contains(vm.txtKeyword));
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


            return View(gyms.ToList());
        }




        public IActionResult GymCreate()
        {
            return View(new Gym());
        }


        [HttpPost]
        public async Task<IActionResult> GymCreate(Gym p) // 注意這裡有 async 和 Task<IActionResult>
        {
            if (ModelState.IsValid)
            {

                // 上傳文件代碼
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/gym", p.FileToUpload.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await p.FileToUpload.CopyToAsync(stream); // 這裡使用了 await，所以需要標記方法為 async
                }

                p.Photo = p.FileToUpload.FileName;
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

                // 讀取表單中的 "GymDescription" 值
                p.GymDescription = Request.Form["GymDescription"];

                _context.Gyms.Add(p);
                await _context.SaveChangesAsync();

                ViewData["FormSubmitted"] = true;
                // 重新傳遞模型到當前視圖以顯示通知
                return View(p);
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
        public IActionResult GymEdit(Gym custIn)
        {
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

                _context.SaveChanges();
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
