using qmanlib.protocol.packet;
using src.protocol.command.address;
using src.protocol.command.service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace src.protocol.command
{
    public interface IControllerCommand : IPacket.IPacketData
    {
        public abstract string Name { get; }

        /// <summary>
        /// <para>This function wich is used by the internal toString function has has the following format:</para>
        /// <para>[Command("Name"),Data(FormatString)]</para>
        /// </summary>
        /// <returns></returns>
        /// 
        protected abstract string FormatString();
    }
    enum COMMAND_SECTIONS : byte
    {
        START = 42,//0x2A
        END = 35 //0x23
    };
    public class CommandSections
    {
        public static byte[] prefix => new byte[] { 81, 66, 85, 83, 0, 4, 0, 1, 1 }; //QBUS
        public static byte START => 42;
        public static byte END => 35;
        public static int HEADER_SIZE => prefix.Length + 4;
        public static int PREFIX_SIZE => prefix.Length;
    }
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class CommandBase : IControllerCommand
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString()!;
        public override string? ToString()
        {
            return $"[Command({Name}),Data({FormatString()})]";
        }

        public abstract QBUS_COMMAND_TYPE type { get; }
        protected abstract byte instruction1 { get; }
        protected abstract byte instruction2 { get;}
        public abstract string Name { get; }
        public abstract string FormatDataString();
        public abstract byte[] DataSerialize();

        public byte[] Serialize()
        {
            IList<byte> SerializedData = new List<byte>();
            SerializedData.Add((byte)COMMAND_SECTIONS.START);
            SerializedData.Add((byte)type);
            SerializedData.Add(instruction1);
            SerializedData.Add(instruction2);
            //DATA add
            byte[] data = DataSerialize();
            SerializedData.Add((byte)data.Length);
            SerializedData.Concat(data);
            SerializedData.Add((byte)COMMAND_SECTIONS.END);
            return SerializedData.ToArray();
        }

        public string FormatString()
        {
            return $"type: {type.ToString()}, {FormatDataString()}";
        }
        internal static CommandBase Create(byte[] data, bool controlPort)
        {
            byte type = data.First();
            QBUS_COMMAND_TYPE commandType = (QBUS_COMMAND_TYPE)type;
            bool isReadCommand = false;
            if ((int)type >= 128)
            {
                isReadCommand = true;
                commandType = (QBUS_COMMAND_TYPE)((int)type - 128);
            }
            if(controlPort) {
                QBUS_CONTROL_COMMAND controlType = (QBUS_CONTROL_COMMAND)type;
                return controlType switch
                {
                    QBUS_CONTROL_COMMAND.VERIFY_PASSWORD => isReadCommand ? throw new NotImplementedException() : new VerifyRequestCommand(data),
                    QBUS_CONTROL_COMMAND.READ_PASSWORD => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.STRING_DATA => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.INITIALISATION_VECTOR => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.ENCRYPTION_KEY => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.CLOUD_LOGIN => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.SHIFT_KEY => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.POSITION_KEY => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.WRITE_PASSWORD => throw new NotImplementedException(),
                    QBUS_CONTROL_COMMAND.ERROR => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
            return commandType switch
            {
                QBUS_COMMAND_TYPE.SERVICE => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CONTROL_PARAMETERS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.ADDRESS_PARAMETERS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.WORK_TEXT => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.ADDRESS_TEXT => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CHANNEL_LIST_TEXT => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.RTC_CLOCK => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.VERSION => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.DATE => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.FAT_DATA => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SWAP_CTL => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CLEAR_POWERDOWN_TIME => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.FIRMWARE_UPLOAD => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CONTROLLER_OPTIONS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CONTROLLER_CLEAR => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CONTROLLER_REBOOT => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.PRESET_CLEAR => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.PRESET_DATA => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.MINI_PRESET_PARAMETERS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.PRESET_PARAMETERS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.HOUR_COUNTERS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CLOCK_CLEAR => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SCHEDULE_DATA => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.HOLIDAY => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.EVENT_LOGS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.LOGIC => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.LOGIC_ANALOG => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.JAGA_DB => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.MODULE_SRAM => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.MODULE_EEPROM => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.MODULE_FLASH => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SIMULATION_DATA => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CHANNEL_LIST_MENU => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CHANNEL_LIST_DATA => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.EXTERNAL_CHANNEL_LIST_DATA => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.ROOM => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.EVENT_READ => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.EVENTS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.ADDRESS_STATUS => isReadCommand ? new AddressRead(data):throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CHANNEL_STATUS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.EXTERNAL_CHANNEL_STATUS => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.ADDRESS_MODE => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.CHANNEL_MODE => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SIMULATION => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SD_SERVICE => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SD_MODULE => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SD_INFO => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SD_DATABASE => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SD_UNKOWN => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.SD_COPY_DATA => throw new NotImplementedException(),
                QBUS_COMMAND_TYPE.COPY_DATA_SD => throw new NotImplementedException(),
                _ => throw new NotSupportedException($"Unknown type {type}")
            };
        }
    }


}
