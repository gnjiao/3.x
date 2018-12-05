using System;
using System.Windows;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class XldSearchingResult
    {
        public int Index { get; set; }

        public string Name
        {
            get { return Definition.Name; }
            set { Definition.Name = value; }
        }

        public bool HasError { get; set; }

        public bool IsNotFound { get; set; }

        public HXLD Xld { get; set; }

        public XldSearchingDefinition Definition { get; set; }

        public XldSearchingResult()
        {
        }

        public XldSearchingResult(int index)
        {
            Index = index;
        }

        public XldSearchingResult(int index, XldSearchingDefinition definition)
        {
            Index = index;
            Definition = definition;
        }

        public XldSearchingResult(string name)
        {
            Name = name;
        }

        Ellipse _ellipse;

        public double FitEllipseContourXld_Radius1
        {
            get
            {
                if (_ellipse == null)
                {
                    var contours = Xld as HXLDCont;
                    if (contours == null)
                        return 0.0;

                    _ellipse = contours.FitEllipseContourXld();
                }
                
                return _ellipse.Radius1;
            }
        }

        public double FitEllipseContourXld_Radius2
        {
            get
            {
                if (_ellipse == null)
                {
                    var contours = Xld as HXLDCont;
                    if (contours == null)
                        return 0.0;

                    _ellipse = contours.FitEllipseContourXld();
                }
                return _ellipse.Radius2;
            }
        }



        public double FitEllipseContourXld_RadiusMin
        {
            get
            {
                if (_ellipse == null)
                {
                    var contours = Xld as HXLDCont;
                    if (contours == null)
                        return 0.0;

                    _ellipse = contours.FitEllipseContourXld();
                }
                return _ellipse.Radius1 < _ellipse.Radius2 ? _ellipse.Radius1 : _ellipse.Radius2;
            }
        }

        public double FitEllipseContourXld_RadiusMax
        {
            get
            {
                if (_ellipse == null)
                {
                    var contours = Xld as HXLDCont;
                    if (contours == null)
                        return 0.0;

                    _ellipse = contours.FitEllipseContourXld();
                }
                return _ellipse.Radius1 > _ellipse.Radius2 ? _ellipse.Radius1 : _ellipse.Radius2;
            }
        }

        public double FitEllipseContourXld_DiameterMin => FitEllipseContourXld_RadiusMin * 2;
        public double FitEllipseContourXld_DiameterMinInWorld => FitEllipseContourXld_DiameterMin
            .ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);

        public double FitEllipseContourXld_DiameterMax => FitEllipseContourXld_RadiusMax * 2;
        public double FitEllipseContourXld_DiameterMaxInWorld => FitEllipseContourXld_DiameterMax
            .ToMillimeterFromPixel(Definition.PixelCellSideLengthInMillimeter);
    }
}