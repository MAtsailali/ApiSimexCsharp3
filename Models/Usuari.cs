using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Usuari
{
    public int Id { get; set; }

    public string Correu { get; set; } = null!;

    public string Contrasenya { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string Cognoms { get; set; } = null!;

    public int RolId { get; set; }

    public int? CompanyId { get; set; }

    public string? Tlfn { get; set; }

    public int? Status { get; set; }

    public DateTime? UltimaConex { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<IaMessage> IaMessages { get; set; } = new List<IaMessage>();

    public virtual ICollection<NotificacionDestinatario> NotificacionDestinatarios { get; set; } = new List<NotificacionDestinatario>();

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();

    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();

    public virtual Rol Rol { get; set; } = null!;
}
