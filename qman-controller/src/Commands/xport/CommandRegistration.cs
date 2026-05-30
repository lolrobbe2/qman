using qman.controller.src;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace src.Commands.xport
{
    internal class CommandRegistration
    {
        public XPORT_BROADCAST_COMMANDS Command { get; }
        public Action<XPORT, UdpClient, byte[], IPEndPoint, EndPoint, XPORT_BROADCAST_COMMANDS> Handler { get; }

        public CommandRegistration(
            XPORT_BROADCAST_COMMANDS command,
            Action<XPORT, UdpClient, byte[], IPEndPoint, EndPoint, XPORT_BROADCAST_COMMANDS> handler)
        {
            Command = command;
            Handler = handler;
        }
}
}
