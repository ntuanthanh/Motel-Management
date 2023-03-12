using Microsoft.EntityFrameworkCore;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;
using System.Collections;
using System.Text;

namespace MotelManagement.Core.Repository
{
    public class BillRepository : BaseRepository<Bill>, IBillRepository
    {
        public BillRepository(MotelManagementContext context) : base(context)
        {
        }

        public async Task<List<Bill>> getListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId, int pageIndex)
        {
            return await _context.Bills.Where(b=>b.PaidTime == b.PaidTime      
                                            && b.PaidTime >= (paidTimeFrom ?? b.PaidTime)
                                            && b.PaidTime <= (paidTimeTo ?? b.PaidTime)
                                            && b.AcceptTime >= (confirmDateFrom ?? b.PaidTime)
                                            && b.AcceptTime <= (confirmDateTo ?? b.PaidTime)
                                            && b.BillState == (isDept??b.BillState)
                                            && b.RoomId == roomId
                                            && b.UserId == (owner??b.UserId)).OrderByDescending(b=>b.PaidTime)
                                            .Skip((pageIndex-1)*(int)PageManagement.PageSize).Take((int)PageManagement.PageSize).AsNoTracking().ToListAsync();
        }


        public async Task<int> countListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId)
        {
            return await _context.Bills.Where(b => b.PaidTime == b.PaidTime
                                            && b.PaidTime >= (paidTimeFrom ?? b.PaidTime)
                                            && b.PaidTime <= (paidTimeTo ?? b.PaidTime)
                                            && b.AcceptTime >= (confirmDateFrom ?? b.PaidTime)
                                            && b.AcceptTime <= (confirmDateTo ?? b.PaidTime)
                                            && b.BillState == (isDept ?? b.BillState)
                                            && b.RoomId == roomId
                                            && b.UserId == (owner ?? b.UserId)).CountAsync();
        }
    }
}
