using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.PropertyItem.Controls;

namespace Hdc.Mv.Halcon
{
    [Serializable]
    [Block("ReadImage", BlockCatagory.Acquisition)]
    public class ReadImageBlock : Block
    {
        private string _fileName;
        public override void Initialize()
        {
            base.Initialize();
                        
            Image?.Dispose();
            Image = null;
        }

        public override void Process()
        {
            if (Image != null && Image.Key != IntPtr.Zero)
            {
                Status = BlockStatus.Valid;
                return;
            }

            var fn = File.Exists(FileName);

            if (!fn)
            {
                Status = BlockStatus.Error;
                Message = "FileName is not exist! " + FileName;
                Exception = new FileNotFoundException(FileName);
                return;
            }

            try
            {
                Image = new HImage();
                Image.ReadImage(FileName);

                if(Math.Abs(ScaleWidth - 1) > 0.000001 || Math.Abs(ScaleHeight - 1) > 0.000001)
                    Image = Image.ZoomImageFactor(ScaleWidth, ScaleHeight, Interpolation);

                Status = BlockStatus.Valid;
            }
            catch (Exception ex)
            {
                Status = BlockStatus.Error;
                Message = "ReadImage is not exist! " + FileName;
                Exception = ex;
            }
        }

        public override void ProcessAndRefresh(HalconViewer imageViewer, bool editMode = false)
        {
            imageViewer.Image = Image;
        }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.Input)]
        [Editor(typeof(ReadImageBlockEditor),typeof(ReadImageBlockEditor))]                
        public string FileName
        {
            get => _fileName;
            set
            {
                if (Equals(value, _fileName)) return;
                _fileName = value;

                Image?.Dispose();
                Image = null;
            }
        }

        [OutputPort]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category(BlockPropertyCategories.Output)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public HImage Image { get; set; }

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("宽度缩放,可取值 0.25, 0.5, 1.5, 2.0")]
        public double ScaleWidth { get; set; } = 1.0;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("高度缩放,可取值 0.25, 0.5, 1.5, 2.0")]
        public double ScaleHeight { get; set; } = 1.0;

        [InputPort]
        [Browsable(true)]
        [Category(BlockPropertyCategories.InputControl)]
        [Description("缩放方式,可取值 'bicubic', 'bilinear', 'constant', 'nearest_neighbor', 'weighted'")]
        public string Interpolation { get; set; } = "constant";

    }
}