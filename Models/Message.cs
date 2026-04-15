using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class Message
{
    public int Id { get; set; }

    public string? Message1 { get; set; }

    public int IdConversations { get; set; }

    public virtual Conversations2 IdConversationsNavigation { get; set; } = null!;
}
