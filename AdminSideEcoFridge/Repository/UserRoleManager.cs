using AdminSideEcoFridge.Models;

namespace AdminSideEcoFridge.Repository
{
    public class UserRoleManager
    {
        private BaseRepository<User> _userAcc;
        private BaseRepository<VwUsersRoleView> _userRole;
        private readonly EcoFridgeDbContext _context;

        public UserRoleManager()
        {
            _userAcc = new BaseRepository<User>();
            _userRole = new BaseRepository<VwUsersRoleView>();
        }

        public VwUsersRoleView GetUserByUserId(int userId)
        {
            return _userRole._table.Where(m => m.UserId == userId).FirstOrDefault();
        }

    }
}
