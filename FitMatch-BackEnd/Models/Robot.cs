using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Robot
{
    public int RobotId { get; set; }

    public string? Type { get; set; }

    public string? DefaultQuestion { get; set; }

    public string? DefaultResponse { get; set; }
}
