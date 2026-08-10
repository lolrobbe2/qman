using mdbreader.src.MdbReader.attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.models
{
    public class Module
    {
        [MdbParam("id")]
        public short Id { get; set; }
        [MdbParam("serienr")]
        public string SerialNumber { get; set; }
        [MdbParam("plaats")]
        // the name of the module
        public string Location { get; set; }
        [MdbParam("reeks")]
        public string Series { get; set; }
        [MdbParam("modtype")]
        public string ModuleType { get; set; }
        [MdbParam("CPUtype")]
        public byte CpuType { get; set; }
        [MdbParam("FWVersie")]
        public string FirmwareVersion { get; set; }
        [MdbParam("input")]
        public short Input0 { get; set; }
        [MdbParam("Disptekst")]
        public string DisplayText { get; set; }
        [MdbParam("Autoverify")]
        public bool AutoVerify { get; set; }
        [MdbParam("type")]
        public string Type { get; set; }
        [MdbParam("LastErrNr")]
        public byte LastErrorNumber { get; set; }
        [MdbParam("nietgebruiken")]
        public bool IsInUse { get; set; } // Negated 'nietgebruiken' for logic clarity
        [MdbParam("gewijzigd")]
        public string IsModified { get; set; } // Matches 'gewijzigd' CHAR
        [MdbParam("output")]
        public short[] Outputs { get; set; } = new short[16];
        [MdbParam("par")]
        public byte[] Parameters { get; set; } = new byte[26];
        [MdbParam("outputExt")]
        public float[] ExtendedOutputs { get; set; } = new float[4];
        [MdbParam("PlaatsId")]
        public short LocationId { get; set; }
        [MdbParam("CTDid")]
        public short CtdId { get; set; }
        [MdbParam("BusNr")]
        public short BusNumber { get; set; }
        [MdbParam("EepromCRC")]
        public string? EepromCrc { get; set; }
        [MdbParam("FlashCRC")]
        public string? FlashCrc { get; set; }
    }
    public enum ModuleType
    {
        SWN04,
        DIM04_500U,
        REL04SA
    }

    public static class ModuleTypeExtensions
    {
        public static string ToModuleName(this ModuleType moduleType)
        {
            switch (moduleType)
            {
                case ModuleType.SWN04:
                    return "SWN04";

                case ModuleType.DIM04_500U:
                    return "DIM04/500U";

                case ModuleType.REL04SA:
                    return "REL04SA";

                default:
                    throw new ArgumentOutOfRangeException(nameof(moduleType));
            }
        }
        public static ModuleType FromModuleName(string moduleName)
        {
            switch (moduleName)
            {
                case "SWN04":
                    return ModuleType.SWN04;

                case "DIM04/500U":
                    return ModuleType.DIM04_500U;

                case "REL04SA":
                    return ModuleType.REL04SA;

                default:
                    throw new ArgumentException(
                        $"Unknown module name '{moduleName}'.",
                        nameof(moduleName));
            }
        }
    }
}
