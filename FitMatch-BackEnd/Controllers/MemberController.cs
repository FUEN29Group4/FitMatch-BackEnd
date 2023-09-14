using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using FitMatch_BackEnd.ViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitMatch_BackEnd.Controllers
{
    public class MemberController : SuperController
    {
        private readonly FitMatchDbContext _context;

        //讀取web根目錄
        public IWebHostEnvironment _enviro = null;
        public MemberController(IWebHostEnvironment enviro, FitMatchDbContext context)
        {
            _enviro = enviro;
            //連結DB
            _context = context;
        }
    
       

        //@@@@ ---- 進入Member Menagement View 1---- @@@@@@

        //--------- M-0:欣彤試連，純出現會員DB資料連結
        //public IActionResult Member(int currentPage = 1)
        //{
        //    //資料庫連接 =>實體化DB
        //    FitMatchDbContext db = new FitMatchDbContext();

        //    //資料定義與處理
        //    //- Member Model(定義資料類型):Member class 
        //    //- FitMAtchDBContext(操作資料表):public virtual DbSet<Member> Members { get; set; } //Members 集合實體
        //    IEnumerable<Member> datas = from d in db.Members select d;

        //    //頁換碼start
        //    int itemsPerPage = 8;// 根據當下頁碼獲取數據
        //    datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

        //    int totalDataCount = db.Robots.Count();
        //    int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

        //    ViewBag.TotalPages = totalPages;
        //    ViewBag.CurrentPage = currentPage;
        //    //頁換碼end


        //    return View(datas);
        //}


        //-----Member List 2 更新keyword 功能版本-----
       
        //跟員工資料連結然後呈現出views
        public IActionResult Member(int currentPage = 1, string txtKeyword = null, bool? memberStatus = null)
        {
            //預設這頁只能放8筆資料
            int itemsPerPage =8;

            IEnumerable<Member> datas = from p in _context.Members select p;
            // 如果有搜尋關鍵字
            if (!string.IsNullOrWhiteSpace(txtKeyword))
            {
                datas = datas.Where(p => p.MemberName.Contains(txtKeyword) || p.Email.Contains(txtKeyword));
            }
            // 如果Status有值
            if (memberStatus.HasValue)
            {
                datas = datas.Where(p => p.Status == memberStatus.Value);
            }
            //把選擇的狀態存進來
            ViewBag.MemberStatus = memberStatus;

            // 根據當下頁碼獲取數據
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = _context.Employees.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Keyword = txtKeyword;  // 將關鍵字存入ViewBag，以便在View中使用
            return View(datas);
        }




        //--------- M-1:會員管理列表（檢視搜尋KeyWord List）
        //public IActionResult List(CKeywordViewModel vm)
        //{

        //    FitMatchDbContext db = new FitMatchDbContext();
        //    IEnumerable<Member> datas = null;
        //    if (string.IsNullOrEmpty(vm.txtKeyword))
        //        datas = from p in db.TCustomers
        //                select p;
        //    else
        //        datas = db.TCustomers.Where(t => t.FName.Contains(vm.txtKeyword)
        //        || t.FPhone.Contains(vm.txtKeyword)
        //        || t.FPhone.Contains(vm.txtKeyword)
        //        || t.FAddress.Contains(vm.txtKeyword));
        //    return View(datas);
        //}

        //--------- M-2:會員新增（Create）
        public IActionResult MemberCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MemberCreate(Member m, IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);
                    m.Photo = base64Image;
                }
            }

            _context.Members.Add(m);
            await _context.SaveChangesAsync();
            return RedirectToAction("Member");
        }


        //判斷性別布林值 （要修修）
        //public IActionResult GenderType(bool? gendertype)
        //{

        //    FitMatchDbContext db = new FitMatchDbContext();
        //    Member g = db.Members.FirstOrDefault(t => t.Gender == gendertype);

        //    ViewBag.g = (bool)gendertype ? "男生" : "女生";
        //    return View(g);

        //}



        //@@@@ ---- 進入Memberdetails View 2---- @@@@@@


        ////--------- M-3:會員修改（Update）& 顯示
        public IActionResult MemberEdit(int? id)
        {
            if (id == null)
                return RedirectToAction("Member");
            FitMatchDbContext db = new FitMatchDbContext();
            Member cust = db.Members.FirstOrDefault(t => t.MemberId == id);
            if (cust == null)
                return RedirectToAction("Member");
            return View(cust);
        }

        [HttpPost]
        public async Task<IActionResult> MemberEdit(MemberWarap custIn)
        {
            if (custIn == null ||
                string.IsNullOrEmpty(custIn.MemberName) ||
                string.IsNullOrEmpty(custIn.Phone) ||
                string.IsNullOrEmpty(custIn.Address) ||
                string.IsNullOrEmpty(custIn.Email) ||
                string.IsNullOrEmpty(custIn.Password))
            {
                ModelState.AddModelError("", "所有欄位都是必填的");
                return View(custIn);
            }

            Member m = _context.Members.FirstOrDefault(t => t.MemberId == custIn.MemberId);

            if (custIn.photo != null && custIn.photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await custIn.photo.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    m.Photo = base64String;
                }
            }

            m.MemberName = custIn.MemberName;
            m.Phone = custIn.Phone;
            m.Email = custIn.Email;
            m.Address = custIn.Address;
            m.Password = custIn.Password;
            m.Gender = custIn.Gender;
            m.CreatedAt = custIn.CreatedAt;
            m.Birth = custIn.Birth;
            m.Status = custIn.Status;

            await _context.SaveChangesAsync();

            return RedirectToAction("Member");
        }



        public IActionResult MemberEditTest(int? id)
        {
            if (id == null)
                return RedirectToAction("Member");
            FitMatchDbContext db = new FitMatchDbContext();
            Member cust = db.Members.FirstOrDefault(t => t.MemberId == id);
            if (cust == null)
                return RedirectToAction("Member");
            return View(cust);
        }
        [HttpPost]
        public async Task<IActionResult> MemberEditTest(Member custIn, IFormFile photo)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Member custDb = db.Members.FirstOrDefault(t => t.MemberId == custIn.MemberId);

            if (custDb != null)
            {
                custDb.MemberName = custIn.MemberName;
                custDb.Phone = custIn.Phone;
                custDb.Email = custIn.Email;
                custDb.Address = custIn.Address;
                custDb.Password = custIn.Password;
                custDb.Gender = custIn.Gender;
                custDb.CreatedAt = custIn.CreatedAt;
                custDb.Birth = custIn.Birth;
                if (photo != null && photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        custDb.Photo = base64String;
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Member");
        }






        ////--------- M-4:會員刪除（Delete）
        public IActionResult MemberDelete(int? id)
        {
            if (id == null)
                return RedirectToAction("Member");
            FitMatchDbContext db = new FitMatchDbContext();
            Member cust = db.Members.FirstOrDefault(t => t.MemberId == id);
            if (cust != null)
            {
                db.Members.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("Member");
        }




        public class CMember
        {
            public int MemberId { get; set; }
            public string? Photo { get; set; }
            public IFormFile photo { get; set; }
        }



    }
}

