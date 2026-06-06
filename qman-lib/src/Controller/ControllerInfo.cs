using src.Commands.xport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace src.Controller
{
    [DebuggerDisplay("[ip(_response.GetAddressString()]")]
    public class ControllerInfo
    {
        private FindResponse _response { get; set; }
        public IPAddress Address { get => _response.GetIp(); set => _response.SetIP(value); }
        public string Name { get => _response.GetName(); set => _response.SetName(value); }

        public ControllerInfo(FindResponse response){
            _response = response;
        }
    }
    
}
