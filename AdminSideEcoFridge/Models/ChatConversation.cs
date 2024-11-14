using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class ChatConversation
{
    public int ChatConversationId { get; set; }

    public int? DonorId { get; set; }

    public int? DoneeId { get; set; }

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual User? Donee { get; set; }

    public virtual User? Donor { get; set; }
}
