﻿using System;
using System.Collections.Generic;

namespace AdminSideEcoFridge.Models;

public partial class VwUsersRoleView
{
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

    public string? Availability { get; set; }

    public string? VerificationCode { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int UserRoleId { get; set; }

    public int? UserPlanId { get; set; }

    public int? StoragePlanId { get; set; }

    public string? StoragePlanName { get; set; }
}
