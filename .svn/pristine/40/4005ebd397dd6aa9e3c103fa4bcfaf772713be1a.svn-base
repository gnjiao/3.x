using System;
using System.Runtime.InteropServices;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv.ImageAcquisition
{
    [Serializable]
    public class LoadImageFileSimCamera : ICamera
    {
        public void Dispose()
        {
        }

        public bool Init()
        {
            return true;
        }

        public ImageInfo Acquisition()
        {
            var bs = ImageFileName.GetBitmapSource();

            var bsi = bs.ToGray8BppBitmapSourceInfo();

            IntPtr unmanagedPointer = Marshal.AllocHGlobal(bsi.Buffer.Length);
            Marshal.Copy(bsi.Buffer, 0, unmanagedPointer, bsi.Buffer.Length);

            var imageInfo = new ImageInfo()
            {
                BitsPerPixel = bs.Format.BitsPerPixel,
                BufferPtr = unmanagedPointer,
                PixelHeight = bsi.PixelHeight,
                PixelWidth = bsi.PixelWidth,
            };

            return imageInfo;
        }

        public string ImageFileName { get; set; }
    }
}