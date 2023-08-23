using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Restaurant
{
    public int RestaurantsId { get; set; }

    public string? RestaurantsName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Photo { get; set; }

    public string? RestaurantsDescription { get; set; }

    public bool? Status { get; set; }
}
