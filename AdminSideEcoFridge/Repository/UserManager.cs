using AdminSideEcoFridge.Models;

namespace AdminSideEcoFridge.Repository
{
    public class UserManager
    {
        private BaseRepository<User> _userRepo;

        public UserManager()
        {
            _userRepo = new BaseRepository<User>();
        }

        public User GetUserById(int userId)
        {
            return _userRepo._table.Where(m => m.UserId == userId).FirstOrDefault();
        }
    }
}
