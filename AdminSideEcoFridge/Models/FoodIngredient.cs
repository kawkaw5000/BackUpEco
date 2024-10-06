using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class FoodIngredient
{
    public int FoodIngredientId { get; set; }

    public int? FoodId { get; set; }

    public string IngredientName { get; set; } = null!;

    public virtual Food? Food { get; set; }
}
