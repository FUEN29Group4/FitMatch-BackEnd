using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMatch_BackEnd.Models;

public partial class Product
{

    [DisplayName("商品編號")]
    public int ProductId { get; set; }//商品編號

    [DisplayName("商品名稱")]
    public string? ProductName { get; set; }//商品名稱

    [DisplayName("商品描述")]
    public string? ProductDescription { get; set; }//商品描述

    

    [DisplayName("價格")]
    public int? Price { get; set; }//價格

    [DisplayName("商品類別編號")]
    public int? TypeId { get; set; }//商品類別編號

    //[DisplayName("商品類別名稱")]
    //public string? TypeName { get; set; }

    [DisplayName("商品庫存")]
    public int? ProductInventory { get; set; }//商品庫存

    [DisplayName("審核通過與否")]
    public bool? Approved { get; set; }//審核通過與否

    [DisplayName("狀態")]
    public bool? Status { get; set; }

    [NotMapped]
    public IFormFile FileToUpload { get; set; }
    
    [DisplayName("照片")]
    public string? Photo { get; set; }//照片
    public virtual ICollection<MemberFavorite> MemberFavorites { get; set; } = new List<MemberFavorite>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    public virtual ProductType? Type { get; set; }
}
