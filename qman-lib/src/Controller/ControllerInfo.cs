using src.Commands.xport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace src.Controller
{
    [DebuggerDisplay("[({Name}, {Address}, {Serial}]")]
    public class ControllerInfo
    {
        private FindResponse _response { get; set; }
        public IPAddress Address { get => _response.GetIp(); set => _response.SetIP(value); }
        public string Name { get => _response.GetName(); set => _response.SetName(value); }
        public string Serial { get => _response.GetSerial(); set => _response.SetSerial(value);  }
        public ControllerInfo(FindResponse response){
            _response = response;
        }
    }
    
}
