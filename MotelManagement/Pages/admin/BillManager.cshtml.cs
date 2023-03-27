using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
                listBills = await _serviceBill.GetListBillsByAdmin(from, to, roomName, owner, pageIndex, true);
                listStates = new List<Status>() {
                    new Status { StatusId = (int)PAYMENT_STATE.PAID, Name = "Đã trả"},
                    new Status { StatusId = (int)PAYMENT_STATE.DEBT, Name = "Còn nợ"},
                    new Status { StatusId = (int)PAYMENT_STATE.UNPAID, Name = "Chưa trả"},
                    new Status { StatusId = (int)PAYMENT_STATE.NOT_CONFIRM, Name = "Chưa xác nhận"},
                };
                int totalRecords = await _serviceBill.CountListBillsByAdmin(from, to, roomName, owner);

                string url = "bill?";
                string url_param = Request.QueryString.ToString();
                if (url_param != null && url_param.Length > 0)
                {
                    url = "bill";
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

                ViewData["from"] = from;
                ViewData["to"] = to;
                ViewData["roomName"] = roomName;
                ViewData["owner"] = owner;
                ViewData["totalPage"] = (totalRecords % 3 == 0) ? (totalRecords / 3) : (totalRecords / 3) + 1;
                ViewData["pageIndex"] = pageIndex;
                ViewData["url"] = url;
            }
            catch(Exception ex) { 
                _logger.LogError(ex.Message.ToString());    
            }
        }

        public async Task OnPostSaveBillAsync(int[] states, int[] billIds, string[] paids)
        {
            List<Bill> listBills = new List<Bill>();
            try
            {
                for(int index=0;index<billIds.Length; index++)
                {
                    Bill bill = await _serviceBill.getBillByIdAsync(billIds[index]);
                    // if bill state is paid => paids can be empty
                    if (states[index] == (int)PAYMENT_STATE.PAID)
                    {
                        bill.BillState = (int)PAYMENT_STATE.PAID;
                        bill.Debt = 0;
                        bill.AcceptTime = DateTime.Now;
                        listBills.Add(bill);
                    }
                    // dept => cannot paid
                    else if (!String.IsNullOrEmpty(paids[index]) && states[index]!=(int) PAYMENT_STATE.PAID)
                    {
                        string paid = paids[index].Substring(0, paids[index].Length-4).Replace(".","");
                        Console.WriteLine(bill.RoomBill+" "+bill.ElectricBill+" "+bill.WaterBill+" "+bill.ServiceBill+" "+paid);
                        bill.Debt = GetDept(bill.RoomBill, bill.ElectricBill, bill.WaterBill, bill.ServiceBill, decimal.Parse(paid));
                        if (bill.Debt > 0)
                        {
                            bill.AcceptTime = DateTime.Now;
                            bill.BillState = (int)PAYMENT_STATE.DEBT;
                            bill.AcceptTime = DateTime.Now;
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
        public async Task<IActionResult> OnGetExportBillAsync(DateTime? from, DateTime? to, string roomName, string owner, int pageIndex)
        {
            listBills = await _serviceBill.GetListBillsByAdmin(from, to, roomName, owner, pageIndex, false);
            // Tạo một tệp Excel mới
            var workbook = new XLWorkbook();

            // Tạo một bảng mới trong tệp Excel
            var worksheet = workbook.Worksheets.Add("Hóa đơn phòng");

            // Ghi tiêu đề cho các cột
            worksheet.Cell(1, 1).Value = "Phòng";
            worksheet.Cell(1, 2).Value = "Tiền phòng";
            worksheet.Cell(1, 3).Value = "Tiền điện";
            worksheet.Cell(1, 4).Value = "Tiền nước";
            worksheet.Cell(1, 5).Value = "Tiền dịch vụ";
            worksheet.Cell(1, 6).Value = "Tên khách hàng";
            worksheet.Cell(1, 7).Value = "Còn nợ";
            worksheet.Cell(1, 8).Value = "Ngày nộp";
            worksheet.Cell(1, 9).Value = "Ngày duyệt";
            worksheet.Cell(1, 10).Value = "Ngày tạo";
            worksheet.Cell(1, 11).Value = "Mô tả";
            // Lấy dữ liệu từ cơ sở dữ liệu hoặc bất kỳ nguồn nào khác
            //var students = _dbContext.Students.ToList();
            worksheet.Columns("B").Style.NumberFormat.Format = "#,##0\" đ\"";
            worksheet.Columns("C").Style.NumberFormat.Format = "#,##0\" đ\"";
            worksheet.Columns("D").Style.NumberFormat.Format = "#,##0\" đ\"";
            worksheet.Columns("E").Style.NumberFormat.Format = "#,##0\" đ\"";
            worksheet.Columns("G").Style.NumberFormat.Format = "#,##0\" đ\"";
            for (int i=1; i<=11; i++){
                worksheet.Row(1).Cell(i).Style.Fill.BackgroundColor = XLColor.Yellow;
            }
            // Ghi dữ liệu vào bảng
            for (int i = 0; i < listBills.Count; i++)
            {
                var bill = listBills[i];
                worksheet.Cell(i + 2, 1).Value = bill.Room.Name;
                worksheet.Cell(i + 2, 2).Value = bill.RoomBill;
                worksheet.Cell(i + 2, 3).Value = bill.ElectricBill;
                worksheet.Cell(i + 2, 4).Value = bill.WaterBill;
                worksheet.Cell(i + 2, 5).Value = bill.ServiceBill;
                worksheet.Cell(i + 2, 6).Value = bill.User.FullName;
                worksheet.Cell(i + 2, 7).Value = bill.Debt;
                worksheet.Cell(i + 2, 8).Value = bill.PaidTime;
                worksheet.Cell(i + 2, 9).Value = bill.AcceptTime;
                worksheet.Cell(i + 2, 10).Value = bill.CreatedDate;
                worksheet.Cell(i + 2, 11).Value = bill.Description;
            }

            // Lưu tệp Excel và gửi về phía người dùng để tải xuống
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            Response.Headers.Add("Refresh", "1;url=/admin/bill");
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HoaDonPhong.xlsx");
        }

        public async Task OnGetFinishBill()
        {

        }

        public async Task<IActionResult> OnPostImportBillAsync(IFormFile file)
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
                        int row = 2;
                        var xl = new XLWorkbook(stream).Worksheets.First();
                        while (true)
                        {
                            if ("".Equals(xl.Row(row).Cell(1).Value.ToString())) break;
                            bill = new Bill();
                            Contract contract = await _serviceRoom.isRoomRented(xl.Row(row).Cell(1).Value.ToString());
                            if (contract != null)
                            {
                                bill.CreatedDate = DateTime.Now;
                                bill.BillState = (int)PAYMENT_STATE.UNPAID;
                                Console.WriteLine(xl.Row(row).Cell(1).Value.ToString());
                                Console.WriteLine(xl.Row(row + 1).Cell(1).Value.ToString());
                                Console.WriteLine(xl.Row(row).Cell(2).ToString());
                                Console.WriteLine(xl.Row(row).Cell(2).Value.ToString());
                                bill.RoomBill = Decimal.Parse(xl.Row(row).Cell(2).Value.ToString());
                                bill.ElectricBill = Decimal.Parse(xl.Row(row).Cell(3).Value.ToString());
                                bill.WaterBill = Decimal.Parse(xl.Row(row).Cell(4).Value.ToString());
                                bill.ServiceBill = Decimal.Parse(xl.Row(row).Cell(5).Value.ToString());
                                bill.UserId = contract.UserId;
                                bill.RoomId = contract.RoomId;
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
            return Redirect("/admin/bill");
        }

        //Common Processcing:
        public decimal GetDept(decimal roomBill, decimal electricBill, decimal waterBill, decimal serviceBill, decimal paid)
        {
            return roomBill+electricBill+waterBill+serviceBill-paid;
        }

        public decimal GetTotal(decimal roomBill, decimal electricBill, decimal waterBill, decimal serviceBill)
        {
            return roomBill + electricBill + waterBill + serviceBill;
        }
    }
}
