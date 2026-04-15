using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class IaMessage
{
    public int Id { get; set; }

    public string? IaMessage1 { get; set; }

    public int IdUser { get; set; }

    public virtual Usuari IdUserNavigation { get; set; } = null!;
}
