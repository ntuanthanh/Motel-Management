using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Business.IService
{
    public interface IBillService
    {
        public Task<int> countListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId);

        public Task<List<Bill>> getListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId, int pageIndex);
        public Task<Hashtable> getOwners(int roomId, int userId);
    }
}
