using Microsoft.EntityFrameworkCore;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Core.Repository
{
    public class PassingRepository : BaseRepository<Passing>, IPassingRepository
    {
        public PassingRepository(MotelManagementContext context) : base(context)
        {

        }
        public async Task<bool> isBookingPassing(int? roomid, int userId)
        {
            Passing passing = await _context.Passings
                                            .Where(b => b.RoomId == roomid && b.UserRequestId == userId && b.Status == (int)REGISTER_ROOM_STATE.REGISTER)
                                            .FirstOrDefaultAsync();
            return passing != null;
        }
    }
}
