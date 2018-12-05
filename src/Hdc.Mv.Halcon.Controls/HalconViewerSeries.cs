using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HalconDotNet;

namespace Hdc.Controls
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for HalconViewerSeries.xaml
    /// </summary>
    public abstract class HalconViewerSeries : Control
    {
        public enum HRegionFillMode
        {
            fill,
            margin,
        }

        public static Dispatcher WorkDispatcher { get; set; }

        static HalconViewerSeries()
        {
            var newWindowThread = new Thread(new ThreadStart(() =>
            {
                Debug.WriteLine("ThreadStart begin");
                WorkDispatcher = Dispatcher.CurrentDispatcher;
                System.Windows.Threading.Dispatcher.Run();
                Debug.WriteLine("ThreadStart end");
            }));
            //newWindowThread.Start();
        }

        public virtual void Refresh()
        {
            //DisplayItems(ItemsSource);
        }

        public HalconViewer HalconViewer { protected get; set; }

        public static object GetPropertyValue(object src, string propName)
        {
            var type = src.GetType();
            var propertyInfo = type.GetProperty(propName);
            return propertyInfo?.GetValue(src, null);
        }

        #region ItemsSource

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(HalconViewerSeries),
            new PropertyMetadata(OnMeasurementItemsSourcePropertyChangedCallback));

        private static void OnMeasurementItemsSourcePropertyChangedCallback(DependencyObject s,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(s is HalconViewerSeries me)) return;

            var ds = e.NewValue as INotifyCollectionChanged;

            void DsOnCollectionChanged(object x, NotifyCollectionChangedEventArgs y) => me.DisplayItems(ds as IEnumerable);

            if (ds != null)
            {
                ds.CollectionChanged += DsOnCollectionChanged;
            }

            if (e.OldValue is INotifyCollectionChanged oldDs)
            {
                oldDs.CollectionChanged -= DsOnCollectionChanged;
            }
        }

        #endregion

        protected virtual void DisplayItems(IEnumerable enumerable)
        {
            Application.Current.MainWindow?.Dispatcher.Invoke(() =>
            {
                if (enumerable == null)
                    return;

                var elements = enumerable as List<object> ?? enumerable.Cast<object>().ToList();

                if (!elements.Any())
                    return;

                BeforeDisplayItems(elements);

                var regions = new List<HRegion>();

                foreach (var element in elements)
                {
                    BeforeDisplayItem(element);

                    HRegion region;
                    var propertyValue = GetPropertyValue(element, "DisplayEnabled");
                    if (propertyValue != null)
                    {
                        var displayEnabled = (bool)propertyValue;
                        if (displayEnabled)
                        {
                            region = GetDisplayRegion(element);
                            if (region != null)
                                regions.Add(region);
                        }
                        else
                            continue;
                    }
                    else
                    {
                        region = GetDisplayRegion(element);
                        region?.DispRegion(HalconViewer.HWindowControlWpf.HalconWindow);
                        if (region != null)
                            regions.Add(region);
                    }
                    var xld = GetDisplayXld(element);
                    xld?.DispObj(HalconViewer.HWindowControlWpf.HalconWindow);
                }

                //var mergedRegion = regions.Union();
                //mergedRegion.DispRegion(HalconViewer.HWindowControlWpf.HalconWindow);

                AfterDisplayItems(elements);
            });
        }

        protected virtual void AfterDisplayItems(List<object> elements)
        {
        }

        protected virtual void BeforeDisplayItem(object element)
        {
            try
            {
                ColorName = (string)GetPropertyValue(element, "ColorName");
                LineWidth = (int)GetPropertyValue(element, "LineWidth");
                RegionFillMode = (HRegionFillMode)GetPropertyValue(element, "RegionFillMode");

                HalconViewer.HWindowControl.HalconWindow.SetColor(ColorName);
                HalconViewer.HWindowControl.HalconWindow.SetDraw(RegionFillMode.ToString());
                HalconViewer.HWindowControl.HalconWindow.SetLineWidth(LineWidth);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            
        }


        protected virtual void BeforeDisplayItems(List<object> elements)
        {
            HalconViewer.HWindowControl.HalconWindow.SetColor(ColorName);
            HalconViewer.HWindowControl.HalconWindow.SetDraw(RegionFillMode.ToString());
            HalconViewer.HWindowControl.HalconWindow.SetLineWidth(LineWidth);
        }

        protected abstract HRegion GetDisplayRegion(object element);

        protected virtual HXLD GetDisplayXld(object element)
        {
            return null;
        }

        #region ColorName

        public string ColorName
        {
            get => (string)GetValue(ColorNameProperty);
            set => SetValue(ColorNameProperty, value);
        }

        public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register(
            "ColorName", typeof(string), typeof(HalconViewerSeries), new FrameworkPropertyMetadata("cyan"));

        #endregion

        #region LineWidth

        public int LineWidth
        {
            get => (int)GetValue(LineWidthProperty);
            set => SetValue(LineWidthProperty, value);
        }

        public static readonly DependencyProperty LineWidthProperty = DependencyProperty.Register(
            "LineWidth", typeof(int), typeof(HalconViewerSeries), new FrameworkPropertyMetadata(4));

        #endregion

        #region RegionFillMode
        
        public HRegionFillMode RegionFillMode
        {
            get => (HRegionFillMode)GetValue(RegionFillModeProperty);
            set => SetValue(RegionFillModeProperty, value);
        }

        public static readonly DependencyProperty RegionFillModeProperty = DependencyProperty.Register(
            "RegionFillMode", typeof(HRegionFillMode), typeof(HalconViewerSeries),
            new FrameworkPropertyMetadata(HRegionFillMode.margin));

        #endregion

        #region DisplayEnabled

        public bool DisplayEnabled
        {
            get => (bool)GetValue(DisplayEnabledProperty);
            set => SetValue(DisplayEnabledProperty, value);
        }

        public static readonly DependencyProperty DisplayEnabledProperty = DependencyProperty.Register(
            "DisplayEnabled", typeof(bool), typeof(HalconViewerSeries));

        #endregion
    }
}