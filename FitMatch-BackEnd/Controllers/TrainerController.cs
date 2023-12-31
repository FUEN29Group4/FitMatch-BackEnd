﻿using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using X.PagedList;
using X.PagedList.Mvc.Core;
using System.Drawing.Printing;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;




namespace FitMatch_BackEnd.Controllers
{
    public class TrainerController : SuperController
    {
        //注入DB 可以在很多方法用他來連結資料庫
        private readonly FitMatchDbContext _context;
        public IWebHostEnvironment _enviro = null;


        //連結DB
        public TrainerController(FitMatchDbContext context)
        {
            _context = context;
        }
        //跟教練資料連結然後呈現出views
        public IActionResult Trainer(int currentPage, string txtKeyword, int? trainerApproved = null, string? city = "", int? approved = null)
        {
            int itemsPerPage = 8;
            //IQueryable<Trainer> datas = from p in _context.Trainers select p;
            //倒序
            IQueryable<Trainer> datas = from p in _context.Trainers orderby p.TrainerId descending select p; // Step 2

            if (!string.IsNullOrWhiteSpace(txtKeyword))
            {
                datas = datas.Where(p => p.TrainerName.Contains(txtKeyword));
            }

            if (!string.IsNullOrEmpty(city))
            {
                datas = datas.Where(p => p.Address.StartsWith(city));
            }

            if (approved.HasValue)
            {
                datas = datas.Where(p => p.Approved == approved.Value);
            }


            int totalDataCount = datas.Count(); // 使用篩選後的資料計算總數
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;
            int validCurrentPage = Math.Max(1, Math.Min(currentPage, totalPages));

            // 在這裡將篩選後的資料進行分頁
            IPagedList<Trainer> pagedData = datas.ToPagedList(validCurrentPage, itemsPerPage);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = validCurrentPage;
            ViewBag.Keyword = txtKeyword;
            ViewBag.City = city;

            return View(pagedData);
        }


        //審核通過
        public async Task<IActionResult> Approve(int id)
        {
            var trainer = _context.Trainers.Find(id);
            //檢查教練是否存在
            if (trainer == null)
            {
                return NotFound();
            }
            //有教練就審核通過
            //trainer.Approved = CApprovalStatus.Approved;
            trainer.Approved = 1;
            //保存到資料庫
            _context.SaveChanges();
            //延遲1.5秒
            await Task.Delay(1500);
            //返回教練列表
            return RedirectToAction("Trainer");
        }
        ////退回申請
        public async Task<IActionResult> Reject(int id)
        {
            var trainer = _context.Trainers.Find(id);
            //檢查教練是否存在
            if (trainer == null)
            {
                return NotFound();
            }
            //教練的申請拒絕
            //trainer.Approved = CApprovalStatus.Rejected;
            trainer.Approved = 2;
            _context.SaveChanges();
            //延遲1.5秒
            await Task.Delay(1500);
            return RedirectToAction("Trainer");
        }

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

        //暫放 讓照片變成base64吃進資料庫

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int trainerId, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                Trainer e = await _context.Trainers.FirstOrDefaultAsync(t => t.TrainerId == trainerId);
                if (e == null)
                {
                    return NotFound(); // 返回404 Not Found
                }

                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        e.Photo = base64String;
                    }

                // 保存更改
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Trainer");
        }


        public class CTrainer
        {
            public int TrainerId { get; set; }
            public string? Photo { get; set; }
            public IFormFile photo { get; set; }
        }
    }
}
