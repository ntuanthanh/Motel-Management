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

        public MyRoomModel(IBillService billService, ILogger<MyRoomModel> logger)
        {
            this._serviceBill = billService;
            this._logger = logger;
        }
        public void OnGet()
        {
            ViewData["countBills"] = 2;
        }

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
                    Console.WriteLine("paidTimeFrom---" + paidTimeFrom + "paidTimeTo-" + paidTimeTo + "confirmDateFrom-" + confirmDateFrom + "confirmDateTo-" + confirmDateTo + "owner-" + owner + "isDept-" + isDept + "roomId-" + roomId + "pageIndex-" + pageNumber);
                    List<Bill> listBills = await _serviceBill.getListBills(paidTimeFrom, paidTimeTo, confirmDateFrom, confirmDateTo,
                                                                        owner == -1 ? (int?)null : owner,
                                                                        isDept == -2 ? (int?)null : isDept,
                                                                        roomId, pageNumber ?? 1);
                    int countBills = await _serviceBill.countListBills(paidTimeFrom, paidTimeTo, confirmDateFrom, confirmDateTo,
                                                                        owner == (int)DEFAULT_VALUE.USER_SELECT ? (int?)null : owner,
                                                                        isDept == (int)DEFAULT_VALUE.PAYMENT_STATE ? (int?)null : isDept,
                                                                        roomId);
                    ViewData["countBills"] = countBills;
                    var result = new {listBills, countBills};
                    Console.WriteLine("------" + countBills);
                    return new JsonResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
            return null;
        }
    }
}
