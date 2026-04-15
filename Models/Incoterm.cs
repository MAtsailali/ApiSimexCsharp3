using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Incoterm
{
    public int Id { get; set; }

    public int? TipusIncontermId { get; set; }

    public int TrackingStepsId { get; set; }

    public virtual ICollection<IncotermPaso> IncotermPasos { get; set; } = new List<IncotermPaso>();

    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();

    public virtual TipusIncoterm? TipusInconterm { get; set; }
}
