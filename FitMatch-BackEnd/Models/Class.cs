﻿using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public int? ClassTypeId { get; set; }

    public int? TrainerDetailId { get; set; }

    public int? GymId { get; set; }

    public int? MemberId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public bool? Approved { get; set; }

    public virtual ClassType? ClassType { get; set; }

    public virtual ICollection<MemberFavorite> MemberFavorites { get; set; } = new List<MemberFavorite>();

    public virtual Trainer? TrainerDetail { get; set; }
}