namespace Hdc.Mv.ImageAcquisition
{
    public class GrabInfo
    {
        public GrabInfo()
        {
        }

        public GrabInfo(int frameTag, string frameGrabberName, ImageInfo imageInfo)
        {
            FrameTag = frameTag;
            FrameGrabberName = frameGrabberName;
            ImageInfo = imageInfo;
        }

        public GrabInfo(string frameGrabberName, ImageInfo imageInfo)
        {
            FrameGrabberName = frameGrabberName;
            ImageInfo = imageInfo;
        }

        public GrabInfo(ImageInfo imageInfo)
        {
            ImageInfo = imageInfo;
        }

        public int FrameTag { get; set; }
        public int FrameGrabberIndex { get; set; }
        public string FrameGrabberName { get; set; }
        public ImageInfo ImageInfo { get; set; }
        public GrabState State { get; set; }
    }
}