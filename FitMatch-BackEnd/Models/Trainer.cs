using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using FitMatch_BackEnd.Models;
using static FitMatch_BackEnd.Controllers.TrainerController;

namespace FitMatch_BackEnd.Models;

public partial class Trainer
{
    public int TrainerId { get; set; }
    [DisplayName("教練編號")]
    public string? TrainerName { get; set; }
    [DisplayName("教練姓名")]
    public bool? Gender { get; set; }
    [DisplayName("性別")]
    public DateTime? Birth { get; set; }
    [DisplayName("生日")]
    public string? Phone { get; set; }
    [DisplayName("電話")]
    public string? Address { get; set; }
    [DisplayName("地址")]
    public string? Email { get; set; }
    public string? Photo { get; set; }
    public string? Certificate { get; set; }
    [DisplayName("專業證照")]
    public string? Expertise { get; set; }
    [DisplayName("專長")]
    public string? Experience { get; set; }
    [DisplayName("經歷")]
    public int? CourseFee { get; set; }
    [DisplayName("課程費用")]
    public string? Password { get; set; }
    public int? Approved { get; set; }
    //public CApprovalStatus? Approved { get; set; }    //判斷審核通過與否

    public DateTime CreatedAt { get; set; }
    [DisplayName("申請時間")]
    public string CreatedAtFormatted => CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");

    public string? Introduce { get; set; }
    [DisplayName("介紹")]
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

}
