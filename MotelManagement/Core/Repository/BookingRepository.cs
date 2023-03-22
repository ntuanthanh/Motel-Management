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

        public async Task<List<Booking>> BookingListByRoomAvailable(int? roomId)
        {
            Room room = _context.Rooms.Where(r => r.RoomId == roomId && r.StatusId == 2).FirstOrDefault();
            List<Booking> listBookings = new List<Booking>(); 
            if (room != null) { 
               listBookings = await _context.Bookings.Include(u => u.User).Include(u => u.Room).Where(b => b.Status == 1 && b.RoomId == roomId).ToListAsync();
            }
            return listBookings;
        }

        public async Task<bool> isBooking(int? roomid, int userId)
        {
            Booking booking = await _context.Bookings
                                            .Where(b=>b.RoomId==roomid&&b.UserId==userId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER)
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

        public async Task updateMeetingDate(DateTime? meetingDate, int? roomId)
        {
            List<Booking> bookings = await BookingListByRoomAvailable(roomId);
            //Booking booking = await _context.Bookings.Where(b => b.BookingId == 12).FirstOrDefaultAsync();
            //booking.MeetingDate = meetingDate;
            //_context.Update(booking);
            //_context.Entry(booking).Property(b => b.MeetingDate).IsModified = true;
            if (bookings != null && bookings.Count > 0)
            {
                foreach (Booking booking in bookings)
                {
                    booking.MeetingDate = meetingDate;
                    //_context.Entry(booking).Property(s => s.MeetingDate).IsModified = true;
                    _context.Update(booking);
                }
            }
        }

        public async Task updateUnRegister(int userId, int roomid)
        {
            Booking booking = await _context.Bookings
                                            .Where(b => b.RoomId == roomid && b.UserId == userId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER)
                                            .FirstOrDefaultAsync();
            booking.Status = (int) REGISTER_ROOM_STATE.UN_REGISTER;
            _context.Entry(booking).Property(s => s.Status).IsModified = true;
        }
    }
}
