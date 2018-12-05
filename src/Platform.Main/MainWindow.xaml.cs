using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using Core;
using Core.Diagnostics;
using Core.Presentation;
using Core.Serialization;
using Core.Unity;
using HalconDotNet;
using Hdc.Mv.Halcon;
using Hdc.Mv.Halcon.Blocks;
using Hdc.Mv.Halcon.Blocks.RegionOfInterest;
using Hdc.Mv.Inspection;
using Hdc.Mv.Mvvm;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Platform.Main.Annotations;
using Platform.Main.Util;
using Platform.Main.Views;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using Xceed.Wpf.AvalonDock.Themes;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Platform.Main
{
    /// <inheritdoc cref="IWorkbench" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : FullScreenEnabledWindow, IWorkbench, System.Windows.Forms.IWin32Window, INotifyPropertyChanged
    {
        private const string MainMenuPath = @"/Platform/MainMenu";
        private const string MainToolPath = @"/Platform/Toolbar";
        private const string LayoutConfig = "Platform.config";

        private ToolBar[] _toolBars;
        private PlatformMainStatusBar _statusBar;
        private DispatcherTimer _dispatcherTimer;

        public event PropertyChangedEventHandler PropertyChanged;
        public static MainWindow Instance { get; private set; }
        Window IWorkbench.MainWindow => this;
        public System.Windows.Forms.IWin32Window MainWin32Window => this;
        public DockingManager DockManager { get; set; }
        private XmlLayoutSerializer Serializer { get; set; }

        private readonly UnityContainer _container = new UnityContainer();        

        private BlockSchema _blockSchema;
        private Block _selectedBlock;
        private PropertyItemBase _selectedPropertyGridItem;
        private Block _editingBlock;
        private PortReference _editingPortReference;
        private HDrawingObject _selectedDrawingObject;

        public DelegateCommand ImportXamlFileCommand { get; set; }
        public DelegateCommand ExportXamlFileCommand { get; set; }
        public DelegateCommand CreateSampleSchemaCommand { get; set; }
        public DelegateCommand RunCommand { get; set; }
        public IntPtr Handle
        {
            get
            {
                var wnd = PresentationSource.FromVisual(this) as IWin32Window;
                return wnd?.Handle ?? IntPtr.Zero;
            }
        }
        
        public static void StartUpMainWindow()
        {
            Instance = new MainWindow();
        }

        public MainWindow()
        {
            RegisterTypes();

            CreateCommands();

            InitializeComponent();

            InitializeMainWindow();            
        }
        
        private void RegisterTypes()
        {

            ServiceLocator.SetLocatorProvider(() => new Microsoft.Practices.Unity.UnityServiceLocator(_container));

            _container.RegisterTypeWithLifetimeManager<IBlock, ImageFilterBlock>(nameof(ImageFilterBlock));
            _container.RegisterTypeWithLifetimeManager<IBlock, ReadImageBlock>(nameof(ReadImageBlock));
            _container.RegisterTypeWithLifetimeManager<IBlock, SpokeCircleFindingBlock>(nameof(SpokeCircleFindingBlock));
            _container.RegisterTypeWithLifetimeManager<IBlock, RakeEdgeFindingBlock>(nameof(RakeEdgeFindingBlock));
            _container.RegisterTypeWithLifetimeManager<IBlock, LineToLineMeasureBlock>(nameof(LineToLineMeasureBlock));
            _container.RegisterTypeWithLifetimeManager<IBlock, CoordinationUsingRegionBlock>(nameof(CoordinationUsingRegionBlock));            
            _container.RegisterTypeWithLifetimeManager<IBlock, RegionExtractorBlock>(nameof(RegionExtractorBlock));
            _container.RegisterTypeWithLifetimeManager<IBlock, TemplateMatchingBlock>(nameof(TemplateMatchingBlock));
            _container.RegisterTypeWithLifetimeManager<IBlock, CreateShapeModelBlock>(nameof(CreateShapeModelBlock));



            _container.RegisterTypeWithLifetimeManager<ImageViewer>();            

            ServiceSingleton.GetRequiredService<IServiceContainer>().AddService(typeof(ILogView), new LogView());
            ServiceSingleton.GetRequiredService<IServiceContainer>().AddService(typeof(IToolBoxView), new ToolBoxView());
            ServiceSingleton.GetRequiredService<IServiceContainer>().AddService(typeof(IPropertyBrowserView), new PropertyBrowserView());
            ServiceSingleton.GetRequiredService<IServiceContainer>().AddService(typeof(ISolutionView), new SolutionView());       
        }

        private void CreateCommands()
        {
            ImportXamlFileCommand = new DelegateCommand(() =>
            {
                var dialog = new OpenFileDialog()
                {                    
                    Filter = "*.xaml|*.xaml"
                };
                if(dialog.ShowDialog(this) != true)
                    return;
                
                var schema = dialog.FileName.DeserializeFromXamlFile<BlockSchema>();
                schema.Blocks.ForEach(x => x.Initialize());
                BlockSchema = schema;

                MessageBox.Show("Import finished successfully.");
            });

            ExportXamlFileCommand = new DelegateCommand(() =>
            {
                var dialog = new SaveFileDialog
                {
                    FileName = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + ".xaml",
                    Filter = "*.xaml|*.xaml"
                };

                if (dialog.ShowDialog(this) != true)
                    return;

                _blockSchema.SerializeToXamlFile(dialog.FileName);

                MessageBox.Show("Export finished successfully.");
                
            });

            CreateSampleSchemaCommand = new DelegateCommand(AddTest1);

            RunCommand = new DelegateCommand(RunBlockSchema);
        }

        public void RunBlockSchema()
        {
            try
            {
                var sw = new NotifyStopwatch("Camera.Acquisition()");
                PlatformServiceTools.Log.Info($"Run blockSchema starting..! DateTime: {DateTime.Now}");

                var engine = new BlockEngine();
                engine.Run(BlockSchema);

                var selected = SelectedBlock;
                var schema = BlockSchema;

                SelectedBlock = null;
                BlockSchema = null;

                BlockSchema = schema;
                //SelectedBlock = selected;

                ImageViewClear();

                foreach (var block in BlockSchema.Blocks)
                {
                    ProcessAndRefresh(block);
                }

                ServiceSingleton.GetRequiredService<ISolutionView>().Refresh();

                PlatformServiceTools.Log.Info($"Run blockSchema end! datetime: {DateTime.Now}, elapsed milliseconds: {sw.ElapsedMilliseconds}");                
                sw.Dispose();
            }
            catch (Exception)
            {
                PlatformServiceTools.Log.Info("Run BlockSchema error!");                               
            }
        }

        private void InitializeMainWindow()
        {
            ImageViewer = ServiceLocator.Current.GetInstance<ImageViewer>();
            Title = PlatformServiceTools.ResourceService.GetString("Base.WindowName");
            DataContext = this;

            _statusBar = new PlatformMainStatusBar { /*Background = System.Windows.Media.Brushes.DodgerBlue */};
            DockManager = new DockingManager {Theme = new Vs2013LightTheme()};
            Serializer = new XmlLayoutSerializer(DockManager);
            Serializer.LayoutSerializationCallback += (sender, args) =>
            {
                switch (args.Model.ContentId)
                {
                    case "LogView":
                        args.Content = ServiceSingleton.GetRequiredService<ILogView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.LogViewTitle");
                        break;

                    case "ImageView":
                        args.Content = ImageViewer;                        
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.ImageViewTitle");
                        args.Model.IsActive = true;
                        break;

                    case "ToolBoxView":
                        args.Content = ServiceSingleton.GetRequiredService<IToolBoxView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.ToolBoxViewTitle");
                        break;

                    case "PropertyBrowserView":
                        args.Content = ServiceSingleton.GetRequiredService<IPropertyBrowserView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.PropertyBrowserView");
                        break;

                    case "Solution":
                        args.Content = ServiceSingleton.GetRequiredService<ISolutionView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.SolutionTitle");
                        break;

                    default:
                        break;
                }
            };

            UpdateMenu();
            UpdateToolBar();
            UpdateMenuStatus();

            DockPanel.Children.Add(DockManager);            
            DockPanel.Children.Insert(DockPanel.Children.Count - 2, _statusBar);
            DockPanel.SetDock(_statusBar, Dock.Bottom);

            Loaded += (sender, e) =>
            {
                if (File.Exists(LayoutConfig))                
                    Serializer.Deserialize(LayoutConfig);
            };
            Closed += (sender, e) =>
            {
                _dispatcherTimer?.Stop();

                Serializer.Serialize($"{LayoutConfig}.Bak");
            };

            _dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(1000) };
            _dispatcherTimer.Tick += (sender, e) =>
            {
                UpdateMenuStatus();
                _statusBar.TxtStatusBarPanel.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            };
            _dispatcherTimer.Start();

            PlatformServiceTools.ResourceService.LanguageChanged += (sender, e) =>
            {
                Title = PlatformServiceTools.ResourceService.GetString("Base.WindowName");
                MenuService.UpdateText(MainMenu.ItemsSource);
                foreach (var tb in _toolBars)                
                    ToolBarService.UpdateText(tb.ItemsSource);                
            };

            BlockSchema = new BlockSchema();

            
        }

        private void UpdateMenuStatus()
        {
            MenuService.UpdateStatus(MainMenu.ItemsSource);

            if (_toolBars == null)
                return;

            foreach (var tb in _toolBars)
            {
                ToolBarService.UpdateStatus(tb.ItemsSource);
                var nCount = tb.Items.OfType<Button>().LongCount(p => p.IsEnabled == true);
                tb.Visibility = nCount > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void UpdateMenu()
        {
            MainMenu.ItemsSource = MenuService.CreateMenuItems(this, this, MainMenuPath,
                activationMethod: "MainMenu", immediatelyExpandMenuBuildersForShortcuts: true);
            MainMenu.Margin = new Thickness(2, 0, 2, 0);

            //mainMenu.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            //mainMenu.HorizontalAlignment = HorizontalAlignment.Stretch;
            //MainMenu.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void UpdateToolBar()
        {
            _toolBars = ToolBarService.CreateToolBars(this, this, MainToolPath);
            ToolBarsPane.Background = System.Windows.Media.Brushes.AliceBlue;
            //ToolBarsPane.Margin = new Thickness(0, 1, 0, 0);

            if (_toolBars != null)
            {
                foreach (var tb in _toolBars)
                {
                    //tb.Background = System.Windows.Media.Brushes.Transparent;
                    //DockPanel.SetDock(tb, Dock.Top);
                    //dockPanel.Children.Insert(1, tb);
                    ToolBarsPane.Children.Add(tb);
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseCommands()
        {            
        }
        
        public BlockSchema BlockSchema
        {
            get => _blockSchema;
            set
            {
                if (Equals(value, _blockSchema)) return;
                _blockSchema = value;
                OnPropertyChanged();
            }
        }

        public Block EditingBlock
        {
            get => _editingBlock;
            set
            {
                if (Equals(value, _editingBlock)) return;
                _editingBlock = value;
                OnPropertyChanged();
            }
        }

        public Block SelectedBlock
        {
            get => _selectedBlock;
            set
            {
                if (Equals(value, _selectedBlock)) return;
                _selectedBlock = value;

                //                var clone = _selectedBlock.DeepClone();
                //                EditingBlock = clone;
                EditingBlock = _selectedBlock;

                OnPropertyChanged();
            }
        }

        public PortReference EditingPortReference
        {
            get { return _editingPortReference; }
            set
            {
                if (Equals(value, _editingPortReference)) return;
                _editingPortReference = value;
                OnPropertyChanged();
            }
        }
        
        public PropertyItemBase SelectedPropertyGridItem
        {
            get => _selectedPropertyGridItem;
            set
            {
                _selectedPropertyGridItem = value;

                RaiseCommands();                

                if (!(_selectedPropertyGridItem is PropertyItemBase))
                {
                    EditingPortReference = null;
                }
                else
                {
                    var propertyName = ((PropertyItemBase)_selectedPropertyGridItem).DisplayName;

                    var existRef = EditingBlock?.PortReferences.SingleOrDefault(x => x.TargetPortName == propertyName);
                    EditingPortReference = existRef;
                }

                if (EditingBlock != null)
                {
                    ProcessAndRefresh(EditingBlock);
                }
                
                OnPropertyChanged();
            }
        }

        public HDrawingObject SelectedDrawingObject
        {
            set
            {
                _selectedDrawingObject = value;

                if(_selectedDrawingObject == null)
                    return;

                if (EditingBlock is RegionOfInterestBlock block)
                {
                    block.Roi = new RegionOfInterest(_selectedDrawingObject);
                }
            }
        }

        public void ImageViewClear()
        {
            ImageViewer?.ClearWindow();
        }

        public void ProcessAndRefresh(IBlock block, bool isEdit= false)
        {
            var blockTypeName = block.GetType().Name;
            
            switch (blockTypeName)
            {
                case nameof(ReadImageBlock):

                    if (block is ReadImageBlock readImageBlock)
                    {
                        readImageBlock.Process();
                        ImageViewer.Show(readImageBlock.Image);
                    }

                    break;

                case nameof(ImageFilterBlock):

                    if (block is ImageFilterBlock imageFilterBlock)
                    {
                        imageFilterBlock.Process();

                        ImageViewer.Show(imageFilterBlock.OutputImage);

                        if(isEdit)
                            ImageViewer.AttachDrawObjToWindow(imageFilterBlock.Roi?.CreateRoiDrawingObject());
                        else
                            ImageViewer.DarwRoiEdgeToWindow(imageFilterBlock.Roi);
                    }
                    break;

                case nameof(SpokeCircleFindingBlock):

                    if (block is SpokeCircleFindingBlock spokeCircleFindingBlock)
                    {
                        spokeCircleFindingBlock.Process();

                        if(spokeCircleFindingBlock.Status != BlockStatus.Valid)
                            return;

                        if (isEdit)
                            ImageViewer.AttachDrawObjToWindow(spokeCircleFindingBlock.Roi?.CreateRoiDrawingObject());
                        else
                        {
                            ImageViewer.Show(spokeCircleFindingBlock.Image);

                            ImageViewer.DefinitionsCircleIndicators.Add(new CircleIndicatorViewModel
                            {
                                Row = spokeCircleFindingBlock.Row,
                                Column = spokeCircleFindingBlock.Column,
                                Radius = spokeCircleFindingBlock.InnerRadius,
                                ColorName = "green",
                                LineWidth = 2,
                                RegionFillMode = RegionFillMode.Margin
                            });

                            ImageViewer.DefinitionsCircleIndicators.Add(new CircleIndicatorViewModel
                            {
                                Row = spokeCircleFindingBlock.Row,
                                Column = spokeCircleFindingBlock.Column,
                                Radius = spokeCircleFindingBlock.OuterRadius,
                                ColorName = "green",
                                LineWidth = 2,
                                RegionFillMode = RegionFillMode.Margin
                            });

                            ImageViewer.DefinitionsCircleIndicators.Add(new CircleIndicatorViewModel
                            {
                                Row = spokeCircleFindingBlock.Circle.CenterX,
                                Column = spokeCircleFindingBlock.Circle.CenterY,
                                Radius = spokeCircleFindingBlock.Circle.Radius,
                                ColorName = "red",
                                LineWidth = 2,
                                RegionFillMode = RegionFillMode.Margin
                            });
                        }
                    }
                    break;

                case nameof(RakeEdgeFindingBlock):

                    if (block is RakeEdgeFindingBlock rakeEdgeFindingBlock)
                    {
                        rakeEdgeFindingBlock.Process();

                        if (rakeEdgeFindingBlock.Status != BlockStatus.Valid)
                            return;

                        if (isEdit)
                            ImageViewer.AttachDrawObjToWindow(rakeEdgeFindingBlock.Roi?.CreateRoiDrawingObject());
                        else
                        {
                            //ImageViewer.Show(rakeEdgeFindingBlock.Image);

                            ImageViewer.DefinitionsRectangle1Indicators.Add(new Rectangle1IndicatorViewModel
                            {
                                Row1 = rakeEdgeFindingBlock.StartY,
                                Column1 = rakeEdgeFindingBlock.StartX,
                                Row2 = rakeEdgeFindingBlock.EndY,
                                Column2 = rakeEdgeFindingBlock.EndX,
                                ColorName = "green",
                                LineWidth = 2,
                                RegionFillMode = RegionFillMode.Margin
                            });
                            
                            ImageViewer.DefinitionsLineIndicators.Add(new LineIndicatorViewModel
                            {
                                Row1 = rakeEdgeFindingBlock.Line.Row1,
                                Column1 = rakeEdgeFindingBlock.Line.Column1,
                                Row2 = rakeEdgeFindingBlock.Line.Row2,
                                Column2 = rakeEdgeFindingBlock.Line.Column2,
                                ColorName = "red",
                                LineWidth = 2,
                                RegionFillMode = RegionFillMode.Margin
                            });
                        }
                    }

                    break;

                case nameof(LineToLineMeasureBlock):

                    if (block is LineToLineMeasureBlock lineToLineMeasureBlock)
                    {
                        lineToLineMeasureBlock.Process();

                        if (lineToLineMeasureBlock.Status != BlockStatus.Valid)
                            return;

                        ImageViewer.DefinitionsLineIndicators.Add(new LineIndicatorViewModel
                        {
                            Row1 = lineToLineMeasureBlock.DistanceLine.Row1,
                            Column1 = lineToLineMeasureBlock.DistanceLine.Column1,
                            Row2 = lineToLineMeasureBlock.DistanceLine.Row2,
                            Column2 = lineToLineMeasureBlock.DistanceLine.Column2,
                            ColorName = "red",
                            LineWidth = 2,
                            RegionFillMode = RegionFillMode.Margin
                        });

                        PlatformServiceTools.Log.Info($"{lineToLineMeasureBlock.Name},Distance:{lineToLineMeasureBlock.Distance}");
                    }

                    break;
                case nameof(RegionExtractorBlock):

                    if (block is RegionExtractorBlock regionExtractorBlock)
                    {
                        regionExtractorBlock.Process();

                    }
                    break;

                case nameof(CoordinationUsingRegionBlock):

                    if (block is CoordinationUsingRegionBlock coordinationUsingRegionBlock)
                    {
                        coordinationUsingRegionBlock.Process();

                    }
                    break;

                case nameof(CreateShapeModelBlock):

                    if (block is CreateShapeModelBlock createShapeModelBlock)
                    {
                        createShapeModelBlock.Process();

                        if (createShapeModelBlock.Status != BlockStatus.Valid)
                            return;

                        if (isEdit)
                            ImageViewer.AttachDrawObjToWindow(createShapeModelBlock.Roi?.CreateRoiDrawingObject());
                        else
                        {
                            ImageViewer.DarwRoiEdgeToWindow(createShapeModelBlock.Roi);

                            createShapeModelBlock.ShapeModel.GetShapeModelOrigin(out var row, out var column);

                            ImageViewer.DefinitionsLineIndicators.Add(new LineIndicatorViewModel
                            {
                                Row1 = row - 20,
                                Column1 = column,
                                Row2 = row + 20,
                                Column2 = column,
                                ColorName = "red",
                                LineWidth = 2,
                                RegionFillMode = RegionFillMode.Margin
                            });

                            ImageViewer.DefinitionsLineIndicators.Add(new LineIndicatorViewModel
                            {
                                Row1 = row,
                                Column1 = column - 20,
                                Row2 = row,
                                Column2 = column + 20,
                                ColorName = "red",
                                LineWidth = 2,
                                RegionFillMode = RegionFillMode.Margin
                            });
                        }
                    }

                    break;

                case nameof(TemplateMatchingBlock):

                    if (block is TemplateMatchingBlock templateMatchingBlock)
                    {
                        templateMatchingBlock.Process();

                        if (templateMatchingBlock.Status != BlockStatus.Valid)
                            return;

                        ImageViewer.Show(templateMatchingBlock.OutputImage);
                    }
                    break;                    

            }            
        }

        [Microsoft.Practices.Unity.Dependency]
        public ImageViewer ImageViewer { get; set; }

        public void AddTest1()
        {
            var readImageFunctionBlock = new ReadImageBlock()
            {
                Name = "ReadImage-01",
                FileName = @"sample.tif",
            };

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(readImageFunctionBlock, 10, 100);

            var rakeEdgeFindingBlock01 = new RakeEdgeFindingBlock()
            {
                Name = "RakeEdgeFinding-01",
            };
            rakeEdgeFindingBlock01.AddPortReference("Image", "ReadImage-01", "Image");

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(rakeEdgeFindingBlock01, 10, 180);

            var rakeEdgeFindingBlock02 = new RakeEdgeFindingBlock()
            {
                Name = "RakeEdgeFinding-02",
            };
            rakeEdgeFindingBlock02.AddPortReference("Image", "ReadImage-01", "Image");

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(rakeEdgeFindingBlock02, 10, 260);


            var lineToLineMeasureBlock = new LineToLineMeasureBlock()
            {
                Name = "LineToLineMeasureBlock-01",
            };
            lineToLineMeasureBlock.AddPortReference("Line1", "RakeEdgeFinding-01", "Line");
            lineToLineMeasureBlock.AddPortReference("Line2", "RakeEdgeFinding-02", "Line");

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(lineToLineMeasureBlock, 10, 340);
            

            /*
            var imageFilterBlock = new ImageFilterBlock
            {
                Name = "ImageFilter-01",
            };
            imageFilterBlock.ImageFilters.Add(new MeanImageFilter(50, 10));

            imageFilterBlock.AddPortReference("InputImage", "ReadImage-01", "Image");

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(imageFilterBlock, 10, 200);

            
            var spokeCircleFindingBlock = new SpokeCircleFindingBlock()
            {
                Name = "SpokeCircleFinding-01"
            };

            spokeCircleFindingBlock.AddPortReference("Image", "ReadImage-01", "Image");

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(spokeCircleFindingBlock, 10, 300);


            
            var imageFilterBlock02 = new ImageFilterBlock
            {
                Name = "ImageFilter-02",
            };
            imageFilterBlock02.ImageFilters.Add(new MeanImageFilter(50, 10));

            imageFilterBlock02.AddPortReference("InputImage", "ReadImage-01", "Image");

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(imageFilterBlock02, 210, 200);

            var rakeEdgeFindingBlock = new RakeEdgeFindingBlock()
            {
                Name = "RakeEdgeFinding-01",
            };
            rakeEdgeFindingBlock.AddPortReference("Image", "ReadImage-01", "Image");

            ServiceSingleton.GetRequiredService<ISolutionView>().CreateBlockModule(rakeEdgeFindingBlock, 210, 300);*/

            _blockSchema = new BlockSchema();
            _blockSchema.Blocks.Add(readImageFunctionBlock);
            _blockSchema.Blocks.Add(rakeEdgeFindingBlock01);
            _blockSchema.Blocks.Add(rakeEdgeFindingBlock02);
            _blockSchema.Blocks.Add(lineToLineMeasureBlock);


            //_blockSchema.Blocks.Add(imageFilterBlock);
            //_blockSchema.Blocks.Add(spokeCircleFindingBlock);

            //_blockSchema.Blocks.Add(imageFilterBlock02);
            //_blockSchema.Blocks.Add(rakeEdgeFindingBlock);

            _blockSchema.SerializeToXamlFile(@"b:\tempSchema.xaml");
            
        }
    }
}
