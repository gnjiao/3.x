// ReSharper disable InconsistentNaming

using System;

namespace Hdc.Mv
{
    public interface IRoiRectangle
    {
        double StartX { get; set; }
        double StartY { get; set; }
        double EndX { get; set; }
        double EndY { get; set; }

        /// <summary>
        /// Half Width
        /// </summary>
        double ROIWidth { get; set; }
    }
}

// ReSharper restore InconsistentNaming