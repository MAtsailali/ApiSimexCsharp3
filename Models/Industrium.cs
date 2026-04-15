using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Industrium
{
    public int Id { get; set; }

    public string? Categoria { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
