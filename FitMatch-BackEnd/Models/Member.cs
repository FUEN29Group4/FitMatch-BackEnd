using System;
using System.Collections.Generic;

//驗證增加此兩行
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMatch_BackEnd.Models;

public partial class Member
{
    public int MemberId { get; set; }



   
    public string? MemberName { get; set; }

   

    public bool? Gender { get; set; }

    public DateTime? Birth { get; set; }

    
    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Photo { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<MemberFavorite> MemberFavorites { get; set; } = new List<MemberFavorite>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();


    //public GenderType MGender { get; set; }

}

//public enum GenderType
//{
//    Male,
//    Female
//}
