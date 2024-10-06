using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class StorageTip
{
    public int StorageTipId { get; set; }

    public string? TipText { get; set; }

    public virtual ICollection<StorageTipForFoodCategory> StorageTipForFoodCategories { get; set; } = new List<StorageTipForFoodCategory>();
}
