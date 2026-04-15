using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Company
{
    public int Id { get; set; }

    public string? CompanyName { get; set; }

    public int IndustriaId { get; set; }

    public virtual Industrium Industria { get; set; } = null!;

    public virtual ICollection<NotificacionDestinatario> NotificacionDestinatarios { get; set; } = new List<NotificacionDestinatario>();

    public virtual ICollection<Usuari> Usuaris { get; set; } = new List<Usuari>();
}
