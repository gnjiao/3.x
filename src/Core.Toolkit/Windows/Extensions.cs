using System.Windows;

namespace Core.Windows
{
    public static class Extensions
    {
        public static Point GetCenterPoint(this Rect rect)
        {
            return new Point(rect.X + rect.Width / 2,
                             rect.Y + rect.Height / 2);
        } 
    }
}