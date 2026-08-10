using qmanlib.protocol.packet;
using Spectre.Console.Cli;
using src.protocol.command;
using System;
using System.Collections.Concurrent;
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
        private Action<IControllerCommand, ControllerConnection> _onControlPort { get; set; }
        private Action<IControllerCommand, ControllerConnection> _onCommandPort { get; set; }
        
        private Dictionary<QBUS_COMMAND_TYPE, Action<Packet<IControllerCommand>>> _handlers { get; set; }

        private ConcurrentQueue<IControllerCommand> _controlPortQueue { get; init; }
        private ConcurrentQueue<IControllerCommand> _commandQueue { get; init; }

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
            Packet<IControllerCommand>? packet = null;
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
        private void HandlePacket(ref Packet<IControllerCommand> packet)
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
                    if(packet.PacketData.controlPort) {
                        if(_onControlPort is not null)
                            _onControlPort.Invoke(packet.Command, this);
                    } else if (_onCommandPort is not null)
                    {
                        _onCommandPort.Invoke(packet.Command, this);
                    }
                }
                //DROP the invalid packet
            }

        }

        private Packet<IControllerCommand> GetHeader()
        {
            Packet<IControllerCommand> packet;
            //read the packet header so we can check if the packet is valid (CommandSections.PREFIX_SIZE + 3)
            byte[] packetHeader = new byte[12];
            int bytesRead = _stream.ReadAtLeast(packetHeader.AsSpan<byte>(), CommandSections.PREFIX_SIZE + 3);
            packet = new Packet<IControllerCommand>(packetHeader);
            return packet;
        }
        #endregion
        public void AddControlPortHandler(Action<IControllerCommand, ControllerConnection> handler)
        {
            if(_onControlPort is null){
                _onControlPort = handler;
            }
            _onControlPort += handler;
        }
        
    }
}
