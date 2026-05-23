using qman.controller.src.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace qman.controller.src
{
    internal class XPORT
    {
        private Dictionary<IPAddress, UdpClient> findClients = new Dictionary<IPAddress, UdpClient>();
        private Dictionary<IPAddress, UdpClient> commandClients = new Dictionary<IPAddress, UdpClient>();

        private Dictionary<IPAddress, TcpListener> tcpListeners = new Dictionary<IPAddress, TcpListener>();
        public bool isListening { get; set; } = false;
        public Action<byte[], IPEndPoint> OnData { get; set; }
        public XPORT(Int16 udpPort = 30700,Int16 udpCommandPort = 30718, Int16 tcpPort = 8445)
        {

            BindFindClients(udpPort);
            BindCommandClients(udpCommandPort);
            BindTcpListeners(tcpPort);
        }
        public void start()
        {
            if (isListening) return;
            isListening = true;
            // Start the async loop without blocking the main thread
            foreach (var listener in tcpListeners)
            {
                listener.Value.Start();
            }
            Listen();

        }
        private void BindTcpListeners(Int16 tcpPort)
        {
            List<IPAddress> addresses = GetEndpoints();
            foreach (IPAddress addr in addresses)
            {
                // Bind to the specific interface IP instead of IPAddress.Any
                TcpListener listener = new TcpListener(addr, tcpPort);
                tcpListeners.Add(addr,listener);
            }
        }
        private void BindFindClients(Int16 udpPort)
        {
            List<IPAddress> addresses = GetEndpoints();
            foreach(IPAddress addr in addresses) {
                var findClient = new UdpClient();
                findClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                findClient.EnableBroadcast = true;
                IPEndPoint localEndpoint = new IPEndPoint(addr,udpPort);
                findClient.Client.Bind(localEndpoint);
                findClients.Add(addr, findClient);
            }
        }
        private void BindCommandClients(Int16 udpPort)
        {
            List<IPAddress> addresses = GetEndpoints();
            foreach (IPAddress addr in addresses)
            {
                var commandClient = new UdpClient();
                commandClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                commandClient.EnableBroadcast = true;
                IPEndPoint localEndpoint = new IPEndPoint(addr, udpPort);
                commandClient.Client.Bind(localEndpoint);
                commandClients.Add(addr, commandClient);
            }
        }
        private List<IPAddress> GetEndpoints()
        {
            List<IPAddress> AddressList = new List<IPAddress>();
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface I in Interfaces)
            {
                if ((I.NetworkInterfaceType == NetworkInterfaceType.Ethernet || I.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) && I.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var Unicast in I.GetIPProperties().UnicastAddresses)
                    {
                        if (Unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            AddressList.Add(Unicast.Address);
                        }
                    }
                }
            }
            return AddressList;
        }

        private void Listen()
        {
            Task.Run(() => { // Inside your start / initialization routine:
                foreach (var kvp in findClients)
                {
                    UdpClient client = kvp.Value;

                    // Pass the individual adapter client into its dedicated async loop runner
                    Task.Run(async () => await FindListener(client));
                }
            });
            Task.Run(() => {
                foreach (var kvp in commandClients)
                {
                    UdpClient client = kvp.Value;

                    // Pass the individual adapter client into its dedicated async loop runner
                    Task.Run(async () => await CommandListener(client));
                }
            });

            Task.Run(() =>
            {
                while (isListening)
                {
                    foreach (var listener in tcpListeners)
                    {
                        if (listener.Value.Pending())
                        {
                            _ = HandleTcpClientAsync(listener.Value.AcceptTcpClient());
                        }
                    }
                    // Small sleep to prevent 100% CPU usage on this thread
                    System.Threading.Thread.Sleep(10);
                }
            }).Wait();
        }

        private async Task FindListener(UdpClient client)
        {
            while (isListening)
            {
                try
                {
                    // FIX: Await the specific interface client passed into the parameter, not the old global field
                    UdpReceiveResult result = await client.ReceiveAsync();
                    await DataCommandHandler(result.Buffer, result.RemoteEndPoint,client.Client.LocalEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private async Task CommandListener(UdpClient client)
        {
            while (isListening)
            {
                try
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    await DataCommandHandler(result.Buffer, result.RemoteEndPoint,client.Client.LocalEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private async Task HandleTcpClientAsync(TcpClient client)
        {
            using (client)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                while (client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Client disconnected

                    // This is where your actual "Serial Data" goes
                    byte[] data = new byte[bytesRead];
                    Array.Copy(buffer, data, bytesRead);

                }
            }
        }
        private async Task DataCommandHandler(byte[] received, IPEndPoint endPoint, EndPoint? localEndPoint)
        {
            if (received.Length == 1)
            {
                 //when the data lenght is 1 we have received a XPORT command
                XPORT_BROADCAST_COMMANDS command = (XPORT_BROADCAST_COMMANDS)received[0];
                Console.WriteLine($"command {command}");
                switch (command)
                {
                    case XPORT_BROADCAST_COMMANDS.NODE_FIND:
                        FindResponse myIdentity = new FindResponse();

                        if (localEndPoint is IPEndPoint localEP)
                        {
                            myIdentity.SetIP(localEP.Address);
                        }

                        myIdentity.SetMac(new byte[] { 0x00, 0x20, 0x4A, 0xBD, 0x2C, 0xD1 });
                        myIdentity.SubnetMaskBits = 24; // 255.255.255.0
                        myIdentity.HardwareID1 = 0x02; // XPort ID
                        myIdentity.HardwareID2 = 0x01;
                        myIdentity.SetName("LaMa");
                        foreach (UdpClient client in findClients.Values)
                        {
                            try
                            {
                                client.Send(myIdentity.ToBuffer(), endPoint);
                            } catch(Exception){

                            }
                        }
                        break;
                }
            } 
            else
            {
                BroadcastCommandStruct command = BroadcastCommandStruct.FromBytes(received);
                Console.WriteLine($"command {command.command}");
                switch (command.command)
                {
                    case XPORT_BROADCAST_COMMANDS.FIRMWARE_QUERRY:
                        FirmawareResponseStruct response = new FirmawareResponseStruct();
                        if (localEndPoint is IPEndPoint localEP)
                        {
                            response.SetIP(localEP.Address);
                        }
                        response.macAddress = new byte[] { 0x00, 0x20, 0x4A, 0xBD, 0x2C, 0xD1 };
                        response.SetVersion(0, 6, 0, 9);
                        response.command.reboot = 0;
                        response.command.command = XPORT_BROADCAST_COMMANDS.FIRMWARE_RESPONSE;
                        var res = response.ToBuffer();

                        foreach (UdpClient client in commandClients.Values)
                        {
                            try
                            {
                                client.Send(res, endPoint);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                }
            }
        }
    }
}
