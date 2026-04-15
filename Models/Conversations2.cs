using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Conversations2
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int? AgentId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastMessageAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
