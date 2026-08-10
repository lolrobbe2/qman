using qman.controller.src;
using src.Commands.xport;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace src.Commands
{
    internal class BroadCastHandlers
    {

        public static void Initialize()
        {
            XPORTInterface.RegisterFindHandler(XPORT_BROADCAST_COMMANDS.NODE_FIND, NodeFindHandler);
        }

        public static void FirmawareQueryHandler(XPORT port, UdpClient client, byte[] received, IPEndPoint endPoint, EndPoint? localEndPoint, XPORT_BROADCAST_COMMANDS command)
        {
            BroadcastCommandStruct s_command = BroadcastCommandStruct.FromBytes(received);

            if(s_command.command != XPORT_BROADCAST_COMMANDS.FIRMWARE_QUERRY && command != s_command.command){
                throw new ArgumentException("invalid command type for handler");
            }

            FirmawareResponseStruct response = new FirmawareResponseStruct();
            if (localEndPoint is IPEndPoint localEP)
            {
                response.SetIP(localEP.Address);
            }
            response.macAddress = port.GetMac();
            response.SetVersion(0, 6, 0, 9);
            response.command.reboot = 0;
            response.command.command = XPORT_BROADCAST_COMMANDS.FIRMWARE_RESPONSE;
            var res = response.ToBuffer();
            client.SendAsync(response.ToBuffer(), endPoint);
        }
        public static void ExtendedVersionQueryHandler(XPORT port, UdpClient client, byte[] received, IPEndPoint endPoint, EndPoint? localEndPoint, XPORT_BROADCAST_COMMANDS command){
            BroadcastCommandStruct s_command = BroadcastCommandStruct.FromBytes(received);

            if (s_command.command != XPORT_BROADCAST_COMMANDS.EXTENDED_VERSION_QUERRY && command != s_command.command)
            {
                throw new ArgumentException("invalid command type for handler");
            }

  

            ExtendedVersionResponse response = new ExtendedVersionResponse();
            response.setVersion(6, 0, 9);
            response.command.reboot = 0;
            response.command.command = XPORT_BROADCAST_COMMANDS.EXTENDED_VERSION_RESPONSE;
            client.SendAsync(response.ToBuffer(),endPoint);
        }
        public static void SetupRecord2Handler(XPORT port, UdpClient client, byte[] received, IPEndPoint endPoint, EndPoint? localEndPoint, XPORT_BROADCAST_COMMANDS command){
            BroadcastCommandStruct s_command = BroadcastCommandStruct.FromBytes(received);

            if (s_command.command != XPORT_BROADCAST_COMMANDS.SETUP_RECORD_2_QUERRY && command != s_command.command)
            {
                throw new ArgumentException("invalid command type for handler");
            }
            SetupRecord2Response response = new SetupRecord2Response();
            response.SetSerial("000059");
            response.SetName("LaMa");
            response.command.reboot = 0;
            response.command.command = XPORT_BROADCAST_COMMANDS.SETUP_RECORD_2_RESPONSE;
            client.SendAsync(response.ToBuffer(), endPoint);
        }
        public static void NodeFindHandler(XPORT port, UdpClient client, byte[] received, IPEndPoint endPoint, EndPoint? localEndPoint, XPORT_BROADCAST_COMMANDS command)
        {
            FindResponse myIdentity = new FindResponse();

            if (localEndPoint is IPEndPoint localEP)
            {
                myIdentity.SetIP(localEP.Address);
            }

            myIdentity.SetMac(new byte[] { 0x00, 0x20, 0x4A, 0xBD, 0x2C, 0xD1 });
            myIdentity.SubnetMaskBits = 24; // 255.255.255.0
            myIdentity.HardwareID1 = 0x02; // XPort ID
            myIdentity.HardwareID2 = 0x01;
            myIdentity.SetSerial("000059");
            myIdentity.SetName("LaMa");
            client.SendAsync(myIdentity.ToBuffer(), endPoint);

        }
    }
}
