using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public int? ClassId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? ReviewDateTime { get; set; }

    public virtual Trainer? Member { get; set; }
}
