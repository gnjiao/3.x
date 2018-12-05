using System;
using System.Threading.Tasks;
using System.Windows;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Calibration
{
    [Serializable]
    public class HalconImageCalibrator : IHalconImageCalibrator
    {
        public string CameraParamsFileName { get; set; }
        public string CameraPoseFileName { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double Rotate { get; set; }
        public bool PreHorizontalFilp { get; set; }
        public bool PreVerticalFilp { get; set; }
        public bool PostHorizontalFilp { get; set; }
        public bool PostVerticalFilp { get; set; }
        public Interpolation Interpolation { get; set; }
        public double PixelCellSideLengthInMillimeter { get; set; }

        public HImage Calibrate(HImage image)
        {
            double rotate = Rotate;
            HTuple lengthPerPixelX, lengthPerPixelY;

            // Local control variables 

            HTuple hv_CameraParams = null;
            //            HTuple hv_CameraPose = null;
            HPose hv_CameraPose = null;
            HTuple hv_Width = null, hv_Height = null, hv_OriginX = null;
            HTuple hv_OriginY = null, hv_XOffsetX = null, hv_XOffsetY = null;
            HTuple hv_YOffsetX = null, hv_YOffsetY = null, hv_OriginXActual = null;
            HTuple hv_OriginYActual = null, hv_XOffsetXActual = null;
            HTuple hv_XOffsetYActual = null, hv_YOffsetXActual = null;
            HTuple hv_YOffsetYActual = null, hv_OffsetX = null, hv_OffsetY = null;
#pragma warning disable 219
            HTuple hv_cut = null, hv_CameraPoseR = null, hv_PoseNewOrigin = null;
#pragma warning restore 219


            // Initialize local and output iconic variables 

            HOperatorSet.ReadCamPar(CameraParamsFileName, out hv_CameraParams);

            hv_CameraPose = new HPose();
            hv_CameraPose.ReadPose(CameraPoseFileName);

            HOperatorSet.GetImageSize(image, out hv_Width, out hv_Height);

            hv_OriginX = 100;
            hv_OriginY = 100;
            hv_XOffsetX = 101;
            hv_XOffsetY = 100;
            hv_YOffsetX = 100;
            hv_YOffsetY = 101;

            hv_OriginXActual = 0;
            hv_OriginYActual = 0;
            hv_XOffsetXActual = 0;
            hv_XOffsetYActual = 0;
            hv_YOffsetXActual = 0;
            hv_YOffsetYActual = 0;

            HOperatorSet.ImagePointsToWorldPlane(hv_CameraParams, hv_CameraPose, hv_OriginY,
                hv_OriginX, 1, out hv_OriginXActual, out hv_OriginYActual);
            HOperatorSet.ImagePointsToWorldPlane(hv_CameraParams, hv_CameraPose, hv_XOffsetY,
                hv_XOffsetX, 1, out hv_XOffsetXActual, out hv_XOffsetYActual);
            HOperatorSet.ImagePointsToWorldPlane(hv_CameraParams, hv_CameraPose, hv_YOffsetY,
                hv_YOffsetX, 1, out hv_YOffsetXActual, out hv_YOffsetYActual);
            HOperatorSet.DistancePp(hv_OriginYActual, hv_OriginXActual, hv_XOffsetYActual,
                hv_XOffsetXActual, out lengthPerPixelX);
            HOperatorSet.DistancePp(hv_OriginYActual, hv_OriginXActual, hv_YOffsetYActual,
                hv_YOffsetXActual, out lengthPerPixelY);

            hv_OffsetX = (hv_Width/2.0 + OffsetX)*lengthPerPixelX;
            hv_OffsetY = (hv_Height/2.0 + OffsetY)*lengthPerPixelY;

            //            hv_cut = ((hv_CameraPose.TupleSelect(5))).TupleInt();
            //            hv_cut = hv_CameraPose[5];
            //            hv_cut = rotate; // default is 90
            hv_CameraPoseR = hv_CameraPose.Clone();
            if (hv_CameraPoseR == null)
                hv_CameraPoseR = new HTuple();
            hv_CameraPoseR[5] = hv_CameraPose[5] - rotate;

            HOperatorSet.SetOriginPose(hv_CameraPoseR, -hv_OffsetX, -hv_OffsetY, 0, out hv_PoseNewOrigin);

            //set_origin_pose (PoseNewOrigin, 0, 0, 0, PoseNewOrigin1)

            // Pre Flip
            HImage oriImage = image;

            if (PreHorizontalFilp)
            {
                HImage mirrorImage = oriImage.MirrorImage("column");
                oriImage.Dispose();
                oriImage = mirrorImage;
            }

            if (PreVerticalFilp)
            {
                HImage mirrorImage = oriImage.MirrorImage("row");
                oriImage.Dispose();
                oriImage = mirrorImage;
            }
            string halconString = Interpolation.ToHalconString();

            if (!(Math.Abs(PixelCellSideLengthInMillimeter - (0)) < 0.0000001))
            {
                lengthPerPixelY = PixelCellSideLengthInMillimeter / 1000000.0;
            }

            HImage mirroredImage = CalibrateImage(oriImage, hv_CameraParams, hv_PoseNewOrigin, hv_Width, hv_Height,
                lengthPerPixelY, halconString);

          

            // Post Flip
            if (PostHorizontalFilp)
            {
                HImage mirrorImage = mirroredImage.MirrorImage("column");
                mirroredImage.Dispose();
                mirroredImage = mirrorImage;
            }

            if (PostVerticalFilp)
            {
                HImage mirrorImage = mirroredImage.MirrorImage("row");
                mirroredImage.Dispose();
                mirroredImage = mirrorImage;
            }

            return mirroredImage;
        }

        public  static HImage CalibrateImage(HImage oriImage, HTuple hv_CameraParams, HTuple hv_PoseNewOrigin,
            HTuple hv_Width,
            HTuple hv_Height, HTuple lengthPerPixelY, string halconString)
        {
            counter++;

            ActiveComputerDeviceInspectorInitializer.ComputeDevice.ActivateComputeDevice();
            Console.WriteLine("ActiveComputerDeviceInspectorInitializer.ComputeDevice.ActivateComputeDevice()");

            HObject hCalibImage = null;
            var sw = new NotifyStopwatch("ImageToWorldPlane, counter[" + counter + "], at " + DateTime.Now);

            try
            {
                HOperatorSet.ImageToWorldPlane(oriImage, out hCalibImage, hv_CameraParams,
                    hv_PoseNewOrigin, hv_Width, hv_Height, lengthPerPixelY, halconString);
                Console.WriteLine("ImageToWorldPlane Error Counter: " + imageToWorldPlaneErrorCounter);
            }
            catch (Exception e)
            {
                Console.WriteLine("ImageToWorldPlane exception: " + e.Message); 
                var hImage = new HImage();
                hImage.GenImageConst("byte", hv_Width, hv_Height);
                hCalibImage = hImage;
                imageToWorldPlaneErrorCounter++;

                if (imageToWorldPlaneErrorCounter > 5)
                    throw;
            }
            sw.Dispose();

            HComputeDevice.DeactivateAllComputeDevices();
            Console.WriteLine("HComputeDevice.DeactivateAllComputeDevices()");
            oriImage.Dispose();

            HImage calibedImage = new HImage(hCalibImage);
            hCalibImage.Dispose();
            return calibedImage;
        }

        private static int counter;
        private static int imageToWorldPlaneErrorCounter;

    }
}