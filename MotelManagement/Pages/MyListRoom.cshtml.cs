using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages
{

    public class MyListRoomModel : PageModel
    {
        private readonly IBookingService _service;
        private readonly ILogger<LoginModel> _logger;
        public MyListRoomModel(IBookingService bookingService, ILogger<LoginModel> logger)
        {
            this._service = bookingService;
            this._logger = logger;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            if (user != null)
            {
                try
                {
                    List<Booking> list = await _service.listBookings(user.UserId);
                    ViewData["list"] = list;
                    return Page();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetUnregisterAsync(int roomid)
        {

            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            if (user != null)
            {
                try
                {
                    bool isExist = await _service.isBooking(roomid, user.UserId);
                    if(isExist&&roomid!=null)
                    {
                        await _service.updateUnRegister(user.UserId, roomid);
                        TempData["Message"] = "Success";
                    }
                    else
                    {
                        TempData["Message"] = "Probs";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    TempData["Message"] = "Probs";
                }
            }
            return Redirect("~/user/mylistroom");
        }


    }
}
