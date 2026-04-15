using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class IncotermPaso
{
    public int Id { get; set; }

    public int IncotermId { get; set; }

    public int TrackingStepId { get; set; }

    public int? Orden { get; set; }

    public virtual Incoterm Incoterm { get; set; } = null!;

    public virtual TrackingStep TrackingStep { get; set; } = null!;
}
