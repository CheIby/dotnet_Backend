using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class Order
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public int PiorityScore {get; set; }

    public string Restaurant { get; set; } = null!;

    public string Detail { get; set; } = null!;

    public string ReceiveLocation { get; set; } = null!;

    public virtual ICollection<OrderComment> OrderComments { get; set; } = new List<OrderComment>();

    public virtual User User { get; set; } = null!;
}
