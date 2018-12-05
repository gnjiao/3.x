using System;
using System.ComponentModel;

namespace Core
{
    public enum PointStatusFlags
    {
        Unknown = 0,
        Good = 1,
        Bad = 2,
        NotUsed = 0xFFFF
    }

    public enum ReadWriteFlags
    {
        ReadOnly = 0,
        WriteOnly = 1,
        ReadWrite = 2
    }

    public interface IPoint : INotifyPropertyChanged
    {
        event EventHandler ValueChanged;

        string Name { get; set; }
        string Id { get; set; }
        string Device { get; set; }
        string FullName { get; }
        Type DataType { get; set; }
        ReadWriteFlags ReadWrite { get; set; }
        object Value { get; set; }
        object DefaultValue { get; set; }
        object Ctrl { get; set; }
        DateTime ModifyTime { get; }
        string Status { get; }
        PointStatusFlags StatusFlags { get; set; }
        object Tag { get; set; }
    }
}
