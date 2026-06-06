using qmanlib.protocol.packet;
using src.protocol.command.address;
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
    internal interface ICommand : IPacket.IPacketData
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
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class CommandBase : ICommand
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
        internal static CommandBase Create(byte[] data)
        {
            QBUS_COMMAND_TYPE type = (QBUS_COMMAND_TYPE)data.Skip(9).First();
            bool isReadCommand = false;
            if ((int)type >= 128)
            {
                isReadCommand = true;
                type = (QBUS_COMMAND_TYPE)((int)type - 128);
            }
            return type switch
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
