using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Tipo
{
    public int Id { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();
}
