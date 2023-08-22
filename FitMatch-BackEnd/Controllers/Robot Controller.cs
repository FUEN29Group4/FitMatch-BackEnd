//using FitMatch_BackEnd.Models;
//using FitMatch_BackEnd.ViewModel;
//using Microsoft.AspNetCore.Mvc;

//namespace FitMatch_BackEnd.Controllers
//{
//    public class Robot_Controller : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }

//        private IWebHostEnvironment _enviro = null;
//        public ProductController(IWebHostEnvironment p)
//        {
//            _enviro = p;
//        }
//        public IActionResult List(CKeywordViewModel vm)
//        {

//            DbDemoContext db = new DbDemoContext();
//            IEnumerable<TProduct> datas = null;
//            if (string.IsNullOrEmpty(vm.txtKeyword))
//                datas = from p in db.TProducts
//                        select p;
//            else
//                datas = db.TProducts.Where(t => t.FName.Contains(vm.txtKeyword));
//            return View(datas);
//        }
//        public IActionResult Create()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult Create(TProduct p)
//        {
//            DbDemoContext db = new DbDemoContext();
//            db.TProducts.Add(p);
//            db.SaveChanges();
//            return RedirectToAction("List");
//        }
//        public IActionResult Delete(int? id)
//        {
//            if (id == null)
//                return RedirectToAction("List");
//            DbDemoContext db = new DbDemoContext();
//            TProduct cust = db.TProducts.FirstOrDefault(t => t.FId == id);
//            if (cust != null)
//            {
//                db.TProducts.Remove(cust);
//                db.SaveChanges();
//            }
//            return RedirectToAction("List");
//        }
//        public IActionResult Edit(int? id)
//        {
//            if (id == null)
//                return RedirectToAction("List");
//            DbDemoContext db = new DbDemoContext();
//            TProduct prod = db.TProducts.FirstOrDefault(t => t.FId == id);
//            if (prod == null)
//                return RedirectToAction("List");
//            CProductWrap prodWp = new CProductWrap();
//            prodWp.product = prod;
//            return View(prodWp);
//        }
//        [HttpPost]
//        public IActionResult Edit(CProductWrap prodIn)
//        {
//            DbDemoContext db = new DbDemoContext();
//            TProduct prodDb = db.TProducts.FirstOrDefault(t => t.FId == prodIn.FId);

//            if (prodDb != null)
//            {
//                if (prodIn.photo != null)
//                {
//                    string photoName = Guid.NewGuid().ToString() + ".jpg";
//                    string path = _enviro.WebRootPath + "/images/" + photoName;
//                    prodIn.photo.CopyTo(new FileStream(path, FileMode.Create));
//                    prodDb.FImagePath = photoName;
//                }

//                prodDb.FName = prodIn.FName;
//                prodDb.FQty = prodIn.FQty;
//                prodDb.FCost = prodIn.FCost;
//                prodDb.FPrice = prodIn.FPrice;

//                db.SaveChanges();
//            }
//            return RedirectToAction("List");
//        }
//    }
//}
