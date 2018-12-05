using System;
using System.Windows;

namespace Core.Windows
{
    public static class ComponentResourceKeyExtensions
    {
        public static ComponentResourceKey GetResourceKey(this object keyId, Type type)
        {
            return new ComponentResourceKey(type, keyId);
        }

        public static ComponentResourceKey GetResourceKey<T>(this object keyId)
        {
            return keyId.GetResourceKey(typeof (T));
        }
    }
}