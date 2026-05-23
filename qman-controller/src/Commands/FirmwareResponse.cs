using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace qman.controller.src.Commands
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FirmawareResponseStruct
    {
        public BroadcastCommandStruct command; //4bytes

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _ip;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _gateway;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _subnet;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] macAddress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] _reserved;


        public void SetIP(IPAddress ip) => _ip = ip.GetAddressBytes();
        public void SetGateway(IPAddress gateway) => _gateway = gateway.GetAddressBytes();

        public void SetSubnet(IPAddress subnet) => _subnet = subnet.GetAddressBytes();
        public void SetVersion(int major, int minor, int revision, int build)
        {
            _version = new byte[] { (byte)major, (byte)minor, (byte)revision, (byte)build };
        }

        internal byte[] ToBuffer()
        {
            return ByteConvertable.GetBytes(this);
        }
    }
}
