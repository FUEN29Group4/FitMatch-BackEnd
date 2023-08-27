using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FitMatch_BackEnd.Controllers
{
    public class OrderController : Controller
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public OrderController(FitMatchDbContext context)
        {
            _context = context;
        }

        //取得訂單管理頁面
        public IActionResult List(CKeywordViewModel vm)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<Order> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from p in db.Orders
                        select p;
            else
                datas = db.Orders.Where(t => t.MemberId.ToString().Contains(vm.txtKeyword));
            return View(datas);
        }

        //刪除商品 
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            FitMatchDbContext db = new FitMatchDbContext();
            Order cust = db.Orders.FirstOrDefault(t => t.OrderId == id);
            if (cust != null)
            {
                db.Orders.Remove(cust);
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
            Order prod = db.Orders.FirstOrDefault(t => t.OrderId == id);
            if (prod == null)
                return RedirectToAction("List");

            return View(prod);

        }
        [HttpPost]
        public IActionResult Edit(Order prodIn)
        {
            FitMatchDbContext db = new FitMatchDbContext();
            Order prodDb = db.Orders.FirstOrDefault(t => t.OrderId == prodIn.OrderId);

            if (prodDb != null)
            {
                // 更新其他屬性
                prodDb.MemberId = prodIn.MemberId;
                prodDb.MemberName = prodIn.MemberName;
                prodDb.TotalPrice = prodIn.TotalPrice;
                prodDb.PaymentMethod = prodIn.PaymentMethod;
                prodDb.ShippingMethod = prodIn.ShippingMethod;
                prodDb.OrderTime = prodIn.OrderTime;

                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        //訂單明細
        public IActionResult OrderDetails(int id)
        {
               var viewModelList = (from c in _context.OrderDetails
                                 join m in _context.Orders on c.OrderId equals m.OrderId
                                 join h in _context.Members on m.MemberId equals h.MemberId
                                 join t in _context.Products on c.ProductId equals t.ProductId
                                 join g in _context.ProductTypes on t.TypeId equals g.TypeId
                       

                                 select new OrderViewModel
                                 {
                                     OrderId = (int)c.OrderId,
                                     MemberId = (int)m.MemberId,
                                     TypeId= (int)g.TypeId,
                                     TypeName = g.TypeName,
                                     Phone = h.Phone,
                                     Address = h.Address,
                                     TotalPrice = m.TotalPrice,
                                     Photo = t.Photo,
                                     ProductId = t.ProductId,
                                     ProductName = t.ProductName,
                                     OrderTime = (DateTime)m.OrderTime,
                                     PayTime = (DateTime)m.PayTime,
                                     MemberName = m.MemberName,
                                     Quantity = (int)c.Quantity,
                                     price = (int)t.Price
                                 }).ToList();

            return View(viewModelList);
        }
    }
}
