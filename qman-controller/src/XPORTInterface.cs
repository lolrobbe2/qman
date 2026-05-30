using qman.controller.src;
using Spectre.Console.Cli;
using src.Commands;
using src.Commands.xport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace src
{
    internal class XPORTInterface
    {

        private static List<CommandRegistration> _findRoutes = new List<CommandRegistration>();
        private XPORT _port { get; set; }
        private IPAddress _localAddr { get; set; }
        private Thread _connectionListnerThread;

        private UdpClient _udpFindClient { get; set; }
        private UdpClient _udpCommandClient { get; set; }
        private UdpClient _udpResponseClient { get; set; }

        private UInt16 _findPort { get; set; }
        private UInt16 _commandPort { get; set; }
        private UInt16 _tcpPort { get; set; }

        private CancellationTokenSource _cts;

        private TcpListener _tcpListener;
        private Action<TcpClient> _onIncommingConnection { get; set; }
        public XPORTInterface(XPORT port, IPAddress interfaceAddress, UInt16 findPort = 30700, UInt16 commandPort = 30718, UInt16 tcpPort = 8445)
        {
            _localAddr = interfaceAddress;
            _findPort = findPort;
            _commandPort = commandPort;
            _tcpPort = tcpPort;
            _port = port;
            IPEndPoint findEndPoint = new IPEndPoint(_localAddr, _findPort);
            IPEndPoint commandEndPoint = new IPEndPoint(_localAddr, _commandPort);
            IPEndPoint responseEndPoint = new IPEndPoint(_localAddr, 0);

            _udpFindClient = new UdpClient();
            _udpFindClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpFindClient.Client.Bind(findEndPoint);

            _udpCommandClient = new UdpClient();
            _udpCommandClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpCommandClient.Client.Bind(commandEndPoint);


            _udpResponseClient = new UdpClient();
            _udpResponseClient.Client.Bind(responseEndPoint);

            _tcpListener = new TcpListener(_localAddr, _tcpPort);
        }

        public void StartListening(){
            _cts = new CancellationTokenSource();
            _udpFindClient.BeginReceive(FindDataReceived, _udpFindClient);
            _udpCommandClient.BeginReceive(CommandDataReceived, _udpCommandClient);
            _connectionListnerThread = new Thread(ListenerAcceptTask);
            _connectionListnerThread.Start();
            _tcpListener.Start();
           
        }
        private void FindDataReceived(IAsyncResult ar)
        {
            UdpClient client = (UdpClient)ar.AsyncState!;
            IPEndPoint RemoteEndPoint = null;
            byte[] receivedBytes = client!.EndReceive(ar, ref RemoteEndPoint);

            XPORT_BROADCAST_COMMANDS command = (XPORT_BROADCAST_COMMANDS)receivedBytes[0];
            var route = _findRoutes.FirstOrDefault(r => r.Command == command);
            if (route != null && route.Handler != null)
            {
                route.Handler(_port, client, receivedBytes, RemoteEndPoint!, _udpFindClient.Client.LocalEndPoint!, command);
            }
            client.BeginReceive(FindDataReceived, ar.AsyncState);
        }
        private void ListenerAcceptTask()
        {
            while(!_cts.IsCancellationRequested){
                _onIncommingConnection?.Invoke(_tcpListener.AcceptTcpClient());
            }
        }
        private void CommandDataReceived(IAsyncResult ar)
        {
            UdpClient client = (UdpClient)ar.AsyncState!;
            IPEndPoint RemoteEndPoint = null;
            byte[] receivedBytes = client!.EndReceive(ar, ref RemoteEndPoint);

            XPORT_BROADCAST_COMMANDS command = (XPORT_BROADCAST_COMMANDS)receivedBytes[3];
            Console.WriteLine($"Command received: {command}");
            switch(command){
                case XPORT_BROADCAST_COMMANDS.FIRMWARE_QUERRY:
                    BroadCastHandlers.FirmawareQueryHandler(_port,_udpResponseClient,receivedBytes,RemoteEndPoint!,_udpCommandClient.Client.LocalEndPoint!,command); 
                    break;
                case XPORT_BROADCAST_COMMANDS.SETUP_RECORD_2_QUERRY:
                    BroadCastHandlers.SetupRecord2Handler(_port, _udpResponseClient, receivedBytes, RemoteEndPoint!, _udpCommandClient.Client.LocalEndPoint!, command);
                    break;
                case XPORT_BROADCAST_COMMANDS.EXTENDED_VERSION_QUERRY:
                    BroadCastHandlers.ExtendedVersionQueryHandler(_port, _udpResponseClient, receivedBytes, RemoteEndPoint!, _udpCommandClient.Client.LocalEndPoint!, command);
                    break;
            }
            client.BeginReceive(CommandDataReceived, ar.AsyncState);
        }
        public static void RegisterFindHandler(XPORT_BROADCAST_COMMANDS type, Action<XPORT, UdpClient, byte[], IPEndPoint, EndPoint, XPORT_BROADCAST_COMMANDS> action)
        {
            _findRoutes.Add(new CommandRegistration(type, action));
        }
        
        public void RegisterIncommingConnectionHandler(Action<TcpClient> action){
            _onIncommingConnection += action;
        }
    }
}
