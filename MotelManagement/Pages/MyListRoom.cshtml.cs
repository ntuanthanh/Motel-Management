using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages
{

    public class MyListRoomModel : PageModel
    {
        private readonly IBookingService _serviceBooking;
        private readonly IPassingService _servicePassingService;
        private readonly IContractService _serviceContract;
        private readonly IRoomService _serviceRoom;
        private readonly ILogger<LoginModel> _logger;
        public MyListRoomModel(IBookingService bookingService, ILogger<LoginModel> logger, IContractService contractService, IRoomService serviceRoom, IPassingService passingService)
        {
            this._serviceBooking = bookingService;
            _servicePassingService = passingService;
            this._serviceContract = contractService;
            _serviceRoom = serviceRoom; 
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
                    List<Booking> list = await _serviceBooking.listBookings(user.UserId);
                    List<Contract> listContracts = await _serviceContract.getListContractsByUserId(user.UserId);
                    List<Passing> listPassings = await _servicePassingService.GetListPassings(user.UserId);
                    ViewData["list"] = list;
                    ViewData["listPassings"] = listPassings;
                    ViewData["listContracts"] = listContracts;
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
                    bool isExist = await _serviceBooking.isBooking(roomid, user.UserId);
                    if(isExist&&roomid!=null)
                    {
                        await _serviceBooking.updateUnRegister(user.UserId, roomid);
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

        public async Task<IActionResult> OnGetUnregisterPassingAsync(int roomid)
        {

            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            if (user != null)
            {
                try
                {
                    bool isExist = await _servicePassingService.isPassing(roomid, user.UserId);
                    if (isExist && roomid != null)
                    {
                        await _servicePassingService.UpdateUnregisterPassing(user.UserId, roomid);
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



        // Thành 
        public async Task<IActionResult> OnGetRegisterAsync(int roomid)
        {
            string json = HttpContext.Session.GetString("user"); 
            User user = UserUtil.getUserFromSession(json);
            if (user != null)
            {
                try
                {
                    bool isExist = await _serviceBooking.isBooking(roomid, user.UserId);
                    // TH user dùng url, nếu phòng đã có nguời đang có người thuê nhưng vẫn booking
                    Room room = await _serviceRoom.getRoomById(roomid);
                    if (!isExist && roomid != null && room.StatusId == (int)ROOM_STATE.PROCESSING)
                    {
                        await _serviceBooking.Register(user.UserId, roomid);
                        TempData["Message"] = "Success";
                    }
                    else if(isExist)
                    {
                        TempData["Message"] = "Registered";
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
        // Thành - Đăng kí Passing
        public async Task<IActionResult> OnGetRegisterPassingAsync(int roomId)
        {
            // Cần RoomId, MemberId, UserRequest 
            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            if (user != null)
            {
                try
                {
                    // Phải check thêm nữa là nếu như là member thì không được đặt phòng đó
                    bool isMemberOfRoom = await _serviceContract.isMemberOfRoom(roomId, user.UserId);

                    // TH user dùng url, nếu phòng đã có nguời đang có người thuê nhưng vẫn booking passing
                    bool isExist = await _servicePassingService.isBookingPassing(roomId, user.UserId);
                    // Check xem room tồn tại không
                    Room room = await _serviceRoom.getRoomById(roomId);

                    if (!isMemberOfRoom && !isExist && roomId != null && room.StatusId == (int)ROOM_STATE.PASSING)
                    {
                        await _servicePassingService.RegisterPassing(user.UserId, roomId);
                        TempData["Message"] = "Success";
                    }
                    else if (isExist)
                    {
                        TempData["Message"] = "Registered";
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
