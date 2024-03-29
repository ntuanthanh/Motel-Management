﻿using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Business.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Booking>> BookingsAvailable(int? roomId)
        {
            List<Booking> bookings = await _unitOfWork.bookingRepository.BookingListByRoomAvailable(roomId);
            return bookings; 
        }
        public async Task<List<Booking>> BookingsAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking,string? fromBooking, string? toBooking)
        {
            List<Booking> bookings = await _unitOfWork.bookingRepository.BookingListByRoomAvailableSearching(roomId,nameBooking,emailBooking,phoneBooking,fromBooking,toBooking);
            return bookings;
        }

        public async Task<bool> isBooking(int? roomid, int userId)
        {
            return await _unitOfWork.bookingRepository.isBooking(roomid, userId); 
        }
        public async Task<bool> isBookingPassing(int? roomid, int userId)
        {
            return await _unitOfWork.bookingRepository.isBookingPassing(roomid, userId);
        }

        public async Task<bool> isRoomRented(int? roomId)
        {
            return await _unitOfWork.roomRepository.isRoomRented(roomId); 
        }

        public async Task<List<Booking>> listBookings(int userId)
        {
            return await _unitOfWork.bookingRepository.listBookings(userId);
        }

        public async Task Register(int userId, int roomid)
        {
            Booking booking = new Booking();
            booking.RoomId = roomid;
            booking.UserId = userId;
            booking.Status = (int)REGISTER_ROOM_STATE.REGISTER;
            booking.BookingTime = DateTime.Now;
            booking.MeetingDate = null; 
            await _unitOfWork.bookingRepository.AddAsync(booking);
            await _unitOfWork.SaveAsync();
        }
        public async Task RegisterPassing(int userId, int roomid)
        {
            // roomId
            // userId là ReuqestUser : người gửi yêu cầu muốn đăng kí đến member
            int member; // không cần, đang set trong trường là null 

            Passing passing = new Passing();
            passing.RoomId = roomid; 
            passing.UserRequestId = userId;
            passing.Status = (int)REGISTER_ROOM_STATE.REGISTER;
            passing.BookingTime = DateTime.Now; 
            passing.MeetingDate = null;
            passing.Member = null;
            //await _unitOfWork.passing.AddAsync(passing); 

            Booking booking = new Booking();
            booking.RoomId = roomid;
            booking.UserId = userId;
            booking.Status = (int)REGISTER_ROOM_STATE.REGISTER;
            booking.BookingTime = DateTime.Now;
            booking.MeetingDate = null;
            await _unitOfWork.bookingRepository.AddAsync(booking);
            await _unitOfWork.SaveAsync();
        }

        public async Task SetUserBeMember(int userId, int roomid, decimal price, int bookingId)
        {
            // Add to  Contract 
            await _unitOfWork.contractRepository.addUsertoRoom(roomid, userId, price);
            // Update status Booking 
            await _unitOfWork.bookingRepository.updateSuccessRejectUsersExceptMember(userId, roomid, bookingId);
            // Update status Room
            await _unitOfWork.roomRepository.UpdateStatusRoom(roomid, (int)ROOM_STATE.RENTED);
            // Save transaction 
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateMeetingDateAllUser(DateTime? meetingDate, int? roomId)
        {
            //List<Booking> bookings = await _unitOfWork.bookingRepository.BookingListByRoomAvailable(roomId);
            //// Nếu phòng có booking avaiable update all date to user 
            //if(bookings != null && bookings.Count > 0)
            //{
            //    foreach(Booking booking in bookings)
            //    {
            //        booking.MeetingDate = meetingDate;
            //        _unitOfWork.bookingRepository.Update(booking); 
            //    }
            //    _unitOfWork.save(); 
            //}
            await _unitOfWork.bookingRepository.updateMeetingDate(meetingDate, roomId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateMeetingForUser(DateTime? dateTime, int? bookingId, int? roomId)
        {
            await _unitOfWork.bookingRepository.updateMeetingDateForUser(dateTime, bookingId);
            await _unitOfWork.SaveAsync();
        }

        public async Task updateUnRegister(int userId, int roomid)
        {
            await _unitOfWork.bookingRepository.updateUnRegister(userId, roomid);
            await _unitOfWork.SaveAsync();
        }
    }
}
