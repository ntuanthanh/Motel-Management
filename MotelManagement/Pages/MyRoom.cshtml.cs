using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;
using System.Collections;
using System.Text.Json;

namespace MotelManagement.Pages
{
    public class MyRoomModel : PageModel
    {
        private readonly IBillService _serviceBill;
        private readonly ILogger<MyRoomModel> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IContractService _serviceContract;
        private readonly IRoomService _serviceRoom;
        public MyRoomModel(IBillService billService, ILogger<MyRoomModel> logger,
            IWebHostEnvironment env, IContractService contractService, IRoomService serviceRoom)
        {
            this._serviceBill = billService;
            this._logger = logger;
            this._env = env;
            this._serviceContract = contractService;
            _serviceRoom = serviceRoom;
        }

        //      First page, load page when user click on room details => get base information into modal
        public async Task OnGet(int roomId)
        {
            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            if (user == null)
            {
                return;
            }
            else
            {
                try
                {
                    List<Bill> listUnPaidBills = await _serviceBill.getListUnPaidBill(user.UserId, roomId);
                    Hashtable owners = await _serviceBill.getOwners(roomId, user.UserId);
                    ViewData["listUnpaidBills"] = listUnPaidBills;
                    ViewData["owners"] = owners;
                    ViewData["roomId"] = roomId;
                    Room room = await _serviceRoom.getRoomById(roomId);
                    ViewData["room"] = room;
                    List<Status> statusList = new List<Status>();
                    statusList.Add(new Status { StatusId = (int)ROOM_STATE.RENTED, Name = "Đã Thuê" });
                    statusList.Add(new Status { StatusId = (int)ROOM_STATE.PROCESSING, Name = "Còn Trống" });
                    statusList.Add(new Status { StatusId = (int)ROOM_STATE.PASSING, Name = "Muốn Pass" });
                    ViewData["statusList"] = statusList;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
        }

        //      Processcing payment, user paid bills.
        public async Task<IActionResult> OnPostSubmitBillAsync(string[] descriptions, IFormFile[] bankingImages, int[] billIds, int roomId)
        {
            UploadFileUnit upload = new UploadFileUnit(_env);
            string[] filename = await upload.UploadFile(bankingImages, "bill", null);
            try
            {
                for (int i = 0; i < billIds.Length; i++)
                {
                    if (String.IsNullOrEmpty(descriptions[i]) || billIds[i] == null || filename[i] == null)
                    {
                        continue;
                    }
                    else
                    {
                        Bill bill = await _serviceBill.getBillByIdAsync(billIds[0]);
                        bill.PaidTime = DateTime.Now;
                        bill.BillState = (int)PAYMENT_STATE.NOT_CONFIRM;
                        bill.BankingImage = filename[0];
                        bill.Description = descriptions[0];
                        bill.AcceptTime = null;
                        await _serviceBill.SubmitBillAsync(bill);
                    }
                }
                TempData["Message"] = "Success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Failed";
                _logger.LogError(ex.ToString());
            }
            return Redirect("/user/manageroom?roomId=" + roomId);
        }

        //      Report broken devices
        public async Task OnPostReportBrokenAsync(string description, IFormFile desImage, int roomId)
        {


        }

        //      Extend a room contract
        public async Task<IActionResult> OnPostExtendContractAsync(int roomId, DateTime expectedDate)
        {
            User user = UserUtil.getUserFromSession(HttpContext.Session.GetString("user"));
            if(user==null)
            {
                return Page();
            }
            Contract c = await _serviceContract.GetContractByRoomId(roomId, user.UserId);
            if (c != null)
            {
                try
                {
                    c.EffectiveTo = expectedDate;
                    await _serviceContract.ExtendsContractAsyn(c);
                    TempData["Message"] = "ExtendsSucc";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    TempData["Message"] = "ExtendsFailed";
                }
            }
            else
            {
                TempData["Message"] = "ExtendsFailed";
            }
            return Redirect("/user/manageroom?roomId=" + roomId);
        }

        //      Passing room <-Chưa test->
        public async Task<IActionResult> OnPostPassingRoomAsync(int roomId)
        {
            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            if (user==null) { return Page(); }
            Room room = await _serviceRoom.getRoomById(roomId);
            try
            {
                Contract contract = await _serviceContract.GetContractByRoomId(roomId, user.UserId);
                if (room.StatusId == (int)ROOM_STATE.RENTED)
                {
                    room.StatusId = (int)ROOM_STATE.PASSING;
                    await _serviceRoom.PassingRoom(room);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return Redirect("/user/manageroom?roomId=" + roomId);
        }

        //      Load list bills history for user
        public async Task<JsonResult> OnGetBillAsync(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId, int? pageNumber)
        {
            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            if (user != null)
            {
                try
                {
                    Hashtable owners = await _serviceBill.getOwners(roomId, user.UserId);
                    ViewData["owners"] = owners;
                    if (pageNumber == null || pageNumber < 0) pageNumber = 1;
                    List<Bill> listBills = await _serviceBill.getListBills(paidTimeFrom, paidTimeTo, confirmDateFrom, confirmDateTo,
                                                                        owner == (int)DEFAULT_VALUE.USER_SELECT ? null : owner,
                                                                        isDept == (int)DEFAULT_VALUE.PAYMENT_STATE ? null : isDept,
                                                                        roomId, pageNumber ?? 1);
                    int countBills = await _serviceBill.countListBills(paidTimeFrom, paidTimeTo, confirmDateFrom, confirmDateTo,
                                                                        owner == (int)DEFAULT_VALUE.USER_SELECT ? null : owner,
                                                                        isDept == (int)DEFAULT_VALUE.PAYMENT_STATE ? null : isDept,
                                                                        roomId);
                    ViewData["countBills"] = countBills;
                    var result = new { listBills, countBills };
                    return new JsonResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
            return null;
        }

        //      Common processing
        public string GetBillState(int state)
        {
            switch (state)
            {
                case (int)PAYMENT_STATE.PAID:
                    return "Đã thanh toán";
                case (int)PAYMENT_STATE.UNPAID:
                    return "Chưa thanh toán";
                case (int)PAYMENT_STATE.DEBT:
                    return "Còn nợ";
                case (int)PAYMENT_STATE.NOT_CONFIRM:
                    return "Đang chờ duyệt";
                default:
                    return "";
            }
        }
        public Decimal GetTotalBill(Decimal roolBill, Decimal waterBill, Decimal electricBill)
        {
            return roolBill + waterBill + electricBill;
        }
    }
}
