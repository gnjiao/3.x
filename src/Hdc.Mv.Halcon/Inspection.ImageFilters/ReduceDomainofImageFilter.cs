using System;
using System.Windows.Markup;
using HalconDotNet;
using System.ComponentModel;//yx

//Creat by YanXin

namespace Hdc.Mv.Inspection
{
    [Serializable]
    [ContentProperty("RegionExtractor")]
    public class ReduceDomainogImageFilter : IImageFilter
    {
        public HImage Process(HImage image)
        {
            var domain = image.GetDomain();
            var reducedImage = image.ReduceDomain(domain);
            domain.Dispose();
            return reducedImage;
        }

        //public IRegionExtractor RegionExtractor { get; set; }
    }
}