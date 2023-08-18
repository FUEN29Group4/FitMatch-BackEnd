using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public string? TrainerName { get; set; }

    public bool? Gender { get; set; }

    public DateTime? Birth { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Photo { get; set; }

    public string? Certificate { get; set; }

    public string? Expertise { get; set; }

    public string? Experience { get; set; }

    public int? CourseFee { get; set; }

    public string? Password { get; set; }

    public bool? Approved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
