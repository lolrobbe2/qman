using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace qmanlib.protocol.command.address
{
    public class AddressRead : CommandBase
    {
        public AddressRead(byte[] data) {
            IEnumerable<byte> commandStart = data.Skip(9); //skip the prefix

            if (type != (QBUS_COMMAND_TYPE)commandStart.ElementAt(0))
                throw new ArgumentException("invalid command type");

            Adress = commandStart.ElementAt(1);
            SubAdress = (SUBADDRESS)commandStart.ElementAt(2);
        }

        public AddressRead()
        {
        }

        public override QBUS_COMMAND_TYPE type => QBUS_COMMAND_TYPE.ADDRESS_STATUS;

        protected override byte instruction1 => Adress;

        protected override byte instruction2 => (byte)SubAdress;

        public override string Name => "AddressRead";

        public byte Adress {  get; set; }
        public SUBADDRESS SubAdress { get; set; }

        public override byte[] DataSerialize()
        {
            return new byte[] { };
        }

        public override string FormatDataString()
        {
            return $"Address: {Adress}, SubAdress: {SubAdress.ToString()}";
        }
    }
}
