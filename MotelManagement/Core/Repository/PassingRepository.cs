using Microsoft.EntityFrameworkCore;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;
using System.Globalization;

namespace MotelManagement.Core.Repository
{
    public class PassingRepository : BaseRepository<Passing>, IPassingRepository
    {
        public PassingRepository(MotelManagementContext context) : base(context)
        {

        }
        public async Task<bool> isBookingPassing(int? roomid, int userId)
        {
            Passing passing = await _context.Passings
                                            .Where(b => b.RoomId == roomid && b.UserRequestId == userId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER)
                                            .FirstOrDefaultAsync();
            return passing != null;
        }

        public async Task<List<Passing>> PassingBookingListByRoomAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, DateTime? fromBooking, DateTime? toBooking)
        {

            //DateTime? from = fromBooking != null ? DateTime.ParseExact(fromBooking, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null;
            //DateTime? to = toBooking != null ? DateTime.ParseExact(toBooking, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null;


            Room room = _context.Rooms.Where(r => r.RoomId == roomId && r.StatusId == 3).FirstOrDefault(); // Phòng đang ở trạng thái passing
            List<Passing> listBookings = new List<Passing>();
            if (room != null)
            {
                listBookings = await _context.Passings.Include(u => u.UserRequest).Include(u => u.Room).Where(b => b.Status == 1 && b.RoomId == roomId
                                                                                                       && b.UserRequest.FullName.Contains(nameBooking ?? b.UserRequest.FullName)
                                                                                                       && b.UserRequest.Email.Contains(emailBooking ?? b.UserRequest.Email)
                                                                                                       && b.UserRequest.Phone.Contains(phoneBooking ?? b.UserRequest.Phone)
                                                                                                       && (b.BookingTime >= (fromBooking ?? b.BookingTime) && b.BookingTime <= (toBooking ?? b.BookingTime))
                                                                                                       ).ToListAsync();
            }
            return listBookings;
        }
        public async Task<List<Passing>> BookingListByRoomAvailable(int? roomId)
        {
            // StatusId = 3 : Passing 
            Room room = _context.Rooms.Where(r => r.RoomId == roomId && r.StatusId == 3).FirstOrDefault();
            List<Passing> listBookings = new List<Passing>();
            if (room != null)
            {
                listBookings = await _context.Passings.Include(u => u.UserRequest).Include(u => u.Room).Where(b => b.Status == 1 && b.RoomId == roomId).ToListAsync();
            }
            return listBookings;
        }

        public async Task updateMeetingDate(DateTime? meetingDate, int? roomId)
        {
            List<Passing> bookings = await BookingListByRoomAvailable(roomId);
            if (bookings != null && bookings.Count > 0)
            {
                foreach (Passing booking in bookings)
                {
                    booking.MeetingDate = meetingDate;
                    //_context.Entry(booking).Property(s => s.MeetingDate).IsModified = true;
                    _context.Update(booking);
                }
            }
        }
        public async Task updateMeetingDateForUser(DateTime? meetingDate, int? bookingId)
        {
            Passing passing = await _context.Passings.Where(b => b.PassingId == bookingId).FirstOrDefaultAsync();
            passing.MeetingDate = meetingDate;
            _context.Update(passing);
        }
        public async Task updateSuccessRejectUsersExceptMember(int memberId, int roomId, int bookingId)
        {
            // Update status member; 
            Passing booking = await _context.Passings.Where(b => b.PassingId == bookingId).FirstOrDefaultAsync();
            booking.Status = (int)REGISTER_ROOM_STATE.Waiting;
            _context.Update(booking);
            // Update status user to be rejected
            List<Passing> bookings = await _context.Passings.Where(b => b.RoomId == roomId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER && b.PassingId != bookingId).ToListAsync();
            foreach (Passing book in bookings)
            {
                book.Status = (int)REGISTER_ROOM_STATE.REJECT;
                _context.Update(book);
            }
        }

        public async Task<List<Passing>> PassingListByRoomWaitingSearching(string? roomBooking, string? nameBooking, string? emailBooking, string? fromBooking, string? toBooking)
        {
            DateTime? from = fromBooking != null ? DateTime.ParseExact(fromBooking, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null;
            DateTime? to = toBooking != null ? DateTime.ParseExact(toBooking, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

            List<Passing> listPassings = new List<Passing>();
            listPassings = await _context.Passings.Include(u => u.UserRequest).Include(u => u.Room).Where(b => b.Status == 4 && b.Room.Name.Contains(roomBooking ?? b.Room.Name)
                                                                                                       && b.UserRequest.FullName.Contains(nameBooking ?? b.UserRequest.FullName)
                                                                                                       && b.UserRequest.Email.Contains(emailBooking ?? b.UserRequest.Email)
                                                                                                       && (b.BookingTime >= (from ?? b.BookingTime) && b.BookingTime <= (to ?? b.BookingTime))
                                                                                                       ).ToListAsync();
             return listPassings;
        }

        public async Task updateStatusPassing(int memberId, int roomId, int bookingId, int status)
        {
            // Update status member; 
            Passing booking = await _context.Passings.Where(b => b.PassingId == bookingId).FirstOrDefaultAsync();
            booking.Status = status;
            _context.Update(booking);
        }

        public async Task<List<Passing>> GetListPassings(int userId)
        {
            return await _context.Passings.Include(m => m.Room.Images).Where(p => p.UserRequestId == userId &&
                                                    (p.Status == (int)REGISTER_ROOM_STATE.REJECT ||
                                                     p.Status == (int)REGISTER_ROOM_STATE.REGISTER ||
                                                     p.Status == (int)REGISTER_ROOM_STATE.Waiting ||
                                                     p.Status == (int)REGISTER_ROOM_STATE.UN_REGISTER)
                                                     ).ToListAsync();

        }

        public async Task<bool> isPassing(int roomid, int userId)
        {
            Passing passing = await _context.Passings
                                            .Where(b => b.RoomId == roomid && b.UserRequestId == userId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER)
                                            .FirstOrDefaultAsync();
            return passing != null;
        }

        public async Task UpdateUnregisterPassing(int userId, int roomid)
        {
            Passing passing = await _context.Passings
                                .Where(b => b.RoomId == roomid && b.UserRequestId == userId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER)
                                .FirstOrDefaultAsync();
            passing.Status = (int)REGISTER_ROOM_STATE.UN_REGISTER;
            _context.Entry(passing).Property(s => s.Status).IsModified = true;
        }
    }
}
