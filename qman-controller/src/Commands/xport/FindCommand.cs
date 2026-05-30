using qman.controller.src.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace src.Commands.xport
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct FindResponse
    {
        // Offset 0-3: Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private byte[] _header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private byte[] _serial;
        // Offset 4-9: MAC Address (6 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        private byte[] _mac;

        // Offset 10-13: IP Address (4 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _ip;

        // Offset 14-17: Gateway (4 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _gateway;

        // Offset 18: Subnet Bits
        public byte SubnetMaskBits;

        // Offset 19: Status
        public byte SettingsStatus;

        // Offset 20-23: Version (4 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _version;

        // Offset 24-25: Hardware ID
        public byte HardwareID1;
        public byte HardwareID2;

        public byte ReservedPadding;
        // Offset 26-28: (3 bytes) Padding/Device Type snippet
        // Note: Your code looks for Name at 29, so we need 3 bytes here.


        // Offset 29-52: Device Name (24 bytes)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        private byte[] _deviceName;

        // --- Fixed Setters for your XPORT simulation ---
        public void SetName(string name)
        {
            _deviceName = new byte[24];
            byte[] nameBytes = Encoding.ASCII.GetBytes(name.PadRight(24, '\0'));
            Array.Copy(nameBytes, _deviceName, Math.Min(nameBytes.Length, 24));
        }

        public void SetMac(byte[] mac) => _mac = mac;
        public void SetIP(IPAddress ip) => _ip = ip.GetAddressBytes();
        public byte[] ToBuffer()
        {
            return ByteConvertable.GetBytes(this);
        }
    }
}
