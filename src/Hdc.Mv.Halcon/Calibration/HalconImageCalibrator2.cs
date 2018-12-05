using System;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Calibration
{
    [Serializable]
    public class HalconImageCalibrator2 : IHalconImageCalibrator
    {
        public int NewImageWidth { get; set; } = 0;
        public int NewImageHeight { get; set; } = 0;

        public string CameraParamsFileName { get; set; }
        public string CameraPoseFileName { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double Rotate { get; set; }

        public int CropX { get; set; }
        public int CropY { get; set; }
        public int CropWidth { get; set; }
        public int CropHeight { get; set; }
        public bool MirrorRow { get; set; }
        public bool MirrorColumn { get; set; }

        public Interpolation Interpolation { get; set; }

        public double PixelCellSideLengthInMillimeter { get; set; }

        // ReSharper disable InconsistentNaming
        public bool ImageCacheSaveToFile_Original_Enabled { get; set; }
        public string ImageCacheSaveToFile_Original_FileName { get; set; }
        public bool ImageCacheSaveToFile_Crop_Enabled { get; set; }
        public string ImageCacheSaveToFile_Crop_FileName { get; set; }
        public bool ImageCacheSaveToFile_Calibrate_Enabled { get; set; }
        public string ImageCacheSaveToFile_Calibrate_FileName { get; set; }
        public bool ImageCacheSaveToFile_Mirror_Enabled { get; set; }
        public string ImageCacheSaveToFile_Mirror_FileName { get; set; }

        // ReSharper restore InconsistentNaming

        public HImage Calibrate(HImage image)
        {
            if (ImageCacheSaveToFile_Original_Enabled)
                image.WriteImageOfTiffLzw(ImageCacheSaveToFile_Original_FileName);

            //
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


            string halconString = Interpolation.ToHalconString();

            if (!(Math.Abs(PixelCellSideLengthInMillimeter - (0)) < 0.0000001))
            {
                lengthPerPixelY = PixelCellSideLengthInMillimeter / 1000000.0;
            }

            int widthL, heightL;
            if (NewImageWidth > 0 && NewImageHeight > 0)
            {
                widthL = NewImageWidth;
                heightL = NewImageHeight;
            }
            else
            {
                widthL = hv_Width;
                heightL = hv_Height;
            }
            HImage calibrateImage = HalconImageCalibrator.CalibrateImage(image, hv_CameraParams, hv_PoseNewOrigin, widthL, heightL,
                lengthPerPixelY, halconString);
            if (ImageCacheSaveToFile_Calibrate_Enabled)
                calibrateImage.WriteImageOfTiffLzw(ImageCacheSaveToFile_Calibrate_FileName);

            //


            // crop image
            HImage cropImage = calibrateImage.MirrorAndCrop(false, false, CropX, CropY, CropWidth, CropHeight);
            if (ImageCacheSaveToFile_Crop_Enabled)
                cropImage.WriteImageOfTiffLzw(ImageCacheSaveToFile_Crop_FileName);


            // mirror
            var mirrorImage = cropImage.MirrorAndCrop(MirrorRow, MirrorColumn, 0, 0, CropWidth, CropHeight);
            if (ImageCacheSaveToFile_Mirror_Enabled)
                mirrorImage.WriteImageOfTiffLzw(ImageCacheSaveToFile_Mirror_FileName);

            calibrateImage.Dispose();
            cropImage.Dispose();

            return mirrorImage;
        }

    }
}