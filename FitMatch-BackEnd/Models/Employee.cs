﻿using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public bool? Gender { get; set; }

    public DateTime? Birth { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Photo { get; set; }

    public string? Position { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }
    public bool Status { get; set; }
}
