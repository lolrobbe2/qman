using src.Commands.xport;
using src.xport;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace src.Controller.xport
{
    public class Xport
    {
        static Int16 BroadCast = 30700;
        static Int16 Command = 30718;
        private static IPAddress _endpoint { get; set; }
        private static UdpClient _udpClient { get; set; }

        Xport(IPAddress address)
        {

        }

        public static List<ControllerInfo> Search(IPAddress endpoint)
        {
            List<ControllerInfo> found = new List<ControllerInfo>();

            using UdpClient BroadCastClient = new UdpClient();
            BroadCastClient.Client.EnableBroadcast = true;

            // Bind to local IP + broadcast port
            IPEndPoint bindEP = new IPEndPoint(endpoint, 64218);
            BroadCastClient.Client.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReuseAddress,
                true
            );
            BroadCastClient.Client.Bind(bindEP);

            // Send NODE FIND broadcast
            IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, BroadCast);

            byte[] data = new byte[] { (byte)XPORT_BROADCAST_COMMANDS.NODE_FIND };
            BroadCastClient.Send(data,1,broadcastEP);

            // Receive responses (non-blocking loop with timeout)
            BroadCastClient.Client.ReceiveTimeout = 500;

            DateTime start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < 5000)
            {
                try
                {
                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 64218);
                    byte[] buffer = BroadCastClient.Receive(ref remote);

                    if (buffer.Length >= Marshal.SizeOf(typeof(FindResponse)))
                    {
                        found.Add(new ControllerInfo(ByteConvertable.ByteArrayToStruct<FindResponse>(buffer)));
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                    break; // timeout
                }
            }

            return found;
        }

    }
}
