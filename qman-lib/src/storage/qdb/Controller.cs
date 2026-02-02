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
        public byte lcdlink { get; set; }
        public IList<int> output { get; set; } = new List<int>(8);
        public string? gewijzigd { get; set; }
        public int ControllerId { get; set; }
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
