using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class EmployeeController : Controller
    {
        //TODO: 員工要做CRUD!!!!

        //注入DB 可以在很多方法用他來連結資料庫
        private readonly FitMatchDbContext _context;

        //連結DB
        public EmployeeController(FitMatchDbContext context)
        {
            _context = context;
        }
        //跟會員資料連結然後呈現出views
        public IActionResult Employee()
        {
            //資料庫連接 =>實體化DB
            //FitMatchDbContext db = new FitMatchDbContext();

            //資料定義與處理
            //- Member Model(定義資料類型):Member class 
            //- FitMAtchDBContext(操作資料表):public virtual DbSet<Member> Members { get; set; } //Members 集合實體
            //IEnumerable<Employee> datas = from d in db.Employees select d; //

            //return View(datas);
            IEnumerable<Employee> datas = from p in _context.Employees select p;
            return View(datas);
        }

        //員工新增
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Employee e)
        {
            //使用ajax
            if (ModelState.IsValid) // 驗證模型
            {
                _context.Employees.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction("Employee");
            }
            return View(e); // 如果模型無效，返回到輸入表單

        }


        //TODO: 員工篩選、全部刪除、性別轉換、在職轉換

        //審核通過
        //public IActionResult Approve(int id)
        //{
        //var Employees = _context.Employees.Find(id);
        ////檢查教練是否存在
        //if (Employees == null)
        //{
        //    return NotFound();
        //}
        ////有教練就審核通過
        //Employees.Approved = CApprovalStatus.Approved;
        ////保存到資料庫
        //_context.SaveChanges();
        ////返回教練列表
        //return RedirectToAction("Trainer");
        //}

        //TODO:  要做確認按鈕!!!!!!!!!!!!!!!

        //教練履歷
        public IActionResult Details(int id)
        {
            var Employees = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (Employees == null)
            {
                return View("Error");
            }
            return View(Employees);
        }
    }
}
