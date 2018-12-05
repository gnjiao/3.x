using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Core;
using HalconDotNet;
using Core.Collections.Generic;
using Core.Diagnostics;
using Hdc.Mv.Calibration;
using Hdc.Mv.Halcon;
using Hdc.Mv.Inspection;
using Core.Reflection;

// ReSharper disable InconsistentNaming

namespace Hdc.Mv
{
    [Serializable]
    public class HalconInspectionSchemaInspector : IHalconInspector
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string ChannelName { get; set; }
        public string FrameGrabberName { get; set; }
        private InspectionSchema _inspectionSchema;
        private InspectionController _inspectionController;

        public InspectionResult Inspect(HImage image)
        {
            _inspectionController = new InspectionController();

            InspectionResult inspectionResult;
#if !DEBUG
            try
            {
#endif
            Debug.WriteLine("_inspectionController.Inspect() start");
            var sw3 = new NotifyStopwatch("_inspectionController.Inspect()");
            var copyImage = image.CopyImage();
            var inspectionSchema = _inspectionSchema.DeepClone();

            _inspectionController.SetInspectionSchema(inspectionSchema);
            _inspectionController.SetImage(copyImage);
            _inspectionController.CreateCoordinate();
            _inspectionController.Inspect();

            Debug.WriteLine("_inspectionController.Inspect() OK");
            sw3.Dispose();
            copyImage.Dispose();
            inspectionResult = _inspectionController.InspectionResult;
            inspectionResult.InspectionSchema = _inspectionController.InspectionSchema;
            inspectionResult.Coordinate = _inspectionController.Coordinate;
            inspectionResult.Coordinates.AddRange(_inspectionController.Coordinates);

#if !DEBUG
            }
            catch (Exception e)
            {
//                MessageBox.Show("_inspectionController error.\n\n" + e.Message + "\n\n" +
//                                e.InnerException.Message);
                Console.WriteLine("_inspectionController error.\n\n" + e.Message + "\n\n" +
                                e.InnerException.Message);
                Debug.WriteLine("_inspectionController error.\n\n" + e.Message + "\n\n" +
                                e.InnerException.Message);

                inspectionResult = new InspectionResult();
            }
#endif

            return inspectionResult;
        }

        public int MaxDefectCount { get; set; }
        public string InspectionSchemaDir { get; set; }

        public InspectionController InspectionController
        {
            get { return _inspectionController; }
        }

        public InspectionSchema InspectionSchema
        {
            get { return _inspectionSchema; }
        }

        public void UpdateInspectionSchemaFromDir()
        {
            var assmDir = typeof(InspectionController).Assembly.GetAssemblyDirectoryPath();
            var dir = Path.Combine(assmDir, InspectionSchemaDir);

            _inspectionSchema = dir.GetInspectionSchemaFromDir();
        }
    }

    // ReSharper restore InconsistentNaming
}