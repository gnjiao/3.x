using System.Threading.Tasks;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;

namespace Hdc.Mv
{
    public static class HalconImageCalibratorExtensions
    {
        public static Task<HImage> CalibrateAsync(this IHalconImageCalibrator calibrator, HImage originalImage)
        {
            return Task.Run(() => calibrator.Calibrate(originalImage));
        }

        public static HImage Calibrate8Bpp(this IHalconImageCalibrator calibrator, ImageInfo imageInfo)
        {
            var swOriToHImageInfo = new NotifyStopwatch("imageInfo.To8BppHImage()");
            HImage hImage = imageInfo.To8BppHImage();
            swOriToHImageInfo.Dispose();

            var swCalibrate = new NotifyStopwatch("HImage.Calibrate()");
            var calibrate = calibrator.Calibrate(hImage);
            swCalibrate.Dispose();

            hImage.Dispose();

            return calibrate;

//            return hImage;
        }

        public static Task<HImage> Calibrate8BppAsync(this IHalconImageCalibrator calibrator, ImageInfo imageInfo)
        {
            return Task.Run(() => calibrator.Calibrate8Bpp(imageInfo));
        }
    }
}