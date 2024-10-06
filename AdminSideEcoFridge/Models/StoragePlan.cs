using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class StoragePlan
{
    public int StoragePlanId { get; set; }

    public string StoragePlanName { get; set; } = null!;

    public int StorageSize { get; set; }

    public int Duration { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    public virtual ICollection<UserPlan> UserPlans { get; set; } = new List<UserPlan>();
}
