using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class OrderComment
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
