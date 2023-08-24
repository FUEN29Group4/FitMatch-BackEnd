using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FitMatch_BackEnd.Controllers
{
    public class OrderDetailController : SuperController
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public OrderDetailController(FitMatchDbContext context)
        {
            _context = context;
        }

        //取得訂單管理頁面
        public IActionResult List(CKeywordViewModel vm)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<OrderDetail> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.OrderDetails
                        select p;
            else
                datas = db.OrderDetails.Where(t => t.OrderDetailId.ToString().Contains(vm.txtKeyword));
            return View(datas);
        }
        public IActionResult Index()
        {
            return View();
        }
        //修改   ***圖片未寫入，商品類別無法寫入***
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            OrderDetail prod = db.OrderDetails.FirstOrDefault(t => t.OrderId == id);
            if (prod == null)
                return RedirectToAction("List");

            return View(prod);

        }
        [HttpPost]
        public IActionResult Edit(OrderDetail prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            OrderDetail prodDb = db.OrderDetails.FirstOrDefault(t => t.OrderId == prodIn.OrderId);

            if (prodDb != null)
            {
                // 更新其他屬性
                prodDb.OrderTime = prodIn.OrderTime;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
