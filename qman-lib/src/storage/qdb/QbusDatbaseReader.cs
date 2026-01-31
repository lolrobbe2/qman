using MMKiwi.MdbReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.qdb
{
    public class QbusDatbaseReader
    {
        public static QbusDatabase Open(string filePath)
        {
            return new QbusDatabase(MdbConnection.Open(filePath));
        }
    }
}
