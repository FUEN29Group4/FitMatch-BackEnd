using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitMatch_BackEnd.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    [MinLength(2, ErrorMessage = "EmployeeName 至少需要2個字符。")]
    [RegularExpression(@"^[\u4e00-\u9fa5]+$", ErrorMessage = "EmployeeName 只能包含中文字符。")]
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
    public bool? Status { get; set; }
}
