﻿namespace MotelManagement.Common
{
    enum ROOM_STATE
    {
        RENTED = 1,
        PROCESSING = 2,
        PASSING = 3
    }

    enum PAYMENT_STATE
    {
        PAID=1,
        UNPAID=-1,
        DEBT=0,
        NOT_CONFIRM = 2
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

    enum DEFAULT_VALUE
    {
        PAYMENT_STATE = -2,
        USER_SELECT = -1
    }
}

