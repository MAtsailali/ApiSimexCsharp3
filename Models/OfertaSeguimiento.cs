using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class OfertaSeguimiento
{
    public int Id { get; set; }

    public int OfertaId { get; set; }

    public int TrackingStepId { get; set; }

    public int Orden { get; set; }

    public int? EstaCompletado { get; set; }

    public DateTime? FechaCompletado { get; set; }

    public string? Observaciones { get; set; }

    public string? DocumentoPath { get; set; }

    public virtual Oferte Oferta { get; set; } = null!;

    public virtual TrackingStep TrackingStep { get; set; } = null!;
}
