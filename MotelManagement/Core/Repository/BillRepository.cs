using Microsoft.EntityFrameworkCore;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Core.Repository
{
    public class BillRepository : BaseRepository<Bill>, IBillRepository
    {
        public BillRepository(MotelManagementContext context) : base(context)
        {
        }

        public Task<List<Bill>> getListBills(string? paidTimeFrom, string? paidTimeTo, string? confirmDateFrom, string? confirmDateTo, int owner, int isDept, int roomId, int pageIndex)
        {
            return _context.Bills.Where(b=>b.PaidTime>=DateTime.Parse(paidTimeFrom ?? b.PaidTime.ToString())
                                            && b.PaidTime<=DateTime.Parse(paidTimeTo ?? b.PaidTime.ToString())
                                            && b.AcceptTime>=DateTime.Parse(confirmDateFrom ?? b.AcceptTime.ToString())
                                            && b.AcceptTime<=DateTime.Parse(confirmDateTo??b.AcceptTime.ToString())
                                            && b.BillState == isDept
                                            && b.RoomId == roomId
                                            && b.UserId == owner)
                                            .Skip((pageIndex-1)*(int)PageManagement.PageSize).Take((int)PageManagement.PageSize).ToListAsync();
        }
    }
}
