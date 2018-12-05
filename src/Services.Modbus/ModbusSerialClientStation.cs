using System;
using System.IO.Ports;
using System.Threading;
using Modbus.Device;

namespace Services.Modbus
{
    [Serializable]
    public class ModbusSerialClientStation : ModbusBaseClientStation, IModbusStation
    {
        public string ComPort { get; set; }
        public ModbusSerialType SerialType { get; set; } = ModbusSerialType.Rtu;
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Parity Parity { get; set; } = Parity.None;
        public Handshake Handshake { get; set; } = Handshake.None;

        private Thread _channelUpdaterThread;

        public ModbusSerialClientStation(string name, string comPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
            : base(name, cycleTimeout, retryTimeout, retryCount, failedCount)
        {
            this.ComPort = comPort;
            //StationActive = false;
        }

        public ModbusSerialClientStation() : base()
        {            
        }


        public new int Start()
        {
            if (base.Start() == 0)
            {
                //// Run Thread
                _channelUpdaterThread = new Thread(new ParameterizedThreadStart(ChannelUpdaterThreadProc));
                _channelUpdaterThread.Start(this);
                return 0;
            }
            else
                return 1;
        }

        public new void Stop()
        {
            base.Stop();

            if (_channelUpdaterThread == null) return;
            //channelUpdaterThread.Abort();

            SendQueueEndWaitEvent.Set();
            _channelUpdaterThread.Join();
            _channelUpdaterThread = null;
        }

        private static void ChannelUpdaterThreadProc(object obj)
        {
            try
            {
                SerialPort sport = null;
                var self = (ModbusSerialClientStation)obj;
                self.RunThread = true;

                while (self.RunThread)
                {
                    try
                    {
                        foreach (ModbusBuffer buf in self.Buffers)
                        {
                            buf.PauseCounter = 0;
                        }
//                        if (self.LoggingLevel >= ModbusLog.logInfos)
//                            Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoSerialStarting,
//                                self.Name, self.ComPort, self.BaudRate, self.Parity, self.DataBits, self.StopBits));

                        //using (SerialPort sport = new SerialPort(self.comPort, self.baudRate, self.parity, self.dataBits, self.stopBits))
                        sport = new SerialPort(self.ComPort, self.BaudRate, self.Parity, self.DataBits, self.StopBits);
                        {
                            sport.Handshake = self.Handshake;
                            sport.Open();

//                            if (self.LoggingLevel >= ModbusLog.logInfos)
//                                Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoSerialStarted,
//                                    self.Name, self.ComPort, self.BaudRate, self.Parity, self.DataBits, self.StopBits));
                            var master = self.SerialType == ModbusSerialType.Ascii ? ModbusSerialMaster.CreateAscii(sport) : ModbusSerialMaster.CreateRtu(sport);
                            master.Transport.Retries = self.RetryCount;
                            master.Transport.WaitToRetryMilliseconds = self.RetryTimeout;

                            while (self.RunThread)
                            {
                                // READING
                                foreach (var buf in self.Buffers)
                                {
                                    // Read an actual Buffer first
                                    self.ReadBuffer(self, master, buf);
                                }   // Foreach buffer

                                // WRITING
                                // This implementation causes new reading cycle after writing
                                // anything to MODBUS
                                // The sending strategy should be also considered and enhanced, but:
                                // As I think for now ... it does not matter...
                                self.SendQueueEndWaitEvent.WaitOne(self.CycleTimeout);
                                self.SendQueueEndWaitEvent.Reset();

                                // fast action first - copy from the queue content to my own buffer
                                lock (self.SendQueueSyncRoot)
                                {
                                    if (self.SendQueue.Count > 0)
                                    {
                                        self.ChannelsToSend.Clear();
                                        while (self.SendQueue.Count > 0)
                                        {
                                            self.ChannelsToSend.Add(self.SendQueue.Dequeue());
                                        }
                                    }
                                }
                                // ... and the slow action last - writing to MODBUS
                                // NO optimization, each channel is written into its own MODBUS message
                                // and waited for an answer
                                if (self.ChannelsToSend.Count > 0)
                                    foreach (var ch in self.ChannelsToSend)
                                    {
                                        self.WriteChannel(self, master, ch);
                                    }
                            }   // for endless
                        }   // Using SerialPort
                    }   // Try
                    catch (Exception e)
                    {
                        self.SendQueueEndWaitEvent.Reset();

                        foreach (var b in self.Failures.Keys)
                        {
                            // All devices in failure
                            self.Failures[b].Value = true;
                        }
//                        if (self.LoggingLevel >= ModbusLog.logWarnings)
//                            Env.Current.Logger.LogWarning(string.Format(StringConstants.ErrException, self.Name, e.Message));
                        if (e is ThreadAbortException)
                            throw e;
                        // if (e is )   // Communication timeout to a device
                    }
                    finally
                    {
                        if (sport != null)
                        {
                            sport.Close();
                            sport.Dispose();
                        }
                        // safety Sleep()
                        Thread.Sleep(5000);
                    }
                }
                if (sport != null)
                {
                    sport.Close();
                    sport.Dispose();
                }
            }
            catch (ThreadAbortException )
            {
//                if (((ModbusSerialClientStation)obj).LoggingLevel >= ModbusLog.logErrors)
//                    Env.Current.Logger.LogError(string.Format(StringConstants.ErrException, ((ModbusSerialClientStation)obj).Name, e.Message));
            }
        }
    }
}
