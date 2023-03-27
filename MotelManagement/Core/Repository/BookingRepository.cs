using Microsoft.EntityFrameworkCore;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;
using System.Globalization;

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

        public async Task<List<Booking>> BookingListByRoomAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, string? fromBooking, string? toBooking)
        {
            //
            DateTime? from = fromBooking != null ? DateTime.ParseExact(fromBooking,"dd/MM/yyyy",CultureInfo.InvariantCulture) : null; 
            DateTime? to = toBooking != null ? DateTime.ParseExact(toBooking, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null;


            Room room = _context.Rooms.Where(r => r.RoomId == roomId && r.StatusId == 2).FirstOrDefault();
            List<Booking> listBookings = new List<Booking>();
            if (room != null)
            {
                listBookings = await _context.Bookings.Include(u => u.User).Include(u => u.Room).Where(b => b.Status == 1 && b.RoomId == roomId
                                                                                                       && b.User.FullName.Contains( nameBooking ?? b.User.FullName)
                                                                                                       && b.User.Email.Contains( emailBooking ?? b.User.Email)
                                                                                                       && b.User.Phone.Contains( phoneBooking ?? b.User.Phone)
                                                                                                       && ( b.BookingTime >= ( from ?? b.BookingTime ) && b.BookingTime <= (to ?? b.BookingTime) ) 
                                                                                                       ).ToListAsync();
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
        public async Task<bool> isBookingPassing(int? roomid, int userId)
        {
            Passing passing = await _context.Passings
                                            .Where(b => b.RoomId == roomid && b.UserRequestId == userId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER)
                                            .FirstOrDefaultAsync();
            return passing != null;
        }

        public async Task<List<Booking>> listBookings(int userId)
        {
            List <Booking> listBooking = await _context.Bookings
                                            .Where(b => b.UserId == userId && 
                                                        (b.Status == (int)REGISTER_ROOM_STATE.REGISTER || b.Status == (int)REGISTER_ROOM_STATE.UN_REGISTER || b.Status==(int)REGISTER_ROOM_STATE.REJECT))                   
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

        public async Task updateMeetingDateForUser(DateTime? meetingDate, int? bookingId)
        {
            Booking booking =  await _context.Bookings.Where(b => b.BookingId == bookingId).FirstOrDefaultAsync();
            booking.MeetingDate = meetingDate;
            _context.Update(booking);
        }

        public async Task updateSuccessRejectUsersExceptMember(int memberId, int roomId, int bookingId)
        {
            // Update status member; 
            Booking booking = await _context.Bookings.Where(b => b.BookingId == bookingId).FirstOrDefaultAsync();
            booking.Status = (int)REGISTER_ROOM_STATE.SUCCESS;
            _context.Update(booking);
            // Update status user to be rejected
            List<Booking> bookings = await _context.Bookings.Where(b => b.RoomId == roomId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER && b.BookingId != bookingId).ToListAsync();
            foreach(Booking book in bookings)
            {
                book.Status = (int)REGISTER_ROOM_STATE.REJECT;
                _context.Update(book); 
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
