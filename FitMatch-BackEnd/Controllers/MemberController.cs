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
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        //欣彤試連 
        //跟會員資料連結
        public IActionResult Member()
        {
            //資料庫連接
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Member> datas = from p in db.Members select p;
            return View(datas);
        }
    }
}

