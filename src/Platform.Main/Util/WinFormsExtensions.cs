using System.Windows;
using System.Windows.Media;

namespace Platform.Main.Util
{
    public static class WinFormsExtensions
    {
        public static System.Drawing.Point ToSystemDrawing(this System.Windows.Point p)
        {
            return new System.Drawing.Point((int)p.X, (int)p.Y);
        }

        public static System.Drawing.Size ToSystemDrawing(this System.Windows.Size s)
        {
            return new System.Drawing.Size((int)s.Width, (int)s.Height);
        }

        public static System.Drawing.Rectangle ToSystemDrawing(this System.Windows.Rect r)
        {
            return new System.Drawing.Rectangle(r.TopLeft.ToSystemDrawing(), r.Size.ToSystemDrawing());
        }

        public static System.Drawing.Color ToSystemDrawing(this System.Windows.Media.Color c)
        {
            return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static System.Windows.Point ToWpf(this System.Drawing.Point p)
        {
            return new System.Windows.Point(p.X, p.Y);
        }

        public static System.Windows.Size ToWpf(this System.Drawing.Size s)
        {
            return new System.Windows.Size(s.Width, s.Height);
        }

        public static System.Windows.Rect ToWpf(this System.Drawing.Rectangle rect)
        {
            return new System.Windows.Rect(rect.Location.ToWpf(), rect.Size.ToWpf());
        }

        public static System.Windows.Media.Color ToWpf(this System.Drawing.Color c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static Rect TransformToDevice(this Rect rect, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return Rect.Transform(rect, matrix);
        }

        public static Rect TransformFromDevice(this Rect rect, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            return Rect.Transform(rect, matrix);
        }

        public static System.Windows.Size TransformToDevice(this System.Windows.Size size, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return new System.Windows.Size(size.Width * matrix.M11, size.Height * matrix.M22);
        }

        public static System.Windows.Size TransformFromDevice(this System.Windows.Size size, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            return new System.Windows.Size(size.Width * matrix.M11, size.Height * matrix.M22);
        }

        public static System.Windows.Point TransformToDevice(this System.Windows.Point point, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            return matrix.Transform(point);
        }

        public static System.Windows.Point TransformFromDevice(this System.Windows.Point point, Visual visual)
        {
            Matrix matrix = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            return matrix.Transform(point);
        }
    }
}
