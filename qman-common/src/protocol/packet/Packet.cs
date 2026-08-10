using src.protocol.command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static qmanlib.protocol.packet.IPacket;

namespace qmanlib.protocol.packet
{

    public class Packet<COMMAND> where COMMAND : IControllerCommand
    {
        byte[] _prefix;
        PacketData _packetData;
        COMMAND_SECTIONS _commandStart;
        COMMAND _command;
        COMMAND_SECTIONS _commandEnd;
        #region PUBLIC_FIELDS
        /// <summary>
        /// The packet information
        /// </summary>
        public PacketData PacketData => _packetData;
        /// <summary>
        /// the instatiated Command 
        /// </summary>
        public COMMAND Command => _command;
        #endregion
        public Packet(COMMAND command)
        {
            _prefix = new byte[] { 81, 66, 85, 83, 0, 4, 0, 1, 1 }; // QBUS..... 
            _command = command;
        }
        public Packet(byte[] header)
        {
            _prefix = new byte[9];

            int copyLength = Math.Min(9, header.Length);
            Array.Copy(header, 0, _prefix, 0, copyLength);
            _packetData.Deserialize(header.Skip(9).ToArray());
        }
        public bool IsValidHeader(){
            return _prefix[0] == 'Q' && _prefix[1] == 'B' && _prefix[2] == 'U' && _prefix[3] == 'S';
        }
        public byte Size()
        {
            return _packetData.length;
        }
        /// <summary>
        /// Serializes the packetBody once the packet header has been verified.
        /// </summary>
        /// <param name="body"></param>
        /// <returns> True when the packet header and command are valid</returns>
        public bool SerializeBody(byte[] body)
        {
            if (!IsValidHeader())
                return false;
            byte[] commandBytes = body.Skip(1).Take(body.Length - 2).ToArray();
            _command = (COMMAND)(IControllerCommand)CommandBase.Create(commandBytes, _packetData.controlPort);
            _commandStart = (COMMAND_SECTIONS)body[0];
            _commandEnd = (COMMAND_SECTIONS)body[body.Length - 1];
            return _commandStart == COMMAND_SECTIONS.START && _commandEnd == COMMAND_SECTIONS.END && commandBytes.Length == _packetData.length - 2;
        }
        public byte[] Serialize()
        {
            return _prefix.Concat(_packetData.Serialize()).Concat(_command.Serialize()).ToArray();
        }
    }
 
}
