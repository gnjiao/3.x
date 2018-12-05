namespace Services.Modbus
{
    public enum ModbusStationType
    {
        TcpMaster,
        SerialMaster,
        TcpSlave,
        SerialSlave
    }

    public enum ModbusConversionType
    {
        SwapNone,
        SwapBytes,
        SwapWords,
        SwapAll
    }
    
    public enum ModbusSerialType
    {
        Rtu,
        Ascii
    }

    public enum ModbusDataTypeEx
    {
        HoldingRegister   = 0,
        InputRegister     = 1,
        Coil              = 2,
        Input             = 3,
        DeviceFailureInfo = 4
    }

    public enum ModbusDeviceDataType
    {
        Boolean,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Single,
        String
    }

    public enum ModbusReadWrite
    {
        ReadOnly,
        WriteOnly,
        ReadAndWrite
    }

    public struct ModbusLog
    {
        public const int LogNone     = 0;
        public const int LogErrors   = 1;
        public const int LogWarnings = 2;
        public const int LogInfos    = 3;
        public const int LogDebug    = 4;
    }
}
