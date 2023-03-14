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
            List<Bill> list = await _context.Bills.Where(b=>b.PaidTime == b.PaidTime      
                                            && b.PaidTime >= (paidTimeFrom ?? b.PaidTime)
                                            && b.PaidTime <= (paidTimeTo ?? b.PaidTime)
                                            && b.AcceptTime >= (confirmDateFrom ?? b.AcceptTime)
                                            && b.AcceptTime <= (confirmDateTo ?? b.AcceptTime)
                                            && b.BillState == (isDept??b.BillState)
                                            && b.RoomId == roomId
                                            && b.UserId == (owner??b.UserId)).OrderByDescending(b=>b.PaidTime)
                                            .Skip((pageIndex-1)*(int)PageManagement.PageSize).Take((int)PageManagement.PageSize).AsNoTracking().ToListAsync();
            return list;
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

        public async Task<List<Bill>> getListUnPaidBills(int userId, int roomId)
        {
            List<int> listOwners = await _context.Contracts.Where(c => c.RoomId == roomId).Select(c=>c.UserId).ToListAsync();
            Console.WriteLine(listOwners[0] + "-" + userId);
            if (!listOwners.Contains(userId)) return null;
            return await _context.Bills.Where(b=>listOwners.Contains(b.UserId)
                                            &&(b.BillState==(int)PAYMENT_STATE.UNPAID
                                            ||b.BillState==(int)PAYMENT_STATE.DEBT)).ToListAsync();
        }

        public async Task SubmitBillAsync(Bill bill)
        {
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
        }
    }
}
