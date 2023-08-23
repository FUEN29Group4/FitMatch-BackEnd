using Microsoft.AspNetCore.Mvc;
using FitMatch_BackEnd.Models;
using System.Linq;
using FitMatch_BackEnd.ViewModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitMatch_BackEnd.Controllers
{
    public class MatchController : Controller
    {
        private readonly FitMatchDbContext _db;

        public MatchController(FitMatchDbContext db)
        {
            _db = db;
        }

        public IActionResult List(string searchField, string searchKeyword, DateTime? start, DateTime? end, string CourseStatus)
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
            // 在篩選過程中，僅當選擇了課程狀態時才進行過濾
            if (!string.IsNullOrEmpty(CourseStatus))
            {
                viewModelList = viewModelList.Where(vm => vm.CourseStatus == CourseStatus).ToList();
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
            //Gym設定
            var availableGyms = _db.Gyms.ToList();
            viewModel.AvailableGyms = availableGyms.Select(g => new SelectListItem { Text = g.GymName, Value = g.GymId.ToString() }).ToList();
            viewModel.SelectedGymId = viewModel.GymId; // 设置默认选择的 GymId
            //ClassType設定
            var availableClassType = _db.ClassTypes.ToList();
            viewModel.AvailableClassType = availableClassType.Select(ct => new SelectListItem {Text = ct.ClassName, Value = ct.ClassTypeId.ToString() }).ToList();
            viewModel.SelectedClassTypeId = viewModel.ClassTypeId; // 设置默认选择的 classTypeId
            //Member設定
            var availableMember = _db.Members.ToList();
            viewModel.AvailableMember = availableMember.Select(m => new SelectListItem { Text = m.MemberName, Value = m.MemberId.ToString() }).ToList();
            viewModel.SelectedMemberId = viewModel.MemberId; // 设置默认选择的 MemberId
            //Trainer設定
            var availableTrainer = _db.Trainers.ToList();
            viewModel.AvailableTrainer = availableTrainer.Select(t => new SelectListItem { Text = t.TrainerName, Value = t.TrainerId.ToString() }).ToList();
            viewModel.SelectedTrainerId = viewModel.TrainerId; // 设置默认选择的 TrainerId



            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Edit(MatchViewModel editedViewModel)
        {

                // 获取要编辑的 Class 实体对象
                Class classData = _db.Classes.FirstOrDefault(c => c.ClassId == editedViewModel.ClassId);

                if (classData != null)
                {
                    // 根据编辑视图模型中的 ClassName 查询数据库，查找对应的 ClassType 实体对象
                    ClassType classTypeData = _db.ClassTypes.FirstOrDefault(ct => ct.ClassTypeId == editedViewModel.SelectedClassTypeId);
                    Gym gymData = _db.Gyms.FirstOrDefault(g => g.GymId == editedViewModel.SelectedGymId);
                    Member member = _db.Members.FirstOrDefault(m=>m.MemberId == editedViewModel.SelectedMemberId);
                    Trainer trainer = _db.Trainers.FirstOrDefault(t=>t.TrainerId == editedViewModel.SelectedTrainerId);
                    if (classTypeData != null)
                    {
                        // 更新 Class 实体对象的 ClassTypeId 为选定的 ClassType 的 ClassTypeId
                        classData.ClassTypeId = classTypeData.ClassTypeId;
                        classData.GymId = gymData.GymId;
                        classData.MemberId = member.MemberId;
                        classData.TrainerId = trainer.TrainerId;
                    classData.CourseStatus = editedViewModel.CourseStatus;
                        // 更新其他属性的修改
                        classData.StartTime = editedViewModel.StartTime;
                        classData.EndTime = editedViewModel.EndTime;
                    classData.BuildTime = DateTime.Now;
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

            return View(editedViewModel); // 如果 ModelState 无效或编辑失败，显示编辑视图
        }

        public IActionResult Delete(int id)
        {
            // 在此方法中，您可以查找要刪除的 Class，然後執行刪除操作
            var classToDelete = _db.Classes.FirstOrDefault(c => c.ClassId == id);

            if (classToDelete != null)
            {
                // 執行刪除操作
                _db.Classes.Remove(classToDelete);
                _db.SaveChanges();

                return RedirectToAction("List"); // 重定向到列表視圖
            }
            else
            {
                return RedirectToAction("List"); // 如果找不到要刪除的對象，也可以重定向到列表視圖
            }
        }

    }
}

