using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList;

namespace FitMatch_BackEnd.Controllers
{
    public class OrderController : Controller
    {
        private readonly FitMatchDbContext _db;

        //連結DB
        public OrderController(FitMatchDbContext db)
        {
            _db = db;
        }
        //public IActionResult<OrderViewModel> GetFilteredData(string)
        //{

        //}
        //取得訂單管理頁面
        public IActionResult List()
        {
            int itemsPerPage = 5;

            //var viewModelList = GetFilteredData(searchField, searchKeyword, start, end, CourseStatus);

            //int totalDataCount = viewModelList.Count();
            //int totalPages = (int)Math.Ceiling((double)totalDataCount / itemsPerPage);
            //int validCurrentPage = Math.Max(1, Math.Min(currentPage, totalPages));

            //viewModelList = viewModelList.Skip((validCurrentPage - 1) * itemsPerPage).Take(itemsPerPage);

            //IPagedList<MatchViewModel> pagedData = new StaticPagedList<MatchViewModel>(viewModelList, validCurrentPage, itemsPerPage, totalDataCount);
            var OrderViewModelList = (from O in _db.Orders
                                      join M in _db.Members on O.MemberId equals M.MemberId
                                      select new OrderViewModel
                                      {
                                          OrderId = O.OrderId,
                                          MemberId = M.MemberId,
                                          TotalPrice = O.TotalPrice,
                                          PayTime = (DateTime)O.PayTime,
                                          ShippingMethod = O.ShippingMethod,
                                          PaymentMethod = O.PaymentMethod,
                                          MemberName = M.MemberName
                                      });
            return View(OrderViewModelList);

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
            var viewModelList = (from c in _db.OrderDetails
                                 join m in _db.Orders on c.OrderId equals m.OrderId
                                 join h in _db.Members on m.MemberId equals h.MemberId
                                 join t in _db.Products on c.ProductId equals t.ProductId
                                 join g in _db.ProductTypes on t.TypeId equals g.TypeId

     
                                 select new OrderViewModel
                                 {
                                     OrderId = (int)c.OrderId,
                                     MemberId = (int)m.MemberId,
                                     TypeId = (int)g.TypeId,
                                     TypeName = g.TypeName,
                                     Phone = h.Phone,
                                     Address = h.Address,
                                     TotalPrice = m.TotalPrice,
                                     Photo = t.Photo,
                                     ProductId = t.ProductId,
                                     ProductName = t.ProductName,
                                     OrderTime = (DateTime)m.OrderTime,
                                     PayTime = (DateTime)m.PayTime,
                                     MemberName = h.MemberName,
                                     Quantity = (int)c.Quantity,
                                     price = (int)t.Price


                                 }).ToList();

            var selectviewmodelList=viewModelList.Where(c=>c.OrderId== id).ToList();

            return View(selectviewmodelList);
        }
    }
}
