using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.PropertyItem.Controls;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("TemplateMatching", BlockCatagory.Coordinate)]    
    public class TemplateMatchingBlock : Block
    {
        public override void Process()
        {
            if (InputImage == null || InputImage.Key == IntPtr.Zero)
            {
                Status = BlockStatus.Error;
                Message = "InputImage is null.";
                return;
            }
            
            try
            {
                InputImage = InputImage.MeanImage(9, 9);

                if (File.Exists(ShapeModelFileName1))
                    ShapeModel1 = new HShapeModel(ShapeModelFileName1);

                if (File.Exists(ShapeModelFileName2))
                    ShapeModel2 = new HShapeModel(ShapeModelFileName2);

                var numLevels1 = ShapeModel1.GetShapeModelParams(out double angleStart1, out double angleExtent1,
                        out double angleStep1, out double scaleMin1, out double scaleMax1, out double scaleStep1,
                        out string metric1, out int minContrast1);

                var numLevels2 = ShapeModel2.GetShapeModelParams(out double angleStart2, out double angleExtent2,
                    out double angleStep2, out double scaleMin2, out double scaleMax2, out double scaleStep2,
                    out string metric2, out int minContrast2);

                ShapeModel1.GetShapeModelOrigin(out var shapeModel1OriginRow, out var shapeModel1OriginColumn);
                ShapeModel2.GetShapeModelOrigin(out var shapeModel2OriginRow, out var shapeModel2OriginColumn);

                InputImage.FindShapeModel(ShapeModel1, angleStart1, angleExtent1,
                    MinScore1, NumMatches1, MaxOverlap1, SubPixel1, numLevels1, Greediness1,
                    out var shapeModel1Row, out var shapeModel1Column1, out var shapeModel1Angle, out var shapeModel1Score);
                
                InputImage.FindShapeModel(ShapeModel2, angleStart2, angleExtent2,
                    MinScore2, NumMatches2, MaxOverlap2, SubPixel2, numLevels2, Greediness2,
                    out var shapeModel2Row, out var shapeModel2Column, out var shapeModel2Angle, out var shapeModel2Score);

                Score1 = shapeModel1Score;
                Score2 = shapeModel2Score;

                var angle = HMisc.AngleLl(shapeModel1OriginRow, shapeModel1OriginColumn, shapeModel2OriginRow, shapeModel2OriginColumn,
                    shapeModel1Row[0].D, shapeModel1Column1[0].D, shapeModel2Row[0].D, shapeModel2Column[0].D);

                var homMat2DIdentity = new HHomMat2D();
                homMat2DIdentity = homMat2DIdentity.HomMat2dRotate(-angle, shapeModel1Row[0].D, shapeModel1Column1[0].D);

                OutputImage = homMat2DIdentity.AffineTransImage(InputImage, "constant", "false");
                
                var qx = homMat2DIdentity.AffineTransPoint2d(shapeModel1Row[0].D, shapeModel1Column1[0].D, out var qy);

                var homMat2DIdentity2 = new HHomMat2D();
                var coordinationRow = shapeModel1OriginRow - qx / 2.0;
                var coordinationColumn = shapeModel1OriginColumn - qy / 2.0;

                homMat2DIdentity2 = homMat2DIdentity2.HomMat2dTranslate(coordinationRow, coordinationColumn);

                OutputImage = homMat2DIdentity2.AffineTransImage(OutputImage, "constant", "false");
                
                Status = BlockStatus.Valid;
                Message = "TemplateMatching ok! ";
            }
            catch (Exception ex)
            {       
                Status = BlockStatus.Error;
                Message = "TemplateMatching error! ";
                Exception = ex;
            }
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
        }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public HImage InputImage { set; get; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public HShapeModel ShapeModel1 { set; get; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        [Editor(typeof(OpenDialogEditor), typeof(OpenDialogEditor))]
        public string ShapeModelFileName1 { set; get; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public HShapeModel ShapeModel2 { set; get; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        [Editor(typeof(OpenDialogEditor), typeof(OpenDialogEditor))]
        public string ShapeModelFileName2 { set; get; }


        ////========================================================================================================================////
                
        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance01Obj)]
        [Description("最低分值(模板多少部分匹配出来，匹配的百分比，建议值0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0)")]
        public double MinScore1 { get; set; } = 0.5;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance01Obj)]
        [Description("匹配实例的个数，默认值1，建议值0, 1, 2, 3, 4, 5, 10, 20")]
        public int NumMatches1 { get; set; } = 1;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance01Obj)]
        [Description("最大重叠度")]
        public double MaxOverlap1 { get; set; } = 0.5;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance01Obj)]
        [Description("是否亚像素精度'none', 'interpolation', 'least_squares', 'least_squares_high', 'least_squares_very_high', 'max_deformation 1', 'max_deformation 2', 'max_deformation 3', 'max_deformation 4', 'max_deformation 5', 'max_deformation 6'")]
        public string SubPixel1 { get; set; } = "least_squares";

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance01Obj)]
        [Description("搜索贪婪度，0安全/慢，1快不稳定有可能搜不到，建议值0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0")]
        public double Greediness1 { get; set; } = 0.9;

        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category(BlockPropertyCategories.OutputObject)]
        public double Score1 { set; get; }

        ////========================================================================================================================////
        
        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance02Obj)]
        [Description("最低分值(模板多少部分匹配出来，匹配的百分比，建议值0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0)")]
        public double MinScore2 { get; set; } = 0.5;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance02Obj)]
        [Description("匹配实例的个数，默认值1，建议值0, 1, 2, 3, 4, 5, 10, 20")]
        public int NumMatches2 { get; set; } = 1;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance02Obj)]
        [Description("最大重叠度")]
        public double MaxOverlap2 { get; set; } = 0.5;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance02Obj)]
        [Description("是否亚像素精度'none', 'interpolation', 'least_squares', 'least_squares_high', 'least_squares_very_high', 'max_deformation 1', 'max_deformation 2', 'max_deformation 3', 'max_deformation 4', 'max_deformation 5', 'max_deformation 6'")]
        public string SubPixel2 { get; set; } = "least_squares";

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Instance02Obj)]
        [Description("搜索贪婪度，0安全/慢，1快不稳定有可能搜不到，建议值0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0")]
        public double Greediness2 { get; set; } = 0.9;

        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category(BlockPropertyCategories.OutputObject)]
        public double Score2 { set; get; }

        ////========================================================================================================================////


        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category(BlockPropertyCategories.Output)]
        public HImage OutputImage { set; get; }
    }

    
}