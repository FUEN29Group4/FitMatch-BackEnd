using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Product
{
    public int ProductId { get; set; }//產品編號

    public string? ProductName { get; set; }//產品名稱

    public string? ProductDescription { get; set; }//產品描述

    public string? Photo { get; set; }//照片

    public int? Price { get; set; }//價格

    public int? TypeId { get; set; }//商品類別編號

    public int? ProductInventory { get; set; }//商品庫存
    
    public bool? Approved { get; set; }//審核通過與否

    public bool? Status { get; set; }

    public virtual ICollection<MemberFavorite> MemberFavorites { get; set; } = new List<MemberFavorite>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ProductType? Type { get; set; }
}
