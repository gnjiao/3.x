using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Mv.Halcon;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("Items")]
    public class CompositeXldProcessor : Collection<IXldProcessor>, IXldProcessor
    {
        public HXLD Process(HXLD xld)
        {
            var xldClone = xld;

            for (int i = 0; i < Items.Count; i++)
            {
                var processor = Items[i];
                var process = processor.Process(xldClone);
                //hRegion.RegionToBin(255, 0, 8192, 12500).WriteImageOfJpeg(@"D:\TestImage_" + i);

                xldClone = process;
            }

            return xldClone;
        }
    }
}