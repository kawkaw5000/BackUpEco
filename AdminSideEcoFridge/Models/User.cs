using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminSideEcoFridge.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    [NotMapped]
    public string ConfirmPassword { get; set; }

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

    public virtual ICollection<Notifcation> NotifcationCreatedByNavigations { get; set; } = new List<Notifcation>();

    public virtual ICollection<Notifcation> NotifcationUpdatedByNavigations { get; set; } = new List<Notifcation>();

    public virtual ICollection<Notifcation> NotifcationUsers { get; set; } = new List<Notifcation>();

    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    public virtual ICollection<UserFood> UserFoods { get; set; } = new List<UserFood>();

    public virtual ICollection<UserPlan> UserPlans { get; set; } = new List<UserPlan>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
