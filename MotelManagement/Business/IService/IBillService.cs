using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Business.IService
{
    public interface IBillService
    {
        Task<List<Bill>> getListBills(string? paidTimeFrom, string? paidTimeTo, string? confirmDateFrom, string? confirmDateTo, int owner, int isDept, int roomId, int pageIndex);
        public Task<Hashtable> getOwners(int roomId, int userId);
    }
}
