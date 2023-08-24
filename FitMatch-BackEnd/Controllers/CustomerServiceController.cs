using FitMatch_BackEnd.Models;
using FitMatch_BackEnd.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitMatch_BackEnd.Controllers
{
    public class CustomerServiceController : Controller
    {
     

        private IWebHostEnvironment _enviro = null;

        public CustomerServiceController(IWebHostEnvironment p)
        {
            _enviro = p;
        }



        public IActionResult CustomerService (CKeywordViewModel vm)
        {

            FitMatchDbContext db = new FitMatchDbContext();
            IEnumerable<CustomerService> datas = from c in db.CustomerServices select c;
            return View(datas);

            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from c in db.CustomerServices
                        select c;
            else
                datas = db.CustomerServices.Where(t => t.MessageContent.Contains(vm.txtKeyword));
            return View(datas);
        }


       

        public IActionResult RobotDelete(int? id)
        {
            if (id == null)
                return RedirectToAction("Robot");
            FitMatchDbContext db = new FitMatchDbContext();
            Robot cust = db.Robots.FirstOrDefault(t => t.RobotId == id);
            if (cust != null)
            {
                db.Robots.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("Robot");
        }



    }
}
