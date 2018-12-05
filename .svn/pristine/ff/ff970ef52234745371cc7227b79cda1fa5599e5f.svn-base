using System;
using System.Windows;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class RadialLineDefinition : DefinitionBase
    {
        public double RelativeOriginX { get; set; }
        public double RelativeOriginY { get; set; }
        public double RelativeRadius { get; set; }

        public double ActualOriginX { get; set; }
        public double ActualOriginY { get; set; }
        public double ActualRadius { get; set; }

        public double Angle { get; set; }

        public Vector StartVector => new Vector(ActualOriginX, ActualOriginY);

        public Vector EndVector
        {
            get
            {
                var radiusVector = new Vector(ActualRadius, 0);
                var rotateVector = radiusVector.Rotate(Angle);
                return StartVector + rotateVector;
            }
        }

        /// <summary>
        /// use Reference points to create RadialLine
        /// if ReferenceRelativeRadius or ReferenceActualRadius >= 0, use the radius
        /// otherwise use distance between ReferenceOrigin and ReferenceFarPoint to be radius
        /// </summary>
        public string ReferenceOriginName { get; set; }
        public string ReferenceFarPointName { get; set; }
        public double ReferenceRelativeRadius { get; set; }  
        public double ReferenceActualRadius { get; set; }
        public double ReferenceAngleOffset { get; set; } // angle for halcon. invert from .net to halcon
    }
}