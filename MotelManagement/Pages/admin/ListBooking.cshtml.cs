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
    }
}
