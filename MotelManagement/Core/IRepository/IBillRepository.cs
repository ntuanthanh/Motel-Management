using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Core.IRepository
{
    public interface IBillRepository
    {
        public Task<int> countListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId);
        public Task<List<Bill>> getListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId, int pageIndex);
        public Task<List<Bill>> GetListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner, int pageIndex, bool isPagging);
        public Task<List<Bill>> getListUnPaidBills(int userId, int roomId);
        public Task CreateBill(List<Bill> listBills);    
        public Task SubmitBillAsync(Bill bill);
        public Task SaveBillAsync(List<Bill> bills);
        public Task<int> CountListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner);
    }
}
