using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Core.Presentation;
using Core.Reflection;
using Hdc.Mv.Halcon;
using Microsoft.Practices.Prism.Commands;
using Platform.Main.Util;
using Block = Hdc.Mv.Halcon.Block;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for SolutionView.xaml
    /// </summary>
    public partial class SolutionView : UserControl, ISolutionView
    {
        private AdornerLayer _layer;
        private Adorner _adorner;
        private GridManager _gridManager;
        private BlockModule _blockModule;

        public static readonly DependencyProperty RemoveBlockCommandProperty =
            DependencyProperty.Register("RemoveBlockCommand", typeof(ICommand), typeof(SolutionView));

        public ICommand RemoveBlockCommand
        {
            get => (ICommand)GetValue(RemoveBlockCommandProperty);
            set => SetValue(RemoveBlockCommandProperty, value);
        }

        public SolutionView()
        {
            InitializeComponent();

            RemoveBlockCommand = new DelegateCommand(() =>
                {
                    if (_adorner != null)
                        _layer.Remove(_adorner);

                    MainWindow.Instance.BlockSchema.Blocks.Remove(_blockModule.Block);

                    this.SolutionCanvas.Children.Remove(_blockModule);
                });

            Loaded += SolutionView_Loaded;
            PreviewMouseLeftButtonDown += SolutionView_PreviewMouseLeftButtonDown;
            PreviewMouseDoubleClick += SolutionView_PreviewMouseDoubleClick;
            PreviewKeyUp += SolutionView_PreviewKeyUp;
            AddHandler(DropEvent, new DragEventHandler(ControlDropWPFRoutedEventHandler), true);
        }

        private void SolutionView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach (BlockModule blockModule in SolutionCanvas.Children)
            {
                var point = _gridManager.GetMousePos();

                var rect = new Rect(
                    Canvas.GetLeft(blockModule),
                    Canvas.GetTop(blockModule),
                    blockModule.Width,
                    blockModule.Height);

                if (rect.Contains(point))
                {
                    var changePortReferenceWindow = new ChangePortReferenceWindow();

                    var attributes = new List<string>();

                    foreach (var item in blockModule.Block.GetType().GetProperties())
                    {
                        var categoryAttributes =
                            (CategoryAttribute[]) item.GetCustomAttributes(typeof(CategoryAttribute), false);
                        
                        if (categoryAttributes.Any(p => p.Category == BlockPropertyCategories.Input))
                        {
                            attributes.Add(item.Name);                            
                        }
                    }

                    if (attributes.Count > 0)
                    {
                        var dialogResult = changePortReferenceWindow.ShowDialog(MainWindow.Instance.BlockSchema,
                            MainWindow.Instance.EditingBlock, attributes);

                        if (dialogResult != true) return;

                        MainWindow.Instance.EditingBlock.PortReferences.Add(changePortReferenceWindow
                            .EditingPortReference);
                        MainWindow.Instance.EditingPortReference = changePortReferenceWindow.EditingPortReference;

                        return;
                    }
                }               
            }
        }

        private void SolutionView_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBox.Show("Delete Module?", "Waring", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (_adorner != null)
                        _layer.Remove(_adorner);

                    MainWindow.Instance.BlockSchema.Blocks.Remove(_blockModule.Block);

                    SolutionCanvas.Children.Remove(_blockModule);
                }
            }
        }

        private void SolutionView_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {            
            foreach (BlockModule blockModule in SolutionCanvas.Children)
            {
                var point = _gridManager.GetMousePos();

                var rect = new Rect(
                    Canvas.GetLeft(blockModule),
                    Canvas.GetTop(blockModule),
                    blockModule.Width,
                    blockModule.Height);

                if (rect.Contains(point))
                {
                    if(MainWindow.Instance.SelectedBlock?.Name == blockModule.Block.Name)
                        return;

                    MainWindow.Instance.SelectedBlock = blockModule.Block;

                    MainWindow.Instance.ImageViewClear();

                    MainWindow.Instance.ProcessAndRefresh(blockModule.Block,true);

                    if (_adorner != null)
                        _layer.Remove(_adorner);

                    _adorner = new CanvasAdorner(blockModule);
                    _layer.Add(_adorner);

                    _blockModule = blockModule;
                }
            }            
        }        

        private void SolutionView_Loaded(object sender, RoutedEventArgs e)
        {
            _layer = AdornerLayer.GetAdornerLayer(SolutionCanvas);
            
            _gridManager = new GridManager(SolutionCanvas)
            {
                GridOn = false,
                ShowGrid = true
            };
        }        

        private void ControlDropWPFRoutedEventHandler(object sender, DragEventArgs e)
        {
            var data = e.Data;

            if (!data.GetDataPresent(typeof(ToolBoxItem))) return;
            var selectedToolBoxItem = data.GetData(typeof(ToolBoxItem)) as ToolBoxItem;
            var selectedBlockEntry = (BlockEntry)selectedToolBoxItem?.Object;

            var block = selectedBlockEntry?.Type.CreateInstance() as Block;
            var existNames = MainWindow.Instance.BlockSchema.Blocks.Select(x => x.Name).ToList();
            if (block != null) block.Name = selectedBlockEntry.Name.GetNameByOrder(existNames, 2, 1);

            var postion = e.GetPosition(SolutionCanvas);

            CreateBlockModule(block, postion.X, postion.Y);
        }

        public void CreateBlockModule(Block block, double left, double top)
        {
            MainWindow.Instance.BlockSchema.Blocks.Add(block);

            _blockModule = new BlockModule(block);            
            SolutionCanvas.Children.Add(_blockModule);

            Canvas.SetLeft(_blockModule, left);
            Canvas.SetTop(_blockModule, top);
        }

        public void Refresh()
        {
            if (_adorner != null)
                _layer.Remove(_adorner);

            foreach (var blockModule in SolutionCanvas.Children)
            {
                ((IBlockModule)blockModule).Refresh();
            }
        }
    }
}
