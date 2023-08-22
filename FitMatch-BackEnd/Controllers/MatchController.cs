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
                                     TrainerID =t.TrainerId,
                                     ClassTypeId= h.ClassTypeId
                                 }).ToList();
            if (start.HasValue && end.HasValue)
            {
                DateTime adjustedEndDate = end.Value.AddDays(1).Date; // 设置为当天的00:00:00
                viewModelList = viewModelList.Where(vm => vm.StartTime >= start.Value && vm.StartTime < adjustedEndDate).ToList();
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
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return RedirectToAction("List");
            }

            Class viewModel = _db.Classes.FirstOrDefault(c => c.ClassId == id);
            if (viewModel == null)
            {
                return RedirectToAction("List");
            }

            MatchViewModel matchViewModel = new MatchViewModel
            {
                ClassId = viewModel.ClassId,
                // Assign other properties from viewModel to matchViewModel
            };

            return View(matchViewModel);
        }


        [HttpPost]
        public IActionResult Edit(MatchViewModel editedViewModel)
        {
            if (ModelState.IsValid)
            {
                Class ClassData = _db.Classes.FirstOrDefault(c => c.ClassId == editedViewModel.ClassId);
                ClassType ClassTypeData = _db.ClassTypes.FirstOrDefault(c => c.ClassTypeId == editedViewModel.ClassTypeId); ;
                Member MemberData = _db.Members.FirstOrDefault(c => c.MemberId == editedViewModel.MemberId);
                Gym GymData = _db.Gyms.FirstOrDefault(c => c.GymId == editedViewModel.GymId);
                if (ClassData != null)
                {
                    // Update the existing data with the edited values

                    ClassTypeData.ClassName = editedViewModel.ClassName;
                    MemberData.MemberName = editedViewModel.MemberName;
                    GymData.GymName = editedViewModel.GymName;
                    ClassData.StartTime = editedViewModel.StartTime;
                    ClassData.EndTime = editedViewModel.EndTime;
                    ClassData.BuildTime = editedViewModel.BuildTime;

                    // Update other properties

                    _db.SaveChanges(); // Save changes to the database

                    return RedirectToAction("List"); // Redirect back to the list view
                }
            }

            return View(editedViewModel); // If ModelState is not valid or editing fails, show the edit view again
        }
    }
}
