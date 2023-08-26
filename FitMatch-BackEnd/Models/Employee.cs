using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMatch_BackEnd.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "*")]
    [MinLength(2, ErrorMessage = "至少2個字")]
    [RegularExpression(@"^[\u4e00-\u9fa5]+$", ErrorMessage = "請輸入中文")]
    public string? EmployeeName { get; set; }
    [Required(ErrorMessage = "*")]
    public bool? Gender { get; set; }
    [Required(ErrorMessage = "*")]

    public DateTime? Birth { get; set; }
    [Required(ErrorMessage = "*")]
    [RegularExpression(@"^[0-9-+\s()]*$", ErrorMessage = "請輸入數字")]

    public string? Phone { get; set; }
    [Required(ErrorMessage = "*")]

    public string? Address { get; set; }
    [Required(ErrorMessage = "*")]
    [EmailAddress(ErrorMessage = "請輸入正確的Email")]

    public string? Email { get; set; }

    [NotMapped]
    public IFormFile FileToUpload { get; set; }

    public string? Photo { get; set; }

    public string? Position { get; set; }
    [Required(ErrorMessage = "*")]
    [MinLength(8, ErrorMessage = "至少8個字")]


    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }
    public bool? Status { get; set; }



}
