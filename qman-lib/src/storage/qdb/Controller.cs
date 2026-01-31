using MMKiwi.MdbReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.qdb
{
   
    public class QdbController
    {
        
        byte addres { get; set; }
        string? mode { get; set; }
        public QdbController(MdbDataRow row)
        {

            addres = row.GetByte((int)ParamIndex.ADRES);
            mode = row.GetString((int)ParamIndex.MODE);
        }

        private enum ParamIndex : int
        {
            ADRES,
            MODE,
            LCDLINK,
            OUTPUT0,
            OUTPUT1,
            OUTPUT2,
            OUTPUT3,
            GEWIJZIGD,
            CONTROLLER_ID,
            OUTPUT4,
            OUTPUT5,
            OUTPUT6,
            OUTPUT7,
            NUM_PARAMS
        };
    }
}
