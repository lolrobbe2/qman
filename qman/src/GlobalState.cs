using qmanlib.src.storage.qdb;
using System;
using System.Collections.Generic;
using System.Text;

namespace qman.src
{
    /// <summary>
    /// class containing all the globaly accesable objects such as the current loaded QbusDatabase
    /// </summary>
    public class GlobalState
    {
        public static QbusDatabase? currentDb { get; set; }
    }
}
