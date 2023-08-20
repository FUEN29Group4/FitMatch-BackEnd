using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class ProductType
{
    
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
