using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Notificacione
{
    public int Id { get; set; }

    public string? Titulo { get; set; }

    public string? Mensaje { get; set; }

    public int? TipoId { get; set; }

    public int? EmisorId { get; set; }

    public virtual Usuari? Emisor { get; set; }

    public virtual ICollection<NotificacionDestinatario> NotificacionDestinatarios { get; set; } = new List<NotificacionDestinatario>();

    public virtual Tipo? Tipo { get; set; }
}
