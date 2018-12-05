using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
namespace Hdc.Mv
{
    public static class BitmapSourceExtensions
    {
        public static ImageInfo ToImageInfo(this BitmapSource bitmapSource)
        {
            if (bitmapSource.Format.BitsPerPixel % 8 > 0)
                throw new NotSupportedException("BitsPerPixel must be 8, 16, 24, 32");

            var stride = bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel / 8;

            var info = new ImageInfo()
            {
                PixelWidth = bitmapSource.PixelWidth,
                PixelHeight = bitmapSource.PixelHeight,
                BitsPerPixel = bitmapSource.Format.BitsPerPixel,
            };

            int bufferSize = stride * bitmapSource.PixelHeight;
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
            bitmapSource.CopyPixels(Int32Rect.Empty, bufferPtr, bufferSize, stride);
            info.BufferPtr = bufferPtr;

            return info;
        }

        public static ImageInfo ToImageInfoWith8Bpp(this BitmapSource bitmapSource)
        {
            var stride = bitmapSource.PixelWidth;

            var info = new ImageInfo()
            {
                PixelWidth = bitmapSource.PixelWidth,
                PixelHeight = bitmapSource.PixelHeight,
                BitsPerPixel = bitmapSource.Format.BitsPerPixel,
            };

            int bufferSize = stride * bitmapSource.PixelHeight;
//            byte[] bytes = new byte[bufferSize];
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
//            Marshal.Copy(bytes, 0, bufferPtr, bytes.Length);
            // Call unmanaged code
            //        Marshal.FreeHGlobal(bufferPtr);

            bitmapSource.CopyPixels(Int32Rect.Empty, bufferPtr, bufferSize, bitmapSource.PixelWidth);
            info.BufferPtr = bufferPtr;

            return info;
        }

        public static ImageInfo ToImageInfoWith24Bpp(this BitmapSource bitmapSource)
        {
            var stride = bitmapSource.PixelWidth * 3;

            var info = new ImageInfo()
            {
                PixelWidth = bitmapSource.PixelWidth,
                PixelHeight = bitmapSource.PixelHeight,
                BitsPerPixel = bitmapSource.Format.BitsPerPixel,
            };

            int bufferSize = stride * bitmapSource.PixelHeight;
//            byte[] bytes = new byte[bufferSize];
            IntPtr bufferPtr = Marshal.AllocHGlobal(bufferSize);
//            Marshal.Copy(bytes, 0, bufferPtr, bytes.Length);
            // Call unmanaged code
            //        Marshal.FreeHGlobal(bufferPtr);

            bitmapSource.CopyPixels(Int32Rect.Empty, bufferPtr, bufferSize, bitmapSource.PixelWidth);
            info.BufferPtr = bufferPtr;

            return info;
        }
    }
}