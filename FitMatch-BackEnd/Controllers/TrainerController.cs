using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class TrainerController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //彥儀試連 需要可取用
        //跟教練資料連結
        public IActionResult Trainer()
        {
            //資料庫連接
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Trainer> datas = from p in db.Trainers select p;
            return View(datas);
        }
        public enum ApprovalStatus
        {
            UnderReview = 0, // 審核中
            Approved = 1,    // 審核通過
            Rejected = 2     // 退回審核
        }
    }
}
