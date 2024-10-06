using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class FoodCategory
{
    public int FoodCategoryId { get; set; }

    public string FoodCategoryName { get; set; } = null!;

    public virtual ICollection<StorageTipForFoodCategory> StorageTipForFoodCategories { get; set; } = new List<StorageTipForFoodCategory>();
}
