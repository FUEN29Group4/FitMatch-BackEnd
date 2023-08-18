using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class MemberFavorite
{
    public int MemberFavoriteId { get; set; }

    public int? MemberId { get; set; }

    public int? ClassId { get; set; }

    public int? TrainerId { get; set; }

    public int? ProductId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Member? ClassNavigation { get; set; }

    public virtual Product? Product { get; set; }
}
