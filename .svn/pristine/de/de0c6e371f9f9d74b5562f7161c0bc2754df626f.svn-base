using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Hdc.Mv.RobotVision
{
    [Serializable]
    [ContentProperty("RobotPoints")]
    public class HandEyeCalibrationSchema
    {
        public Collection<RobotPoint>  RobotPoints { get; set; } = new Collection<RobotPoint>();

        public Vector CalibToolInBaseVector { get; set; }
        public Vector OriToolInBaseVector { get; set; }
        public Vector RefToolInBaseVector { get; set; }
        public Vector OriInCamVector { get; set; }
        public Vector RefInCamVector { get; set; }
        public Vector OriInBaseVector { get; set; }
        public Vector RefInBaseVector { get; set; }
        public Vector CenterInBaseVector { get; set; }

        public IList<Vector> GetRobotVectors()
        {
            return RobotPoints.Select(x => new Vector(x.BaseX, x.BaseY)).ToList();
        }

        public IList<Vector> GetCameraVectors()
        {
            return RobotPoints.Select(x => new Vector(x.CamX, x.CamY)).ToList();
        }
    }
}