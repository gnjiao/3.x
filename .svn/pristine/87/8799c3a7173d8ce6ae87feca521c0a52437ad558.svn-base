using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modbus
{
    public interface IModbusStation
    {
        string Name { get; }
        void Stop();
        int Start();
        void ClearChannels();
        void AddChannel(ModbusPoint channel);
        int CycleTimeout { get; set; }
        int RetryTimeout { get; set; }
        int RetryCount { get; set; }
        int FailedCount { get; set; }
        int LoggingLevel { get; set; }
        bool StationActive { get; set; }
    }
}
