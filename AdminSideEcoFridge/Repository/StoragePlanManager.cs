using AdminSideEcoFridge.Models;

namespace AdminSideEcoFridge.Repository
{
    public class StoragePlanManager
    {
        private BaseRepository<StoragePlan> _storagePlan;

        public StoragePlanManager()
        {
            _storagePlan = new BaseRepository<StoragePlan>();
        }

        public List<StoragePlan> ListOfPlans()
        {
            return _storagePlan.GetAll();
        }
    }
}
