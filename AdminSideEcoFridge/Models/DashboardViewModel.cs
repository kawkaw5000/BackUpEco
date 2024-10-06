namespace AdminSideEcoFridge.Models
{
    public class DashboardViewModel
    {
        public List<VwUsersRoleView> UserList { get; set; }
        public List<VwUsersFoodItem> FoodList { get; set; }
        public List<User> User { get; set; }

        public int AdminCount { get; set; }
        public int DonorCount { get; set; }
        public int FoodBusinessCount { get; set; }
        public int OrganizationCount { get; set; }
        public int TotalUsers { get; set; }
    }
}
