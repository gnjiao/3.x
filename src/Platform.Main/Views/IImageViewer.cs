using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;

namespace Platform.Main.Views
{
    public interface IImageViewer
    {
        void Show(HImage image);
        void AttachDrawObjToWindow(HDrawingObject hDrawingObject);
        void DarwRoiEdgeToWindow(RegionOfInterest regionOfInterest);
        void ZoomFit();
        void ZoomActual();
        void ClearWindow();
        HalconViewer GetHalconViewer();
    }
}
