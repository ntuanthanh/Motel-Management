using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages.admin
{
    public class ListPassingModel : PageModel
    {
        private readonly IPassingService _passingService; 
        public List<Passing> bookings { get; set; }
        public ListPassingModel(IPassingService passingService)
        {
            _passingService = passingService;   
        }
        public async Task<IActionResult> OnGetAsync(string? roomBooking, string? nameBooking, string? emailBooking, string? fromBooking, string? toBooking)
        {
            // Danh sách passing booking của roomi id
            bookings =  await _passingService.PassingsWaitingSearching(roomBooking, nameBooking, emailBooking, fromBooking, toBooking);

            // search history 
            ViewData["nameBooking"] = nameBooking;
            ViewData["emailBooking"] = emailBooking;
            ViewData["roomBooking"] = roomBooking;
            ViewData["fromBooking"] = fromBooking;
            ViewData["toBooking"] = toBooking;

            return Page();
        }
        // Set User to be Member 
        public async Task<IActionResult> OnGetSetMemberToRoomAsync(int roomId, int userId, decimal price, int bookingId)
        {
            // set member to room
            await _passingService.SetUserBeMember(userId, roomId, price, bookingId);
            return Redirect("/admin/room/passing/list");
        }
        // Reject Passing 
        public async Task<IActionResult> OnGetRejectPassingAsync(int roomId, int userId, int bookingId)
        {
            // set member to room
            await _passingService.RejectPassing(userId, roomId, bookingId);
            return Redirect("/admin/room/passing/list");
        }
    }
}
