using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace Services.Modbus
{
    [Serializable]
    [ContentProperty(nameof(Channels))]
    public class ModbusBuffer
    {
        public ModbusDataTypeEx DataTypeEx { get; set; }
        public ushort StartAddress { get; set; }
        public ushort LastAddress { get; set; }
        public ushort NumInputs { get; set; }
        public byte SlaveId { get; set; }
        public int  PauseCounter { get; set; }
        public List<ModbusPoint> Channels { get; set; } = new List<ModbusPoint>();
    }
}
