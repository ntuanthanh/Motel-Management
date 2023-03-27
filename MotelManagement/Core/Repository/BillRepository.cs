using Microsoft.AspNetCore.Components.Forms;
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
            List<Bill> list = new List<Bill>();
            IQueryable<Bill> query = _context.Bills.AsQueryable();
            if (paidTimeFrom == null && paidTimeTo == null && confirmDateFrom == null && confirmDateTo == null)
            {
                query = query.Where(b => b.BillState == (isDept ?? b.BillState)
                                            && b.RoomId == roomId
                                            && b.UserId == (owner ?? b.UserId));
            }
            else
            {
                query = query.Where(b => b.PaidTime >= (paidTimeFrom ?? b.PaidTime)
                                            && b.PaidTime <= (paidTimeTo ?? b.PaidTime)
                                            && b.AcceptTime >= (confirmDateFrom ?? b.AcceptTime)
                                            && b.AcceptTime <= (confirmDateTo ?? b.AcceptTime)
                                            && b.BillState == (isDept ?? b.BillState)
                                            && b.RoomId == roomId
                                            && b.UserId == (owner ?? b.UserId));
            }

            list = await query.OrderByDescending(b=>b.PaidTime)
                                            .Skip((pageIndex-1)*3).Take(3).AsNoTracking().ToListAsync();
            return list;
        }


        public async Task<int> countListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId)
        {
            IQueryable<Bill> query = _context.Bills.AsQueryable();
            if (paidTimeFrom == null && paidTimeTo == null && confirmDateFrom == null && confirmDateTo == null)
            {
                query = query.Where(b => b.BillState == (isDept ?? b.BillState)
                                            && b.RoomId == roomId
                                            && b.UserId == (owner ?? b.UserId));
            }
            else
            {
                query = query.Where(b => b.PaidTime >= (paidTimeFrom ?? b.PaidTime)
                                            && b.PaidTime <= (paidTimeTo ?? b.PaidTime)
                                            && b.AcceptTime >= (confirmDateFrom ?? b.AcceptTime)
                                            && b.AcceptTime <= (confirmDateTo ?? b.AcceptTime)
                                            && b.BillState == (isDept ?? b.BillState)
                                            && b.RoomId == roomId
                                            && b.UserId == (owner ?? b.UserId));
            }
            return await query.CountAsync();
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

        public async Task<List<Bill>> GetListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner, int pageIndex, bool isPagging)
        {
            IQueryable<Bill>  query = _context.Bills.Include(b => b.User).Include(b => b.Room)
                                    .Where(b => b.CreatedDate >= (from ?? b.CreatedDate) &&
                                    b.CreatedDate <= (to ?? b.CreatedDate) &&
                                    b.Room.Name.Contains(roomName ?? "") &&
                                    b.User.FullName.Contains(owner ?? "")).OrderByDescending(b => b.CreatedDate).AsQueryable();
            if(isPagging) {
                return await query.Skip((pageIndex - 1) * (int)PageManagement.PageSize).Take((int)PageManagement.PageSize)
                                    .ToListAsync<Bill>();
            }
            else
            {
                return await query.ToListAsync<Bill>();   
            }
        }

        public async Task CreateBill(List<Bill> listBills)
        {
            await _context.Bills.AddRangeAsync(listBills);
            await _context.SaveChangesAsync();
        }

        public async Task SaveBillAsync(List<Bill> bills)
        {
            foreach(var bill in bills) {
                _context.Attach(bill).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public Task<int> CountListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner)
        {
            return _context.Bills.Include(b => b.User).Include(b => b.Room)
                                    .Where(b => b.CreatedDate >= (from ?? b.CreatedDate) &&
                                    b.CreatedDate <= (to ?? b.CreatedDate) &&
                                    b.Room.Name.Contains(roomName ?? "") &&
                                    b.User.FullName.Contains(owner ?? ""))                           
                                    .CountAsync();
        }
    }
}
