using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HalconDotNet;

namespace Hdc.Controls
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for HalconViewer.xaml
    /// </summary>
    [ContentProperty("Series")]
    public partial class HalconViewer : UserControl
    {        
        private readonly ObservableCollection<HalconViewerSeries> _series =
            new ObservableCollection<HalconViewerSeries>();

        
                
        public HalconViewer()
        {
            InitializeComponent();

            _series.CollectionChanged += _series_CollectionChanged;
        }        

        private void _series_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var newItem in e.NewItems)
            {
                if (!(newItem is HalconViewerSeries indicatorSeriesBase)) continue;

                indicatorSeriesBase.HalconViewer = this;
                SeriesCanvas.Children.Add(indicatorSeriesBase);
            }

            Refresh();
        }        
 
        #region Image

        public HImage Image
        {
            get { return (HImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(HImage), typeof(HalconViewer), new FrameworkPropertyMetadata(OnImageChangedCallback));

        private static void OnImageChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var me = s as HalconViewer;
            if (me == null) return;

            me.HWindowControlWpf.HalconWindow.ClearWindow();

            var image = e.NewValue as HImage;
            if (image == null || image.Key == IntPtr.Zero)
            {
                me.HWindowControlWpf.HalconWindow.DetachBackgroundFromWindow();
            }
            else
            {
                me.HWindowControlWpf.HalconWindow.AttachBackgroundToWindow(image);
            }

            me.Refresh();
        }

        #endregion

        #region Scale

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(HalconViewer), new FrameworkPropertyMetadata(1.0, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var me = s as HalconViewer;

            me?.Refresh();
        }

        #endregion

        #region X

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(HalconViewer), new FrameworkPropertyMetadata(PropertyChangedCallback));

        #endregion

        #region Y

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(HalconViewer), new FrameworkPropertyMetadata(PropertyChangedCallback));

        #endregion       

        public Collection<HalconViewerSeries> Series
        {
            get { return _series; }
            //            set { _series = value; }
        }

        public void ZoomFit()
        {
            if (Image == null)
                return;

            if (Image.Key == IntPtr.Zero)
                return;

            X = 0;
            Y = 0;

            int width, height;
            Image.GetImageSize(out width, out height);

            int rowExtent, columnExtent, widthExtent, heightExtent;
            HWindowControlWpf.HalconWindow.GetWindowExtents(out rowExtent, out columnExtent,
                out widthExtent, out heightExtent);

            var ratioImage = (double)width / (double)height;
            var ratioWindow = (double)widthExtent / (double)heightExtent;

            double finalRatio = 0;
            if (ratioImage > ratioWindow)
            {
                finalRatio = (double)widthExtent / (double)width;
                Y = 0 + (height - heightExtent / finalRatio) / 2;
            }
            else
            {
                finalRatio = (double)heightExtent / (double)height;
                X = 0 + (width - widthExtent / finalRatio) / 2;
            }

            Scale = finalRatio;

            Refresh();
        }
        
        public void ZoomIn()
        {
            int rowExtent, columnExtent, widthExtent, heightExtent;
            HWindowControlWpf.HalconWindow.GetWindowExtents(out rowExtent, out columnExtent,
                out widthExtent, out heightExtent);

            var imagePart2 = HWindowControlWpf.HImagePart;

            var width1 = widthExtent / Scale;
            var height1 = heightExtent / Scale;

            Scale /= 0.75;

            var width2 = widthExtent / Scale;
            var height2 = heightExtent / Scale;


            var newMarginX = (width1 - width2) / 2.0;
            var newLeft = imagePart2.Left + newMarginX;

            var newMarginY = (height1 - height2) / 2.0;
            var newTop = imagePart2.Top + newMarginY;

            X = newLeft;
            Y = newTop;

            Refresh();
        }

        public void ZoomOut()
        {
            int rowExtent, columnExtent, widthExtent, heightExtent;
            HWindowControlWpf.HalconWindow.GetWindowExtents(out rowExtent, out columnExtent,
                out widthExtent, out heightExtent);

            var imagePart2 = HWindowControlWpf.HImagePart;

            var width1 = widthExtent / Scale;
            var height1 = heightExtent / Scale;

            Scale *= 0.75;

            var width2 = widthExtent / Scale;
            var height2 = heightExtent / Scale;


            var newMarginX = (width2 - width1) / 2.0;
            var newLeft = imagePart2.Left - newMarginX;

            var newMarginY = (height2 - height1) / 2.0;
            var newTop = imagePart2.Top - newMarginY;

            X = newLeft;
            Y = newTop;

            Refresh();
        }

        public void ZoomActual()
        {
            X = 0;
            Y = 0;
            Scale = 1.0;

            Refresh();
        }

        private void Refresh()
        {
            if (Image == null)
                return;

            if (Image.Key == IntPtr.Zero)
                return;

            int width, height;
            Image.GetImageSize(out width, out height);

            int rowExtent, columnExtent, widthExtent, heightExtent;
            HWindowControlWpf.HalconWindow.GetWindowExtents(out rowExtent, out columnExtent,
                out widthExtent, out heightExtent);            

            HWindowControlWpf.HImagePart = new Rect
            (                
                X,
                Y,
                widthExtent / Scale,
                heightExtent / Scale
            );

            foreach (var s in _series)
            {
                s.Refresh();
            }
        }        

        public HSmartWindowControlWPF HWindowControl => HWindowControlWpf;

        public void SmartShow(HImage hImage)
        {
            if (Image == null)
                return;

            if (Image.Key == IntPtr.Zero)
                return;
            if(hImage != null)
                HWindowControlWpf.HalconWindow.DispImage(hImage);
        }

        public void ShowHbject(HObject obj)
        {
            HWindowControlWpf.HalconWindow.DispObj(obj);
        }

        public void ClearWindow()
        {
            HWindowControlWpf.HalconWindow?.ClearWindow();
        }        

        public void AttachDrawingObjectToWindow(HDrawingObject drawingObject)
        {
            HWindowControl.HalconWindow.AttachDrawingObjectToWindow(drawingObject);
        }

        public void DetachDrawingObjectFromWindow(HDrawingObject drawingObject)
        {
            HWindowControl.HalconWindow.DetachDrawingObjectFromWindow(drawingObject);
        }

        public void ZoomToRectangle(double centerX, double centerY, double width, double height, double scaleFactor = 1)
        {
            if (Math.Abs(width) < 0.000001 & Math.Abs(height) < 0.000001)
                return;

            var wRatio = ActualWidth / width;
            var hRatio = ActualHeight / height;

            var scale = wRatio > hRatio ? hRatio : wRatio;
            scale = scale * scaleFactor;

            Scale = scale;

            X = (+centerX) - ActualWidth / scale / 2;
            Y = (+centerY) - ActualHeight / scale / 2;

            Refresh();
        }        

        public bool HMoveContent
        {
            set => HWindowControlWpf.HMoveContent = value;
            get => HWindowControlWpf.HMoveContent;
        }
        public bool HHandContent
        {
            set => HWindowControlWpf.HMoveContent = !value;
            get => !HWindowControlWpf.HMoveContent;
        }
        
        public bool HKeepAspectRatio
        {        
            set => HWindowControlWpf.HKeepAspectRatio = value;
            get => HWindowControlWpf.HKeepAspectRatio;
        }        

    }
}