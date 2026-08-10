using Spectre.Console.Cli;
using src.Commands.controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace src
{
    internal class ControllerConnection
    {
        private Controller _controller { get; set; }
        private TcpClient _tcpClient { get; set; }
        private Thread _commThread{ get; set; }
        private NetworkStream _stream => _tcpClient.GetStream();
        private Action<EndPoint> _onClose { get; set; }
        private Action<AbstractCommand, ControllerConnection> _onControlPort { get; set; }
        private Action<AbstractCommand, ControllerConnection> _onCommandPort { get; set; }

        private Dictionary<QBUS_COMMAND_TYPE, Action<AbstractCommand>> _handlers { get; set; }
        private AbstractCommand _command { get; set; }
        public ControllerConnection(TcpClient tcpClient, Controller controller){
            _tcpClient = tcpClient;
            _controller = controller;
            _commThread = new Thread(CommCommand);
            _commThread.Start();
        }
        public EndPoint GetRemoteEndPoint(){
            return _tcpClient.Client.RemoteEndPoint!;
        }
        public void CommCommand()
        {
            QBUSPacket? packet = null;
            while (_tcpClient.Connected)
            {
                if (_stream.DataAvailable)
                {
                    HandlePacket(ref packet);
                }
            }
            _onClose.Invoke(_tcpClient.Client.RemoteEndPoint!);
        }
        #region PACKET_RECEIVING
        private void HandlePacket(ref QBUSPacket packet)
        {
            if (packet == null)
            {
                packet = GetHeader();
            }
            else
            {
                //read the packet body to check validity
                byte[] packetBody = new byte[packet.Size()];
                int bytesRead = _stream.ReadAtLeast(packetBody.AsSpan<byte>(), packet.Size());
                if (packet.SerializeBody(packetBody))
                {
                    _command = new AbstractCommand(packet.GetCommand());
                    if(packet.IsXPORTCommand()) {
                        _onControlPort.Invoke(_command, this);
                    } else {
                        _onCommandPort.Invoke(_command, this);
                    }
                }
                //DROP the invalid packet
            }

        }

        private QBUSPacket GetHeader()
        {
            QBUSPacket packet;
            //read the packet header so we can check if the packet is valid (CommandSections.PREFIX_SIZE + 3)
            byte[] packetHeader = new byte[12];
            int bytesRead = _stream.ReadAtLeast(packetHeader.AsSpan<byte>(), CommandSections.PREFIX_SIZE + 3);
            packet = new QBUSPacket(packetHeader);
            return packet;
        }
        #endregion
        public void AddControlPortHandler(Action<AbstractCommand, ControllerConnection> handler)
        {
            _onControlPort += handler;
        }
        
    }
}
