using qman.controller.src.Commands;
using src;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace qman.controller.src
{
    internal class XPORT
    {

        private ConcurrentDictionary<IPAddress, XPORTInterface> _interfaces = new ConcurrentDictionary<IPAddress, XPORTInterface>();
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] _macAddress = new byte[6];
        public bool isListening { get; set; } = false;
        public XPORT(UInt16 udpPort = 30700, UInt16 udpCommandPort = 30718, UInt16 tcpPort = 8445)
        {

            SetMac(0x00, 0x20, 0x4A, 0xBD, 0x2C, 0xD1);
            foreach (var addr in GetEndpoints())
            {
                _interfaces.TryAdd(addr, new XPORTInterface(this,addr, udpPort, udpCommandPort));
            }
        }
        public void start()
        {
            if (isListening) return;
            isListening = true;
            // Start the async loop without blocking the main thread
            foreach(XPORTInterface _interface in _interfaces.Values)
            {
                _interface.StartListening();
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

        #region MAC_ADDR
        #region SET
        public void SetMac(byte[] mac)
        {
            if (mac == null || mac.Length != 6)
                throw new ArgumentException("MAC address must be exactly 6 bytes.");
            _macAddress = (byte[])mac.Clone();
        }

        // 2. Set using a Hex String (e.g., "00204ABD2CD1")
        public void SetMac(string macString)
        {
            // Remove colons or dashes if present
            string cleanMac = macString.Replace(":", "").Replace("-", "");

            if (cleanMac.Length != 12)
                throw new ArgumentException("MAC string must be 12 hex characters.");

            for (int i = 0; i < 6; i++)
            {
                _macAddress[i] = Convert.ToByte(cleanMac.Substring(i * 2, 2), 16);
            }
        }

        public void SetMac(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6)
        {
            _macAddress[0] = b1;
            _macAddress[1] = b2;
            _macAddress[2] = b3;
            _macAddress[3] = b4;
            _macAddress[4] = b5;
            _macAddress[5] = b6;
        }

        // Optional: Get it as a formatted string for logs
        public string MacString()
        {
            return BitConverter.ToString(_macAddress).Replace("-", ":");
        }
        #endregion
        public byte[] GetMac()
        {
            return _macAddress;
        }
        public void RegisterIncommingConnectionHandler(Action<TcpClient> action)
        {
            foreach (var interfacePair in _interfaces)
            {
                interfacePair.Value.RegisterIncommingConnectionHandler(action);
            }
        }
        #endregion
    }
}
