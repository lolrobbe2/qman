using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.protocol.command.address
{
    public enum SUBADDRESS : byte
    {
        FIRST = 0,
        SECOND = 1,
        THIRD = 2,
        FOURTH = 3,
        FIFTH = 4,
        SIXT = 5,
        SEVENT = 6,
        EIGHT = 7,
        ALL = 255, // 0x0FF
    };
}
