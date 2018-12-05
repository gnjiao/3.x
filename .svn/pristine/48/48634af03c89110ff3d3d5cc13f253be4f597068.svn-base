using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class SimpleSelectShapeRegionProcessor : IRegionProcessor
    {
        public HRegion Process(HRegion region)
        {
            HTuple features = new HTuple();
            HTuple min = new HTuple();
            HTuple max = new HTuple();

            string operation = LogicOperation.And.ToHalconString();

            HRegion selectedRegion = region;

            var feature = Feature.ToHalconString();
            features.Append(feature);

            min.Append(Min);
            max.Append(Max);

            region = selectedRegion.SelectShape(features, operation, min, max);
            return region;
        }

        public ShapeFeature Feature { get; set; } = ShapeFeature.Area;

        public double Min { get; set; }
        public double Max { get; set; }
    }
}