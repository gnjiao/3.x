using System;
using System.ComponentModel;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;
using Hdc.Mv.PropertyItem.Controls;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("CreateShapeModel", BlockCatagory.Coordinate)]
    public class CreateShapeModelBlock : RegionOfInterestBlock
    {
        public override void Process()
        {
            if (InputImage == null || InputImage.Key == IntPtr.Zero)
            {
                Status = BlockStatus.Error;
                Message = "InputImage is null";
                Exception = new BlockException("InputImage is null");
                return;
            }

            if (Roi?.RoiRegion == null) return;

            InputImage = InputImage.MeanImage(9, 9);

            var shapeModelImage = InputImage.ReduceDomain(Roi.RoiRegion);

            try
            {
                ShapeModel = new HShapeModel();

                ShapeModel.CreateShapeModel(shapeModelImage, NumLevels, AngleStart, AngleEXtent, Anglestep,
                    Optimization, Metric,
                    Contrast, MinContrast);

                ShapeModel.FindShapeModel(InputImage, AngleStart, AngleEXtent, MinScore, NumMatches, MaxOverlap,
                    SubPixel, NumLevels, Greediness,
                    out var row, out var column, out var angle, out var score);

                Score = score;
                ShapeModel.SetShapeModelOrigin(row,column);

                if (FileName != null)
                    ShapeModel.WriteShapeModel(FileName);
                
                Status = BlockStatus.Valid;
                Message = "Process OK";
            }
            catch (Exception)
            {
                Status = BlockStatus.Error;
                Message = "CreateShapeModel error";
                Exception = new BlockException("CreateShapeModel error!");
            }
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
           
        }
        
        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("金字塔层数,可取值 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,'auto'")]
        public int NumLevels { get; set; } = 8;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("寻找的开始角度,建议值-3.14, -1.57, -0.79, -0.39, -0.20, 0.0")]
        public double AngleStart { get; set; } = -3.14;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("角度范围，建议值6.29, 3.14, 1.57, 0.79, 0.39")]
        public double AngleEXtent { get; set; } = 6.29;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("建议值'auto', 0.0175, 0.0349, 0.0524, 0.0698, 0.0873")]
        public string Anglestep { get; set; } = "auto";

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description(
            "优化选项，是否减少模板匹配点数，可选值：'auto', 'no_pregeneration', 'none', 'point_reduction_high', 'point_reduction_low', 'point_reduction_medium', 'pregeneration'")]
        public string Optimization { get; set; } = "auto";

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description(
            "匹配度极性选择，可选值：'ignore_color_polarity', 'ignore_global_polarity', 'ignore_local_polarity', 'use_polarity'")]
        public string Metric { get; set; } = "use_polarity";

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description(
            "阀值或滞后阀值表示对比度，建议值：'auto', 'auto_contrast', 'auto_contrast_hyst', 'auto_min_size', 10, 20, 30, 40, 60, 80, 100, 120, 140, 160")]
        public string Contrast { get; set; } = "auto";

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("最小对比度，建议值'auto', 1, 2, 3, 5, 7, 10, 20, 30, 40")]
        public string MinContrast { get; set; } = "auto";

        ////========================================================================================================////        
        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [Description("最低分值(模板多少部分匹配出来，匹配的百分比，建议值0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0)")]
        public double MinScore { get; set; } = 0.5;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [Description("匹配实例的个数，默认值1，建议值0, 1, 2, 3, 4, 5, 10, 20")]
        public int NumMatches { get; set; } = 1;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [Description("最大重叠度")]
        public double MaxOverlap { get; set; } = 0.5;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [Description("是否亚像素精度'none', 'interpolation', 'least_squares', 'least_squares_high', 'least_squares_very_high', 'max_deformation 1', 'max_deformation 2', 'max_deformation 3', 'max_deformation 4', 'max_deformation 5', 'max_deformation 6'")]
        public string SubPixel { get; set; } = "least_squares";

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Parameter)]
        [Description("搜索贪婪度，0安全/慢，1快不稳定有可能搜不到，建议值0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0")]
        public double Greediness { get; set; } = 0.9;

        ////========================================================================================================////        
        
        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        public HImage InputImage { set; get; }        

        [OutputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.OutputObject)]
        [Editor(typeof(SaveDialogEditor), typeof(SaveDialogEditor))]
        public string FileName { set; get; }

        [OutputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.OutputObject)]
        public double Score { set; get; }

        [OutputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Output)]
        public HShapeModel ShapeModel { set; get; }
    }
}
