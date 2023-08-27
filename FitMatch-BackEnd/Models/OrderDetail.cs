using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FitMatch_BackEnd.Models;

public partial class OrderDetail
{
    [DisplayName("訂單明細編號")]
    public int OrderDetailId { get; set; }//訂單明細編號

    [DisplayName("訂單編號")]
    public int? OrderId { get; set; }//訂單編號

    [DisplayName("產品編號")]
    public int? ProductId { get; set; }//產品編號

    [DisplayName("數量")]
    public int? Quantity { get; set; }//數量

}
