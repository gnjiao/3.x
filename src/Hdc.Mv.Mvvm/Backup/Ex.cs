using System.Windows.Media;
using Hdc.Mv.Inspection;

namespace Hdc.Mv.Mvvm
{
    public static class Ex
    {
        public static RectangleIndicatorViewModel ToViewModel(this DefectInfo dr)
        {
            var regionIndicator = new RectangleIndicatorViewModel
                                  {
                                      CenterX = dr.X,
                                      CenterY = dr.Y,
                                      Width = dr.Width,
                                      Height = dr.Height,
                                      IsHidden = false,
                                      Stroke = Brushes.Lime,
                                      StrokeThickness = 2,
                                  };
            return regionIndicator;
        }
    }
}