using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using HalconDotNet;
using Hdc.Controls;
using Hdc.Mv.Mvvm;
using Hdc.Mv.Halcon;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;
using Microsoft.Practices.Prism.Commands;
using Platform.Main.Annotations;
using Platform.Main.Util;

namespace Platform.Main.Views
{
    /// <inheritdoc>
    ///     <cref>HalconViewer.cs</cref>
    /// </inheritdoc>
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public sealed partial class ImageViewer : UserControl, INotifyPropertyChanged, IImageViewer
    {
        public ObservableCollection<Rectangle1IndicatorViewModel> DefinitionsRectangle1Indicators { get; set; } =
            new ObservableCollection<Rectangle1IndicatorViewModel>();
        public ObservableCollection<Rectangle2IndicatorViewModel> DefinitionsRectangle2Indicators { get; set; } =
            new ObservableCollection<Rectangle2IndicatorViewModel>();
        public ObservableCollection<CircleIndicatorViewModel> DefinitionsCircleIndicators { get; set; } =
            new ObservableCollection<CircleIndicatorViewModel>();
        public ObservableCollection<EllipseIndicatorViewModel> DefinitionsEllipseIndicators { get; set; } =
            new ObservableCollection<EllipseIndicatorViewModel>();
        public ObservableCollection<LineIndicatorViewModel> DefinitionsLineIndicators { get; set; } =
            new ObservableCollection<LineIndicatorViewModel>();

        public DelegateCommand ZoomFitCommand { get; set; }
        public DelegateCommand ZoomActualCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _createOrder = null;

        private HDrawingObject _selectedDrawingObject;
        private List<HDrawingObject> _drawingObjects = new List<HDrawingObject>();

        public ImageViewer()
        {
            ZoomFitCommand = new DelegateCommand(ZoomFit);

            ZoomActualCommand = new DelegateCommand(ZoomActual);            

            InitializeComponent();

            ImageViewToolBar.Loaded += ImageViewToolBar_Loaded;

            HalconViewerImp.HWindowControl.HInitWindow += HWindowControl_HInitWindow;
            
        }

        private void HWindowControl_HInitWindow(object sender, System.EventArgs e)
        {
            HalconViewerImp.HWindowControl.HMouseMove += HWindowControl_HMouseMove;
            HalconViewerImp.HWindowControl.AddHandler(HSmartWindowControlWPF.DropEvent, new DragEventHandler(HSmartWindowControlWPFRoutedEventHandler), true);            
        }

        private void HWindowControl_HMouseMove(object sender, HSmartWindowControlWPF.HMouseEventArgsWPF e)
        {
            Column = e.Column;
            Row = e.Row;
            CursorPosition = $"Column:{Column:N1},Row:{Row:N1}";

            try
            {
                if (!string.IsNullOrEmpty(_createOrder))
                {
                    if (_selectedDrawingObject != null)
                    {
                        _selectedDrawingObject.Dispose();
                        _selectedDrawingObject = null;
                    }

                    switch (_createOrder)
                    {
                        case "Rectangle1":
                            _selectedDrawingObject = HDrawingObject.CreateDrawingObject(
                                HDrawingObject.HDrawingObjectType.RECTANGLE1, Row, Column, Row + 200, Column + 200);
                            break;

                        case "Rectangle2":
                            _selectedDrawingObject = HDrawingObject.CreateDrawingObject(
                                HDrawingObject.HDrawingObjectType.RECTANGLE2, Row, Column, 0, 100, 100);
                            break;

                        case "Ellipse":
                            _selectedDrawingObject = HDrawingObject.CreateDrawingObject(
                                HDrawingObject.HDrawingObjectType.ELLIPSE, Row, Column, 0, 100, 50);
                            break;

                        case "Circle":
                            _selectedDrawingObject = HDrawingObject.CreateDrawingObject(
                                HDrawingObject.HDrawingObjectType.CIRCLE, Row, Column, 100);
                            break;

                        default:
                            return;
                    }

                    AttachDrawObjToWindow(_selectedDrawingObject);

                    _createOrder = null;
                }
            }
            catch (Exception ex)
            {
                PlatformServiceTools.Log.Error(ex.Message);
            }            
        }

        public void AttachDrawObjToWindow(HDrawingObject hDrawingObject)
        {
            try
            {
                if (HalconViewerImp.Image != null && HalconViewerImp.Image.Key != IntPtr.Zero)
                {
                    _selectedDrawingObject = hDrawingObject;

                    if (_selectedDrawingObject != null && _selectedDrawingObject.Handle != IntPtr.Zero)
                    {
                        _selectedDrawingObject.SetDrawingObjectParams("color", "green");
                        AttachDrawObj(_selectedDrawingObject);
                    }
                }
            }
            catch (Exception ex)
            {
                PlatformServiceTools.Log.Error(ex.Message);
            }
        }

        public void DarwRoiEdgeToWindow(RegionOfInterest regionOfInterest)
        {
            try
            {
                if (HalconViewerImp.Image == null || HalconViewerImp.Image.Key == IntPtr.Zero ||
                    regionOfInterest == null)
                    return;

                switch (regionOfInterest.RoiType)
                {
                    case RegionOfInterestType.rectangle1:

                        DefinitionsRectangle1Indicators.Add(new Rectangle1IndicatorViewModel
                        {
                            Row1 = regionOfInterest.Row1,
                            Column1 = regionOfInterest.Column1,
                            Row2 = regionOfInterest.Row2,
                            Column2 = regionOfInterest.Column2,
                            ColorName = "green",
                            LineWidth = 2,
                            RegionFillMode = RegionFillMode.Margin
                        });
                        break;
                    case RegionOfInterestType.rectangle2:

                        DefinitionsRectangle2Indicators.Add(new Rectangle2IndicatorViewModel
                        {
                            Row = regionOfInterest.Row,
                            Column = regionOfInterest.Column,
                            Phi = regionOfInterest.Phi,
                            Length1 = regionOfInterest.Length1,
                            Length2 = regionOfInterest.Length2,
                            ColorName = "green",
                            LineWidth = 2,
                            RegionFillMode = RegionFillMode.Margin
                        });
                        break;

                    case RegionOfInterestType.circle:

                        DefinitionsCircleIndicators.Add(new CircleIndicatorViewModel
                        {
                            Row = regionOfInterest.Row,
                            Column = regionOfInterest.Column,
                            Radius = regionOfInterest.Radius,
                            ColorName = "green",
                            LineWidth = 2,
                            RegionFillMode = RegionFillMode.Margin
                        });
                        break;

                    case RegionOfInterestType.ellipse:
                        DefinitionsEllipseIndicators.Add(new EllipseIndicatorViewModel
                        {
                            Row = regionOfInterest.Row,
                            Column = regionOfInterest.Column,
                            Phi = regionOfInterest.Phi,
                            Radius1 = regionOfInterest.Radius1,
                            Radius2 = regionOfInterest.Radius2,
                            ColorName = "green",
                            LineWidth = 2,
                            RegionFillMode = RegionFillMode.Margin
                        });
                        break;
                }
            }
            catch (Exception ex)
            {
                PlatformServiceTools.Log.Error(ex.Message);
            }
        }

        private void HSmartWindowControlWPFRoutedEventHandler(object sender, DragEventArgs e)
        {         
            var data = e.Data;

            if (data.GetDataPresent(typeof(string)))
            {
                _createOrder = data.GetData(typeof(string)) as string;
            }
        }

        private void SobelFilter(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                HImage image = HalconViewerImp.Image;
                HRegion region = new HRegion(dobj.GetDrawingObjectIconic());
                hwin.SetWindowParam("flush", "false");
                hwin.ClearWindow();
                hwin.DispObj(image.ReduceDomain(region).SobelAmp("sum_abs", 11));
                hwin.SetWindowParam("flush", "true");
                hwin.FlushBuffer();
            }
            catch (HalconException)
            {
                //
            }
            catch (Exception ex)
            {
                PlatformServiceTools.Log.Error(ex.Message);
            }
        }
        private void OnSelectDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            _selectedDrawingObject = dobj;
            MainWindow.Instance.SelectedDrawingObject = _selectedDrawingObject;
            SobelFilter(dobj, hwin, type);
        }

        private void OnResizeDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            _selectedDrawingObject = dobj;
            MainWindow.Instance.SelectedDrawingObject = _selectedDrawingObject;
            SobelFilter(dobj, hwin, type);
        }

        private void OnDragDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            _selectedDrawingObject = dobj;
            MainWindow.Instance.SelectedDrawingObject = _selectedDrawingObject;
            SobelFilter(dobj, hwin, type);
        }
        private void AttachDrawObj(HDrawingObject obj)
        {
            try
            {
                _drawingObjects.Add(obj);

                obj.OnDrag(OnDragDrawingObject);
                obj.OnAttach(SobelFilter);
                obj.OnResize(OnResizeDrawingObject);
                obj.OnSelect(OnSelectDrawingObject);                

                if (_selectedDrawingObject == null)
                    _selectedDrawingObject = obj;

                MainWindow.Instance.SelectedDrawingObject = _selectedDrawingObject;

                HalconViewerImp.HWindowControl.HalconWindow.AttachDrawingObjectToWindow(obj);
            }
            catch (Exception ex)
            {
                PlatformServiceTools.Log.Error(ex.Message);
            }            
        }

        #region Row
        public double Row
        {
            get { return (double)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        public static readonly DependencyProperty RowProperty = DependencyProperty.Register(
            "Row", typeof(double), typeof(HalconViewer));

        #endregion

        #region Column
        public double Column
        {
            get { return (double)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(
            "Column", typeof(double), typeof(HalconViewer));        

        #endregion

        private void ImageViewToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as ToolBar;

            if (toolBar?.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
                overflowGrid.Visibility = Visibility.Collapsed;

            if (toolBar?.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
                mainPanelBorder.Margin = new Thickness(0);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        public void Show(HImage hImage)
        {
            HalconViewerImp.Image = hImage;

            ZoomFit();
        }

//        public void SmartShow(HImage hImage)
//        {
//            HalconViewerImp.SmartShow(hImage);
//        }        

        public void ZoomFit()
        {
            HalconViewerImp.ZoomFit();
        }

        public void ZoomActual()
        {
            HalconViewerImp.ZoomActual();
        }

        public void ClearWindow()
        {
            DefinitionsRectangle1Indicators.Clear();
            DefinitionsRectangle2Indicators.Clear();
            DefinitionsCircleIndicators.Clear();
            DefinitionsEllipseIndicators.Clear();
            DefinitionsLineIndicators.Clear();

            if (_selectedDrawingObject != null)
            {
                HalconViewerImp.HWindowControl.HalconWindow.DetachDrawingObjectFromWindow(_selectedDrawingObject);
                _selectedDrawingObject.Dispose();
                _selectedDrawingObject = null;
            }

            HalconViewerImp?.ClearWindow();
        }
        
        public HalconViewer GetHalconViewer()
        {
            return HalconViewerImp;
        }
        
        public bool HMoveContent
        {
            set
            {
                _contentLoaded = value;
                HalconViewerImp.HMoveContent = true;
                OnPropertyChanged(nameof(HMoveContent));
                OnPropertyChanged(nameof(HHandContent));
            }

            get => HalconViewerImp?.HMoveContent != null && HalconViewerImp.HMoveContent;
        }

        public bool HHandContent
        {
            set
            {
                _contentLoaded = value;
                HalconViewerImp.HMoveContent = false;
                OnPropertyChanged(nameof(HMoveContent));
                OnPropertyChanged(nameof(HHandContent));
            }

            get => HalconViewerImp?.HMoveContent != null && !HalconViewerImp.HMoveContent;
        }

        private string _cursorPosition;
        public string CursorPosition
        {
            get => _cursorPosition;
            set
            {
                _cursorPosition = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(CursorPosition));
            }
        }
    }
}