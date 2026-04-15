using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Divisa
{
    public int Id { get; set; }

    public string? Tipus { get; set; }

    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();
}
