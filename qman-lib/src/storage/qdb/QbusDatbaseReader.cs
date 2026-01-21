using MMKiwi.MdbReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.qdb
{
    public class QbusDatbaseReader
    {
        public static void Open(string filePath)
        {
            MdbConnection handle = MdbConnection.Open(filePath);
            var tables = handle.Tables;
        }
    }
}
