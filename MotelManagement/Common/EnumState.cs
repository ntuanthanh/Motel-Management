namespace MotelManagement.Common
{
    enum ROOM_STATE
    {
        RENTED = 1,
        PROCESSING = 2,
        PASSING = 3
    }

    enum PAYMENT_STATE
    {
        PAID,
        UNPAID,
        DEBT 
    }
    enum IMAGE_STATE
    {
        ROOM,
        BILL,
        REPORT
    }

    enum REGISTER_ROOM_STATE
    {
        REGISTER = 1,
        UN_REGISTER = 0,
        SUCCESS = 2
    }

    enum PageManagement
    {
        PageSize = 9
    }
}

