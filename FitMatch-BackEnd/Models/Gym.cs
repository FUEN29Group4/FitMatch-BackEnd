using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitMatch_BackEnd.Models;

public partial class Gym
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GymId { get; set; }


    public string? GymName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? OpentimeStart { get; set; }

    public DateTime? OpentimeEnd { get; set; }

    public bool? Approved { get; set; }
    public string? GymDescription { get; set; }

}
