using System;
using System.Collections.Generic;

namespace server.Entities;

public partial class User
{
    public int Id { get; set; } = 0;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime EnrollmentDate { get; set; }
}
