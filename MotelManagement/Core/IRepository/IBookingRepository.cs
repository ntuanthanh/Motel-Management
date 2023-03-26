using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        public Task<bool> isBooking(int? roomid, int userId);
        public Task<bool> isBookingPassing(int? roomid, int userId);
        public Task<List<Booking>> listBookings(int userId);
        Task updateUnRegister(int userId, int roomid);
        Task updateSuccessRejectUsersExceptMember(int memberId, int roomId, int bookingId); 
        Task updateMeetingDate(DateTime? meetingDate, int? roomId); 
        Task updateMeetingDateForUser(DateTime? meetingDate, int? bookingId);
        public Task<List<Booking>> BookingListByRoomAvailable(int? roomId);
        public Task<List<Booking>> BookingListByRoomAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, string? fromBooking, string? toBooking);

    }
}
