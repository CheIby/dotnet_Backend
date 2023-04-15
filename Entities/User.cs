using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class User
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Score { get; set; }

    public virtual ICollection<OrderComment> OrderComments { get; set; } = new List<OrderComment>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
