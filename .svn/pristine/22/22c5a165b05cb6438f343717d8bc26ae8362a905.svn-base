using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Core.Serialization
{
    public static class JsonSerializationExtensions
    {
        public static void SerializeObjectToJson<T>(this T t, string filename) where T : class
        {
            File.WriteAllText(filename,JsonConvert.SerializeObject(t));
        }

        public static T DeserializeObjectFromJson<T>(this string filename) where T : class
        {
            var serializer = new JsonSerializer();

            var sr = new StringReader(File.ReadAllText(filename));
            
            return (T)serializer.Deserialize(new JsonTextReader(sr), typeof(T));
        }

        public static T DeserializeJsonToObject<T>(this string json) where T : class
        {
            var serializer = new JsonSerializer();
            
            var sr = new StringReader(json);
            var o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            
            return o as T;
        }
        
        public static List<T> DeserializeJsonToList<T>(this string json) where T : class
        {
            var serializer = new JsonSerializer();
            var sr = new StringReader(json);
            var o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            
            return o as List<T>;
        }
        
        public static T DeserializeJsonAnonymousType<T>(this string json, T anonymousTypeObject) where T : class
        {
            var t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}
