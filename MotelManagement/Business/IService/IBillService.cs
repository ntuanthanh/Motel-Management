using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Business.IService
{
    public interface IBillService
    {
        public Task<int> countListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId);
        public Task<List<Bill>> getListUnPaidBill(int userId, int roomId);
        public Task<List<Bill>> getListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId, int pageIndex);
        public Task<Hashtable> getOwners(int roomId, int userId);

        public Task<Bill> getBillByIdAsync(int billId);
        public Task SubmitBillAsync(Bill bill);
        public Task CreateBill(List<Bill> listBills);
        public Task<List<Bill>> GetListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner, int pageIndex, bool isPagging);
        public Task SaveBillAsync(List<Bill> listBills);
        public Task<int> CountListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner);
    }
}
