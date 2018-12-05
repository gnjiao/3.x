using System;

namespace Hdc.Mv.Halcon
{
    public interface IBlock//: IDisposable
    {
        void Initialize();

        void Uninitialize();

        void Process();

        BlockStatus Status { get; }

        string Name { get; set; }        

        string Message { get; set; }

        Exception Exception { get; }
    }
}