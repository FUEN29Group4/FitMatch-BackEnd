using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitMatch_BackEnd.Models;


public partial class Gym
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GymId { get; set; }

    [Required(ErrorMessage = "場館名稱是必填的")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "場館名稱應在5到50個字之間")]
    public string? GymName { get; set; }
 
    [Required(ErrorMessage = "場館電話是必填的")]
    [RegularExpression(@"^[0-9]{2}-[0-9]{7}$", ErrorMessage = "電話號碼格式不正確 正確格式為##-#######")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "場館地址是必填的")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "地址應在10到100個字之間")]
    public string? Address { get; set; }

    public DateTime? OpentimeStart { get; set; }

    public DateTime? OpentimeEnd { get; set; }

    public bool? Approved { get; set; }

    [Required(ErrorMessage = "場館簡介是必填的")]
    [StringLength(500, MinimumLength = 20, ErrorMessage = "場館簡介應在20到500個字符之間")]
    public string? GymDescription { get; set; }

    [NotMapped] 
    public IFormFile FileToUpload { get; set; }
    public string? Photo { get; set; }

 
}
