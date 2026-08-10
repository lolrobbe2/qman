using System;
using System.Collections.Generic;
using System.Text;

namespace src.protocol.command.service
{
    public enum SERVICE_COMMANDS : byte
    {
        VERIFY_PASSWORD = 0,
        READ_PASSWORD = 1,
        STRING_DATA = 2,
        INITIALISATION_VECTOR = 3,
        ENCRYPTION_KEY = 4,
        CLOUD_LOGIN = 10,
        SHIFT_KEY = 16,
        POSITION_KEY = 17,
        WRITE_PASSWORD = 129,
        ERROR = 255,
    }
}
