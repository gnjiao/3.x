using System;
using System.Runtime.InteropServices;

namespace Core
{
    public static class IntPtrExtensions
    {
        public static void FreeHGlobal(this IntPtr intPtr)
        {
            Marshal.FreeHGlobal(intPtr);
        }
    }
}