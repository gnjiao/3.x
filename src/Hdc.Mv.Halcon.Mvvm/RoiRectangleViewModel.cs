using Hdc.Mvvm;

namespace Hdc.Mv.Halcon.Mvvm
{
    public class RoiRectangleViewModel: ViewModel
    {
        private double _startX;

        public double StartX
        {
            get { return _startX; }
            set
            {
                if (Equals(_startX, value)) return;
                _startX = value;
                RaisePropertyChanged(() => StartX);
            }
        }

        private double _startY;

        public double StartY
        {
            get { return _startY; }
            set
            {
                if (Equals(_startY, value)) return;
                _startY = value;
                RaisePropertyChanged(() => StartY);
            }
        }

        private double _endX;

        public double EndX
        {
            get { return _endX; }
            set
            {
                if (Equals(_endX, value)) return;
                _endX = value;
                RaisePropertyChanged(() => EndX);
            }
        }

        private double _endY;

        public double EndY
        {
            get { return _endY; }
            set
            {
                if (Equals(_endY, value)) return;
                _endY = value;
                RaisePropertyChanged(() => EndY);
            }
        }

        private double _halfWidth;

        public double HalfWidth
        {
            get { return _halfWidth; }
            set
            {
                if (Equals(_halfWidth, value)) return;
                _halfWidth = value;
                RaisePropertyChanged(() => HalfWidth);
            }
        }

        private bool _displayEnabled = true;

        public bool DisplayEnabled
        {
            get { return _displayEnabled; }
            set
            {
                if (Equals(_displayEnabled, value)) return;
                _displayEnabled = value;
                RaisePropertyChanged(() => DisplayEnabled);
            }
        }
    }
}