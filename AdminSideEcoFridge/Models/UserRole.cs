using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Donee> Donees { get; set; } = new List<Donee>();

    public virtual ICollection<Donor> Donors { get; set; } = new List<Donor>();

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
