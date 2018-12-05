using System.Collections.Generic;
using System.Linq;
using System.Windows;
using HalconDotNet;
using Hdc.Mv.RobotVision;

namespace Hdc.Mv.Halcon
{
    public class HHomConverter
    {
        private readonly Vector _calibToolInBaseVector;
        private readonly HHomMat2D _pToQMat;
        private readonly HHomMat2D _qToPMat;

        public HHomConverter(HandEyeCalibrationSchema calibrationSchema) :
                this(calibrationSchema.GetCameraVectors(),
                    calibrationSchema.GetRobotVectors(),
                    calibrationSchema.CalibToolInBaseVector)
        {
        }

        public HHomConverter(IEnumerable<Vector> objInCamVectors, IEnumerable<Vector> objInBaseVectors,
            Vector calibToolInBaseVector)
        {
            _calibToolInBaseVector = calibToolInBaseVector;

            var inCamVectors = objInCamVectors as IList<Vector> ?? objInCamVectors.ToList();
            var inBaseVectors = objInBaseVectors as IList<Vector> ?? objInBaseVectors.ToList();

            var px = inCamVectors.Select(x => x.X).ToArray();
            var py = inCamVectors.Select(x => x.Y).ToArray();

            var qx = inBaseVectors.Select(x => x.X).ToArray();
            var qy = inBaseVectors.Select(x => x.Y).ToArray();

            _pToQMat = new HHomMat2D();
            _pToQMat.VectorToHomMat2d(px, py, qx, qy);

            _qToPMat = new HHomMat2D();
            _qToPMat.VectorToHomMat2d(qx, qy, px, py);
        }

        public Vector ConvertToBase(Vector objInCam, Vector toolInBaseVector)
        {
            var offsetInBase = toolInBaseVector - _calibToolInBaseVector;

            var objInBase = _pToQMat.AffineTransPoint2d(objInCam);

            var vectorX = objInBase + offsetInBase;
            return vectorX;
        }
    }
}