﻿using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class ClassType
{
    public int ClassTypeId { get; set; }

    public string? ClassName { get; set; }

    public string? Photo { get; set; }

    public string? Introduction { get; set; }

    public int? Status { get; set; }

}
