using AdminSideEcoFridge.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminSideEcoFridge.Repository
{
    public class DonationTransactionManager
    {
        private readonly BaseRepository<VwDonationTransactionMasterUserView> _vwDonation;
        private readonly BaseRepository<DonationTransactionDetail> _donationTran;
        private readonly BaseRepository<VwVwDonationTransactionDetailsUserView> _vwTransactionItem;
        public DonationTransactionManager() 
        {
            _vwDonation = new BaseRepository<VwDonationTransactionMasterUserView>();
            _donationTran = new BaseRepository<DonationTransactionDetail>();
            _vwTransactionItem = new BaseRepository<VwVwDonationTransactionDetailsUserView>();
        }

        public List<VwDonationTransactionMasterUserView> DonationTransactionList()
        {
            return _vwDonation.GetAll()
                    .OrderByDescending(donation => donation.DonationTransactionMasterId)
                    .ToList();
        }

        public List<VwVwDonationTransactionDetailsUserView> donationItemsDetails(int id)
        {


            return _vwTransactionItem._table
                    .Where(e => e.DonationTransactionMasterId == id)               
                    .ToList();
        }

    }
}
