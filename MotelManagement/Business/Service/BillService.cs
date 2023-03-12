using MotelManagement.Business.IService;
using MotelManagement.Core.IRepository;
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
    }
}
