using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Core;
using Modbus;
using Modbus.Device;

namespace Services.Modbus
{
    [Serializable]
    public abstract class ModbusBaseClientStation : IModbusStation
    {
        public List<ModbusPoint> Channels { get; set; } = new List<ModbusPoint>();
        protected List<ModbusBuffer> Buffers { get; set; } = new List<ModbusBuffer>();

        protected readonly Dictionary<byte, ModbusPoint> Failures = new Dictionary<byte, ModbusPoint>();
        protected readonly Queue<ModbusPoint> SendQueue = new Queue<ModbusPoint>();
        protected readonly object SendQueueSyncRoot = new object();
        protected readonly ManualResetEvent SendQueueEndWaitEvent = new ManualResetEvent(false);
        protected volatile bool RunThread;
        protected readonly List<ModbusPoint> ChannelsToSend = new List<ModbusPoint>();

        public string Name { get; set; }
        public int CycleTimeout { get; set; }
        public int RetryTimeout { get; set; }
        public int RetryCount { get; set; }
        public int FailedCount { get; set; }
        public int LoggingLevel { get; set; }
        public bool StationActive { get; set; }

        protected ModbusBaseClientStation()
        {
            Name = Guid.NewGuid().ToString();
            CycleTimeout = 10;
            RetryTimeout = 100;
            RetryCount = 1;
            FailedCount = 1;
            LoggingLevel = 0;
            StationActive = true;
        }

        protected ModbusBaseClientStation(string name, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            Name = name;
            CycleTimeout = Math.Max(cycleTimeout, 10);
            RetryTimeout = Math.Max(retryTimeout, 100);
            RetryCount = Math.Max(retryCount, 1);
            FailedCount = Math.Max(failedCount, 1);
            LoggingLevel = 0;
            StationActive = true;
        }

        public void SendValueUpdateToModbusLine(ModbusPoint ch)
        {
            if (!RunThread) return;

            lock (SendQueueSyncRoot)
            {
                SendQueue.Enqueue(ch);
            }

            SendQueueEndWaitEvent.Set();
        }

        public bool Running => RunThread;

        public void Stop()
        {
            if (!StationActive) return;

            RunThread = false;

            lock (SendQueueSyncRoot)
            {
                SendQueue.Clear();
            }
        }

        public int Start()
        {
            if (!StationActive) return 1;

            Buffers.Clear();
            Failures.Clear();

            if (Channels.Count > 0)
            {
                Channels.Sort(Channels[0].Compare);

                foreach (var ch in Channels)
                {
                    if (ch.ReadWrite != ReadWriteFlags.WriteOnly && ch.DataTypeEx != ModbusDataTypeEx.DeviceFailureInfo)
                    {
                        var found = false;
                        foreach (var buf in Buffers)
                        {
                            if (ch.SlaveId == buf.SlaveId && ch.DataTypeEx == buf.DataTypeEx)
                            {
                                int mult;
                                if (ch.DataTypeEx == ModbusDataTypeEx.Input || ch.DataTypeEx == ModbusDataTypeEx.Coil)
                                    mult = 16;
                                else
                                    mult = 1;
                                if (((ch.DataAddress - buf.LastAddress) < 4 * mult) && ((buf.LastAddress - buf.StartAddress + ch.DataLen) < 120 * mult))
                                // Optimization - "holes" in address space less than 4 words
                                // will be included in one read until max frame 2*120 bytes is reached
                                {
                                    buf.LastAddress = (ushort)((ch.DataAddress + ch.DataLen - 1) > buf.LastAddress ? ch.DataAddress + ch.DataLen - 1 : buf.LastAddress);
                                    buf.NumInputs = (ushort)((buf.LastAddress - buf.StartAddress + ch.DataLen) > buf.NumInputs ? buf.LastAddress - buf.StartAddress + ch.DataLen : buf.NumInputs);
                                    buf.Channels.Add(ch);
                                    ch.StatusFlags = PointStatusFlags.Bad;
                                    found = true;
                                }
                            }
                        }
                        if (!found)
                        {
                            var buf = new ModbusBuffer
                            {
                                SlaveId = ch.SlaveId,
                                NumInputs = ch.DataLen,
                                StartAddress = ch.DataAddress,
                                LastAddress = (ushort)(ch.DataAddress + ch.DataLen - 1),
                                DataTypeEx = ch.DataTypeEx
                            };
                            buf.Channels.Add(ch);
                            ch.StatusFlags = PointStatusFlags.Bad;
                            buf.PauseCounter = 0;
                            Buffers.Add(buf);
                        }
                    }

                    if (ch.DataTypeEx == ModbusDataTypeEx.DeviceFailureInfo)
                    {
                        if (Failures.ContainsKey(ch.SlaveId))
                        {
                            // failure signal already defined, parameterization error
                            //if (LoggingLevel >= ModbusLog.LogErrors)
                                //Env.Current.Logger.LogError(string.Format(StringConstants.ErrFailureTwice, this.Name, Failures[ch.SlaveId].Name, ch.Name));
                        }
                        else
                        {
                            Failures.Add(ch.SlaveId, ch);
                            ch.Value = true;
                        }
                    }
                }
                return 0;
            }
            return 1;
        }

        public void ClearChannels()
        {
            Channels.Clear();
        }

        public void AddChannel(ModbusPoint channel)
        {
            Channels.Add(channel);
        }

        protected void ReadBuffer(ModbusBaseClientStation self, IModbusMaster master, ModbusBuffer buf)
        {
            try
            {
                if (buf.PauseCounter == 0)
                {
                    var startAddress = buf.StartAddress;
                    var numInputs = buf.NumInputs;
                    switch (buf.DataTypeEx)
                    {
                        case ModbusDataTypeEx.InputRegister:
                        case ModbusDataTypeEx.HoldingRegister:
                            var registers = buf.DataTypeEx == ModbusDataTypeEx.InputRegister ? master.ReadInputRegisters(buf.SlaveId, startAddress, numInputs) 
                                : master.ReadHoldingRegisters(buf.SlaveId, startAddress, numInputs);
                            var dt = DateTime.Now;
                            foreach (var ch in buf.Channels)
                            {
                                byte[] adr;
                                switch (ch.DeviceDataType)
                                {
                                    case ModbusDeviceDataType.Int16:

                                        short shortResult;
                                        adr = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress]);
                                        switch (ch.ConversionType)
                                        {
                                            case ModbusConversionType.SwapBytes:
                                                var tmp = adr[0]; adr[0] = adr[1]; adr[1] = tmp;
                                                shortResult = BitConverter.ToInt16(adr, 0);
                                                break;
                                            default:
                                                shortResult = BitConverter.ToInt16(adr, 0);
                                                break;
                                        }
                                        if (ch.DataType == typeof(short))
                                        {
                                            ch.DoUpdate(shortResult, dt, PointStatusFlags.Good);
                                        }
                                        else if (ch.DataType == typeof(double))
                                        {
                                            ch.DoUpdate((double)(ch.K * shortResult + ch.D), dt, PointStatusFlags.Good);
                                        }
//                                        else
//                                            if (self.LoggingLevel >= ModbusLog.LogWarnings)
//                                                Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                                    self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                        break;

                                    case ModbusDeviceDataType.UInt16:

                                        ushort ushortResult;
                                        adr = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress]);
                                        switch (ch.ConversionType)
                                        {
                                            case ModbusConversionType.SwapBytes:
                                                byte tmp = adr[0]; adr[0] = adr[1]; adr[1] = tmp;
                                                ushortResult = BitConverter.ToUInt16(adr, 0);
                                                break;
                                            default:
                                                ushortResult = BitConverter.ToUInt16(adr, 0);
                                                break;
                                        }
                                        if (ch.DataType == typeof(ushort))
                                        {
                                            ch.DoUpdate(ushortResult, dt, PointStatusFlags.Good);
                                        }
                                        else if (ch.DataType == typeof(double))
                                        {
                                            ch.DoUpdate((double)(ch.K * ushortResult + ch.D), dt, PointStatusFlags.Good);
                                        }
//                                        else
//                                            if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                                Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                                    self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                        break;
                                    case ModbusDeviceDataType.Int32:

                                        var adr0 = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress]);
                                        var adr1 = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress + 1]);                                        
                                        var res = self.SwapBytesIn(adr0, adr1, ch.ConversionType);
                                        var intResult = BitConverter.ToInt32(res, 0);
                                        if (ch.DataType == typeof(int))
                                        {
                                            ch.DoUpdate(intResult, dt, PointStatusFlags.Good);
                                        }
                                        else if (ch.DataType == typeof(double))
                                        {
                                            ch.DoUpdate((double)(ch.K * intResult + ch.D), dt, PointStatusFlags.Good);
                                        }
//                                        else
//                                            if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                                self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                        break;
                                    case ModbusDeviceDataType.UInt32:

                                        adr0 = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress]);
                                        adr1 = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress + 1]);
                                        res = self.SwapBytesIn(adr0, adr1, ch.ConversionType);
                                        var uintResult = BitConverter.ToUInt32(res, 0);
                                        if (ch.DataType == typeof(uint))
                                        {
                                            ch.DoUpdate(uintResult, dt, PointStatusFlags.Good);
                                        }
                                        else if (ch.DataType == typeof(double))
                                        {
                                            ch.DoUpdate((double)(ch.K * uintResult + ch.D), dt, PointStatusFlags.Good);
                                        }
//                                        else
//                                            if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                                self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                        break;
                                    case ModbusDeviceDataType.Single:

                                        adr0 = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress]);
                                        adr1 = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress + 1]);
                                        res = self.SwapBytesIn(adr0, adr1, ch.ConversionType);
                                        var floatResult = BitConverter.ToSingle(res, 0);
                                        if (ch.DataType == typeof(double))
                                        {
                                            ch.DoUpdate((double)(ch.K * floatResult + ch.D), dt, PointStatusFlags.Good);
                                        }
//                                        else
//                                            if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                                self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                        break;
                                    case ModbusDeviceDataType.Boolean:

                                        var bit = (registers[ch.DataAddress - buf.StartAddress] & (0x01 << ch.BitIndex)) > 0;
                                        if (ch.DataType == typeof(bool))
                                        {
                                            ch.DoUpdate(bit, dt, PointStatusFlags.Good);
                                        }
//                                        else
//                                            if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                                self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                        break;
                                    case ModbusDeviceDataType.String:

                                        var str = new byte[2 * ch.DataLen];
                                        var ascii = new ASCIIEncoding().GetDecoder();
                                        var j = 0;
                                        // Conversion strategy: FIRST NONPRINTABLE CHARACTER (ORD < 32) BREAKS CONVERSION, string consists of printables converted before
                                        for (var i = 0; i < ch.DataLen; i++)
                                        {
                                            var word = BitConverter.GetBytes(registers[ch.DataAddress - buf.StartAddress + i]);
                                            if (ch.ConversionType == ModbusConversionType.SwapBytes)
                                            {
                                                if (word[1] < 32)
                                                    break;  // nonprintable character
                                                str[j++] = word[1];
                                                if (word[0] < 32)
                                                    break;  // nonprintable character
                                                str[j++] = word[0];
                                            }
                                            else
                                            {
                                                if (word[0] < 32)
                                                    break;  // nonprintable character
                                                str[j++] = word[0];
                                                if (word[1] < 32)
                                                    break;  // nonprintable character
                                                str[j++] = word[1];
                                                //Array.Copy(BitConverter.GetBytes(registers[ch.ModbusDataAddress - buf.startAddress + i]), 0, str, 2 * i, 2);
                                            }
                                        }
                                        string sresult;
                                        if (j > 0)
                                        {
                                            var chars = new char[j];
                                            ascii.Convert(str, 0, j, chars, 0, j, true, out var bytesUsed, out var charsUsed, out var completed);
                                            sresult = new string(chars);
                                        }
                                        else
                                            sresult = "";
                                        if (ch.DataType == typeof(string))
                                        {
                                            ch.DoUpdate(sresult, dt, PointStatusFlags.Good);
                                        }
//                                        else
//                                            if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                                self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                                        break;
                                }
                            }
                            break;
                        case ModbusDataTypeEx.Coil:
                        case ModbusDataTypeEx.Input:
                            var inputs = buf.DataTypeEx == ModbusDataTypeEx.Coil ? master.ReadCoils(buf.SlaveId, startAddress, numInputs) : 
                                master.ReadInputs(buf.SlaveId, startAddress, numInputs);
                            dt = DateTime.Now;
                            foreach (var ch in buf.Channels)
                            {
                                if (ch.DataType == typeof(uint))
                                {
                                    uint val = (uint)(inputs[ch.DataAddress - buf.StartAddress] ? 1 : 0);
                                    ch.DoUpdate(val, dt, PointStatusFlags.Good);
                                }
                                else if (ch.DataType == typeof(bool))
                                {
                                    var val = inputs[ch.DataAddress - buf.StartAddress];
                                    ch.DoUpdate(val, dt, PointStatusFlags.Good);
                                }
//                                else
//                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                        self.Name, ch.Name, ch.DeviceDataType.ToString(), ch.ModbusFs2InternalType.ToString()));
                            }
                            break;
                    }   // Case
                    if (self.Failures.ContainsKey(buf.SlaveId))
                    {
                        // failure signal defined
                        self.Failures[buf.SlaveId].Value = false;
                    }
                }   // If
                else
                {
                    buf.PauseCounter--;
                }
            }   // Try
            catch (SlaveException )
            {
                buf.PauseCounter = self.FailedCount;
                if (self.Failures.ContainsKey(buf.SlaveId))
                {
                    // failure signal defined
                    self.Failures[buf.SlaveId].Value = true;
                }
                foreach (var ch in buf.Channels)
                {
                    ch.StatusFlags = PointStatusFlags.Bad;
                }
//                if (self.LoggingLevel >= ModbusLog.logWarnings)
//                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrReceive,
//                        self.Name, buf.slaveId, buf.ModbusDataType.ToString(), buf.startAddress, buf.numInputs, e.Message));
            }
            catch (TimeoutException )
            {
                buf.PauseCounter = self.FailedCount;
                if (self.Failures.ContainsKey(buf.SlaveId))
                {
                    // failure signal defined
                    self.Failures[buf.SlaveId].Value = true;
                }
                foreach (var ch in buf.Channels)
                {
                    ch.StatusFlags = PointStatusFlags.Bad;
                }
//                if (self.LoggingLevel >= ModbusLog.logWarnings)
//                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrReceive,
//                        self.Name, buf.slaveId, buf.ModbusDataType.ToString(), buf.startAddress, buf.numInputs, e.Message));
            }
        }

        protected void WriteChannel(ModbusBaseClientStation self, IModbusMaster master, ModbusPoint ch)
        {
            try
            {
                var adr = new byte[4];
                var convOk = true;
                
                switch (ch.DataTypeEx)
                {
                    case ModbusDataTypeEx.HoldingRegister:
                        switch (ch.DeviceDataType)
                        {
                            case ModbusDeviceDataType.Int16:
                            case ModbusDeviceDataType.UInt16:
                                if (ch.DataType == typeof(short))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    var v = (short)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                else if (ch.DataType == typeof(ushort))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    var v = (ushort)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                else if (ch.DataType == typeof(double))    // ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                {
                                    var d = (double)ch.Value;
                                    d = (d - ch.D) / ch.K;
                                    if (ch.DeviceDataType == ModbusDeviceDataType.Int16)
                                    {
                                        var s = (short)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                    else
                                    {
                                        var s = (ushort)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                }
                                else
                                {
//                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                                    convOk = false;
                                }
                                if (convOk)
                                {
                                    if (ch.ConversionType == ModbusConversionType.SwapBytes)
                                    {
                                        var tmp = adr[0]; adr[0] = adr[1]; adr[1] = tmp;
                                    }
                                    master.WriteSingleRegister(ch.SlaveId, ch.DataAddress, BitConverter.ToUInt16(adr, 0));
                                }
                                break;
                            case ModbusDeviceDataType.Int32:
                            case ModbusDeviceDataType.UInt32:
                            case ModbusDeviceDataType.Single:
                                if (ch.DataType == typeof(int))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    var v = (int)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                else if (ch.DataType == typeof(uint))     // ch.ModbusFs2InternalType == ModbusFs2InternalType.Int32)
                                {
                                    var v = (uint)ch.Value;
                                    adr = BitConverter.GetBytes(v);
                                }
                                else if (ch.DataType == typeof(double))    // ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                                {
                                    var d = (double)ch.Value;
                                    d = (d - ch.D) / ch.K;
                                    if (ch.DeviceDataType == ModbusDeviceDataType.Int32)
                                    {
                                        var s = (int)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                    else if (ch.DeviceDataType == ModbusDeviceDataType.UInt32)
                                    {
                                        var s = (uint)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                    else
                                    {
                                        //float
                                        var s = (float)d;
                                        adr = BitConverter.GetBytes(s);
                                    }
                                }
                                else
                                {
//                                    if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                        Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                                    convOk = false;
                                }
                                if (convOk)
                                {
                                    SwapBytesOut(adr, out var adr0, out var adr1, ch.ConversionType);
                                    var registers = new ushort[] { BitConverter.ToUInt16(adr0, 0), BitConverter.ToUInt16(adr1, 0) };
                                    master.WriteMultipleRegisters(ch.SlaveId, ch.DataAddress, registers);
                                }
                                break;
                            case ModbusDeviceDataType.String:
                            case ModbusDeviceDataType.Boolean:
//                                if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvertImpl,
//                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                                break;
                        }
                        break;
                    case ModbusDataTypeEx.Coil:
                        if (ch.DataType == typeof(bool))
                        {
                            master.WriteSingleCoil(ch.SlaveId, ch.DataAddress, (bool)ch.Value);
                        }
                        else
                        {
//                            if (self.LoggingLevel >= ModbusLog.logWarnings)
//                                Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrConvert,
//                                        self.Name, ch.Name, ch.ModbusFs2InternalType.ToString(), ch.DeviceDataType.ToString()));
                        }
                        break;
                }
            }
            catch (SlaveException )
            {
//                if (self.LoggingLevel >= ModbusLog.logWarnings)
//                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
//                        self.Name, e.Message));
            }
            catch (OverflowException )
            {
//                if (self.LoggingLevel >= ModbusLog.logWarnings)
//                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
//                        self.Name, e.Message));
            }
            catch (TimeoutException )
            {
//                if (self.LoggingLevel >= ModbusLog.logWarnings)
//                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
//                        self.Name, e.Message));
            }
            catch (InvalidCastException )
            {
//                if (self.LoggingLevel >= ModbusLog.logWarnings)
//                    Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException,
//                        self.Name, e.Message));
            }
        }

        private byte[] SwapBytesIn(byte[] adr0, byte[] adr1, ModbusConversionType con)
        {
            var res = new byte[4];
            switch (con)
            {
                case ModbusConversionType.SwapBytes:
                    res[0] = adr1[1];
                    res[1] = adr1[0];
                    res[2] = adr0[1];
                    res[3] = adr0[0];
                    break;
                case ModbusConversionType.SwapWords:
                    res[0] = adr0[0];
                    res[1] = adr0[1];
                    res[2] = adr1[0];
                    res[3] = adr1[1];
                    break;
                case ModbusConversionType.SwapAll:
                    res[0] = adr0[1];
                    res[1] = adr0[0];
                    res[2] = adr1[1];
                    res[3] = adr1[0];
                    break;
                case ModbusConversionType.SwapNone:
                default:
                    res[0] = adr1[0];
                    res[1] = adr1[1];
                    res[2] = adr0[0];
                    res[3] = adr0[1];
                    break;
            }
            return res;
        }

        private void SwapBytesOut(byte[] inp, out byte[] adr0, out byte[] adr1, ModbusConversionType con)
        {
            adr0 = new byte[2];
            adr1 = new byte[2];
            switch (con)
            {
                case ModbusConversionType.SwapBytes:
                    adr1[1] = inp[0];
                    adr1[0] = inp[1];
                    adr0[1] = inp[2];
                    adr0[0] = inp[3];
                    break;
                case ModbusConversionType.SwapWords:
                    adr0[0] = inp[0];
                    adr0[1] = inp[1];
                    adr1[0] = inp[2];
                    adr1[1] = inp[3];
                    break;
                case ModbusConversionType.SwapAll:
                    adr0[1] = inp[0];
                    adr0[0] = inp[1];
                    adr1[1] = inp[2];
                    adr1[0] = inp[3];
                    break;
                case ModbusConversionType.SwapNone:
                default:
                    adr1[0] = inp[0];
                    adr1[1] = inp[1];
                    adr0[0] = inp[2];
                    adr0[1] = inp[3];
                    break;
            }
        }
    }
}
