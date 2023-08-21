using Microsoft.AspNetCore.Mvc;
using FitMatch_BackEnd.Models;
using System.Linq;
using FitMatch_BackEnd.ViewModel;
using System.Diagnostics;

namespace FitMatch_BackEnd.Controllers
{
    public class MatchController : Controller
    {
        private readonly FitMatchDbContext _db;

        public MatchController(FitMatchDbContext db)
        {
            _db = db;
        }

        public IActionResult List(string searchField, string searchKeyword, DateTime? start, DateTime? end)
        {
            
            
            var viewModelList = (from c in _db.Classes
                                 join m in _db.Members on c.MemberId equals m.MemberId
                                 join t in _db.Trainers on c.TrainerId equals t.TrainerId
                                 join g in _db.Gyms on c.GymId equals g.GymId
                                 join h in _db.ClassTypes on c.ClassTypeId equals h.ClassTypeId

                                 select new MatchViewModel
                                 {
                                     ClassId = c.ClassId,
                                     MemberId = c.MemberId,
                                     ClassName = h.ClassName,
                                     BuildTime = (DateTime)c.BuildTime,
                                     StartTime = (DateTime)c.StartTime,
                                     EndTime = (DateTime)c.EndTime,
                                     MemberName = m.MemberName,
                                     TrainerName = t.TrainerName,
                                     GymName = g.GymName,
                                     CourseStatus=c.CourseStatus,
                                     TrainerID =t.TrainerId
                                 }).ToList();
            if (start.HasValue && end.HasValue)
            {
                viewModelList = viewModelList.Where(vm => vm.StartTime >= start.Value && vm.StartTime <= end.Value).ToList();
            }
            if (!string.IsNullOrEmpty(searchField) && !string.IsNullOrEmpty(searchKeyword))
            {
                switch (searchField)
                {
                    case "ClassName":
                        viewModelList = viewModelList.Where(vm => vm.ClassName.Contains(searchKeyword)).ToList();
                        break;
                    case "MemberName":
                        viewModelList = viewModelList.Where(vm => vm.MemberName.Contains(searchKeyword)).ToList();
                        break;
                    case "TrainerName":
                        viewModelList = viewModelList.Where(vm => vm.TrainerName.Contains(searchKeyword)).ToList();
                        break;
                    case "GymName":
                        viewModelList = viewModelList.Where(vm => vm.GymName.Contains(searchKeyword)).ToList();
                        break;
                    default:
                        // 默认处理
                        break;
                }
            }

            return View(viewModelList);
        }
    }
}
