using src.Commands.controller;
using System;
using System.Collections.Generic;
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
        private Thread _readerThread{ get; set; }
        private NetworkStream _stream => _tcpClient.GetStream();
        private Action<EndPoint> _onClose { get; set; }
        private AbstractCommand _command { get; set; }
        public ControllerConnection(TcpClient tcpClient, Controller controller){
            _tcpClient = tcpClient;
            _controller = controller;
            _readerThread = new Thread(ReadCommand);
            _readerThread.Start();
        }
        public EndPoint GetRemoteEndPoint(){
            return _tcpClient.Client.RemoteEndPoint!;
        }
        public void ReadCommand(){
            QBUSPacket? packet = null;
            while (_tcpClient.Connected)
            {
                if(_stream.DataAvailable)
                {
                    if(packet == null){
                        byte[] packetHeader = new byte[12];
                        int bytesRead = _stream.ReadAtLeast(packetHeader.AsSpan<byte>(), CommandSections.PREFIX_SIZE + 3);
                        packet = new QBUSPacket(packetHeader);
                    } else{
                        byte[] packetBody = new byte[packet.Size()];
                        int bytesRead = _stream.ReadAtLeast(packetBody.AsSpan<byte>(), packet.Size());
                        if(packet.SerializeBody(packetBody)){
                            Console.WriteLine("VALID packet received");
                        } else {
                            Console.WriteLine("INVALID packet received");
                        }
                    }

                }
            }
            _onClose.Invoke(_tcpClient.Client.RemoteEndPoint!);
        }
    }
}
