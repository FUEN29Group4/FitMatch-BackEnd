using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitMatch_BackEnd.Controllers
{
    public class ArticleController : SuperController
    {
        private readonly FitMatchDbContext _context;

        //連結DB
        public ArticleController(FitMatchDbContext context)
        {
            _context = context;
        }

        //跟場館資料連結然後呈現出views
        public IActionResult Article()
        {

            IEnumerable<Article> datas = from p in _context.Articles select p;
            return View(datas);
        }
    }
}
