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
                                     TrainerId = t.TrainerId,
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

            var viewModel = (from c in _db.Classes
                             join ct in _db.ClassTypes on c.ClassTypeId equals ct.ClassTypeId
                             join m in _db.Members on c.MemberId equals m.MemberId
                             join t in _db.Trainers on c.TrainerId equals t.TrainerId
                             join g in _db.Gyms on c.GymId equals g.GymId
                             where c.ClassId == id
                             select new MatchViewModel
                             {
                                 ClassId = c.ClassId,
                                 ClassName = ct.ClassName,
                                 BuildTime = (DateTime)c.BuildTime,
                                 StartTime = (DateTime)c.StartTime,
                                 EndTime = (DateTime)c.EndTime,
                                 MemberName = m.MemberName,
                                 TrainerName = t.TrainerName,
                                 GymName = g.GymName,
                                 CourseStatus = c.CourseStatus,
                                 TrainerId = (int)c.TrainerId,
                                 MemberId = m.MemberId,
                                 GymId = g.GymId,
                                 ClassTypeId = (int)c.ClassTypeId
                             }).FirstOrDefault();

            if (viewModel == null)
            {
                return RedirectToAction("List");
            }

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Edit(MatchViewModel editedViewModel)
        {
            if (ModelState.IsValid)
            {
                // 获取要编辑的 Class 实体对象
                Class classData = _db.Classes.FirstOrDefault(c => c.ClassId == editedViewModel.ClassId);

                if (classData != null)
                {
                    // 根据编辑视图模型中的 ClassName 查询数据库，查找对应的 ClassType 实体对象
                    ClassType classTypeData = _db.ClassTypes.FirstOrDefault(ct => ct.ClassName == editedViewModel.ClassName);
                    Gym gymData = _db.Gyms.FirstOrDefault(g => g.GymId == editedViewModel.GymId);
                    Member member = _db.Members.FirstOrDefault(m=>m.MemberId == editedViewModel.MemberId);
                    Trainer trainer = _db.Trainers.FirstOrDefault(t=>t.TrainerId == editedViewModel.TrainerId);
                    if (classTypeData != null)
                    {
                        // 更新 Class 实体对象的 ClassTypeId 为选定的 ClassType 的 ClassTypeId
                        classData.ClassTypeId = classTypeData.ClassTypeId;
                        classData.GymId = gymData.GymId;
                        classData.MemberId = member.MemberId;
                        classData.TrainerId = trainer.TrainerId;
                        // 更新其他属性的修改
                        classData.StartTime = editedViewModel.StartTime;
                        classData.EndTime = editedViewModel.EndTime;
                        classData.BuildTime = editedViewModel.BuildTime;
                        // ... 根据需要应用其他属性的修改

                        // 保存更改到数据库
                        _db.SaveChanges();

                        return RedirectToAction("List"); // 重定向到列表视图
                    }
                    else
                    {
                        ModelState.AddModelError("", "无法找到匹配的 ClassType，请选择有效的 ClassName。");
                        // 返回编辑视图，显示错误消息
                        return View(editedViewModel);
                    }
                }
            }

            return View(editedViewModel); // 如果 ModelState 无效或编辑失败，显示编辑视图
        }

    }
}

