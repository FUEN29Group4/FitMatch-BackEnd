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
    public class ProductController : SuperController
    {
        //連結DB
        private readonly FitMatchDbContext _context;


        public ProductController(FitMatchDbContext context, IWebHostEnvironment p)
        {
            _context = context;
            _enviro = p;
        }

        public IWebHostEnvironment _enviro = null;

        //***篩選功能***

        //取得商品管理頁面  ***上架狀態要修***圖片未抓取***
        public IActionResult List(CKeywordViewModel vm, int currentPage = 1)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Product> datas = db.Products;


            if (vm.txtKeyword != null || vm.StatusFilter != null || vm.ProductFilter != null)
            {
                if (!string.IsNullOrEmpty(vm?.StatusFilter))
                {
                    switch (vm.StatusFilter)
                    {
                        case "上架":
                            datas = db.Products.Where(t => t.Status == true);
                            break;
                        case "下架":
                            datas = db.Products.Where(t => t.Status == false);
                            break;
                    }
                    if (!string.IsNullOrEmpty(vm?.ProductFilter))
                    {
                        datas = datas.Where(t => t.TypeId.ToString().Contains(vm.ProductFilter));
                    }
                    if (!string.IsNullOrEmpty(vm?.txtKeyword))
                    {
                        datas = datas.Where(t => t.ProductName.Contains(vm.txtKeyword));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(vm?.ProductFilter))
                    {
                        datas = db.Products.Where(t => t.TypeId.ToString().Contains(vm.ProductFilter));
                        if (!string.IsNullOrEmpty(vm?.txtKeyword))
                        {
                            datas = datas.Where(t => t.ProductName.Contains(vm.txtKeyword));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(vm?.txtKeyword))
                        {
                            datas = db.Products.Where(t => t.ProductName.Contains(vm.txtKeyword));
                        }
                    }

                }
            }
            else
            {
                datas = from p in db.Products
                        select p;
            }

            int itemsPerPage = 8;

            // 根據當下頁碼獲取datas
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = _context.Products.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            return View(datas);

        }

        public class CKeywordViewModel
        {
            public string txtKeyword { get; set; }
            public string ProductFilter { get; set; }
            public string StatusFilter { get; set; }  // 使用 string
        }

        //新增商品  ***圖片未寫入，商品類別無法寫入***

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product p)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            db.Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        //刪除商品 
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Product cust = db.Products.FirstOrDefault(t => t.ProductId == id);
            if (cust != null)
            {
                db.Products.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        //修改   ***圖片未寫入，商品類別無法寫入***
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Product prod = db.Products.FirstOrDefault(t => t.ProductId == id);
            if (prod == null)
                return RedirectToAction("List");

            return View(prod);

        }
        [HttpPost]
        public IActionResult Edit(Product prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Product prodDb = db.Products.FirstOrDefault(t => t.ProductId == prodIn.ProductId);

            if (prodDb != null)
            {
                // 更新其他屬性
                prodDb.ProductName = prodIn.ProductName;
                prodDb.Photo = prodIn.Photo;
                prodDb.ProductDescription = prodIn.ProductDescription;
                prodDb.Price = prodIn.Price;
                prodDb.ProductInventory = prodIn.ProductInventory;
                prodDb.Approved = prodIn.Approved;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}