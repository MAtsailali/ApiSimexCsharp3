using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class ConverstionsUser
{
    public int? IdConversation { get; set; }

    public int? IdUsuaris { get; set; }

    public virtual Conversations2? IdConversationNavigation { get; set; }

    public virtual Usuari? IdUsuarisNavigation { get; set; }
}
