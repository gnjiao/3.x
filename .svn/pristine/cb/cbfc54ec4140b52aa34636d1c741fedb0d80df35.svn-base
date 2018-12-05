using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core.Collections;
using Core.Toolkit.Collections;
using Platform.Main.Annotations;
using Platform.Main.Util;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="ILogView" />
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl, ILogView, INotifyPropertyChanged
    {
        private const int LogListCount = 3000;
        public AsyncObservableCollectionReadOnlyCopy<LogInfo> LogList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public LogView()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                if (LogList == null)
                    LogList = new AsyncObservableCollectionReadOnlyCopy<LogInfo>();
                
                PlatformServiceTools.Log.OnLoggingEvent += Log_OnLoggingEvent;
            };

            Unloaded += (sender, e) =>
            {
                PlatformServiceTools.Log.OnLoggingEvent -= Log_OnLoggingEvent;
            };
        }

        private void Log_OnLoggingEvent(object sender, Core.LoggingArgs args)
        {
            try
            {
                var newInfo = new LogInfo
                {
                    EvtType = args.EvtType.ToString(),
                    Message = args.Message,
                    Time = DateTime.Now
                };

                lock (newInfo)
                {
                    if (LogList.Count > LogListCount)
                    {
                        lock (LogList)
                        {
                            LogList.UnsafeRemoveAt(LogList.Count - 1);
                        }                        
                    }

                    LogList.InsertAsFirst(newInfo);
                }

                OnPropertyChanged(nameof(LogList));
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    
}
