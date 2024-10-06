using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class DonationTransaction
{
    public int DonationTransactionId { get; set; }

    public int? DonorId { get; set; }

    public int? DoneeId { get; set; }

    public int? UserFoodId { get; set; }

    public int? DonationDate { get; set; }

    public string? Status { get; set; }

    public virtual Donee? Donee { get; set; }

    public virtual Donor? Donor { get; set; }

    public virtual UserFood? UserFood { get; set; }
}
