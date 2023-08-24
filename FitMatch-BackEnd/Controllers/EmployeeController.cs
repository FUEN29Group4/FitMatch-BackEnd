using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FitMatch_BackEnd.Controllers
{
    public class EmployeeController : Controller
    {
        //注入DB 可以在很多方法用他來連結資料庫
        private readonly FitMatchDbContext _context;

        //連結DB
        public EmployeeController(FitMatchDbContext context)
        {
            _context = context;
        }
        //跟員工資料連結然後呈現出views
        public IActionResult Employee(int currentPage = 1, string txtKeyword = null, bool? employeeStatus = null)
        {
            int itemsPerPage = 5;
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
            var employee = new Employee
            {
                Status = true // 預設"在職中"
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Employee e)
        {
            //使用ajax
            if (ModelState.IsValid)
            {
                e.CreatedAt = DateTime.Now; // 設置當前的日期和時間
                e.Status = true; // 新增時在職狀況預設為在職中
                _context.Employees.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction("Employee");
            }
            return View(e);

        }


        //TODO: 全部刪除、照片、驗證

        public IActionResult Details(int id)
        {
            var Employees = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (Employees == null)
            {
                return View("Error");
            }
            return View(Employees);
        }
        //TODO: 跳提醒 已刪除成功
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Employee");
            //var Employees = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            Employee e = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);

            if (e != null)
            {
                _context.Employees.Remove(e);
                _context.SaveChanges();
            }
            return RedirectToAction("Employee");
        }
       
        //TODO: 要確認編輯
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
        public IActionResult Edit(Employee custIn)
        {
            Employee e = _context.Employees.FirstOrDefault(t => t.EmployeeId == custIn.EmployeeId);

            if (e != null)
            {
                e.EmployeeName = custIn.EmployeeName;
                e.Phone = custIn.Phone;
                e.Email = custIn.Email;
                e.Address = custIn.Address;
                e.Password = custIn.Password;
                e.Gender = custIn.Gender;
                //e.CreatedAt = custIn.CreatedAt;
                e.Photo = custIn.Photo;
                e.Birth = custIn.Birth;
                e.Status = custIn.Status;
                _context.SaveChanges();
            }
            return RedirectToAction("Employee");
        }
    }
}
