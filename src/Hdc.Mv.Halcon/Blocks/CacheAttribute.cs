using System;

namespace Hdc.Mv.Halcon
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CacheAttribute : Attribute
    {
        public CacheAttribute()
        {
        }

        public CacheAttribute(object key)
        {
            Key = key;
        }

        public object Key { get; set; }
    }
}