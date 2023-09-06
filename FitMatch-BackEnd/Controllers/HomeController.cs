using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FitMatch_BackEnd.ViewModel;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace FitMatch_BackEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string userid = HttpContext.Session.GetString("User");
                CUserViewModel user = JsonSerializer.Deserialize<CUserViewModel>(userid);

                var viewModel = new CListViewModel
                {
                    User = user
                };

                return View(viewModel);
            }
            return RedirectToAction("Login");
        }


        public ActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(CLoginViewModel vm)
        {
            string result256 = Get_SHA256_Hash(vm.txtPassword).ToUpper();
            vm.txtPassword = result256.PadRight(16);


            Employee user = (new FitMatchDbContext()).Employees.FirstOrDefault(t => t.Email.Equals(vm.txtAccount) && t.Password.Equals(vm.txtPassword));
            if (user != null && user.Password.Equals(vm.txtPassword) && user.Status == true)
            {
                //if (user == null)
                //{
                //    return RedirectToAction("Login");
                //}
                string json = "";
                CUserViewModel userviewModel = new CUserViewModel();

                userviewModel.UserId = user.EmployeeId;
                userviewModel.UserName = user.EmployeeName;
                userviewModel.UserPhoto = user.Photo;

                json = JsonSerializer.Serialize(userviewModel);
                HttpContext.Session.SetString("User", json);

                string jsonlogin = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, jsonlogin);


                //return RedirectToAction("Index", user);
                _logger.LogInformation("Redirecting to Index");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "");
            return PartialView("Login", ModelState);
        }


        public static string Get_SHA256_Hash(string value)
        {
            using var hash = SHA256.Create();
            var byteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(byteArray).ToLower();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}