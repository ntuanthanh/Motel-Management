using MotelManagement.Business.IService;
using MotelManagement.Core.IRepository;
using MotelManagement.Core.Repository;
using MotelManagement.Data.Models;
using MotelManagement.Pages;
using System.Collections;

namespace MotelManagement.Business.Service
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Bill>> getListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId, int pageIndex)
        {
            return await _unitOfWork.billRepository.getListBills(paidTimeFrom, paidTimeTo, confirmDateFrom, confirmDateTo, owner, isDept, roomId, pageIndex);
        }
        public async Task<int> countListBills(DateTime? paidTimeFrom, DateTime? paidTimeTo, DateTime? confirmDateFrom, DateTime? confirmDateTo, int? owner, int? isDept, int roomId)
        {
            return await _unitOfWork.billRepository.countListBills(paidTimeFrom, paidTimeTo, confirmDateFrom, confirmDateTo, owner, isDept, roomId);
        }


        public async Task<Hashtable> getOwners(int roomId, int userId)
        {
            return await _unitOfWork.contractRepository.getOwners(roomId, userId);
        }

        public async Task<List<Bill>> getListUnPaidBill(int userId, int roomId)
        {
            return await _unitOfWork.billRepository.getListUnPaidBills(userId, roomId);
        }

        public async Task<Bill> getBillByIdAsync(int billId)
        {
            return await ((BillRepository)_unitOfWork.billRepository).GetByIdAsync(billId);
        }

        public async Task SubmitBillAsync(Bill bill)
        {
             await _unitOfWork.billRepository.SubmitBillAsync(bill);
        }

        public async Task<List<Bill>> GetListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner, int pageIndex, bool isPagging)
        {
            return await _unitOfWork.billRepository.GetListBillsByAdmin(from, to, roomName, owner, pageIndex, isPagging);
        }

        public async Task CreateBill(List<Bill> listBills)
        {
            await _unitOfWork.billRepository.CreateBill(listBills);
        }

        public async Task SaveBillAsync(List<Bill> bills)
        {
            await _unitOfWork.billRepository.SaveBillAsync(bills);
        }

        public async Task<int> CountListBillsByAdmin(DateTime? from, DateTime? to, string roomName, string owner)
        {
            return await _unitOfWork.billRepository.CountListBillsByAdmin(from, to, roomName, owner);
        }
    }
}
