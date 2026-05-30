using qman.controller.src;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
 #nullable enable
namespace src
{
    internal class Controller
    {
        XPORT? _xport { get; set; } = null;
        ConcurrentDictionary<EndPoint, ControllerConnection>? _connections { get; set; } = null;

        public void InitializeXport(){
            _xport = new XPORT();
            _connections = new ConcurrentDictionary<EndPoint, ControllerConnection>();

            _xport.start();
            _xport.RegisterIncommingConnectionHandler(IncommingConnection);
            
        }
        void IncommingConnection(TcpClient client) {
            ControllerConnection connection = new(client, this);
            _connections!.TryAdd(connection.GetRemoteEndPoint(), connection);
        }
    }
}
