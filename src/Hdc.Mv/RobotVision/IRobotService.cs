using System;

namespace Hdc.Mv.RobotVision
{
    public interface IRobotService
    {
        void Init();
        void StartAutoCalib();
        IObservable<RobotPoint> RobotPointUpdatedEvent { get; }
        IObservable<bool> SessionChangedEvent { get; }
        RobotPoint GetHere();
        RobotPoint GetOriToolInBase();
        RobotPoint GetRefToolInBase();
        void GoOriToolInBase();
        void GoRefToolInBase();

        void GoHereXInc10mm();
        void GoHereXDec10mm();
        void GoHereXInc1mm();
        void GoHereXDec1mm();
        void GoHereXInc100um();
        void GoHereXDec100um();
        void GoHereXInc10um();
        void GoHereXDec10um();

        void GoHereYInc10mm();
        void GoHereYDec10mm();
        void GoHereYInc1mm();
        void GoHereYDec1mm();
        void GoHereYInc100um();
        void GoHereYDec100um();
        void GoHereYInc10um();
        void GoHereYDec10um();

        void OpenRobotManager();
    }
}