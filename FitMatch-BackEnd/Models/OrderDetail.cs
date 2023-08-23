using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }//訂單明細編號

    public int? OrderId { get; set; }//訂單編號

    public int? ProductId { get; set; }//產品編號

    public int? Quantity { get; set; }//數量

    public string? ProductName { get; set; }//產品名稱

    public string? Photo { get; set; }//照片

    public int? Price { get; set; }//價格

    public int? TypeID { get; set; }//商品類別編號

    public DateTime? OrderTime { get; set; }//訂單時間

    public DateTime? PayTime { get; set; }//付款時間

    public string? MemberName { get; set; }//會員姓名

    public string? Phone { get; set; }//電話

    public int? TotalPrice { get; set; }//訂單總價

    public DateTime? SendItemTime { get; set; }//出貨時間


    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
