USE [master]
GO
/****** Object:  Database [EcoFridgeDB]    Script Date: 04/10/2024 11:38:46 AM ******/
CREATE DATABASE [EcoFridgeDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EcoFridgeDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EcoFridgeDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EcoFridgeDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EcoFridgeDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EcoFridgeDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EcoFridgeDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EcoFridgeDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [EcoFridgeDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EcoFridgeDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EcoFridgeDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EcoFridgeDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EcoFridgeDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EcoFridgeDB] SET  MULTI_USER 
GO
ALTER DATABASE [EcoFridgeDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EcoFridgeDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EcoFridgeDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EcoFridgeDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EcoFridgeDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EcoFridgeDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EcoFridgeDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [EcoFridgeDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EcoFridgeDB]
GO
/****** Object:  Table [dbo].[User]    Script Date: 04/10/2024 11:38:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[FoodBusinessName] [varchar](100) NULL,
	[DoneeOrganizationName] [varchar](100) NULL,
	[Birthdate] [date] NULL,
	[Gender] [char](1) NULL,
	[Province] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[Barangay] [varchar](50) NULL,
	[ProfilePicturePath] [varchar](255) NULL,
	[ProofPicturePath] [varchar](255) NULL,
	[AccountApproved] [bit] NULL,
	[EmailConfirmed] [bit] NULL,
	[StorageSize] [int] NULL,
	[FoodStoredCount] [int] NULL,
 CONSTRAINT [PK__User__1788CC4CECF201F5] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FoodCategory]    Script Date: 04/10/2024 11:38:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodCategory](
	[FoodCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[FoodCategoryName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FoodCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Food]    Script Date: 04/10/2024 11:38:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[FoodId] [int] IDENTITY(1,1) NOT NULL,
	[FoodCategoryId] [int] NULL,
	[FoodName] [varchar](255) NULL,
	[Quantity] [int] NULL,
	[Unit] [varchar](50) NULL,
	[DateAdded] [datetime] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[FoodPicturePath] [varchar](255) NULL,
 CONSTRAINT [PK__Food__856DB3EBF2F4140B] PRIMARY KEY CLUSTERED 
(
	[FoodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserFood]    Script Date: 04/10/2024 11:38:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFood](
	[UserFoodId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[FoodId] [int] NULL,
 CONSTRAINT [PK__UserFood__AA76FA87695A2980] PRIMARY KEY CLUSTERED 
(
	[UserFoodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_usersFoodItem]    Script Date: 04/10/2024 11:38:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_usersFoodItem]
as
SELECT        f.FoodId, f.FoodName, f.Quantity, f.Unit, f.ExpiryDate, fc.FoodCategoryId, fc.FoodCategoryName, f.DateAdded, f.FoodPicturePath, u.UserId, u.Email, u.Password, u.FirstName, u.LastName, u.FoodBusinessName, 
                         u.DoneeOrganizationName, u.Birthdate, u.Gender, u.Province, u.City, u.Barangay, u.ProfilePicturePath, u.ProofPicturePath, u.AccountApproved, u.EmailConfirmed, u.StorageSize, u.FoodStoredCount
FROM            dbo.UserFood AS uf INNER JOIN
                         dbo.Food AS f ON uf.FoodId = f.FoodId INNER JOIN
                         dbo.[User] AS u ON u.UserId = uf.UserId INNER JOIN
                         dbo.FoodCategory AS fc ON fc.FoodCategoryId = f.FoodCategoryId
GO
/****** Object:  Table [dbo].[Notifcation]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifcation](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[FoodId] [int] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[Title] [varchar](255) NULL,
	[Content] [varchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[LastModified] [datetime] NULL,
	[HasBeenViewed] [bit] NULL,
 CONSTRAINT [PK__Notifcat__20CF2E12E86A1819] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_FoodNotification]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_FoodNotification]
as
select n.NotificationId, n.Title, n.Content, n.HasBeenViewed, n.CreatedAt, n.LastModified, DATEDIFF(DAY, GETDATE(), vw.ExpiryDate) AS DaysToExpire, DATEDIFF(DAY, n.CreatedAt, GETDATE()) AS TimePast, vw.* 
from Notifcation n
inner join vw_usersFoodItem vw on vw.FoodId = n.FoodId
GO
/****** Object:  Table [dbo].[Role]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserRoleId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[RoleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_usersRoleView]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_usersRoleView]
AS
SELECT        u.UserId, u.Email, u.Password, u.FirstName, u.LastName, u.FoodBusinessName, u.DoneeOrganizationName, u.Birthdate, u.Gender, u.Province, u.City, u.Barangay, u.ProfilePicturePath, u.ProofPicturePath, u.AccountApproved, 
                         u.EmailConfirmed, u.StorageSize, u.FoodStoredCount, r.RoleId, r.RoleName, ur.UserRoleId
FROM            dbo.[User] AS u INNER JOIN
                         dbo.UserRole AS ur ON u.UserId = ur.UserId INNER JOIN
                         dbo.Role AS r ON r.RoleId = ur.RoleId
GO
/****** Object:  View [dbo].[vw_FoodBeforeExpirationDays]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vw_FoodBeforeExpirationDays]
as
select f.*, uf.UserFoodId, fc.FoodCategoryName, u.UserId, DATEDIFF(DAY, GETDATE(), f.ExpiryDate) AS DaysToExpire
from UserFood uf
inner join [User] u on uf.UserId = u.UserId
inner join Food f on f.FoodId = uf.FoodId
inner join FoodCategory fc on fc.FoodCategoryId = f.FoodCategoryId
GO
/****** Object:  Table [dbo].[DonationTransaction]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonationTransaction](
	[DonationTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[DonorId] [int] NULL,
	[DoneeId] [int] NULL,
	[UserFoodId] [int] NULL,
	[DonationDate] [int] NULL,
	[Status] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[DonationTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Donee]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donee](
	[DoneeId] [int] IDENTITY(1,1) NOT NULL,
	[UserRoleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[DoneeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Donor]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donor](
	[DonorId] [int] IDENTITY(1,1) NOT NULL,
	[UserRoleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[DonorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FoodIngredient]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodIngredient](
	[FoodIngredientId] [int] IDENTITY(1,1) NOT NULL,
	[FoodId] [int] NULL,
	[IngredientName] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FoodIngredientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentTransaction]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentTransaction](
	[PaymentTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[StoragePlanId] [int] NULL,
	[PaymentDate] [datetime] NULL,
	[Amount] [decimal](10, 2) NULL,
	[PaymentMethod] [varchar](50) NULL,
	[TransactionStatus] [varchar](50) NULL,
	[TransactionReferenceId] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecommendedFood]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecommendedFood](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FoodName] [varchar](255) NULL,
	[RecommendedRecipe] [varchar](max) NULL,
	[IncludedFoods] [varchar](255) NULL,
	[Steps] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StoragePlan]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoragePlan](
	[StoragePlanId] [int] IDENTITY(1,1) NOT NULL,
	[StoragePlanName] [varchar](50) NOT NULL,
	[StorageSize] [int] NOT NULL,
	[Duration] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StoragePlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StorageTip]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageTip](
	[StorageTipId] [int] IDENTITY(1,1) NOT NULL,
	[TipText] [varchar](max) NULL,
 CONSTRAINT [PK__StorageT__D0D77EBC9C4A7F48] PRIMARY KEY CLUSTERED 
(
	[StorageTipId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StorageTipForFoodCategory]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageTipForFoodCategory](
	[StorageTipFoFoodCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[StorageTipId] [int] NULL,
	[FoodCategoryId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StorageTipFoFoodCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImg]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImg](
	[TempImgId] [int] IDENTITY(1,1) NOT NULL,
	[FilePath] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[TempImgId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unit](
	[UnitId] [int] IDENTITY(1,1) NOT NULL,
	[UnitName] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[UnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPlan]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPlan](
	[UserPlanId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[StoragePlanId] [int] NULL,
	[SubscriptionDate] [datetime] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Notifcation] ADD  CONSTRAINT [DF__Notifcati__Creat__6F7F8B4B]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Notifcation] ADD  CONSTRAINT [DF__Notifcati__LastM__7073AF84]  DEFAULT (getdate()) FOR [LastModified]
GO
ALTER TABLE [dbo].[Notifcation] ADD  CONSTRAINT [DF__Notifcati__HasBe__7167D3BD]  DEFAULT ((0)) FOR [HasBeenViewed]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__User__StorageSiz__4AB81AF0]  DEFAULT ((5)) FOR [StorageSize]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_FoodStoredCount]  DEFAULT ((0)) FOR [FoodStoredCount]
GO
ALTER TABLE [dbo].[DonationTransaction]  WITH CHECK ADD FOREIGN KEY([DoneeId])
REFERENCES [dbo].[Donee] ([DoneeId])
GO
ALTER TABLE [dbo].[DonationTransaction]  WITH CHECK ADD FOREIGN KEY([DonorId])
REFERENCES [dbo].[Donor] ([DonorId])
GO
ALTER TABLE [dbo].[DonationTransaction]  WITH CHECK ADD  CONSTRAINT [FK__DonationT__UserF__04E4BC85] FOREIGN KEY([UserFoodId])
REFERENCES [dbo].[UserFood] ([UserFoodId])
GO
ALTER TABLE [dbo].[DonationTransaction] CHECK CONSTRAINT [FK__DonationT__UserF__04E4BC85]
GO
ALTER TABLE [dbo].[Donee]  WITH CHECK ADD FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRole] ([UserRoleId])
GO
ALTER TABLE [dbo].[Donor]  WITH CHECK ADD FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRole] ([UserRoleId])
GO
ALTER TABLE [dbo].[FoodIngredient]  WITH CHECK ADD  CONSTRAINT [FK__FoodIngre__FoodI__5812160E] FOREIGN KEY([FoodId])
REFERENCES [dbo].[Food] ([FoodId])
GO
ALTER TABLE [dbo].[FoodIngredient] CHECK CONSTRAINT [FK__FoodIngre__FoodI__5812160E]
GO
ALTER TABLE [dbo].[Notifcation]  WITH CHECK ADD  CONSTRAINT [FK__Notifcati__Creat__73501C2F] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Notifcation] CHECK CONSTRAINT [FK__Notifcati__Creat__73501C2F]
GO
ALTER TABLE [dbo].[Notifcation]  WITH CHECK ADD  CONSTRAINT [FK__Notifcati__FoodI__7BE56230] FOREIGN KEY([FoodId])
REFERENCES [dbo].[Food] ([FoodId])
GO
ALTER TABLE [dbo].[Notifcation] CHECK CONSTRAINT [FK__Notifcati__FoodI__7BE56230]
GO
ALTER TABLE [dbo].[Notifcation]  WITH CHECK ADD  CONSTRAINT [FK__Notifcati__HasBe] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifcation] CHECK CONSTRAINT [FK__Notifcati__HasBe]
GO
ALTER TABLE [dbo].[Notifcation]  WITH CHECK ADD  CONSTRAINT [FK__Notifcati__HasBe__725BF7F6] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Notifcation] CHECK CONSTRAINT [FK__Notifcati__HasBe__725BF7F6]
GO
ALTER TABLE [dbo].[Notifcation]  WITH CHECK ADD  CONSTRAINT [FK__Notifcati__Updat__74444068] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Notifcation] CHECK CONSTRAINT [FK__Notifcati__Updat__74444068]
GO
ALTER TABLE [dbo].[PaymentTransaction]  WITH CHECK ADD FOREIGN KEY([StoragePlanId])
REFERENCES [dbo].[StoragePlan] ([StoragePlanId])
GO
ALTER TABLE [dbo].[PaymentTransaction]  WITH CHECK ADD  CONSTRAINT [FK__PaymentTr__UserI__6FE99F9F] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[PaymentTransaction] CHECK CONSTRAINT [FK__PaymentTr__UserI__6FE99F9F]
GO
ALTER TABLE [dbo].[StorageTipForFoodCategory]  WITH CHECK ADD FOREIGN KEY([FoodCategoryId])
REFERENCES [dbo].[FoodCategory] ([FoodCategoryId])
GO
ALTER TABLE [dbo].[StorageTipForFoodCategory]  WITH CHECK ADD  CONSTRAINT [FK__StorageTi__Stora__60A75C0F] FOREIGN KEY([StorageTipId])
REFERENCES [dbo].[StorageTip] ([StorageTipId])
GO
ALTER TABLE [dbo].[StorageTipForFoodCategory] CHECK CONSTRAINT [FK__StorageTi__Stora__60A75C0F]
GO
ALTER TABLE [dbo].[UserFood]  WITH CHECK ADD  CONSTRAINT [FK__UserFood__UserId__7E37BEF6] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserFood] CHECK CONSTRAINT [FK__UserFood__UserId__7E37BEF6]
GO
ALTER TABLE [dbo].[UserFood]  WITH CHECK ADD  CONSTRAINT [FK_User_UserFood] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserFood] CHECK CONSTRAINT [FK_User_UserFood]
GO
ALTER TABLE [dbo].[UserFood]  WITH CHECK ADD  CONSTRAINT [FK_UserFood_Food] FOREIGN KEY([FoodId])
REFERENCES [dbo].[Food] ([FoodId])
GO
ALTER TABLE [dbo].[UserFood] CHECK CONSTRAINT [FK_UserFood_Food]
GO
ALTER TABLE [dbo].[UserPlan]  WITH CHECK ADD  CONSTRAINT [FK__UserPlan__Expiry__66603565] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserPlan] CHECK CONSTRAINT [FK__UserPlan__Expiry__66603565]
GO
ALTER TABLE [dbo].[UserPlan]  WITH CHECK ADD FOREIGN KEY([StoragePlanId])
REFERENCES [dbo].[StoragePlan] ([StoragePlanId])
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK__UserRole__UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK__UserRole__UserId]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK__UserRole__UserId__4F7CD00D] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK__UserRole__UserId__4F7CD00D]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [CK__User__Gender__49C3F6B7] CHECK  (([GENDER]='O' OR [GENDER]='F' OR [GENDER]='M'))
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [CK__User__Gender__49C3F6B7]
GO
/****** Object:  StoredProcedure [dbo].[SearchUsers]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE
[dbo].[SearchUsers]
	@Keyword VARCHAR(100)
AS
BEGIN 
	SELECT *
	FROM [dbo].[vw_usersRoleView]
	WHERE Email LIKE '%' + @Keyword + '%'
	OR FirstName LIKE '%' + @Keyword + '%'
	OR LastName LIKE '%' + @Keyword + '%'
	OR FoodBusinessName LIKE '%' + @Keyword + '%'
	OR DoneeOrganizationName LIKE '%' + @Keyword + '%';
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetNotificationCount]    Script Date: 04/10/2024 11:38:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetNotificationCount]
    @UserId INT, @result int output
AS
BEGIN
	SELECT 
       @result = COUNT(*)
    FROM 
        Notifcation n
    WHERE 
		n.UserId = @UserId and n.HasBeenViewed = 0;
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "u"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 261
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ur"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 251
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "r"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 234
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_usersRoleView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_usersRoleView'
GO
USE [master]
GO
ALTER DATABASE [EcoFridgeDB] SET  READ_WRITE 
GO
