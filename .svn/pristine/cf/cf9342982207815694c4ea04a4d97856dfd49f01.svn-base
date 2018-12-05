using System;
using System.Windows.Markup;
using System.Collections.Generic;

namespace Core.Services
{
    [Serializable]
    [ContentProperty("PluginObjects")]
    public class PluginService : IDisposable
    {        
        private const string OpenmoduleFunc  = "Openmodule";       
        private const string StartServerFunc = "StartServer";        
        private const string StopServerFunc  = "StopServer";
        
        private string _error;

        public List<PluginObject> PluginObjects = new List<PluginObject>();
        public List<string> PluginDirectorys = new List<string>();

        #region IDisposable 成员
        public void Dispose()
        {
            // TODO:  添加 ModulePlus.Dispose 实现

        }
        #endregion

        //Get Error
        public string GetLastError()
        {
            return _error;
        }

        public bool StartPluginService(string pluginPath = "")
        {
            if(!string.IsNullOrEmpty(pluginPath))
                PluginDirectorys.Add(pluginPath);

            foreach (var pluginObject in PluginObjects)
            {
                //StartServer(pluginObject,
            }

            return true;
        }

        public bool ModuleRun(PluginObject plugin, object gd, object param)
        {
            var obj = new object[2] { gd, param };

            try
            {
                return (bool)plugin.Invoke(OpenmoduleFunc, obj);
            }
            catch (Exception e)
            {
                _error = e.Message;
                return false;
            }
        }

        public bool StartServer(PluginObject plugin, object gd, object param)
        {
            var obj = new object[2] { gd, param };

            try
            {
                return (bool)plugin.Invoke(StartServerFunc, obj);
            }
            catch (Exception e)
            {
                _error = e.Message;
                return false;
            }
        }

        public bool StopServer(PluginObject plugin)
        {
            try
            {
                plugin.Invoke(StopServerFunc, null);

                return true;
            }
            catch (Exception e)
            {
                _error = e.Message;
                return false;
            }
        }

    }
}
