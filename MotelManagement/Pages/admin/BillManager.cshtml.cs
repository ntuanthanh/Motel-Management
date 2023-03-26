using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Data.Models;

namespace MotelManagement.Pages.admin
{
    public class BillManagerModel : PageModel
    {
        private readonly IBillService _serviceBill;
        private readonly ILogger<BillManagerModel> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IContractService _serviceContract;
        private readonly IRoomService _serviceRoom;
        public List<Bill> listBills { get; set; }
        public List<Status> listStates { get; set; }
        public BillManagerModel(IBillService billService, ILogger<BillManagerModel> logger, IRoomService roomService)
        {
            this._serviceBill = billService;
            this._serviceRoom = roomService;
            this._logger = logger;
        }
        public async Task OnGet(DateTime? from, DateTime? to, string roomName, string owner, int pageIndex)
        {
            try
            {
                if (pageIndex == null || pageIndex < 1) pageIndex = 1;
                listBills = await _serviceBill.GetListBillsByAdmin(from, to, roomName, owner, pageIndex);
                listStates = new List<Status>() {
                    new Status { StatusId = (int)PAYMENT_STATE.PAID, Name = "Đã trả"},
                    new Status { StatusId = (int)PAYMENT_STATE.DEBT, Name = "Còn nợ"},
                    new Status { StatusId = (int)PAYMENT_STATE.UNPAID, Name = "Chưa trả"},
                    new Status { StatusId = (int)PAYMENT_STATE.NOT_CONFIRM, Name = "Chưa xác nhận"},
                };
                ViewData["from"] = from;
                ViewData["to"] = to;
                ViewData["roomName"] = roomName;
                ViewData["owner"] = owner;
            }
            catch(Exception ex) { 
                _logger.LogError(ex.Message.ToString());    
            }
        }

        public async Task OnPostSaveBillAsync(int[] states, int[] billIds, decimal[] paid)
        {
            List<Bill> listBills = new List<Bill>();
            try
            {
                for(int index=0;index<billIds.Length; index++)
                {
                    if (paid[index] != null)
                    {
                        Bill bill = await _serviceBill.getBillByIdAsync(index);
                        bill.Debt = GetDept(bill.RoomBill, bill.ElectricBill, bill.WaterBill, bill.ServiceBill, paid[index]);
                        if (bill.Debt > 0)
                        {
                            bill.BillState = states[index];
                            listBills.Add(bill);
                        }
                        else continue;
                    }
                    else continue;
                }
                await _serviceBill.SaveBillAsync(listBills);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        public async Task OnGetExportBillAsync(DateTime date)
        {

        }

        public async Task OnGetFinishBill()
        {

        }

        public async Task OnPostImportBillAsync(IFormFile file)
        {
            List<Bill> listBills = new List<Bill>();
            try
            {
                if (file != null & file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        Bill bill = new Bill();
                        int row = 1;
                        var xl = new XLWorkbook(stream).Worksheets.First();
                        Console.WriteLine(xl.LastRow().RowNumber());
                        while (true)
                        {
                            if ("".Equals(xl.Row(row).Cell(1).Value.ToString())) break;
                            bill = new Bill();
                            if (await _serviceRoom.isRoomRented(xl.Row(row).Cell(1).Value.ToString())){
                                bill.CreatedDate = DateTime.Now;
                                bill.BillState = (int)PAYMENT_STATE.UNPAID;
                                listBills.Add(bill);
                            }
                            row += 1;
                        }
                    }
                    await _serviceBill.CreateBill(listBills);
                }
            }
            catch(Exception ex) {
                _logger.LogError(ex.ToString());
            }
        }

        //Common Processcing:
        public decimal GetDept(decimal roomBill, decimal electricBill, decimal waterBill, decimal serviceBill, decimal paid)
        {
            return roomBill+electricBill+waterBill+serviceBill-paid;
        }

    }
}
