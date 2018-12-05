using System.IO;

namespace Hdc.Mv.Inspection
{
    public class ImageFileSavedResult
    {
        public string FileFullName { get; set; }
        public string FileName { get; set; }
        public FileInfo FileInfo { get; set; }
        public FrameGrabberSchema FrameGrabberSchema { get; set; }
    }
}