using Microsoft.EntityFrameworkCore;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Core.Repository
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(MotelManagementContext context) : base(context)
        {

        }

        public async Task<List<Room>> advancedSearchRoom(string nameRoom, int? roomTypeId, int? status, decimal fromPrice, decimal toPrice, int fromSizePerson, int toSizePerson, int offSet, int count)
        {
            return await _context.Rooms.Where(r => 
                r.Name.Contains(nameRoom ?? r.Name) && 
                r.RoomTypeId  == (roomTypeId ?? r.RoomTypeId) &&
                r.StatusId == (status ?? r.StatusId) &&
                r.Price >= fromPrice && r.Price <= toPrice &&
                r.MaxPerson >= fromSizePerson && r.MaxPerson <= toSizePerson
            ).Include(r => r.Images).Include(b => b.Bookings).Skip(offSet - 1).Take(count).ToListAsync(); 
        }

        public async Task<int> Count(string nameRoom, int? roomTypeId, int? status, decimal fromPrice, decimal toPrice, int fromSizePerson, int toSizePerson)
        {
            return _context.Rooms.Where(r =>
                r.Name.Contains(nameRoom ?? r.Name) &&
                r.RoomTypeId == (roomTypeId ?? r.RoomTypeId) &&
                r.StatusId == (status ?? r.StatusId) &&
                r.Price >= fromPrice && r.Price <= toPrice &&
                r.MaxPerson >= fromSizePerson && r.MaxPerson <= toSizePerson
            ).Count(); 
        }

        public async Task<Room> getRoomById(int? id)
        {
            // Query get room by id 
            return _context.Rooms.Include(r => r.Images).Include(b => b.Bookings).FirstOrDefault(r => r.RoomId == id);
        }

        public async Task<List<Room>> RoomSimilar(int? status)
        {
            return await _context.Rooms.Where(r => r.StatusId == status).OrderBy(r => r.Price).Take(4).ToListAsync();
        }

        public async Task<List<Room>> Top4BestRoom()
        {
            return await _context.Rooms.Where(r => r.StatusId == 2).OrderBy(r => r.Price).Take(4).ToListAsync();
        }

        public async Task UpdateStatusRoom(int roomId, int status)
        {
            Room room = await _context.Rooms.Where(r => r.RoomId == roomId).FirstOrDefaultAsync(); 
            room.StatusId = status;
            _context.Update(room);
        }
    }
}
