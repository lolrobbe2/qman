using src.Commands.controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static Microsoft.VisualStudio.Threading.AsyncReaderWriterLock;

namespace src
{
    internal class QBUSPacket
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        byte[] _prefix;
        byte _checksum;
        byte _unkown;
        byte _packetBodySize;
        byte _commandStart;
        byte[]? _command;
        byte _commandEnd;

        public bool valid => IsValid();

        public QBUSPacket(byte[] data) {
            _prefix = new byte[11];

            int copyLength = Math.Min(11, data.Length);
            Array.Copy(data, 0, _prefix, 0, copyLength);
            _checksum = data[9];
            _unkown = data[10];
            _packetBodySize = data.Length > 0 ? data[data.Length - 1] : (byte)0;
        }
        bool IsValid(){
            return _prefix[0] == 'Q' && _prefix[1] == 'B' && _prefix[2] == 'U' && _prefix[3] == 'S';
        }
        public byte Size(){
            return _packetBodySize;
        }
        public bool SerializeBody(byte[] body){
            if (!IsValid())
                return false;
            _command = body.Skip(1).Take(body.Length - 2).ToArray();
            _commandStart = body[0];
            _commandEnd = body[body.Length - 1];
            return _commandStart == CommandSections.START && _commandEnd == CommandSections.END && _command.Length == _packetBodySize - 2;
        }
        public byte[] ToArray()
        {
            using MemoryStream memoryStream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write(_prefix);
            writer.Write(_checksum);
            writer.Write(_unkown);
            writer.Write(_packetBodySize);
            writer.Write(_commandStart);
            writer.Write(_command!);
            writer.Write(_commandEnd);
            writer.Flush();

            return memoryStream.ToArray();
        }
    }
}
