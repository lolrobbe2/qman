using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.models
{
    public class Module
    {
        public short Id { get; set; }
        public string SerialNumber { get; set; }
        public string Location { get; set; }
        public string Series { get; set; }
        public string ModuleType { get; set; }
        public byte CpuType { get; set; }
        public string FirmwareVersion { get; set; }
        public short Input0 { get; set; }
        public string DisplayText { get; set; }
        public bool AutoVerify { get; set; }
        public string Type { get; set; }
        public byte LastErrorNumber { get; set; }
        public bool IsInUse { get; set; } // Negated 'nietgebruiken' for logic clarity
        public string IsModified { get; set; } // Matches 'gewijzigd' CHAR
        public short[] Outputs { get; set; } = new short[16];
        public byte[] Parameters { get; set; } = new byte[26];
        public float[] ExtendedOutputs { get; set; } = new float[4];
        public short LocationId { get; set; }
        public short CtdId { get; set; }
        public short BusNumber { get; set; }
        public string? EepromCrc { get; set; }
        public string? FlashCrc { get; set; }
    }
}
