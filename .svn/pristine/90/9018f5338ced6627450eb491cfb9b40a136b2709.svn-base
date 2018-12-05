using System;
using System.Net.Sockets;
using System.Threading;
using Core;
using Modbus.Device;

namespace Services.Modbus
{
    [Serializable]
    public class ModbusTcpClientStation : ModbusBaseClientStation, IModbusStation
    {
        public string IpAddress { get; set; }
        public int TcpPort { get; set; }

        private Thread _channelUpdaterThread;


        public ModbusTcpClientStation() : base()
        {            
        }

        public ModbusTcpClientStation(string name, string ipAddress, int tcpPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
            : base(name, cycleTimeout, retryTimeout, retryCount, failedCount)
        {
            this.IpAddress = ipAddress;
            this.TcpPort = tcpPort;
        }
        
        public new int Start()
        {
            if (!StationActive) return 1;
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
            if (!StationActive) return;
            base.Stop();

            if (_channelUpdaterThread == null) return;

            //channelUpdaterThread.Abort();
            _channelUpdaterThread.Join();
            _channelUpdaterThread = null;
        }

        private static void ChannelUpdaterThreadProc(object obj)
        {
            ModbusTcpClientStation self = null;
            try
            {
                self = (ModbusTcpClientStation)obj;
                self.RunThread = true;
                while (self.RunThread)
                {
                    try
                    {
                        foreach (var buf in self.Buffers)
                        {
                            buf.PauseCounter = 0;
                        }
//                        if (self.LoggingLevel >= ModbusLog.logInfos)
//                            Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoTCPStarting, self.Name, self.ipAddress, self.tcpPort));
                        using (var client = new TcpClient(self.IpAddress, self.TcpPort))
                        {
//                            if (self.LoggingLevel >= ModbusLog.logInfos)
//                                Env.Current.Logger.LogInfo(string.Format(StringConstants.InfoTCPStarted, self.Name, self.ipAddress, self.tcpPort));
                            var master = ModbusIpMaster.CreateIp(client);
                            master.Transport.Retries = self.RetryCount;
                            master.Transport.WaitToRetryMilliseconds = self.RetryTimeout;
                            // Before a new TCP connection is used, delete all data in send queue
                            lock (self.SendQueueSyncRoot)
                            {
                                self.SendQueue.Clear();
                            }

                            while (self.RunThread)
                            {
                                // READING -------------------------------------------------------- //
                                foreach (var buf in self.Buffers)
                                {
                                    // Read an actual Buffer first
                                    self.ReadBuffer(self, master, buf);
                                }   // Foreach buffer

                                // WRITING -------------------------------------------------------- //
                                // This implementation causes new reading cycle after writing
                                // anything to MODBUS
                                // The sending strategy should be also considered and enhanced, but:
                                // As I think for now ... it does not matter...
                                self.SendQueueEndWaitEvent.WaitOne(self.CycleTimeout);

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
                                {
                                    for (int i = self.ChannelsToSend.Count; i > 0; i--)
                                    {
                                        var ch = self.ChannelsToSend[i - 1];
                                        // One try ONLY
                                        self.ChannelsToSend.RemoveAt(i - 1);
                                        self.WriteChannel(self, master, ch);
                                    }
                                }
                            }   // while runThread
                        }   // Using TCP client
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
                    }
                    // safety Sleep()
                    Thread.Sleep(5000);
                }  // while runThread
            }   // try
            catch (ThreadAbortException )
            {
//                if (((ModbusTCPClientStation)obj).LoggingLevel >= ModbusLog.logErrors)
//                    Env.Current.Logger.LogError(string.Format(StringConstants.ErrException, ((ModbusTCPClientStation)obj).Name, e.Message));
            }
            finally
            {
                if (self != null)
                    foreach (var ch in self.Channels)
                    {
                        if (ch.StatusFlags != PointStatusFlags.Unknown)
                            ch.StatusFlags = PointStatusFlags.Bad;
                    }
            }
        }
    }
}
