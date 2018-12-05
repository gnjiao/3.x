using System;
using System.IO;
using System.IO.Compression;
using System.Xaml;
using System.Windows.Markup;
using System.Xml;

namespace Core.Serialization
{
    public static class XamlSerializationExtensions
    {
        public static void SerializeToXamlFile<T>(this T instance, string fileName)
        {
            XamlServices.Save(fileName, instance);
        }

        public static void SerializeToXamlFileOfZip<T>(this T instance, string fileName)
        {
            using (FileStream fs = File.OpenWrite(fileName))
            {
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress))
                {
                    XamlServices.Save(zipStream, instance);
                }
            }
        }

        public static T DeserializeFromXamlFile<T>(this string fileName)
        {
            return (T) XamlServices.Load(fileName);
        }

        public static T DeserializeFromXamlFileOfZip<T>(this string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Decompress))
                {
                    return (T)XamlServices.Load(zipStream);
                }
            }
        }

        public static T DeserializeFromXamlFileWithCheckIfZipped<T>(this string fileName)
        {
            if (fileName.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                return fileName.DeserializeFromXamlFileOfZip<T>();
            }

            return fileName.DeserializeFromXamlFile<T>();
        }

        public static object CloneUsingXamlSerialization(this object source)
        {
            if (source == null)
            {
                return null;
            }

            string s = System.Windows.Markup.XamlWriter.Save(source);

//            var stringReader = new StringReader(s);
//
//            XmlReader xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings());

            return System.Windows.Markup.XamlReader.Parse(s);
        }
    }
}