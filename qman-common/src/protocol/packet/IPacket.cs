using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static qmanlib.protocol.packet.IPacket;

namespace qmanlib.protocol.packet
{
    public class IPacket
    {
        public interface IPacketData
        {
            public byte[] Serialize();
        }
    }
}
