using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Core.Boot
{
    public class BootstrapperRunner
    {
        public UnityContainer Container { get; set; } = new UnityContainer();

        private readonly IList<IBootstrapperExtension> _extensions = new List<IBootstrapperExtension>();
        private readonly IList<Type> _extensionTypes = new List<Type>();

        private Action<Exception> _exceptionHandler;

        private void RunAll()
        {
            foreach (var extType in _extensionTypes)
            {
                var extension = Container.Resolve(extType) as IBootstrapperExtension;
                _extensions.Add(extension);
            }

            foreach (var extension in _extensions)
            {
                extension.Initialize(Container);
            }
        }

        public BootstrapperRunner Run()
        {
#if (DEBUG)
            RunInDebugMode();
#else
            RunInReleaseMode();
#endif
            return this;
        }

        private void RunInDebugMode()
        {
            RunAll();
        }

        private void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException(e.ExceptionObject as Exception);

            try
            {
                RunAll();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            _exceptionHandler?.Invoke(ex);
        }
//
//        public interface IHandlerException
//        {
//            void Handle(Action<Exception> exceptionHandler);
//        }
//
//        private class HandlerException : IHandlerException
//        {
//            private readonly BootstrapperRunner _bootstrapperRunner;
//
//            public HandlerException(BootstrapperRunner bootstrapperRunner)
//            {
//                _bootstrapperRunner = bootstrapperRunner;
//            }
//
//            public void Handle(Action<Exception> exceptionHandler)
//            {
//                _bootstrapperRunner._exceptionHandler = exceptionHandler;
//            }
//        }

        public BootstrapperRunner AddExceptionHandler(Action<Exception> exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
            return this;
        }

        public BootstrapperRunner AddExtension<TExtension>() where TExtension : IBootstrapperExtension
        {
            _extensionTypes.Add(typeof(TExtension));
            return this;
        }

        public BootstrapperRunner AddExtension(IBootstrapperExtension extension)
        {
            _extensions.Add(extension);
            return this;
        }
    }
}