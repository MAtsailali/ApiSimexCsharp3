using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Port
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public int? IdCiutat { get; set; }

    public virtual Ciutat? IdCiutatNavigation { get; set; }

    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();
}
