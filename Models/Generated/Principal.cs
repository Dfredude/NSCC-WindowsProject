using System;
using System.Collections.Generic;

namespace IMDB.Models;

public partial class Principal
{
    public string TitleId { get; set; } = null!;

    public int Ordering { get; set; }

    public string? NameId { get; set; }

    public string? JobCategory { get; set; }

    public string? Job { get; set; }

    public string? Characters { get; set; }

    public virtual Name? Name { get; set; }
}
