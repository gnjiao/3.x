using System;
using System.Security.RightsManagement;
using System.Windows;
using HalconDotNet;
using Core.Diagnostics;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Hdc.Mv.Halcon;
using Hdc.Mv.RobotVision;


namespace Hdc.Mv.Calibration
{
    [Serializable]

    //此函数作用将输入的图像使用image_to_world_plane 转化到世界坐标系
    public class ImageToWorldPlaneUsingRobotHalconImageCalibrator : IHalconImageCalibrator
    {
        public string CameraParamsFileName { get; set; }
        // Pose的第一个和第二个参数由CamRobotPoint通过放射变换算出来
        public string CameraPoseFileName { get; set; }
        public double RobotX{ get; set; }
        public double RobotY{ get; set; }


        //public HHomMat2D HomMatRobotToPose { get; set; }
        public double param0 { get; set; }
        public double param1 { get; set; }
        public double param2 { get; set; }
        public double param3 { get; set; }
        public double param4 { get; set; }
        public double param5 { get; set; }
        
        public double Offsetdx { get; set; } = -0.045;
        public double Offsetdy { get; set; } = -0.045;
        public double Imagewidth { get; set; } = 10000;
        public double Imageheight { get; set; } = 10000;
        public double Scalenumber { get; set; } = 0.00001;
        public string Interpolationstring { get; set; } = "bilinear";
 

        public bool ImageCacheSaveToFile_Original_Enabled { get; set; }
        public string ImageCacheSaveToFile_Original_FileName { get; set; }
      

        public HImage Calibrate(HImage image)
        {
            HHomMat2D HomMatRobotToPose = new HHomMat2D();
            HomMatRobotToPose[0] = param0;
            HomMatRobotToPose[1] = param1;
            HomMatRobotToPose[2] = param2;
            HomMatRobotToPose[3] = param3;
            HomMatRobotToPose[4] = param4;
            HomMatRobotToPose[5] = param5;
            if (ImageCacheSaveToFile_Original_Enabled)
                image.WriteImageOfTiffLzw(ImageCacheSaveToFile_Original_FileName);
            var  camRobotPoint = new Vector();
            camRobotPoint.X = RobotX;
            camRobotPoint.Y = RobotY;

            double x=HomMatRobotToPose.RawData[1];
            var PosePoint = new Vector();
            PosePoint = AffineTransPoint2dEx.AffineTransPoint2d(HomMatRobotToPose, camRobotPoint);

            HTuple hv_CameraParams = null;
            HPose hv_CameraPose = null;
            HOperatorSet.ReadCamPar(CameraParamsFileName, out hv_CameraParams);
            hv_CameraPose = new HPose();
            hv_CameraPose.ReadPose(CameraPoseFileName);
            //将Posepoint的值赋给hv_CamraPose；
            hv_CameraPose[0] = PosePoint.X;
            hv_CameraPose[1] = PosePoint.Y;

            HTuple CameraPose = null;
            HOperatorSet.SetOriginPose(hv_CameraPose, Offsetdx, Offsetdx, 0, out CameraPose);
            HObject partimage;
            HOperatorSet.ImageToWorldPlane(image, out partimage, hv_CameraParams, CameraPose, Imagewidth, Imageheight,
                Scalenumber, Interpolationstring);
            HImage outImage = new HImage(partimage);
            outImage = outImage.FullDomain();

            return outImage;


        }

    }
}
