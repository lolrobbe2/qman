using mdbreader.src.MdbReader.attributes;
using MMKiwi.MdbReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.qdb
{
    [MdbRow]
    public class QdbController
    {
        [MdbParam("adres")]        
        
        public byte addres { get; set; }
        public string? mode { get; set; }

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
