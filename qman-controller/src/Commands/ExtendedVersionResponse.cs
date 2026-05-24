using qman.controller.src.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace src.Commands
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ExtendedVersionResponse
    {
        public BroadcastCommandStruct command; //4bytes
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        private byte[] _unkown;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        private byte[] _versionNr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        private byte[] _reserved;
        public byte WebPages;

        public ExtendedVersionResponse()
        {
        }
        public void setVersion(byte major, byte minor, byte build){
            _versionNr = new byte[5];
            _versionNr[0] = (byte)'6';
            _versionNr[1] = (byte)'.';
            _versionNr[2] = (byte)'0';
            _versionNr[3] = (byte)'.';
            _versionNr[4] = (byte)'9';
        }
        internal byte[] ToBuffer()
        {
            return ByteConvertable.GetBytes(this);
        }
    }
}
