using System;
using System.IO;
using System.Runtime.InteropServices;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv.ImageAcquisition
{
    [Serializable]
    public class LoadImageFilesSimCamera : ICamera
    {
        private int _index;

        public void Dispose()
        {
        }

        private string[] _files;

        public bool Init()
        {
            _files = Directory.GetFiles(ImageDirectory);
            return true;
        }

        public ImageInfo Acquisition()
        {
            string fileName = _files[_index];

            var bs = fileName.GetBitmapSource();

            var bsi = bs.ToGray8BppBitmapSourceInfo();

            IntPtr unmanagedPointer = Marshal.AllocHGlobal(bsi.Buffer.Length);
            Marshal.Copy(bsi.Buffer, 0, unmanagedPointer, bsi.Buffer.Length);

            var imageInfo = new ImageInfo()
            {
                BitsPerPixel = bs.Format.BitsPerPixel,
                BufferPtr = unmanagedPointer,
                PixelHeight = bsi.PixelHeight,
                PixelWidth = bsi.PixelWidth,
                Index = _index,
            };

            _index++;
            if (_index == _files.Length)
                _index = 0;

            return imageInfo;
        }

        public string ImageDirectory { get; set; }
    }
}