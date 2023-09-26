using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FitMatch_BackEnd.Models;

public partial class Class
{
    public int ClassId { get; set; }


    public int? TrainerId { get; set; }

    public int? GymId { get; set; }

    public int MemberId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
    public int? CourseUnitPrice { get; set; }
    public int? VenueReservationID { get; set; }
    public DateTime? BuildTime { get; set; }
    public string? CourseStatus { get; set; }
    [DisplayName("課程狀態")]



    public virtual ICollection<MemberFavorite> MemberFavorites { get; set; } = new List<MemberFavorite>();

    public Member Member { get; set; }

    public Trainer Trainer { get; set; }

    public Gym Gym { get; set; }
}
