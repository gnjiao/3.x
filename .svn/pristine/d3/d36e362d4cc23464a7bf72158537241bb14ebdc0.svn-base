using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Core;
using HalconDotNet;
using Core.Diagnostics;
using Hdc.Mv.Halcon;
using Core.Reflection;
using Core.Windows.Media.Imaging;

namespace Hdc.Mv.Inspection
{
    public class XldInspector : IXldInspector
    {
//        private readonly string _cacheImageDir = typeof (Mv.HdcMvEx).Assembly.GetAssemblyDirectoryPath() + "\\CacheImages";

        public XldSearchingResult SearchXld(HImage image, XldSearchingDefinition definition)
        {
            var swSearchEdge = new NotifyStopwatch($"{nameof(SearchXld)}: " + definition.Name);

            var result = new XldSearchingResult
            {
                Definition = definition.DeepClone(),
                Name = definition.Name
            };

            var rectImage = HDevelopExport.Singletone.ChangeDomainForRectangle(
                image,
                definition.RoiActualLine,
                definition.RoiHalfWidth);

            if (definition.SaveCacheImageEnabled)
            {
                rectImage.WriteImageOfTiffLzwOfCropDomain(definition.SaveCacheImageFileName);
            }

            if (definition.XldExtractor == null)
                result.Xld = null;
            else
            {
                //            var swLineExtractor = new NotifyStopwatch("EdgeInspector.LineExtractor: " + definition.Name);
                var xld = definition.XldExtractor.Extract(rectImage);
                //            swLineExtractor.Dispose();

                result.Xld = xld;
            }


            swSearchEdge.Dispose();
            return result;
        }
    }
}