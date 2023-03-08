using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages
{
    public class DetailRoomModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public DetailRoomModel(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }
        public async Task OnGetAsync(int ?id)
        {
            // Phần tìm kiếm nâng cao to Room/List 
            // All RoomType 
            List<Status> statusList = new List<Status>();
            statusList.Add(new Status { StatusId = (int)ROOM_STATE.RENTED, Name = "Đã Thuê" });
            statusList.Add(new Status { StatusId = (int)ROOM_STATE.PROCESSING, Name = "Còn Trống" });
            statusList.Add(new Status { StatusId = (int)ROOM_STATE.PASSING, Name = "Muốn Pass" });
            ViewData["statusList"] = statusList;

            // All RoomType 
            List<RoomType> roomTypes = await _roomTypeService.GetAll();
            ViewData["roomTypes"] = roomTypes;

            // Get Room by Id 
            Room room = await _roomService.getRoomById(id);
            ViewData["room"] = room;

            // Get List Similar 
            List<Room> roomsSimilar = await _roomService.roomSimilar(room.StatusId);
            ViewData["roomsSimilar"] = roomsSimilar;
        }
    }
}
