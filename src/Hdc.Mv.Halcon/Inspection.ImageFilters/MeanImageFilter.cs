using System;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class MeanImageFilter : IImageFilter
    {
#pragma warning disable 169
        private int counter;
#pragma warning restore 169
        public HImage Process(HImage image)
        {
            var domain = image.GetDomain();


            var domainCount = domain.CountObj();

            int maskHeight;
            int maskWidth;
            int imageWidth;
            int imageHeight;

            if (domainCount == 0)
            {
                maskWidth = MaskWidth;
                maskHeight = MaskHeight;
            }
            else
            {
                imageWidth = image.GetWidth();
                imageHeight = image.GetHeight();

                maskWidth = MaskWidth == 0 ? imageWidth : MaskWidth;
                maskHeight = MaskHeight == 0 ? imageHeight : MaskHeight;

                if (maskWidth > (imageWidth * 2)) maskWidth = imageWidth * 2 - 1;
                if (maskHeight > (imageHeight * 2)) maskHeight = imageHeight * 2 - 1;
            }

            //            var swMeanImage = new NotifyStopwatch("MeanFilter.MeanImage");
            HImage enhancedImage = image.MeanImage(maskWidth, maskHeight);
            //            swMeanImage.Dispose();

            return enhancedImage;
        }

        [DefaultValue(9)]
        [Browsable(true)] //yx
        [Description("滤波内核的宽度，建议值：3, 5, 7, 9, 11, 15, 23, 31, 43, 61, 101")]
        public int MaskWidth { get; set; } = 9;

        [DefaultValue(9)]
        [Browsable(true)] //yx
        [Description("滤波内核的高度，建议值：3, 5, 7, 9, 11, 15, 23, 31, 43, 61, 101")]
        public int MaskHeight { get; set; } = 9;

        public MeanImageFilter()
        {
        }

        public MeanImageFilter(int maskWidth, int maskHeight)
        {
            MaskWidth = maskWidth;
            MaskHeight = maskHeight;
        }
    }
}