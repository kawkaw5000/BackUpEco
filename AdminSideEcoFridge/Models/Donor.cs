using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class Donor
{
    public int DonorId { get; set; }

    public int? UserRoleId { get; set; }

    public virtual UserRole? UserRole { get; set; }
}
