using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Platform.Main.Services;
using Platform.Main.Util;

namespace Platform.Main.Startup
{
    using Core;

    public static class Bootstrapper
    {
        [STAThread]
        public static void Main(string[] args)
        {
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif            
            var exe = typeof(Bootstrapper).Assembly;
            FileUtility.ApplicationRootPath = Path.GetDirectoryName(exe.Location);

            var splashScreen = new SplashScreen("SplashScreen.png");
            splashScreen.Show(false);

            Console.WriteLine("Starting Core Services...");

            var container = new PlatformServiceContainer();
            container.AddFallbackProvider(ServiceSingleton.FallbackServiceProvider);
            container.AddService(typeof(ILoggingService), new Log4NetLoggingService());
            ServiceSingleton.ServiceProvider = container;

            var coreStartup = new CoreStartup("CSharp.Core.Framework 3.0.1");
            var configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "addin");
            var dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "data");
            var propertyService = new Services.PropertyService(DirectoryName.Create(configDirectory), DirectoryName.Create(dataDirectory),"custom");            
            coreStartup.StartCoreServices(propertyService);

            PlatformServiceTools.ResourceService.Language = "en-US"; //language: zh-Hans simple chinese, en-US english
            PlatformServiceTools.ResourceService.RegisterNeutralStrings(new ResourceManager("Platform.Main.Properties.StringResources", exe));
            PlatformServiceTools.ResourceService.RegisterNeutralImages(new ResourceManager("Platform.Main.Properties.ImageResuorces", exe));

            CommandWrapper.LinkCommandCreator = link => new LinkCommand(link);
            CommandWrapper.WellKnownCommandCreator = Core.Presentation.MenuService.GetKnownCommand;
            CommandWrapper.RegisterConditionRequerySuggestedHandler = (eh => CommandManager.RequerySuggested += eh);
            CommandWrapper.UnregisterConditionRequerySuggestedHandler = (eh => CommandManager.RequerySuggested -= eh);
            
            Console.WriteLine("Looking for AddIns...");

            // Searches for ".addin" files in the application directory.
            coreStartup.AddAddInsFromDirectory(FileUtility.ApplicationRootPath);
            coreStartup.RunInitialization();

            Console.WriteLine("Initializing MainForm...");

            try
            {
                Console.WriteLine("Running application...");
#if WPF
                MainWindow.StartUpMainWindow();

                MainWindow.Instance.Loaded += (sender, e) =>
                {
                    splashScreen.Close(new TimeSpan(10));
                    (sender as MainWindow)?.Activate();
                };

                new App().Run(MainWindow.Instance);
#endif
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                Debug.WriteLine(s);
            }
            finally
            {
                try
                {
                    //coreStartup.RunDestructor();
                    // Save changed properties
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    MessageService.ShowException(ex, "Error storing properties");
                }
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var fullName = Path.GetFullPath("error.log");

            using (var stream = new System.IO.StreamWriter(fullName, true))
            {
                //stream.WriteLine("Crash log for " + Application.ProductName);
                stream.WriteLine("Time of crash: " + DateTime.Now);
                stream.WriteLine();

                var writeMe = (Exception)e.ExceptionObject;
                var first = true;

                while (writeMe != null)
                {
                    if (!first)
                    {
                        stream.WriteLine();
                        stream.Write("Inner ");
                    }

                    stream.WriteLine("Exception details:");
                    stream.WriteLine(writeMe.ToString());
                    writeMe = writeMe.InnerException;
                    first = false;
                }

                stream.WriteLine("------------------------------------------------------------------------------");
            }

            //Utility.ErrorBox(null, "There was an unhandled error, and MainProc must be closed. Refer to '" + fullName + "' for more information.");

            if (!e.IsTerminating)
            {
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
