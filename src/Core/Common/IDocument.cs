using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public enum ProjectEntityType
    {
        Schema,
        Channel,
        Image,
        Script,
        Trend,
        Report,
        EventList,
        AlarmList,
        VariableList,
        Archiver,
        Settings
    }
    public interface IDocument
    {
        object Content { get; }
        object Load(string schemaName);
        string Name { get; }
        void Save(string name, object content);
        void Save(string name);
        ProjectEntityType Type { get; }
    }
}
