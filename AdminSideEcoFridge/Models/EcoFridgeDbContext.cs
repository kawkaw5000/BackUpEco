using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdminSideEcoFridge.Models;

public partial class EcoFridgeDbContext : DbContext
{
    public EcoFridgeDbContext()
    {
    }

    public EcoFridgeDbContext(DbContextOptions<EcoFridgeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DonationTransaction> DonationTransactions { get; set; }

    public virtual DbSet<Donee> Donees { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<FoodCategory> FoodCategories { get; set; }

    public virtual DbSet<FoodIngredient> FoodIngredients { get; set; }

    public virtual DbSet<Notifcation> Notifcations { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<RecommendedFood> RecommendedFoods { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StoragePlan> StoragePlans { get; set; }

    public virtual DbSet<StorageTip> StorageTips { get; set; }

    public virtual DbSet<StorageTipForFoodCategory> StorageTipForFoodCategories { get; set; }

    public virtual DbSet<TempImg> TempImgs { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFood> UserFoods { get; set; }

    public virtual DbSet<UserPlan> UserPlans { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwFoodBeforeExpirationDay> VwFoodBeforeExpirationDays { get; set; }

    public virtual DbSet<VwFoodNotification> VwFoodNotifications { get; set; }

    public virtual DbSet<VwUsersFoodItem> VwUsersFoodItems { get; set; }

    public virtual DbSet<VwUsersRoleView> VwUsersRoleViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("workstation id=EcoFridgeDB.mssql.somee.com;packet size=4096;user id=lord24_SQLLogin_1;pwd=2xsxpaogou;data source=EcoFridgeDB.mssql.somee.com;persist security info=False;initial catalog=EcoFridgeDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DonationTransaction>(entity =>
        {
            entity.HasKey(e => e.DonationTransactionId).HasName("PK__Donation__24AA58DDB2663727");

            entity.ToTable("DonationTransaction");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Donee).WithMany(p => p.DonationTransactions)
                .HasForeignKey(d => d.DoneeId)
                .HasConstraintName("FK__DonationT__Donee__03F0984C");

            entity.HasOne(d => d.Donor).WithMany(p => p.DonationTransactions)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__DonationT__Donor__02FC7413");

            entity.HasOne(d => d.UserFood).WithMany(p => p.DonationTransactions)
                .HasForeignKey(d => d.UserFoodId)
                .HasConstraintName("FK__DonationT__UserF__04E4BC85");
        });

        modelBuilder.Entity<Donee>(entity =>
        {
            entity.HasKey(e => e.DoneeId).HasName("PK__Donee__8E69438E404EA638");

            entity.ToTable("Donee");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Donees)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__Donee__UserRoleI__76969D2E");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donor__052E3F7838A607BC");

            entity.ToTable("Donor");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Donors)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__Donor__UserRoleI__73BA3083");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3EBF2F4140B");

            entity.ToTable("Food");

            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FoodName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FoodPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FoodCategory>(entity =>
        {
            entity.HasKey(e => e.FoodCategoryId).HasName("PK__FoodCate__5451B7EBF4B90333");

            entity.ToTable("FoodCategory");

            entity.Property(e => e.FoodCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FoodIngredient>(entity =>
        {
            entity.HasKey(e => e.FoodIngredientId).HasName("PK__FoodIngr__CB78CEA6B46AFB3F");

            entity.ToTable("FoodIngredient");

            entity.Property(e => e.IngredientName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Food).WithMany(p => p.FoodIngredients)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__FoodIngre__FoodI__5812160E");
        });

        modelBuilder.Entity<Notifcation>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifcat__20CF2E12E86A1819");

            entity.ToTable("Notifcation");

            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HasBeenViewed).HasDefaultValue(false);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NotifcationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Notifcati__Creat__73501C2F");

            entity.HasOne(d => d.Food).WithMany(p => p.Notifcations)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__Notifcati__FoodI__7BE56230");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.NotifcationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Notifcati__Updat__74444068");

            entity.HasOne(d => d.User).WithMany(p => p.NotifcationUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notifcati__HasBe__725BF7F6");
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.PaymentTransactionId).HasName("PK__PaymentT__C22AEFE01C2A3737");

            entity.ToTable("PaymentTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionReferenceId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.StoragePlan).WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.StoragePlanId)
                .HasConstraintName("FK__PaymentTr__Stora__70DDC3D8");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__PaymentTr__UserI__6FE99F9F");
        });

        modelBuilder.Entity<RecommendedFood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recommen__3214EC079E0820F8");

            entity.ToTable("RecommendedFood");

            entity.Property(e => e.FoodName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IncludedFoods)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RecommendedRecipe).IsUnicode(false);
            entity.Property(e => e.Steps).IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AA3B71A03");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StoragePlan>(entity =>
        {
            entity.HasKey(e => e.StoragePlanId).HasName("PK__StorageP__4F8B77F4296B6E5D");

            entity.ToTable("StoragePlan");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StoragePlanName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StorageTip>(entity =>
        {
            entity.HasKey(e => e.StorageTipId).HasName("PK__StorageT__D0D77EBC9C4A7F48");

            entity.ToTable("StorageTip");

            entity.Property(e => e.TipText).IsUnicode(false);
        });

        modelBuilder.Entity<StorageTipForFoodCategory>(entity =>
        {
            entity.HasKey(e => e.StorageTipFoFoodCategoryId).HasName("PK__StorageT__2602B46EBAEE01B8");

            entity.ToTable("StorageTipForFoodCategory");

            entity.HasOne(d => d.FoodCategory).WithMany(p => p.StorageTipForFoodCategories)
                .HasForeignKey(d => d.FoodCategoryId)
                .HasConstraintName("FK__StorageTi__FoodC__619B8048");

            entity.HasOne(d => d.StorageTip).WithMany(p => p.StorageTipForFoodCategories)
                .HasForeignKey(d => d.StorageTipId)
                .HasConstraintName("FK__StorageTi__Stora__60A75C0F");
        });

        modelBuilder.Entity<TempImg>(entity =>
        {
            entity.HasKey(e => e.TempImgId).HasName("PK__TempImg__232A163F10F9354F");

            entity.ToTable("TempImg");

            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Unit__44F5ECB54BAA0D42");

            entity.ToTable("Unit");

            entity.Property(e => e.UnitName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CECF201F5");

            entity.ToTable("User");

            entity.Property(e => e.Barangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FoodBusinessName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FoodStoredCount).HasDefaultValue(0);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProofPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StorageSize).HasDefaultValue(5);
        });

        modelBuilder.Entity<UserFood>(entity =>
        {
            entity.HasKey(e => e.UserFoodId).HasName("PK__UserFood__AA76FA87695A2980");

            entity.ToTable("UserFood");

            entity.HasOne(d => d.Food).WithMany(p => p.UserFoods)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK_UserFood_Food");

            entity.HasOne(d => d.User).WithMany(p => p.UserFoods)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserFood__UserId__7E37BEF6");
        });

        modelBuilder.Entity<UserPlan>(entity =>
        {
            entity.HasKey(e => e.UserPlanId).HasName("PK__UserPlan__B2231FE1EABE836D");

            entity.ToTable("UserPlan");

            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionDate).HasColumnType("datetime");

            entity.HasOne(d => d.StoragePlan).WithMany(p => p.UserPlans)
                .HasForeignKey(d => d.StoragePlanId)
                .HasConstraintName("FK__UserPlan__Storag__6754599E");

            entity.HasOne(d => d.User).WithMany(p => p.UserPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserPlan__Expiry__66603565");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A355DF1379B");

            entity.ToTable("UserRole");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRole__RoleId__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRole__UserId__4F7CD00D");
        });

        modelBuilder.Entity<VwFoodBeforeExpirationDay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_FoodBeforeExpirationDays");

            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FoodCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FoodName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FoodPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwFoodNotification>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_FoodNotification");

            entity.Property(e => e.Barangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.DoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FoodBusinessName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FoodCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FoodName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FoodPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProofPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwUsersFoodItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_usersFoodItem");

            entity.Property(e => e.Barangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.DoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FoodBusinessName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FoodCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FoodName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FoodPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProofPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwUsersRoleView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_usersRoleView");

            entity.Property(e => e.Barangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FoodBusinessName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProofPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
