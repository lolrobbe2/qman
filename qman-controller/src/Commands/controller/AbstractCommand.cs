using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace src.Commands.controller
{
    class CommandSections {
        public static byte[] prefix => new byte[] { 81, 66, 85, 83, 0, 4, 0, 1, 1 }; //QBUS
        public static byte START => 42;
        public static byte END => 35;
        public static int HEADER_SIZE => prefix.Length + 4;
        public static int PREFIX_SIZE => prefix.Length;
    }
    [DebuggerDisplay("[COMMAND({type}, Instruction1 = {instruction1}, Instruction2 = {instruction2}, Data = {data.Count} bytes)]")]
    internal class AbstractCommand
    {
        QBUS_COMMAND_TYPE type {  get; set; }
        byte instruction1 { get; set; }
        byte instruction2 {  get; set; }

        List<byte> data { get; set; } = new List<byte>();

        public AbstractCommand(byte[] command) {
            type = (QBUS_COMMAND_TYPE)command[0];
            instruction1 = command[1];
            instruction2 = command[2];
            data = command.Skip(3).ToList();
        }
    }
}
