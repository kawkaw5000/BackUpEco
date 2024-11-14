using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class DonationTransactionMaster
{
    public int DonationTransactionMasterId { get; set; }

    public int? DonorId { get; set; }

    public int? DoneeId { get; set; }

    public string? Status { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual ICollection<DonationTransactionDetail> DonationTransactionDetails { get; set; } = new List<DonationTransactionDetail>();

    public virtual User? Donee { get; set; }

    public virtual User? Donor { get; set; }
}
