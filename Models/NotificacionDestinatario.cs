using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class NotificacionDestinatario
{
    public int Id { get; set; }

    public int? NotificacionId { get; set; }

    public int? UserId { get; set; }

    public int? CompanyId { get; set; }

    public int? Leida { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Notificacione? Notificacion { get; set; }

    public virtual Usuari? User { get; set; }
}
