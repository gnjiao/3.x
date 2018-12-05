using System;
using System.IO.Ports;
using System.Threading;
using Hdc.Mv.ImageAcquisition;

namespace Hdc.Mv
{
    [Serializable]
    public class E2VChangeScanDirectionInitializer : IFrameGrabberInitializer
    {
        private SerialPort _port;

        public void Dispose()
        {
            _port.Close();
            _port.Dispose();
        }

        public void Initialize()
        {
            _port = new SerialPort(PortName, BaudRate, Parity.None, 8, StopBits.One);
            _port.Open();

            if (!Port.IsOpen)
                Port.Open();

            Port.Write("w scdi 0");
            Port.Write(new byte[] { 13 }, 0, 1);
//            Thread.Sleep(500);
        }

        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public SerialPort Port
        {
            get { return _port; }
        }
    }
}