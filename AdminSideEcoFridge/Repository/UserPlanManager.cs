using AdminSideEcoFridge.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminSideEcoFridge.Repository
{
    public class UserPlanManager
    {
        private BaseRepository<UserPlan> _userPlan;

        public UserPlanManager()
        {
            _userPlan = new BaseRepository<UserPlan>();
        }

        public List<UserPlan> GetAll()
        {
            return _userPlan._table
                .Include(e => e.StoragePlan)
                .ToList();
        }
    }
}
