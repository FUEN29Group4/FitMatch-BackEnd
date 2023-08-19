using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitMatch_BackEnd.Controllers
{
    public class MemberController : Controller
    {
        // GET: /<controller>/  預設先註解掉
        public IActionResult Index()
        {
            return View();
        }

        //--------- M-0:欣彤試連，會員DB資料連結test （SQL 要修修）
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


        //--------- M-1:會員管理列表（檢視list）=> 前端畫面 index_Backage_member_management.html




        //--------- M-2:會員新增（Create）






        //--------- M-3:會員修改（Update）







        //--------- M-4:會員刪除（Delete）

    }
}

