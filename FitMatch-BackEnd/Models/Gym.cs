using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Gym
{
    public int GymId { get; set; }

    public string? GymName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? OpentimeStart { get; set; }

    public DateTime? OpentimeEnd { get; set; }

    public bool? Approved { get; set; }
}
