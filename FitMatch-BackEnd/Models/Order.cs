using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? MemberId { get; set; }

    public int? TotalPrice { get; set; }

    public DateTime? OrderTime { get; set; }

    public string? PaymentMethod { get; set; }

    public string? ShippingMethod { get; set; }

    public virtual Member? Member { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
