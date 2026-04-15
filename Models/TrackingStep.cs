using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class TrackingStep
{
    public int Id { get; set; }

    public int? Ordre { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<IncotermPaso> IncotermPasos { get; set; } = new List<IncotermPaso>();

    public virtual ICollection<OfertaSeguimiento> OfertaSeguimientos { get; set; } = new List<OfertaSeguimiento>();
}
