using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using Core;
using Core.Collections.Generic;
using Core.Serialization;

namespace Services.Modbus
{
    [Serializable]
    public class ModbusService : IDisposable
    {
        public ObservableCollection<IPoint> Points { get; set; } = new ObservableCollection<IPoint>();
        public ObservableCollection<IModbusStation> Stations { get; set; } = new ObservableCollection<IModbusStation>();
        private string Name => nameof(ModbusService);
        private string PluginId => nameof(ModbusService);
        private bool ServiceFlag { get; set; }

        private event EventHandler PointsChanged;

        private IPoint[] ModbusPoints
        {
            get => Points.ToArray();
            set
            {
                Points.Clear();
                Points.AddRange(value);
                Points.RemoveWhere(ch => ch == null);
                PointsChanged?.Invoke(this, new EventArgs());
            }
        }

        public IModbusStation[] ModbusStations
        { 
            get => Stations.ToArray();
            set
            {
                Stations.Clear();
                Stations.AddRange(value);
                Stations.RemoveWhere(st => st == null);
            }
        }

        public void Initialize()
        {
            //LoadSettings();       

            ModbusSerialClientStation serialClientStation = new ModbusSerialClientStation("station1","com1",300,300,3,3);
            ModbusTcpClientStation tcpClientStation = new ModbusTcpClientStation("station2","127.0.0.1",8081,300,300,3,3);

            Stations.Add(serialClientStation);
            //Stations.Add(tcpClientStation);

            ModbusPoint modbusPoint = new ModbusPoint
            {                             
                Station = "RcpStation1",
                SlaveId = 1,
                DataTypeEx = ModbusDataTypeEx.Coil,
                DataAddress = 0x0001,
                DataLen = 10,
                ConversionType = ModbusConversionType.SwapAll,
                DeviceDataType = ModbusDeviceDataType.Int32                
            };

            ModbusPoint modbusPoint2 = new ModbusPoint
            {
                Station = "RcpStation2",
                SlaveId = 2,
                DataTypeEx = ModbusDataTypeEx.Coil,
                DataAddress = 0x0001,
                DataLen = 2,
                ConversionType = ModbusConversionType.SwapAll,
                DeviceDataType = ModbusDeviceDataType.Int32
            };

            Points.Add(modbusPoint);
            Points.Add(modbusPoint2);

            this.SerializeToXamlFile(@"B:\aa.xaml");

        }
        public bool StartServer()
        {
            if (ServiceFlag)
                return false;

            GC.Collect();

            if (Stations?.Count > 0)
            {
                foreach (var station in Stations)
                    station.Start();
            }


            if (Points?.Count > 0)
            {
                Points.CollectionChanged += Points_CollectionChanged;
            }

            return ServiceFlag = true;
        }

        private void Points_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            throw new NotImplementedException();
        }

        public void StopServer()
        {
            if (!ServiceFlag)
                return;

            Task.WaitAll(Stations.Select(station => Task.Run(() => { station.Stop(); })).ToArray(), 10000);
            
            GC.Collect();

            ServiceFlag = false;
        }
         
        public void Dispose()
        {
            foreach (var station in Stations)
                station.Stop();
        }        
    }
}
 