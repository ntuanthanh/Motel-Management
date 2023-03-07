using Microsoft.EntityFrameworkCore;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Core.Repository
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository 
    {
        public BookingRepository(MotelManagementContext context) : base(context)
        {

        }

        public async Task<bool> isBooking(int? roomid, int userId)
        {
            Booking booking = await _context.Bookings
                                            .Where(b=>b.RoomId==roomid&&b.UserId==userId)
                                            .FirstOrDefaultAsync();
            return booking != null;
        }

        public async Task<List<Booking>> listBookings(int userId)
        {
            List <Booking> listBooking = await _context.Bookings
                                            .Where(b => b.UserId == userId && 
                                                        (b.Status == (int)REGISTER_ROOM_STATE.REGISTER || b.Status == (int)REGISTER_ROOM_STATE.UN_REGISTER))                   
                                            .Include(r=>r.Room)
                                            .ThenInclude(r => r.Images)
                                            .ToListAsync();
            return listBooking;
        }

        public async Task updateUnRegister(int userId, int roomid)
        {
            Booking booking = await _context.Bookings
                                            .Where(b => b.RoomId == roomid && b.UserId == userId)
                                            .FirstOrDefaultAsync();
            booking.Status = (int) REGISTER_ROOM_STATE.UN_REGISTER;
            _context.Entry(booking).Property(s => s.Status).IsModified = true;
        }
    }
}
