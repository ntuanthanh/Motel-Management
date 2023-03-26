using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IBookingService
    {
        public Task<bool> isBooking(int? roomid, int userId);
        public Task<bool> isBookingPassing(int? roomid, int userId); 
        public Task<List<Booking>> listBookings(int userId);

        public Task updateUnRegister(int userId, int roomid);
        public Task Register(int userId, int roomid);
        public Task RegisterPassing(int userId, int roomid);   
        public Task<List<Booking>> BookingsAvailable(int? roomId);
        public Task<List<Booking>> BookingsAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, string? fromBooking, string? toBooking);
        public Task UpdateMeetingDateAllUser(DateTime? dateTime, int? roomId);
        public Task UpdateMeetingForUser(DateTime? dateTime,int? bookingId, int? roomId);
        public Task SetUserBeMember(int userId, int roomid, decimal price, int bookingId);
        public Task<bool> isRoomRented(int? roomId); 
    }
}
