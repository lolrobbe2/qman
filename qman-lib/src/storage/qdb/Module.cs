using mdbreader.src.MdbReader.attributes;
using MMKiwi.MdbReader;
using qmanlib.src.storage.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qmanlib.src.storage.qdb
{
    public enum QdbColumnIndex
    {
        Id = 0, Serienr = 1, Location = 2, Output0 = 3, Output1 = 4, Output2 = 5, Output3 = 6,
        Param0 = 7, Param1 = 8, Param2 = 9, Modified = 10, Output4 = 11, Output5 = 12,
        Output6 = 13, Output7 = 14, Param3 = 15, Series = 16, Param4 = 17, Param5 = 18,
        Param6 = 19, Param7 = 20, ModuleType = 21, CpuType = 22, FirmwareVersion = 23,
        Param8 = 24, Param9 = 25, Param10 = 26, Param11 = 27, Input0 = 28, DisplayText = 29,
        Param12 = 30, Output8 = 31, Output9 = 32, Output10 = 33, Output11 = 34, Output12 = 35,
        Output13 = 36, Output14 = 37, Output15 = 38, Param13 = 39, AutoVerify = 40,
        Param14 = 41, Param15 = 42, Type = 43, LastErrorNumber = 44, Param16 = 45,
        Param17 = 46, Param18 = 47, Param19 = 48, Param20 = 49, Param21 = 50,
        Param22 = 51, Param23 = 52, Param24 = 53, Param25 = 54, DoNotUse = 55,
        OutputExt0 = 56, OutputExt1 = 57, OutputExt2 = 58, OutputExt3 = 59,
        LocationId = 60, CtdId = 61, BusNumber = 62, EepromCrc = 63, FlashCrc = 64
    }
    [MdbRow]
    public class QdbModule : Module
    {
   
    }
}
