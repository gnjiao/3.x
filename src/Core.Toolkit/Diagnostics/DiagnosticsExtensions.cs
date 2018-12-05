using System.Diagnostics;
using System.Threading;

namespace Core.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

 

    public static class DiagnosticsExtensions
    {
        public static bool WaitUntilTrue(this Func<bool> func, int timeout, int timeToCheck)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds < timeout)
            {
                if (func())
                    return true;
                Thread.Sleep(timeToCheck);
            }
            return false;
        }
    }
}