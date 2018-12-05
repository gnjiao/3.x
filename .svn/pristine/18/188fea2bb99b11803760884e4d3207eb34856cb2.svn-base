namespace Core.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class NotifyStopwatch : Stopwatch, IDisposable
    {
        private readonly string _message;

        public NotifyStopwatch(string message)
        {
            _message = message;
            Debug.WriteLine("<< Watch Started >> " + _message + ", Begin:: " + DateTime.Now.ToLongTimeString());
            string RecordBeforeStart = "<< Watch Started >> " + _message + ", Begin:: " + DateTime.Now.ToLongTimeString();
            Console.WriteLine("{0}", RecordBeforeStart);
            Start();
        }

        public void Dispose()
        {
            Stop();
            Debug.WriteLine("<< Watch Stoped  >> " + _message 
                + ", End  :: " + DateTime.Now.ToLongTimeString() 
                + ", Elapsed: " + Elapsed.TotalMilliseconds.ToString());
            string RecordSthAfterStop = "<< Watch Stoped  >> " + _message
                + ", End  :: " + DateTime.Now.ToLongTimeString()
                + ", Elapsed: " + Elapsed.TotalMilliseconds.ToString();
            Console.WriteLine("{0}", RecordSthAfterStop);
        }
    }
}