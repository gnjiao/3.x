using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("Items")]
    public class CompositeImageFilter : Collection<IImageFilter>, IImageFilter
    {
        public HImage Process(HImage image)
        {
            var processImage = image;

            foreach (var imageFilter in Items)
            {
                var hImage = imageFilter.Process(processImage);
//                processImage.Dispose();
                processImage = hImage;
            }

            if (SaveCacheImageEnabled)
            {
                image.WriteImage("tiff", 0, SaveCacheImageFileName + ".ori.tif");
                processImage.WriteImage("tiff", 0, SaveCacheImageFileName + ".painted.tif");
            }

            return processImage;
        }

        public bool SaveCacheImageEnabled { get; set; }

        public string SaveCacheImageFileName { get; set; }
    }
}