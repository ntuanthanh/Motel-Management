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
        public MyRoomModel(IBillService billService, ILogger<MyRoomModel> logger, IWebHostEnvironment env)
        {
            this._serviceBill = billService;
            this._logger = logger;
            this._env = env;
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
                for(int i = 0; i < billIds.Length; i++)
                {
                    if (String.IsNullOrEmpty(descriptions[i]) || billIds[i]==null || filename[i]==null)
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
                TempData["BillMessage"] = "Success";
            }
            catch(Exception ex) 
            {
                TempData["BillMessage"] = "Failed";
                _logger.LogError(ex.ToString());
            }
            return Redirect("/user/manageroom?roomId=" + roomId);
        }

//      Report broken devices
        public async Task OnPostReportBrokenAsync()
        {

        }

//      Extend a room contract
        public async Task OnPostExtendContractAsync()
        {

        }

//      Passing room
        public async Task OnPostPassingRoomAsync()
        {

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
                    var result = new {listBills, countBills};
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
            switch(state)
            {
                case (int) PAYMENT_STATE.PAID:
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
