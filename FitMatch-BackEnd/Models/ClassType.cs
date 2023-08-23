﻿using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class ClassType
{
    public int ClassTypeId { get; set; }

    public string? ClassName { get; set; }

    public string? Photo { get; set; }

    public string? Introduction { get; set; }

    public DateTime? CreateAt { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
