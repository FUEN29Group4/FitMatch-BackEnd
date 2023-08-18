using System;
using System.Collections.Generic;

namespace FitMatch_BackEnd.Models;

public partial class Article
{
    public int ArticleId { get; set; }

    public string? Title { get; set; }

    public string? Photo { get; set; }

    public string? ContentText { get; set; }

    public string? ArticleTypeName { get; set; }

    public bool? Approved { get; set; }

    public DateTime? PublicationDate { get; set; }
}
