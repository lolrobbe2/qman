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
    public class QdbModule : Module
    {
        private static readonly int[] OutputColumnMap = Enumerable.Range(0, 16)
            .Select(i => (int)Enum.Parse<QdbColumnIndex>($"Output{i}"))
            .ToArray();
        private static readonly int[] ParamColumnMap = Enumerable.Range(0, 25)
          .Select(i => (int)Enum.Parse<QdbColumnIndex>($"Param{i}"))
          .ToArray();


        public QdbModule(MdbDataRow item)
        {
            Id = item.GetInt16((int)QdbColumnIndex.Id);
            SerialNumber = item.GetStringNotNull((int)QdbColumnIndex.Serienr);
            Location = item.GetStringNotNull((int)QdbColumnIndex.Location);
            IsModified = item.GetStringNotNull((int)QdbColumnIndex.Modified);
            Series = item.GetStringNotNull((int)QdbColumnIndex.Series);
            ModuleType = item.GetStringNotNull((int)QdbColumnIndex.ModuleType);
            CpuType = (item.IsNull((int)QdbColumnIndex.CpuType) ? (byte)0: item.GetByte((int)QdbColumnIndex.CpuType));
            FirmwareVersion = item.GetStringNotNull((int)QdbColumnIndex.FirmwareVersion);
            Input0 = item.GetInt16((int)QdbColumnIndex.Input0);
            DisplayText = item.GetStringNotNull((int)QdbColumnIndex.DisplayText);
            AutoVerify = item.GetBoolean((int)QdbColumnIndex.AutoVerify);
            Type = item.GetString((int)QdbColumnIndex.Type) ?? "";
            LastErrorNumber = item.GetByte((int)QdbColumnIndex.LastErrorNumber);
            IsInUse = item.GetBoolean((int)QdbColumnIndex.DoNotUse);

            LocationId = item.GetInt16((int)QdbColumnIndex.LocationId);
            CtdId = item.IsNull((int)QdbColumnIndex.CtdId) ? (Int16)0 : item.GetInt16((int)QdbColumnIndex.CtdId);
            BusNumber = item.IsNull((int)QdbColumnIndex.BusNumber) ? (Int16)0 : item.GetInt16((int)QdbColumnIndex.BusNumber);
            EepromCrc = item.GetString((int)QdbColumnIndex.EepromCrc);
            FlashCrc = item.GetString((int)QdbColumnIndex.FlashCrc);

            // Populate Outputs 0-15
            // Note: This assumes your Enum maintains the sequential order of Output0 through Output15
            for (int i = 0; i < 16; i++)
            {
                int actualColIndex = OutputColumnMap[i];

                if (!item.IsNull(actualColIndex))
                {
                    Outputs[i] = item.GetInt16(actualColIndex);
                }
            }

            // Populate Parameters 0-25
            // Note: If Params are non-sequential in the Enum, use: Enum.Parse<QdbColumnIndex>($"Param{i}")
            for (int i = 0; i < 25; i++)
            {
                int actualColIndex = ParamColumnMap[i];

                if (!item.IsNull(actualColIndex))
                {
                    Parameters[i] = item.GetByte(actualColIndex);
                }
            }

            // Populate Extended Outputs 0-3
            ExtendedOutputs[0] = item.GetSingle((int)QdbColumnIndex.OutputExt0);
            ExtendedOutputs[1] = item.GetSingle((int)QdbColumnIndex.OutputExt1);
            ExtendedOutputs[2] = item.GetSingle((int)QdbColumnIndex.OutputExt2);
            ExtendedOutputs[3] = item.GetSingle((int)QdbColumnIndex.OutputExt3);

        }
    }
}
