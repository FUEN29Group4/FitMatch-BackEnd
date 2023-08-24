using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FitMatch_BackEnd.Controllers
{
    public class TrainerController : Controller
    {
        //TODO: 全選刪除還沒做
        //TODO: 照片、合作場館未完成

        //注入DB 可以在很多方法用他來連結資料庫
        private readonly FitMatchDbContext _context;

        //連結DB
        public TrainerController(FitMatchDbContext context)
        {
            _context = context;
        }
        //TODO: 篩選列
        //跟教練資料連結然後呈現出views
        public IActionResult Trainer(int currentPage = 1, string txtKeyword = null)
        {
            int itemsPerPage = 5;
            IEnumerable<Trainer> datas = from p in _context.Trainers select p;
            // 如果有搜尋關鍵字
            if (!string.IsNullOrWhiteSpace(txtKeyword))
            {
                datas = datas.Where(p => p.TrainerName.Contains(txtKeyword) || p.Email.Contains(txtKeyword));
            }
            // 如果Approved有值
            //if (trainerapproved.HasValue)
            //{
            //    datas = datas.Where(p => p.approved = trainerapproved);
            //}
            //把選擇的狀態存進來
            //ViewBag.TrainerApproved = trainerapproved;

            // 根據當下頁碼獲取數據
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = _context.Trainers.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Keyword = txtKeyword;  // 將關鍵字存入ViewBag，以便在View中使用
            return View(datas);
            //int itemsPerPage = 5;
            //IEnumerable<Trainer> datas = from p in _context.Trainers select p;

            //// 根據當下頁碼獲取數據
            //datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            //int totalDataCount = _context.Trainers.Count();
            //int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            //ViewBag.TotalPages = totalPages;
            //ViewBag.CurrentPage = currentPage;
            //return View(datas);
            
        }

        //審核通過
        public IActionResult Approve(int id)
        {
            var trainer = _context.Trainers.Find(id);
            //檢查教練是否存在
            if (trainer == null)
            {
                return NotFound();
            }
            //有教練就審核通過
            trainer.Approved = CApprovalStatus.Approved;
            //保存到資料庫
            _context.SaveChanges();
            //返回教練列表
            return RedirectToAction("Trainer");
        }
        ////退回申請
        public IActionResult Reject(int id)
        {
            var trainer = _context.Trainers.Find(id);
            //檢查教練是否存在
            if (trainer == null)
            {
                return NotFound();
            }
            //教練的申請拒絕
            trainer.Approved = CApprovalStatus.Rejected;
            _context.SaveChanges();

            return RedirectToAction("Trainer");
        }
        //代辦事項: 要做確認按鈕!!!!!!!!!!!!!!!

        //教練履歷
        public IActionResult Details(int id)
        {
            var trainer = _context.Trainers.FirstOrDefault(t => t.TrainerId == id);
            if (trainer == null)
            {
                return View("Error");
            }
            return View(trainer);
        }
    }
}
