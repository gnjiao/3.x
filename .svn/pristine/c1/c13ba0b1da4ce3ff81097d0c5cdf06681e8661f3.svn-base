using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Core;
using Platform.Main.Util;

namespace Platform.Main.Services
{
    class PropertyService : PropertyServiceImpl
    {
        DirectoryName dataDirectory;
        DirectoryName configDirectory;
        FileName propertiesFileName;

        public PropertyService(DirectoryName configDirectory, DirectoryName dataDirectory, string propertiesName)
            : base(LoadPropertiesFromStream(configDirectory.CombineFile(propertiesName + ".xml")))
        {
            this.dataDirectory = dataDirectory;
            this.configDirectory = configDirectory;
            propertiesFileName = configDirectory.CombineFile(propertiesName + ".xml");
        }

        public override DirectoryName ConfigDirectory
        {
            get
            {
                return configDirectory;
            }
        }

        public override DirectoryName DataDirectory
        {
            get
            {
                return dataDirectory;
            }
        }
        static Core.Properties LoadPropertiesFromStream(FileName fileName)
        {
            if (!File.Exists(fileName))
            {
                return new Core.Properties();
            }
            try
            {
                using (LockPropertyFile())
                {
                    return Core.Properties.Load(fileName);
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine("Error loading properties: " + ex.Message + "\nSettings have been restored to default values.");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error loading properties: " + ex.Message + "\nSettings have been restored to default values.");
            }
            return new Core.Properties();
        }

        public override void Save()
        {
            using (LockPropertyFile())
            {
                this.MainPropertiesContainer.Save(propertiesFileName);
            }
        }

        /// <summary>
        /// Acquires an exclusive lock on the properties file so that it can be opened safely.
        /// </summary>
        static IDisposable LockPropertyFile()
        {
            Mutex mutex = new Mutex(false, "PropertyServiceSave-30F32619-F92D-4BC0-BF49-AA18BF4AC313");
            mutex.WaitOne();
            return new CallbackOnDispose(
                delegate {
                    mutex.ReleaseMutex();
                    mutex.Close();
                });
        }

        FileName GetExtraFileName(string key)
        {
            return configDirectory.CombineFile("preferences/" + key.GetStableHashCode().ToString("x8") + ".xml");
        }

        public override Core.Properties LoadExtraProperties(string key)
        {
            var fileName = GetExtraFileName(key);
            using (LockPropertyFile())
            {
                if (File.Exists(fileName))
                    return Core.Properties.Load(fileName);
                else
                    return new Core.Properties();
            }
        }

        public override void SaveExtraProperties(string key, Core.Properties p)
        {
            var fileName = GetExtraFileName(key);
            using (LockPropertyFile())
            {
                Directory.CreateDirectory(fileName.GetParentDirectory());
                p.Save(fileName);
            }
        }
    }
}
