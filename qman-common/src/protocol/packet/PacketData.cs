using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static qmanlib.protocol.packet.IPacket;

namespace qmanlib.protocol.packet
{
    public struct PacketData : IPacketData
    {
        byte m_controlPort;
        byte m_length;
        byte m_lengthShifted;

        public bool controlPort
        {
            get => m_controlPort == 250;
            set => m_controlPort = value ? (byte)250 : (byte)255;
        }

        public byte length
        {
            get => m_length;
            set
            {
                m_length = (byte)(value - 1);
                m_lengthShifted = (byte)((value - 1) >> 8);
            }
        }

        public byte[] Serialize() => new byte[] { m_controlPort, m_lengthShifted, m_length };

        public void Deserialize(byte[] data)
        {
            if (data.Length != 3) throw new ArgumentException("[QBUS] packetData needs to be 3 bytes");
            m_controlPort = data[0];
            m_lengthShifted = data[1];
            m_length = data[2];
        }

        public PacketData(byte[] data) : this()
        {
            Deserialize(data);
        }
    }
}
