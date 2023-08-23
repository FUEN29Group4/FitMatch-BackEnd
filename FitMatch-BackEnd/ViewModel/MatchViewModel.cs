using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitMatch_BackEnd.ViewModel
{
    public class MatchViewModel
    {
        public int ClassId { get; set; }
        public int TrainerId { get; set; }
        public string ClassName { get; set; }
        public DateTime BuildTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MemberId { get; set; }

        public string MemberName { get; set; }
        public string TrainerName { get; set; }
        public string GymName { get; set; }
        public string CourseStatus {get; set; }
        public int GymId { get; set; }

        public int ClassTypeId { get; set; }
        public List<SelectListItem> AvailableGyms { get; set; } // 新添加的属性
        public List<SelectListItem> AvailableClassType { get; set; } // 新添加的属性
        public List<SelectListItem> AvailableMember { get; set; } // 新添加的属性
        public List<SelectListItem> AvailableTrainer { get; set; } // 新添加的属性
        public int SelectedGymId { get; set; } // 用户选择的 GymId
        public int SelectedClassTypeId { get; set;}
        public int SelectedMemberId { get; set;}
        public int SelectedTrainerId { get; set; }




    }
}
