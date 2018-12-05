using System;
using HalconDotNet;
using System.ComponentModel;
using Hdc.Mv.Halcon;
namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class EdgesImageImageFilter: ImageFilterBase
    {
        protected override HImage ProcessInner(HImage image)
        {
            HImage imgDir;

            return image.EdgesImage(out imgDir, Filter.ToString(), Alpha, NMS, Low, High);
        }

        /// <summary>
        /// List of values: 'canny', 'deriche1', 'deriche1_int4', 'deriche2', 'deriche2_int4', 'lanser1', 'lanser2', 'mshen', 'shen', 'sobel_fast'
        /// List of values (for compute devices): 'canny', 'sobel_fast'
        /// </summary>
        
        
        [Description("应用边缘算子，可选值有： ‘canny’, ‘deriche1’, ‘deriche1_int4’, ‘deriche2’, ‘deriche2_int4’, ‘lanser1’, ‘lanser2’, ‘mshen’, ‘shen’, ‘sobel_fast’")]
        public Edeg_Filter Filter { get; set; } = Edeg_Filter.canny;
        [Description("滤波参数，值越小滤波越强，保留的细节信息越少。建议值：0.1, 0.2, 0.3, 0.4, 0.5, 0.7, 0.9,1.0， 1.1")]
        public double Alpha { get; set; } = 1.0;

        /// <summary>
        /// List of values: 'hvnms', 'inms', 'nms', 'none'
        /// </summary>
        /// 
        [Description("非最大抑制")]
        public string NMS { get; set; } = "nms";

        /// <summary>
        /// Typical range of values: 1 ≤ Low ≤ 255
        /// </summary>
        [Description("滞后阈值操作的下阈值（如果不需要阈值，则为负),建议值：5, 10, 15, 20, 25, 30, 40")]
        public int Low { get; set; } = 20;

        /// <summary>
        /// Typical range of values: 1 ≤ High ≤ 255
        /// </summary>
         [Description("滞后阈值操作的上阈值（如果不需要阈值，则为负),建议值：10, 15, 20, 25, 30, 40, 50, 60, 70")]
        public int High { get; set; } = 40;
    }
}