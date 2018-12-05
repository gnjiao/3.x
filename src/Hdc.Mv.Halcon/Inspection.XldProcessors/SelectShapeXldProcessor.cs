using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;
using HalconDotNet;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("Items")]
    public class SelectShapeXldProcessor : Collection<SelectShapeXldEntry>, IXldProcessor
    {
        public LogicOperation Operation { get; set; } = LogicOperation.And;

        public HXLD Process(HXLD xld)
        {
            HTuple features = new HTuple();
            HTuple min = new HTuple();
            HTuple max = new HTuple();

            string operation = Operation.ToHalconString();

            HXLD selectedRegion = xld;

            foreach (var entry in Items)
            {
                var feature = entry.Feature.ToHalconString();
                features.Append(feature);

                min.Append(entry.Min);
                max.Append(entry.Max);
            }

            xld = selectedRegion.SelectShapeXld(features, operation, min, max);
            return xld;
        }
    }
}