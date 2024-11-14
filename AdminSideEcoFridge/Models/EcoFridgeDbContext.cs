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

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatConversation> ChatConversations { get; set; }

    public virtual DbSet<DonationTransactionDetail> DonationTransactionDetails { get; set; }

    public virtual DbSet<DonationTransactionMaster> DonationTransactionMasters { get; set; }

    public virtual DbSet<Donee> Donees { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<FoodCategory> FoodCategories { get; set; }

    public virtual DbSet<FoodIngredient> FoodIngredients { get; set; }

    public virtual DbSet<FoodShelfLife> FoodShelfLives { get; set; }

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

    public virtual DbSet<VwChatConversationView> VwChatConversationViews { get; set; }

    public virtual DbSet<VwDonationTransactionMasterUserView> VwDonationTransactionMasterUserViews { get; set; }

    public virtual DbSet<VwDoneeChatConversation> VwDoneeChatConversations { get; set; }

    public virtual DbSet<VwDonorChatConversation> VwDonorChatConversations { get; set; }

    public virtual DbSet<VwFoodBeforeExpirationDay> VwFoodBeforeExpirationDays { get; set; }

    public virtual DbSet<VwFoodNotification> VwFoodNotifications { get; set; }

    public virtual DbSet<VwManageDonationView> VwManageDonationViews { get; set; }

    public virtual DbSet<VwUsersFoodItem> VwUsersFoodItems { get; set; }

    public virtual DbSet<VwUsersRoleView> VwUsersRoleViews { get; set; }

    public virtual DbSet<VwVwDonationTransactionDetailsUserView> VwVwDonationTransactionDetailsUserViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=EcoFridgeDB.mssql.somee.com;Initial Catalog=EcoFridgeDB;User ID=lord24_SQLLogin_1;Password=2xsxpaogou;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__Chat__A9FBE7C6936CD95F");

            entity.ToTable("Chat");

            entity.Property(e => e.ChatMessage).IsUnicode(false);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ChatConversation).WithMany(p => p.Chats)
                .HasForeignKey(d => d.ChatConversationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Chat__ChatConver__236943A5");

            entity.HasOne(d => d.Sender).WithMany(p => p.Chats)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Chat__SenderId__245D67DE");
        });

        modelBuilder.Entity<ChatConversation>(entity =>
        {
            entity.HasKey(e => e.ChatConversationId).HasName("PK__ChatConv__FA706609634D0AEF");

            entity.ToTable("ChatConversation");

            entity.HasOne(d => d.Donee).WithMany(p => p.ChatConversationDonees)
                .HasForeignKey(d => d.DoneeId)
                .HasConstraintName("FK__ChatConve__Donee__1AD3FDA4");

            entity.HasOne(d => d.Donor).WithMany(p => p.ChatConversationDonors)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ChatConve__Donor__19DFD96B");
        });

        modelBuilder.Entity<DonationTransactionDetail>(entity =>
        {
            entity.HasKey(e => e.DonationTransactionDetailsId).HasName("PK__Donation__21C692FA891F01B0");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(dateadd(hour,(8),getdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.DonationTransactionMaster).WithMany(p => p.DonationTransactionDetails)
                .HasForeignKey(d => d.DonationTransactionMasterId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DonationT__Donat__489AC854");
        });

        modelBuilder.Entity<DonationTransactionMaster>(entity =>
        {
            entity.HasKey(e => e.DonationTransactionMasterId).HasName("PK__Donation__0EE80146C6DF7CED");

            entity.ToTable("DonationTransactionMaster");

            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(dateadd(hour,(8),getdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Donee).WithMany(p => p.DonationTransactionMasterDonees)
                .HasForeignKey(d => d.DoneeId)
                .HasConstraintName("FK__DonationT__Donee__40058253");

            entity.HasOne(d => d.Donor).WithMany(p => p.DonationTransactionMasterDonors)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DonationT__Donor__3F115E1A");
        });

        modelBuilder.Entity<Donee>(entity =>
        {
            entity.HasKey(e => e.DoneeId).HasName("PK__Donee__8E69438E791CF6CB");

            entity.ToTable("Donee");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Donees)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__Donee__UserRoleI__5165187F");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donor__052E3F7871F232E0");

            entity.ToTable("Donor");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Donors)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__Donor__UserRoleI__52593CB8");
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
            entity.HasKey(e => e.FoodCategoryId).HasName("PK__FoodCate__5451B7EB0EEC8BEF");

            entity.ToTable("FoodCategory");

            entity.Property(e => e.FoodCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FoodIngredient>(entity =>
        {
            entity.HasKey(e => e.FoodIngredientId).HasName("PK__FoodIngr__CB78CEA6F52B9D42");

            entity.ToTable("FoodIngredient");

            entity.Property(e => e.IngredientName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Food).WithMany(p => p.FoodIngredients)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__FoodIngre__FoodI__5812160E");
        });

        modelBuilder.Entity<FoodShelfLife>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FoodShel__3214EC0701EA6B75");

            entity.ToTable("FoodShelfLife");

            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContainedIngredients).IsUnicode(false);
            entity.Property(e => e.FoodName)
                .HasMaxLength(255)
                .IsUnicode(false);
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
            entity.HasKey(e => e.PaymentTransactionId).HasName("PK__PaymentT__C22AEFE0DCC766C0");

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
                .HasConstraintName("FK__PaymentTr__Stora__5812160E");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__PaymentTr__UserI__6FE99F9F");
        });

        modelBuilder.Entity<RecommendedFood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recommen__3214EC070E6DE4F7");

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
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AB67F2267");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StoragePlan>(entity =>
        {
            entity.HasKey(e => e.StoragePlanId).HasName("PK__StorageP__4F8B77F43C6D9D6D");

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
            entity.HasKey(e => e.StorageTipFoFoodCategoryId).HasName("PK__StorageT__2602B46E38E2CABD");

            entity.ToTable("StorageTipForFoodCategory");

            entity.HasOne(d => d.FoodCategory).WithMany(p => p.StorageTipForFoodCategories)
                .HasForeignKey(d => d.FoodCategoryId)
                .HasConstraintName("FK__StorageTi__FoodC__59FA5E80");

            entity.HasOne(d => d.StorageTip).WithMany(p => p.StorageTipForFoodCategories)
                .HasForeignKey(d => d.StorageTipId)
                .HasConstraintName("FK__StorageTi__Stora__60A75C0F");
        });

        modelBuilder.Entity<TempImg>(entity =>
        {
            entity.HasKey(e => e.TempImgId).HasName("PK__TempImg__232A163FF309913A");

            entity.ToTable("TempImg");

            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Unit__44F5ECB5BAEA8004");

            entity.ToTable("Unit");

            entity.Property(e => e.UnitName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CECF201F5");

            entity.ToTable("User");

            entity.Property(e => e.Availability)
                .HasMaxLength(255)
                .IsUnicode(false);
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

            entity.Property(e => e.Display).HasDefaultValue(true);

            entity.HasOne(d => d.Food).WithMany(p => p.UserFoods)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK_UserFood_Food");

            entity.HasOne(d => d.User).WithMany(p => p.UserFoods)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserFood__UserId__7E37BEF6");
        });

        modelBuilder.Entity<UserPlan>(entity =>
        {
            entity.HasKey(e => e.UserPlanId).HasName("PK__UserPlan__B2231FE1477EB2E5");

            entity.ToTable("UserPlan");

            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionDate).HasColumnType("datetime");

            entity.HasOne(d => d.StoragePlan).WithMany(p => p.UserPlans)
                .HasForeignKey(d => d.StoragePlanId)
                .HasConstraintName("FK__UserPlan__Storag__5EBF139D");

            entity.HasOne(d => d.User).WithMany(p => p.UserPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserPlan__Expiry__66603565");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A354B33FD13");

            entity.ToTable("UserRole");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRole__RoleId__5FB337D6");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRole__UserId__4F7CD00D");
        });

        modelBuilder.Entity<VwChatConversationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ChatConversationView");

            entity.Property(e => e.Availability)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BusinessProofPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ChatMessage).IsUnicode(false);
            entity.Property(e => e.DoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DoneesBarangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DoneesCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DoneesEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DoneesProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DoneesProofPicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DoneesProvince)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DonorsBarangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DonorsBusinesName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DonorsCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DonorsEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DonorsFirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DonorsLastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DonorsProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DonorsProvince)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SentAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwDonationTransactionMasterUserView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_donationTransactionMasterUserView");

            entity.Property(e => e.AccountType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Availability)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Barangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DoneeOrgName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DoneeProfilePicPath)
                .HasMaxLength(255)
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
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwDoneeChatConversation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DoneeChatConversation");

            entity.Property(e => e.Availability)
                .HasMaxLength(255)
                .IsUnicode(false);
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
            entity.Property(e => e.LastChat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastSentAt)
                .HasMaxLength(1)
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
            entity.Property(e => e.ThisFirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThisFoodBusinessName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ThisLastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThisProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwDonorChatConversation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DonorChatConversation");

            entity.Property(e => e.Availability)
                .HasMaxLength(255)
                .IsUnicode(false);
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
            entity.Property(e => e.LastChat)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastSentAt)
                .HasMaxLength(1)
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
            entity.Property(e => e.ThisDoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ThisProfilePicturePath)
                .HasMaxLength(255)
                .IsUnicode(false);
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

        modelBuilder.Entity<VwManageDonationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_manageDonationView");

            entity.Property(e => e.Barangay)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.DoneeOrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DonorsFirstName)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.DonorsLastName)
                .HasMaxLength(1)
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
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
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

            entity.Property(e => e.Availability)
                .HasMaxLength(255)
                .IsUnicode(false);
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

        modelBuilder.Entity<VwVwDonationTransactionDetailsUserView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_vw_DonationTransactionDetailsUserView");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
