using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class DonationTransactionDetail
{
    public int DonationTransactionDetailsId { get; set; }

    public int? DonationTransactionMasterId { get; set; }

    public int? FoodId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual DonationTransactionMaster? DonationTransactionMaster { get; set; }
}
