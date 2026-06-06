using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace src.Commands.xport
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FindResponse
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

        #region NAME
        public void SetName(string name)
        {
            _deviceName = new byte[24];
            byte[] nameBytes = Encoding.ASCII.GetBytes(name.PadRight(24, '\0'));
            Array.Copy(nameBytes, _deviceName, Math.Min(nameBytes.Length, 24));
        }
        public string GetName()
        {
            if (_deviceName == null)
                return string.Empty;

            // Convert bytes → ASCII string, then trim nulls and whitespace
            return Encoding.ASCII.GetString(_deviceName).TrimEnd('\0', ' ');
        }

        public string Name { get => GetName(); set => SetName(value); }
        #endregion

        #region MAC
        public void SetMac(byte[] mac)
        {
            if (mac == null || mac.Length != 6)
                throw new ArgumentException("MAC address must be exactly 6 bytes.");

            _mac = new byte[6];
            Array.Copy(mac, _mac, 6);
        }

        public byte[] GetMac(){
            return _mac;
        }

        #endregion

        #region IP
        public void SetIP(IPAddress ip) => _ip = ip.GetAddressBytes();
        public IPAddress GetIp() => new IPAddress(_ip);

        public IPAddress Address { get => GetIp(); set => SetIP(value); }

        public string GetAddressString(){
            return $"{_ip[0].ToString()}.{_ip[1].ToString()}.{_ip[2].ToString()}.{_ip[3].ToString()}";
        }
        #endregion
        public byte[] ToBuffer()
        {
            return ByteConvertable.GetBytes(this);
        }


    }
}
