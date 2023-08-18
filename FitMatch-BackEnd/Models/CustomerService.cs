using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class CustomerService
{
    public int CustomerServiceId { get; set; }

    public int? MemberId { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? DateTime { get; set; }

    public int? TrainerId { get; set; }

    public int? MessageId { get; set; }
}
