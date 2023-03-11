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
        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetBillAsync(string paidTimeFrom, string paidTimeTo, string confirmDateFrom, string confirmDateTo, int owner, int isDept, int roomId, int? pageIndex)
        {
            string json = HttpContext.Session.GetString("user");
            User user = UserUtil.getUserFromSession(json);
            try
            {

            }catch(Exception ex)
            {

            }
            if (user != null)
            {
                Hashtable owners = await _serviceBill.getOwners(roomId, user.UserId);
                ViewData["owners"] = owners;
                List<Bill> listBills = await _serviceBill.getListBills(paidTimeFrom, paidTimeTo, confirmDateFrom, confirmDateTo, owner, isDept, roomId, pageIndex??1);
                ViewData["listBills"] = listBills;
                return new JsonResult(listBills);
            }
            return null;

        }
    }
}
