using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Core.IRepository
{
    public interface IBillRepository
    {
        Task<List<Bill>> getListBills(string? paidTimeFrom, string? paidTimeTo, string? confirmDateFrom, string? confirmDateTo, int owner, int isDept, int roomId, int pageIndex);
    }
}
