using qmanlib.protocol.command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static qmanlib.protocol.packet.IPacket;

namespace qmanlib.protocol.packet
{

    internal class Packet<COMMAND> where COMMAND : ICommand
    {
        byte[] _prefix;

        PacketData _packetData;
        COMMAND _command;

        public Packet(COMMAND command)
        {
            _prefix = new byte[] { 81, 66, 85, 83, 0, 4, 0, 1, 1 }; // QBUS.....
            _command = command;
        }

        public byte[] Serialize()
        {
            return _prefix.Concat(_packetData.Serialize()).Concat(_command.Serialize()).ToArray();
        }
    }
 
}
