using src.protocol.command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
#nullable enable
namespace src
{
    public class QBUSPacket
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        byte[] _prefix;
        byte _login;
        byte _lengthShifted;
        byte _length;
        COMMAND_SECTIONS _commandStart;
        byte[]? _command;
        COMMAND_SECTIONS _commandEnd;

        public bool valid => IsValid();

        public QBUSPacket(byte[] data) {
            _prefix = new byte[11];

            int copyLength = Math.Min(11, data.Length);
            Array.Copy(data, 0, _prefix, 0, copyLength);
            _login = data[9];
            _lengthShifted = data[10];
            _length = data.Length > 0 ? data[data.Length - 1] : (byte)0;
        }
        bool IsValid(){
            return _prefix[0] == 'Q' && _prefix[1] == 'B' && _prefix[2] == 'U' && _prefix[3] == 'S';
        }
        public byte Size(){
            return _length;
        }
        public bool SerializeBody(byte[] body){
            if (!IsValid())
                return false;
            _command = body.Skip(1).Take(body.Length - 2).ToArray();
            _commandStart = (COMMAND_SECTIONS)body[0];
            _commandEnd = (COMMAND_SECTIONS)body[body.Length - 1];
            return _commandStart == COMMAND_SECTIONS.START && _commandEnd == COMMAND_SECTIONS.END && _command.Length == _length - 2;
        }
        public byte[] ToArray()
        {
            using MemoryStream memoryStream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write(_prefix);
            writer.Write(_login);
            writer.Write(_lengthShifted);
            writer.Write(_length);
            writer.Write((byte)_commandStart);
            writer.Write(_command!);
            writer.Write((byte)_commandEnd);
            writer.Flush(); 

            return memoryStream.ToArray();
        }
    }
}
