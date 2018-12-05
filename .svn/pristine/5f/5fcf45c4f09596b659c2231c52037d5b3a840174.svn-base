using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IO;
using Core.Reflection;
using Core.Serialization;

namespace Services.Modbus
{
    public class ConfigProvider
    {
        static ConfigProvider()
        {
            var assemblyDirectoryPath = typeof(ConfigProvider).Assembly.GetAssemblyDirectoryPath();
#if Xaml            
            var configFileName = Path.Combine(AssemblyDirectoryPath, "Services.Modbus.Config.xaml");

            if (!configFileName.IsFileExist())
            {
                var newConfig = new Config();
                newConfig.SerializeToXamlFile(configFileName);
            }

            var config = configFileName.DeserializeFromXamlFile<Config>();
#endif

#if Json
            var configFileName = Path.Combine(assemblyDirectoryPath, "Services.Modbus.Config.Json");

            if (!configFileName.IsFileExist())
            {
                var newConfig = new Config();
                newConfig.SerializeObjectToJson(configFileName);
            }

            var config = configFileName.DeserializeObjectFromJson<Config>();

            Config = config;
#endif


        }

        public static Config Config { get; set; }

    }
}
