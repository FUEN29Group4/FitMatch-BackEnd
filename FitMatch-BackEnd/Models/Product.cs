using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public string? Photo { get; set; }

    public int? Price { get; set; }

    public int? TypeId { get; set; }

    public bool? Approved { get; set; }
    public int? ProductInventory { get; set; }
    
    public virtual ICollection<MemberFavorite> MemberFavorites { get; set; } = new List<MemberFavorite>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ProductType? Type { get; set; }
}
