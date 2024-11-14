using AdminSideEcoFridge.Models;

namespace AdminSideEcoFridge.Repository
{
    public class DonationTransactionManager
    {
        private readonly BaseRepository<VwDonationTransactionMasterUserView> _vwDonation;
        public DonationTransactionManager() 
        {
            _vwDonation = new BaseRepository<VwDonationTransactionMasterUserView>();
        }

        public List<VwDonationTransactionMasterUserView> DonationTransactionList()
        {
            return _vwDonation.GetAll()
                    .OrderByDescending(donation => donation.DonationTransactionMasterId)
                    .ToList();
        }
    }
}
