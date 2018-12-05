using System.Collections;
using System.Collections.Generic;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    public interface IDatumPlaneCreator
    {
        void UpdateRelativeCoordinate(IRelativeCoordinate relativeCoordinate);

        DatumPlaneResult Create(HImage image);
    }
}