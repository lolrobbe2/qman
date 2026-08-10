using src.protocol.command.address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace src.protocol.command.service
{
    public class VerifyRequestCommand : CommandBase
    {
        public VerifyRequestCommand(byte[] data) {

        }

        public VerifyRequestCommand(string userName, string passWord)
        {
            _userName = new byte[16];
            _password = new byte[16];

            byte[] userNameBytes = Encoding.ASCII.GetBytes(userName);
            byte[] passwordBytes = Encoding.ASCII.GetBytes(passWord);

            Array.Copy(userNameBytes, _userName, Math.Min(userNameBytes.Length, _userName.Length));
            Array.Copy(passwordBytes, _password, Math.Min(passwordBytes.Length, _password.Length));
        }
        public override string Name => "VerifyCommand";

        public override QBUS_COMMAND_TYPE type => (QBUS_COMMAND_TYPE)QBUS_CONTROL_COMMAND.VERIFY_PASSWORD;

        protected override byte instruction1 => _userName[0];

        protected override byte instruction2 => _userName[1];
        private byte[] _userName;

        private byte[] _password;
        public override byte[] DataSerialize()
        {
            return _userName.Skip(2)
                    .Concat(_password)
                    .ToArray();
        }

        public override string FormatDataString()
        {
            return "No you don't :)";
        }
    }
    public class VerifyeResponseCommand : CommandBase
    {
        public override QBUS_COMMAND_TYPE type => throw new NotImplementedException();

        public override string Name => throw new NotImplementedException();

        protected override byte instruction1 => throw new NotImplementedException();

        protected override byte instruction2 => throw new NotImplementedException();

        public override byte[] DataSerialize()
        {
            throw new NotImplementedException();
        }

        public override string FormatDataString()
        {
            throw new NotImplementedException();
        }
    }
}
