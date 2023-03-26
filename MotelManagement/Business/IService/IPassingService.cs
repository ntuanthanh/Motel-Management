namespace MotelManagement.Business.IService
{
    public interface IPassingService
    {
        public Task<bool> isBookingPassing(int? roomid, int userId);

        public Task RegisterPassing(int userId, int roomid);
    }
}
