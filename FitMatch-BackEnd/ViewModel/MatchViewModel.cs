﻿namespace FitMatch_BackEnd.ViewModel
{
    public class MatchViewModel
    {
        public int ClassId { get; set; }
        public int TrainerID { get; set; }
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

    }
}
