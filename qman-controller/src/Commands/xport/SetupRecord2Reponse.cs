using qman.controller.src.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace src.Commands.xport
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetupRecord2Response
    {
        public BroadcastCommandStruct command;

        // Bytes 4-92: The gap (89 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 93)]
        public byte[] _gap;

        // Bytes 93-98: Serial (6 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] _serialRaw;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] _padding;
        // Bytes 99-124: Device Name (26 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
        public byte[] _deviceName;

        // Bytes 125-129: Remainder (5 bytes)


        // --- REMAINING DATA ---
        // The code only processes up to index 125.
        // There may be 4 bytes remaining (126-129) to reach the 130-byte total.
        public void SetName(string name)
        {
            _deviceName = new byte[26];
            byte[] nameBytes = Encoding.ASCII.GetBytes(name.PadRight(26, '\0'));
            Array.Copy(nameBytes, _deviceName, Math.Min(nameBytes.Length, 26));
        }
        public void SetSerial(string serial)
        {
            // Ensure the serial is strictly 6 characters.
            // If the input is shorter, pad with '0'; if longer, truncate.
            string formattedSerial = serial.PadRight(6, '0');
            if (formattedSerial.Length > 6)
                formattedSerial = formattedSerial.Substring(0, 6);

            _serialRaw = Encoding.ASCII.GetBytes(formattedSerial).AsSpan<byte>(0,6).ToArray();
        }
        internal byte[] ToBuffer()
        {
            return ByteConvertable.GetBytes(this);
        }
    }
}
