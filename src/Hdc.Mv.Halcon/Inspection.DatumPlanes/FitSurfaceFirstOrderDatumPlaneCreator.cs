using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using HalconDotNet;
using Hdc.Mv.Halcon;

// ReSharper disable InconsistentNaming
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class FitSurfaceFirstOrderDatumPlaneCreator : IDatumPlaneCreator
    {
        public Collection<RegionSearchingDefinition> DatumPlaneRegions { get; set; } = new Collection<RegionSearchingDefinition>();

        public bool DatumPlane_SaveOriginalImageEnabled { get; set; }
        public string DatumPlane_SaveOriginalImageFileName { get; set; }
        public bool DatumPlane_SaveDatumPlaneImageEnabled { get; set; }
        public string DatumPlane_SaveDatumPlaneImageFileName { get; set; }
        public bool DatumPlane_SaveFitPlaneImageEnabled { get; set; } = true;
        public string DatumPlane_SaveFitPlaneImageFileName { get; set; } = "B:\\Plane.tif";

        public void UpdateRelativeCoordinate(IRelativeCoordinate relativeCoordinate)
        {
            DatumPlaneRegions.UpdateRelativeCoordinate(relativeCoordinate);
        }

        public DatumPlaneResult Create(HImage image)
        {
            var result = new DatumPlaneResult();

            var oriImage = image.CopyImage();

            if (DatumPlane_SaveOriginalImageEnabled)
            {
                oriImage.WriteImageOfTiffLzwOfCropDomain(DatumPlane_SaveOriginalImageFileName);
            }

            //
            var inspector = new RegionSearchingInspector();
            var results = inspector.SearchRegions(oriImage, DatumPlaneRegions);

            var datumPlaneImage = FitSurface(oriImage,
                results.Select(x => x.Region).ToList(),
                results.Select(x => x.Definition.ZOffsetInMillimeter).ToList());

            oriImage.Dispose();

            //
            if (DatumPlane_SaveDatumPlaneImageEnabled)
            {
                datumPlaneImage.WriteImageOfTiffLzwOfCropDomain(DatumPlane_SaveDatumPlaneImageFileName);
            }

            result.DatumPlaneImage = datumPlaneImage;
            result.RegionSearchingResults = results;

            return result;
        }

        private HImage FitSurface(HImage oriImage, List<HRegion> regions, List<double> zOffsets)
        {
            double beta;
            double gamma;
            int width;
            int height;
            HImage Plane = new HImage();
            HImage ResultImage = new HImage();
            HImage image1 = new HImage();
            HImage image2 = new HImage();
            /*
            HRegion concatRegion = regions.FirstOrDefault();
            foreach (var hRegion in regions)
            {
                concatRegion.ConcatObj(hRegion);
            }
            */
            var Regions = new HRegion();
            Regions.GenEmptyRegion();
            for (int i = 0; i < regions.Count; i++)
            {
                Regions = Regions.Union2(regions[i]);
            }
            //MessageBox.Show(regions.Count.ToString());

            //var enhancedImage = oriImage.ReduceDomain(concatRegion);

            var alpha = oriImage.FitSurfaceFirstOrder(Regions, Algorithm, Iterations,
                ClippingFactor, out beta, out gamma);

            oriImage.GetImageSize(out width, out height);
            image1 = oriImage.Rgb3ToGray(oriImage, oriImage);
            image2 = image1.ConvertImageType("uint2");
            //MessageBox.Show(image2.GetImageType() + image2.CountChannels());
            Plane.GenImageSurfaceFirstOrder(Type, alpha, beta, gamma, 0, 0, width, height);
            if (DatumPlane_SaveFitPlaneImageEnabled)
            {
                Plane.WriteImage("tiff", 0, DatumPlane_SaveFitPlaneImageFileName);
            }
            //MessageBox.Show(Plane.GetImageType() + Plane.CountChannels());
            ResultImage = image2.SubImage(Plane, Mult, Add);
            image1.Dispose();
            image2.Dispose();
            return ResultImage;
        }

        [DefaultValue("tukey")]
        public string Algorithm { set; get; } = "tukey";

        [DefaultValue(5)]
        public int Iterations { set; get; } = 5;

        [DefaultValue(2)]
        public double ClippingFactor { set; get; } = 2;

        [DefaultValue("uint2")]
        public string Type { set; get; } = "uint2";

        [DefaultValue(1)]
        public double Mult { set; get; } = 1;

        [DefaultValue(0)]
        public double Add { set; get; } = 0;
    }
}