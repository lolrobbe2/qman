using src.protocol.command;
using System;
using System.Collections.Generic;
using System.Text;

namespace src.Commands.controller
{
    internal class ControlPort
    {
        Dictionary<QBUS_CONTROL_COMMAND, Action<IControllerCommand, ControllerConnection>> _commandHandlers { get; init; }

        public ControlPort()
        {
            _commandHandlers = new();
            RegisterInternallHandlers();
        }

        private void RegisterInternallHandlers() {
            _commandHandlers.Add(QBUS_CONTROL_COMMAND.VERIFY_PASSWORD, HandleVerifyCommand);
        }
        private void HandleVerifyCommand(IControllerCommand command, ControllerConnection connection){
            Console.WriteLine("test");
        }
        public void ReceiveCommand(IControllerCommand command, ControllerConnection connection)
        {
            QBUS_CONTROL_COMMAND commandType = (QBUS_CONTROL_COMMAND)((CommandBase)command).type;
            if(_commandHandlers.TryGetValue(commandType,out Action<IControllerCommand, ControllerConnection> handler)){
                handler.Invoke(command, connection);
            }
        }
    }
}
