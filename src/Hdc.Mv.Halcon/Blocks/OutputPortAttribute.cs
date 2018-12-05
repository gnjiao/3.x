using System;

namespace Hdc.Mv.Halcon
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputPortAttribute : Attribute
    {
        public OutputPortAttribute()
        {
        }

        public OutputPortAttribute(object key)
        {
            Key = key;
        }

        public object Key { get; set; }
    }
}