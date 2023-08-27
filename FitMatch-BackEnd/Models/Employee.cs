using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMatch_BackEnd.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }


    [Required(ErrorMessage = "姓名必填")]

    public string? EmployeeName { get; set; }
    [Required(ErrorMessage = "性別必填")]
    public bool? Gender { get; set; }
    [Required(ErrorMessage = "生日必填")]


    public DateTime? Birth { get; set; }
    [Required(ErrorMessage = "電話號碼必填")]
    [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機號碼格式不正確.")]

    public string? Phone { get; set; }
    [Required(ErrorMessage = "地址必填")]
    [MinLength(5, ErrorMessage = "地址格式不正確.")]

    public string? Address { get; set; }

    [Required(ErrorMessage = "電子信箱必填")]
    [EmailAddress(ErrorMessage = "電子信箱格式不正確")]

    public string? Email { get; set; }

    [NotMapped]
    public IFormFile FileToUpload { get; set; }

    public string? Photo { get; set; }

    public string? Position { get; set; }
    [Required(ErrorMessage = "密碼必填")]
    [MinLength(5, ErrorMessage = "密碼至少需要5個字")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "密碼只能包含英文和數字")]


    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }
    public bool? Status { get; set; }



}
