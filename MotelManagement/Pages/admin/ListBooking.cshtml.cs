using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages.admin
{
    public class ListBookingModel : PageModel
    {
        private readonly IBookingService _bookingService;
        public List<Booking> bookings { get; set; }
        public ListBookingModel(IBookingService booking)
        {
            _bookingService = booking;
        }
        public async Task<IActionResult> OnGetAsync(int? roomId)
        {
            // Danh sách booking của room id 
            
            
            bookings  = await _bookingService.BookingsAvailable(roomId);
            return Page(); 
        }
        // Gửi thông tin lịch hẹn cho từng user
        public async Task<IActionResult> OnPostAllUserAsync(DateTime? meetingdate, int? roomId)
        {
             await _bookingService.UpdateMeetingDateAllUser(meetingdate, roomId);
            return Redirect("/admin/room/booking/list?roomId=" + roomId);
        }
        // Gửi thông tin lịch hẹn cho riêng user 
        public async Task<IActionResult> OnGetChangeMeetingAsync(DateTime? meetingDate, int? bookingId, int? roomId)
        {
            await _bookingService.UpdateMeetingForUser(meetingDate, bookingId, roomId);
            return Redirect("/admin/room/booking/list?roomId=" + roomId);
        }
    }
}
