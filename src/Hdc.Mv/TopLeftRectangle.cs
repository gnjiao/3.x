using System.Windows;

namespace Hdc.Mv
{
    public class TopLeftRectangle
    {
        public int TopLeftX{ get; set; } 
        public int TopLeftY { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }

        public TopLeftRectangle()
        {
        }

        public TopLeftRectangle(int topLeftX, int topLeftY, int width, int height)
        {
            TopLeftX = topLeftX;
            TopLeftY = topLeftY;
            Width = width;
            Height = height;
        }

        public int Row1 => TopLeftY;
        public int Column1 => TopLeftX;
        public int Row2 => TopLeftY + Height - 1;
        public int Column2 => TopLeftX + Width - 1;

    }
}