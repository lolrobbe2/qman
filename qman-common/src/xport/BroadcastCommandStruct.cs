using src.Commands.xport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace src.xport
{
    [DebuggerDisplay("Cmd: {command} | Reboot: {reboot}")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BroadcastCommandStruct
    {
        private byte header;
        public byte reboot;
        private byte seperator;
        public XPORT_BROADCAST_COMMANDS command;
        public static BroadcastCommandStruct FromBytes(byte[] received)
        {
            if (received.Length != 4) return new BroadcastCommandStruct();
            return new BroadcastCommandStruct() { header = received[0], reboot = received[1], seperator = received[2], command = (XPORT_BROADCAST_COMMANDS)received[3] };
        }
    }
}
