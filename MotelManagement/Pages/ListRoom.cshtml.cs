using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;
using System.Collections.Generic;

namespace MotelManagement.Pages
{
    public class ListRoomModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public ListRoomModel(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }
        public async Task<IActionResult> OnGetAsync(string nameRoom, int? roomTypeId, int? status, string price, string sizePerson, int pageIndex)
        {
            // All RoomType 
            List<Status> statusList = new List<Status>();
            statusList.Add(new Status { StatusId = (int)ROOM_STATE.RENTED, Name = "Đã Thuê"} );
            statusList.Add(new Status { StatusId = (int)ROOM_STATE.PROCESSING, Name = "Còn Trống" });
            statusList.Add(new Status { StatusId = (int)ROOM_STATE.PASSING, Name = "Muốn Pass" });
            ViewData["statusList"] = statusList;

            // All RoomType 
            List<RoomType> roomTypes = await _roomTypeService.GetAll();
            ViewData["roomTypes"] = roomTypes;

            // Convert page 
            if(pageIndex <= 0)
                pageIndex = 1;

            // Advanced Search 
            List<Room> rooms = await _roomService.advancedSearchRoomList(nameRoom, roomTypeId, status, price, sizePerson, pageIndex);
 
            // Count để phân trang
            int totalPage = await _roomService.totalPage(nameRoom, roomTypeId, status, price, sizePerson);

            // Phần RECOMMENDED ( Lấy top 4 phòng còn trống có giá thấp nhất )
            List<Room> roomsRecommendeds = await _roomService.roomRecommended(); 

            // Lấy Url 
            // Lấy Url để phân trang cho avancedSearch 
            string url = "List?";
            string url_param = Request.QueryString.ToString();
            if (url_param != null && url_param.Length > 0)
            {
                url = "List";
                if (url_param.EndsWith("pageIndex=" + pageIndex))
                {
                    url_param = url_param.Replace("pageIndex=" + pageIndex, "");
                }
                // nếu nó không rời vào trường hợp book?page=x và thiếu & thì thêm vào
                if (!url_param.Equals("") && !url_param.EndsWith("&") && !url_param.EndsWith("?"))
                {
                    url_param += "&";
                }
                url += (url_param);
            }
            ViewData["url"] = url;
            ViewData["totalPage"] = totalPage;
            ViewData["pageIndex"] = pageIndex;
            ViewData["rooms"] = rooms;
            ViewData["roomsRecommendeds"] = roomsRecommendeds;
            // History Search 
            ViewData["nameRoom"] = nameRoom;
            ViewData["roomTypeId"] = roomTypeId;
            ViewData["status"] = status;
            ViewData["price"] = price;
            ViewData["sizePerson"] = sizePerson; 
            return Page();
        }
    }
}
