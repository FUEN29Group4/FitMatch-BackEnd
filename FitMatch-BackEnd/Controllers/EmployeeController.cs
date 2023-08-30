using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace FitMatch_BackEnd.Controllers
{
    public class EmployeeController : SuperController

    {
        //注入DB 可以在很多方法用他來連結資料庫
        private readonly FitMatchDbContext _context;
        //讀取web根目錄
        public IWebHostEnvironment _enviro = null;
        public EmployeeController(IWebHostEnvironment enviro, FitMatchDbContext context)
        {
            _enviro = enviro;
            //連結DB
            _context = context;
        }
        
        //public EmployeeController(FitMatchDbContext context)
        //{
        //    _context = context;
        //}
        //跟員工資料連結然後呈現出views
        public IActionResult Employee(int currentPage = 1, string txtKeyword = null, bool? employeeStatus = null)
        {
            int itemsPerPage = 8;
            IEnumerable<Employee> datas = from p in _context.Employees select p;
            // 如果有搜尋關鍵字
            if (!string.IsNullOrWhiteSpace(txtKeyword))
            {
                datas = datas.Where(p => p.EmployeeName.Contains(txtKeyword) || p.Email.Contains(txtKeyword));
            }
            // 如果Status有值
            if (employeeStatus.HasValue)
            {
                datas = datas.Where(p => p.Status == employeeStatus.Value);
            }
            //把選擇的狀態存進來
            ViewBag.EmployeeStatus = employeeStatus;

            // 根據當下頁碼獲取數據
            datas = datas.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);

            int totalDataCount = _context.Employees.Count();
            int totalPages = (totalDataCount + itemsPerPage - 1) / itemsPerPage;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = currentPage;
            ViewBag.Keyword = txtKeyword;  // 將關鍵字存入ViewBag，以便在View中使用
            return View(datas);
        }

        //員工新增
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee e)
        {

            if (e.FileToUpload == null)
            {
                ModelState.Remove("FileToUpload");
            }
            //使用ajax
            if (ModelState.IsValid)
            {

                if (e.FileToUpload != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/員工", photoName);
                    // 保存照片
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await e.FileToUpload.CopyToAsync(stream);
                    }
                    // 保存新照片名存到DB
                    e.Photo = photoName;
                }
               
                e.CreatedAt = DateTime.Now; // 設置當前的日期和時間
                e.Status = true; // 新增時在職狀況預設為在職中
                _context.Employees.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction("Employee");
            }
            return View(e);
        }



        public IActionResult Details(int id)
        {
            var Employees = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (Employees == null)
            {
                return View("Error");
            }
            return View(Employees);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Employee");

            Employee e = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (e != null)
            {
                _context.Employees.Remove(e);
                _context.SaveChanges();

                return RedirectToAction("Employee", new { deletedId = id });
            }

            return RedirectToAction("Employee");
        }
       
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Employee");
            Employee e = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (e == null)
                return RedirectToAction("Employee");
            return View(e);
        }

        [HttpPost]
       
        public async Task<IActionResult> Edit(CEmployeeWrap prodIn)
        {
            Employee e = await _context.Employees.FirstOrDefaultAsync(t => t.EmployeeId == prodIn.EmployeeID);
            //檢查
            if (e == null)
            {
                return NotFound(); // 返回404 Not Found
            }
            if (prodIn != null)
            {
                if (prodIn.photo != null && prodIn.photo.Length > 0)
                {
                    // 如果之前有舊圖片，先刪除
                    if (!string.IsNullOrEmpty(e.Photo))
                    {
                        var oldPath = Path.Combine(_enviro.WebRootPath, "img", "員工", e.Photo);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/img/員工/" + photoName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await prodIn.photo.CopyToAsync(fileStream);
                    }
                    //prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
                    e.Photo = photoName;
                }
                e.EmployeeName = prodIn.EmployeeName;
                e.Gender = prodIn.Gender;
                e.Birth = prodIn.Birth;
                e.Phone = prodIn.Phone;
                e.Address = prodIn.Address;
                e.Email = prodIn.Email;
                e.Position = prodIn.Position;
                e.Password = prodIn.Password;
                e.Status = prodIn.Status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Employee");
        }

    }
}
