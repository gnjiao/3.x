using System;
using System.IO.Ports;
using System.Xml;

namespace Services.Modbus
{
    sealed class StationFactory
    {
        //Prevent class reation
        private StationFactory() { }

        public static IModbusStation CreateStation(XmlElement node)
        {
            IModbusStation ist = null;
            string name = node.Attributes["name"].Value;
            string type;
            try { type = node.Attributes["type"].Value; }
            catch { type = "ModbusTCPClientStation"; }
            int cycleTimeout = 100;
            try { cycleTimeout = int.Parse(node.Attributes["cycleTimeout"].Value); }    // Backward compatibility
            catch { };
            int retryTimeout = 1000;
            try { retryTimeout = int.Parse(node.Attributes["retryTimeout"].Value); }
            catch { };
            int retryCount = 3;
            try { retryCount = int.Parse(node.Attributes["retryCount"].Value); }
            catch { };
            int failedCount = 20;
            try { failedCount = int.Parse(node.Attributes["failedCount"].Value); }
            catch { };
            int loggingLevel = 0;
            try { loggingLevel = int.Parse(node.Attributes["loggingLevel"].Value); }
            catch { };
            bool stationActive = true;
            try { stationActive = bool.Parse(node.Attributes["stationActive"].Value); }
            catch { };
            switch (type)
            {
                case "ModbusTCPClientStation":
                    string ipAddress = node.Attributes["ipAddress"].Value;
                    int tcpPort = int.Parse(node.Attributes["tcpPort"].Value);
                    ist = CreateTcpClientStation(name, ipAddress, tcpPort, cycleTimeout, retryTimeout, retryCount, failedCount);
                    break;
                case "ModbusSerialClientStation":
                    string comPort = node.Attributes["comPort"].Value;
                    ist = CreateSerialClientStation(name, comPort, cycleTimeout, retryTimeout, retryCount, failedCount);
                    try { ((ModbusSerialClientStation) ist).BaudRate = int.Parse(node.Attributes["baudRate"].Value); }
                    catch { }
                    try { ((ModbusSerialClientStation) ist).DataBits = int.Parse(node.Attributes["dataBits"].Value); }
                    catch { }
                    try { ((ModbusSerialClientStation) ist).SerialType = (ModbusSerialType)Enum.Parse(typeof(ModbusSerialType), node.Attributes["serialType"].Value); }
                    catch { }
                    try { ((ModbusSerialClientStation) ist).StopBits = (StopBits)Enum.Parse(typeof(StopBits), node.Attributes["stopBits"].Value); }
                    catch { }
                    try { ((ModbusSerialClientStation) ist).Parity = (Parity)Enum.Parse(typeof(Parity), node.Attributes["parity"].Value); }
                    catch { }
                    try { ((ModbusSerialClientStation) ist).Handshake = (Handshake)Enum.Parse(typeof(Handshake), node.Attributes["handshake"].Value); }
                    catch { }
                    break;
            }
            ist.LoggingLevel = loggingLevel;
            ist.StationActive = stationActive;
            return ist;
        }

        public static IModbusStation CreateTcpClientStation(string name, string ipAddress, int tcpPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            return new ModbusTcpClientStation(name, ipAddress, tcpPort, cycleTimeout, retryTimeout, retryCount, failedCount);
        }

        public static IModbusStation CreateSerialClientStation(string name, string comPort, int cycleTimeout, int retryTimeout, int retryCount, int failedCount)
        {
            return new ModbusSerialClientStation(name, comPort, cycleTimeout, retryTimeout, retryCount, failedCount);
        }

        public static void SaveStation(XmlElement node, IModbusStation stat)
        {
            node.SetAttribute("name", stat.Name);
            if (stat is ModbusTcpClientStation)
            {
                ModbusTcpClientStation tcpstat = (ModbusTcpClientStation)stat;
                node.SetAttribute("type", tcpstat.GetType().Name);
                node.SetAttribute("ipAddress", tcpstat.IpAddress);
                node.SetAttribute("tcpPort", tcpstat.TcpPort.ToString());
            }
            if (stat is ModbusSerialClientStation)
            {
                ModbusSerialClientStation serstat = (ModbusSerialClientStation)stat;
                node.SetAttribute("type", serstat.GetType().Name);
                node.SetAttribute("comPort", serstat.ComPort);
                node.SetAttribute("serialType", serstat.SerialType.ToString());
                node.SetAttribute("baudRate", serstat.BaudRate.ToString());
                node.SetAttribute("dataBits", serstat.DataBits.ToString());
                node.SetAttribute("parity", serstat.Parity.ToString());
                node.SetAttribute("stopBits", serstat.StopBits.ToString());
                node.SetAttribute("handshake", serstat.Handshake.ToString());
            }
            node.SetAttribute("cycleTimeout", stat.CycleTimeout.ToString());
            node.SetAttribute("retryTimeout", stat.RetryTimeout.ToString());
            node.SetAttribute("retryCount", stat.RetryCount.ToString());
            node.SetAttribute("failedCount", stat.FailedCount.ToString());
            node.SetAttribute("loggingLevel", stat.LoggingLevel.ToString());
            node.SetAttribute("stationActive", stat.StationActive.ToString());
        }
    }
}
