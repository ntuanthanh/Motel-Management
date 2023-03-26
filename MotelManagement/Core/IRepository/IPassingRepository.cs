using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IPassingRepository : IGenericRepository<Passing>
    {
        public Task<bool> isBookingPassing(int? roomid, int userId);
        public Task<List<Passing>> PassingBookingListByRoomAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, DateTime? fromBooking, DateTime? toBooking);
        Task updateMeetingDate(DateTime? meetingDate, int? roomId);
        Task updateMeetingDateForUser(DateTime? meetingDate, int? bookingId);
        public Task<List<Passing>> BookingListByRoomAvailable(int? roomId);
        Task updateSuccessRejectUsersExceptMember(int memberId, int roomId, int bookingId);

    }
}
