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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Member m)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.Members.Add(m);
            db.SaveChanges();
            return RedirectToAction("List");
        }



        //@@@@ ---- 進入Memberdetails View 2---- @@@@@@


        //Member details 顯示會員詳細資料
        public IActionResult MemberDetails(int id)
        {
            var member = _context.Members.FirstOrDefault(t => t.MemberId == id);
            //if (member == null)
            //{
            //    return View("Error");
            //}
            return View(member);
        }


        



        ////--------- M-3:會員修改（Update）
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //        return RedirectToAction("List");
        //    DbDemoContext db = new DbDemoContext();
        //    TCustomer cust = db.TCustomers.FirstOrDefault(t => t.FId == id);
        //    if (cust == null)
        //        return RedirectToAction("List");
        //    return View(cust);
        //}
        //[HttpPost]
        //public IActionResult Edit(TCustomer custIn)
        //{
        //    DbDemoContext db = new DbDemoContext();
        //    TCustomer custDb = db.TCustomers.FirstOrDefault(t => t.FId == custIn.FId);

        //    if (custDb != null)
        //    {
        //        custDb.FName = custIn.FName;
        //        custDb.FPhone = custIn.FPhone;
        //        custDb.FEmail = custIn.FEmail;
        //        custDb.FAddress = custIn.FAddress;
        //        custDb.FPassword = custIn.FPassword;
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}




        ////--------- M-4:會員刪除（Delete）
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //        return RedirectToAction("List");
        //    DbDemoContext db = new DbDemoContext();
        //    TCustomer cust = db.TCustomers.FirstOrDefault(t => t.FId == id);
        //    if (cust != null)
        //    {
        //        db.TCustomers.Remove(cust);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}




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

