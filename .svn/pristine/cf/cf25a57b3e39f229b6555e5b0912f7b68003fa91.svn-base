using System;
using System.Collections.ObjectModel;
using Core.Collections.Generic;

namespace Hdc.Mv
{
    [Serializable]
    public class AppConfig
    {
        private readonly Collection<string> _assemblyFileNames = new Collection<string>();

        public Collection<string> AssemblyFileNames
        {
            get { return _assemblyFileNames; }
        }

        public AppConfig()
        {
        }

        public AppConfig(params string[] assemblyFileNames)
        {
            _assemblyFileNames.AddRange(assemblyFileNames);
        }
    }
}