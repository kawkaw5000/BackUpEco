using Microsoft.AspNetCore.Mvc;
using AdminSideEcoFridge.Repository;
using AdminSideEcoFridge.Models;
using AdminSideEcoFridge.Models.CustomModels;

namespace AdminSideEcoFridge.Controllers
{
    public class BaseController : Controller
    {
        public EcoFridgeDbContext _db;
        public UserSearcRepository _userSearchRepository;
        public UserRoleManager _roleManager;
        public BaseRepository<User> _userRepo;
        public BaseRepository<Role> _roleRepo;
        public BaseRepository<Notifcation> _notificationRepo;
        public BaseRepository<DonationTransaction> _donationRepo;
        public BaseRepository<UserRole> _userRoleRepo;
        public BaseRepository<UserPlan> _userPlanRepo;
        public BaseRepository<UserFood> _userFoodRepo;
        public BaseRepository<StoragePlan> _storagePlanRepo;
        public BaseRepository<StorageTip> _storageTipRepo;
        public BaseRepository<StorageTipForFoodCategory> _storageTipForFoodCategoryRepo;
        public BaseRepository<Food> _foodRepo;
        public BaseRepository<FoodCategory> _foodCategoryRepo;
        public BaseRepository<PaymentTransaction> _paymentTransactionRepo;
        public BaseRepository<FoodIngredient> _foodIngredientRepo;
        public BaseRepository<Donee> _doneeRepo;
        public BaseRepository<Donor> _donorRepo;
        public BaseRepository<VwUsersRoleView> _vwUsersRoleViewRepo;
        public BaseRepository<VwUsersFoodItem> _vwUsersFoodItemRepo;
        public BaseController()
        {   
            _db = new EcoFridgeDbContext();
            _userSearchRepository = new UserSearcRepository();
            _roleManager = new UserRoleManager();
            _notificationRepo = new BaseRepository<Notifcation>();
            _userRepo = new BaseRepository<User>();
            _roleRepo = new BaseRepository<Role>();
            _donationRepo = new BaseRepository<DonationTransaction>();
            _userRoleRepo = new BaseRepository<UserRole>();
            _userPlanRepo = new BaseRepository<UserPlan>();
            _userFoodRepo = new BaseRepository<UserFood>();
            _storagePlanRepo = new BaseRepository<StoragePlan>();
            _storageTipRepo = new BaseRepository<StorageTip>();
            _storageTipForFoodCategoryRepo = new BaseRepository<StorageTipForFoodCategory>();
            _foodRepo = new BaseRepository<Food>();
            _foodCategoryRepo = new BaseRepository<FoodCategory>();
            _paymentTransactionRepo = new BaseRepository<PaymentTransaction>();
            _foodIngredientRepo = new BaseRepository<FoodIngredient>();
            _doneeRepo = new BaseRepository<Donee>();
            _donorRepo = new BaseRepository<Donor>();
            _vwUsersRoleViewRepo = new BaseRepository<VwUsersRoleView>();
            _vwUsersFoodItemRepo = new BaseRepository<VwUsersFoodItem>();
        }
    }
}
