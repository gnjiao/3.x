using System;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public abstract class ImageFilterBase: IImageFilter
    {
        protected abstract HImage ProcessInner(HImage image);

        public HImage Process(HImage image)
        {
            var processInner = ProcessInner(image);

            if (SaveCacheImageEnabled)
            {
                image.WriteImage("tiff", 0, SaveCacheImageFileName + ".ori.tif");
                processInner.WriteImage("tiff", 0, SaveCacheImageFileName + ".processed.tif");
            }

            return processInner;
        }

        public bool SaveCacheImageEnabled { get; set; }

        public string SaveCacheImageFileName { get; set; }
    }
}