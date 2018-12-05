using System;
using System.Collections.Generic;
using Core;

namespace Services.Modbus
{    
    [Serializable]
    public class ModbusPoint : BasePoint, IComparer<ModbusPoint>
    {
        public string Station { get; set; }
        public string DataTypeStr { get;}
        public ModbusDataTypeEx DataTypeEx { get; set; }
        public ModbusDeviceDataType DeviceDataType { get; set; }
        public ModbusConversionType ConversionType { get; set; }
        public byte SlaveId { get; set; }
        public ushort DataAddress { get; set; }
        public ushort DataLen { get; set; }
        public int BitIndex { get; set; }
        public double K { get; set; }
        public double D { get; set; }        
        public string Mode { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public object MinVal { get; set; }
        public object MaxVal { get; set; }
        public object DeadZone { get; set; }
       
        public ModbusPoint() : base($"variable", "", GetTypeByString("string"), ReadWriteFlags.ReadOnly, "")
        {            
        }        

        public ModbusPoint(
            string name,
            string device,
            string datatype,
            ReadWriteFlags readwrite,
            object defaultVal,
            object tag,

            string station,
            ModbusDeviceDataType deviceDataType,
            ModbusDataTypeEx dataTypeEx,
            byte slaveId,
            ushort dataAddress,
            ushort dataLen,
            ModbusConversionType conversionType,
            int bitIndex,
            double k,
            double d,

            string mode,
            object minVal,
            object maxVal,
            object deadZone,
            string description,
            string unit)
            : base(name, device, GetTypeByString(datatype), readwrite, defaultVal)
        {
            Station = station;
            DataTypeStr = datatype;
            DataTypeEx = dataTypeEx;
            DeviceDataType = deviceDataType;
            SlaveId = slaveId;
            DataAddress = dataAddress;
            DataLen = dataLen;
            ConversionType = conversionType;
            BitIndex = bitIndex;
            K = k;
            D = d;

            MinVal = minVal;
            MaxVal = maxVal;
            Mode = mode;
            DeadZone = deadZone;
            Description = description;
            Unit = unit;
            Tag = tag;            
        }

        public override void DoUpdate()
        {
        }

        private static Type GetTypeByString(string type)
        {
            switch (type.ToLower())
            {
                case "bool":
                    return Type.GetType("System.Boolean", true, true);
                case "byte":
                    return Type.GetType("System.Byte", true, true);
                case "sbyte":
                    return Type.GetType("System.SByte", true, true);
                case "char":
                    return Type.GetType("System.Char", true, true);
                case "decimal":
                    return Type.GetType("System.Decimal", true, true);
                case "double":
                    return Type.GetType("System.Double", true, true);
                case "float":
                    return Type.GetType("System.Single", true, true);
                case "int":
                    return Type.GetType("System.Int32", true, true);
                case "uint":
                    return Type.GetType("System.UInt32", true, true);
                case "long":
                    return Type.GetType("System.Int64", true, true);
                case "ulong":
                    return Type.GetType("System.UInt64", true, true);
                case "object":
                    return Type.GetType("System.Object", true, true);
                case "short":
                    return Type.GetType("System.Int16", true, true);
                case "ushort":
                    return Type.GetType("System.UInt16", true, true);
                case "string":
                    return Type.GetType("System.String", true, true);
                case "date":
                case "datetime":
                    return Type.GetType("System.DateTime", true, true);
                case "guid":
                    return Type.GetType("System.Guid", true, true);
                default:
                    return Type.GetType(type, true, true);
            }
        }

        public int Compare(ModbusPoint x, ModbusPoint y)
        {
            if (x?.SlaveId > y?.SlaveId)
                return 1;
            if (x?.SlaveId < y?.SlaveId)
                return -1;
            if (x?.DataTypeEx > y?.DataTypeEx)
                return 1;
            if (x?.DataTypeEx < y?.DataTypeEx)
                return -1;
            if (x?.DataAddress > y?.DataAddress)
                return 1;
            if (x?.DataAddress < y?.DataAddress)
                return -1;            
            return 0;
        }
    }
}
