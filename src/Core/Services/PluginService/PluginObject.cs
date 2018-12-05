using System;
using System.IO;
using System.Reflection;

namespace Core.Services
{
    [Serializable]
    public class PluginObject
    {
        public string FullPluginPath
        {
            get => _filePath;
            set => _filePath = value;
        } 
        public int Index { set; get; } = -1;

        public string IdentifyName => $"{_className}.{_nameSpace}@{Index}";

        private readonly string _nameSpace;
        private readonly string _className;
        private string _filePath;

        private readonly Type _pluginType;
        private readonly object _instance;
        private readonly MethodInfo[] _methods;

        public PluginObject(string filepath)
        {
            _filePath = filepath;
            _nameSpace = Assembly.LoadFrom(filepath).GetName().Name;
            _className = Path.GetFileNameWithoutExtension(filepath);
            _pluginType = Assembly.LoadFrom(filepath).GetType($"{_nameSpace}.{_className}", true);
            _instance = Activator.CreateInstance(_pluginType);
            _methods = _pluginType.GetMethods();
        }

        public PluginObject(string filepath, string classname)
        {
            _filePath = filepath;
            _className = classname;
            _nameSpace = Assembly.LoadFrom(filepath).GetName().Name;            
            _pluginType = Assembly.LoadFrom(filepath).GetType($"{_nameSpace}.{_className}", true);
            _instance = Activator.CreateInstance(_pluginType);
            _methods = _pluginType.GetMethods();
        }

        public object Invoke(int methodNum, object[] parameters)
        {
            var mi = _methods[methodNum];

            return _instance != null && mi != null ? mi.Invoke(_instance, parameters) : null;
        }
        
        public object Invoke(string methodName, object[] parameters)
        {
            var mi = _pluginType.GetMethod(methodName);
            
            return _instance != null && mi != null ? mi.Invoke(_instance, parameters) : null;
        }

    }
}
