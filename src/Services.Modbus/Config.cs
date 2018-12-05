using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Modbus
{
    [Serializable]
    public class Config 
    {       
        public Config()
        {
            SerialPort port = new SerialPort();            
        }

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public string Parity { get; set; }
        public string Handshake { get; set; }
        public string Encoding { get; set; }
        public int ReadBufferSize { get; set; } = 4096;
        public int ReadTimeout { get; set; } = 2000;
        public int WriteBufferSize { get; set; } = 4096;
        public int WriteTimeout { get; set; } = 2000;
    }
}
