﻿using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class VwVwDonationTransactionDetailsUserView
{
    public int DonationTransactionDetailsId { get; set; }

    public int? DonationTransactionMasterId { get; set; }

    public int FoodId { get; set; }

    public string? FoodName { get; set; }

    public int? Quantity { get; set; }

    public string? Unit { get; set; }

    public DateTime ExpiryDate { get; set; }

    public int FoodCategoryId { get; set; }

    public string FoodCategoryName { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public string? FoodPicturePath { get; set; }

    public string? Storagemethod { get; set; }

    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FoodBusinessName { get; set; }

    public string? DoneeOrganizationName { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string? Gender { get; set; }

    public string? Province { get; set; }

    public string? City { get; set; }

    public string? Barangay { get; set; }

    public string? ProfilePicturePath { get; set; }

    public string? ProofPicturePath { get; set; }

    public bool? AccountApproved { get; set; }

    public bool? EmailConfirmed { get; set; }

    public int? StorageSize { get; set; }

    public int? FoodStoredCount { get; set; }

    public bool? Display { get; set; }
}
