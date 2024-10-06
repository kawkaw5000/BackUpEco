using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class UserFood
{
    public int UserFoodId { get; set; }

    public int? UserId { get; set; }

    public int? FoodId { get; set; }

    public virtual ICollection<DonationTransaction> DonationTransactions { get; set; } = new List<DonationTransaction>();

    public virtual Food? Food { get; set; }

    public virtual User? User { get; set; }
}
