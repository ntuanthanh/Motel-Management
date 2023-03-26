using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages
{
    public class ListPassingModel : PageModel
    {
        private readonly IPassingService _passingService;
        public List<Passing> bookings { get; set; }

        public ListPassingModel(IPassingService passingService)
        {
            _passingService = passingService;
        }
        public async Task<IActionResult> OnGetAsync(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, DateTime? fromBooking, DateTime? toBooking)
        {
            // Danh sách booking của room id 
            bookings = await _passingService.PassingBookingsAvailableSearching(roomId, nameBooking, emailBooking, phoneBooking, fromBooking, toBooking);

            // Check if có phòng đã có người thuê thì không hiện danh sách booking
            bool isRoomRented = await _passingService.isRoomRented(roomId);
            ViewData["isRoomRented"] = isRoomRented;

            // Check if có phòng đã có đồng ý cho user thuê chỉ chờ bên admin duyệt
            bool isRoomWaiting = await _passingService.isRoomWaiting(roomId);
            ViewData["isRoomWaiting"] = isRoomWaiting;

            // search history 
            ViewData["roomId"] = roomId;
            ViewData["nameBooking"] = nameBooking;
            ViewData["emailBooking"] = emailBooking;
            ViewData["phoneBooking"] = phoneBooking;
            ViewData["fromBooking"] = fromBooking;
            ViewData["toBooking"] = toBooking;
            return Page();
        }
        // Gửi thông tin lịch hẹn cho từng user
        public async Task<IActionResult> OnPostAllUserAsync(DateTime? meetingdate, int? roomId)
        {
            await _passingService.UpdateMeetingDateAllUser(meetingdate, roomId);
            return Redirect("/user/room/passing/list?roomId=" + roomId);
        }
        // Gửi thông tin lịch hẹn cho riêng user 
        public async Task<IActionResult> OnGetChangeMeetingAsync(DateTime? meetingDate, int? bookingId, int? roomId)
        {
            await _passingService.UpdateMeetingForUser(meetingDate, bookingId, roomId);
            return Redirect("/user/room/passing/list?roomId=" + roomId);
        }
        // Set User to be Member 
        public async Task<IActionResult> OnGetSetMemberToRoomAsync(int roomId, int userId, int bookingId)
        {
            // set member to room
             await _passingService.PushUserToAdmin(userId, roomId, bookingId);
            return Redirect("/user/room/passing/list?roomId=" + roomId);
        }
    }
}
