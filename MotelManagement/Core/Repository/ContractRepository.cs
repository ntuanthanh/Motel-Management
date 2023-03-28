using Microsoft.EntityFrameworkCore;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Core.Repository
{
    public class ContractRepository : BaseRepository<Contract>, IContractRepository
    {
        public ContractRepository(MotelManagementContext context) : base(context)
        {
        }

        public async Task addUsertoRoom(int roomId, int userId, decimal price )
        {
            int timeContract = (int)TimeContract.time;
            Contract contract = new Contract();
            contract.RoomId = roomId;
            contract.UserId = userId;
            contract.Price = price;
            contract.EffectiveFrom = DateTime.Now;
            contract.EffectiveTo = contract.EffectiveFrom.AddMonths(timeContract);
            contract.IsActive = true; 
            _context.Add(contract);
        }

        public async Task ExtendsContractAsyn(Contract c)
        {
            _context.Contracts.Attach(c);
            _context.Entry(c).Property(c => c.EffectiveTo).IsModified = true;
        }

        public async Task<Contract> GetContractByRoomId(int roomId, int userId)
        {
            return await _context.Contracts.Where(c=>c.RoomId == roomId 
                                                     && c.UserId == userId
                                                     && c.IsActive==true).FirstOrDefaultAsync();
        }

        public async Task<List<Contract>> getListContractsByUserId(int userId)
        {
            return await _context.Contracts.Where(c => c.UserId==userId)
                                    .Include(c=>c.Room)
                                        .ThenInclude(c=> c.Images)
                                    .ToListAsync();

        }

        public async Task<Hashtable> getOwners(int roomId, int userId)
        {
            bool isPermit = false;
            List<Contract> contracts = await _context.Contracts.Where(c=>c.RoomId==roomId
                                                                        &&c.IsActive==true).Include(u=>u.User).ToListAsync();
            Hashtable onwers = new Hashtable();
            foreach(Contract c in contracts)
            {
                onwers.Add(c.User.UserId, c.User.FullName);
                if(c.UserId==userId) isPermit = true;
            }
            if (isPermit) return onwers;
            else return null;
        }

        public async Task<bool> IsMemberOfRoom(User user, int roomId)
        {
             Contract contract =  _context.Contracts.Where(c => c.RoomId == roomId 
                                                             && c.User.UserId == user.UserId 
                                                             && c.IsActive == true).FirstOrDefault();
            if(contract != null)
                return true;
            else
                return false;
        }

        public async Task UpdateMoveContract(int roomId)
        {
            // Query 
            List<Contract> contracts = new List<Contract>();
            contracts = await _context.Contracts.Where(c => c.RoomId == roomId && c.IsActive == true).ToListAsync(); 
            foreach(Contract contract in contracts)
            {
                contract.IsActive = false;
                _context.Update(contract); 
            }
        }
    }
}
