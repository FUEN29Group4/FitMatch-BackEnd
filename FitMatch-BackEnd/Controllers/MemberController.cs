using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitMatch_BackEnd.Controllers
{
    public class MemberController : Controller
    {
        // GET: /<controller>/  預設先註解掉
        //public IActionResult Index()
        //{
        //    return View();
        //}


        private readonly FitMatchDbContext _context;

        //連結DB
        public MemberController(FitMatchDbContext context)
        {
            _context = context;
        }


        //@@@@ ---- 進入Member Menagement View 1---- @@@@@@

        //--------- M-0:欣彤試連，純出現會員DB資料連結
        public IActionResult Member()
        {
            //資料庫連接 =>實體化DB
            FitMatchDbContext db = new FitMatchDbContext();

            //資料定義與處理
            //- Member Model(定義資料類型):Member class 
            //- FitMAtchDBContext(操作資料表):public virtual DbSet<Member> Members { get; set; } //Members 集合實體
            IEnumerable<Member> datas = from d in db.Members select d; //
            
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
        public IActionResult MemberCreate(Member m)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.Members.Add(m);
            db.SaveChanges();
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
        public IActionResult MemberEdit(Member custIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Member custDb = db.Members.FirstOrDefault(t => t.MemberId== custIn.MemberId);

            if (custDb != null)
            {
                custDb.MemberName = custIn.MemberName;
                custDb.Phone = custIn.Phone;
                custDb.Email = custIn.Email;
                custDb.Address = custIn.Address;
                custDb.Password = custIn.Password;
                custDb.Gender = custIn.Gender;
                custDb.CreatedAt = custIn.CreatedAt;
                custDb.Photo = custIn.Photo;
                custDb.Birth = custIn.Birth;
                db.SaveChanges();
            }
            return RedirectToAction("Edit");
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
        public IActionResult MemberEditTest(Member custIn)
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
                custDb.Photo = custIn.Photo;
                custDb.Birth = custIn.Birth;
                db.SaveChanges();
            }
            return RedirectToAction("Member");
        }


        //public IActionResult MemberCancel()
        //{
        //    return View("Member");
        //}



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




        //-----參考彥儀Trianer-----


        ////審核通過
        //public IActionResult Approve(int id)
        //{
        //    var trainer = _context.Trainers.Find(id);
        //    //檢查教練是否存在
        //    if (trainer == null)
        //    {
        //        return NotFound();
        //    }
        //    //有教練就審核通過
        //    trainer.Approved = CApprovalStatus.Approved;
        //    //保存到資料庫
        //    _context.SaveChanges();
        //    //返回教練列表
        //    return RedirectToAction("Trainer");
        //}
        //////退回申請
        //public IActionResult Reject(int id)
        //{
        //    var trainer = _context.Trainers.Find(id);
        //    //檢查教練是否存在
        //    if (trainer == null)
        //    {
        //        return NotFound();
        //    }
        //    //教練的申請拒絕
        //    trainer.Approved = CApprovalStatus.Rejected;
        //    _context.SaveChanges();

        //    return RedirectToAction("Trainer");
        //}
        ////代辦事項: 要做確認按鈕!!!!!!!!!!!!!!!




    }
}

