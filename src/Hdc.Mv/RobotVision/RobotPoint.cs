using System;

namespace Hdc.Mv.RobotVision
{
    [Serializable]
    public class RobotPoint
    {
        public int Index { get; set; }

        public double BaseX { get; set; }
        public double BaseY { get; set; }
        public double BaseZ { get; set; }
        public double BaseU { get; set; }

        public double CamX { get; set; }
        public double CamY { get; set; }
        public double CamZ { get; set; }
        public double CamU { get; set; }

        public int ToolIndex { get; set; } // Tool Coordinate Index
        public int LocalIndex { get; set; } // Local Coordinate Index

        public RobotPoint()
        {
        }

        public RobotPoint(double baseX = 0, double baseY = 0, double baseZ = 0, double baseU = 0)
        {
            BaseX = baseX;
            BaseY = baseY;
            BaseZ = baseZ;
            BaseU = baseU;
        }

        public bool UsePointInRobot_Enabled { get; set; }

        public int UsePointInRobot_PointIndex { get; set; }

        /// <summary>
        /// millisecondsTimeout
        /// </summary>
        public int Timeout { get; set; } = -1;

        /// <summary>
        /// disable sensor in position event
        /// </summary>
        public bool DisableSensorInPositionEvent { get; set; }

        public RobotMoveType RobotMoveType { get; set; }

        public int SpeedBefore { get; set; }
        public int SpeedAfter { get; set; }
        public int AccelBefore { get; set; }
        public int AccelAfter { get; set; }
    }
}