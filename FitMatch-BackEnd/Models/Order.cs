using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Order
{
    public int OrderId { get; set; }//訂單編號

    public int? MemberId { get; set; }//  會員編號

    public int? TotalPrice { get; set; }//訂單總價

    public DateTime? OrderTime { get; set; }//訂單時間

    public string? PaymentMethod { get; set; }//支付方式

    public string? ShippingMethod { get; set; }//運送方式

    public string? MemberName { get; set; }//會員姓名

    public virtual Member? Member { get; set; }



    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
