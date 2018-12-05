using System;
using System.Globalization;
using System.IO;
using Core;
using log4net;
using log4net.Config;

namespace Platform.Main.Services
{
    internal sealed class Log4NetLoggingService : ILoggingService
    {
        private readonly ILog _log;

        public event LoggingEvent OnLoggingEvent;

        public Log4NetLoggingService()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            _log = LogManager.GetLogger(typeof(Log4NetLoggingService));
        }

        public void Debug(object message)
        {
            _log.Debug(message);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Debug, message.ToString(), null));
        }

        public void DebugFormatted(string format, params object[] args)
        {
            _log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Info(object message)
        {
            _log.Info(message);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Info, message.ToString(), null));
        }

        public void InfoFormatted(string format, params object[] args)
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Warn(object message)
        {
            _log.Warn(message);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Warn, message.ToString(), null));
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Warn, message.ToString(), exception.Message));
        }

        public void WarnFormatted(string format, params object[] args)
        {
            _log.WarnFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Error(object message)
        {
            _log.Error(message);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Error, message.ToString(), null));
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Error, message.ToString(), exception.Message));
        }

        public void ErrorFormatted(string format, params object[] args)
        {
            _log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Fatal, message.ToString(), null));
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);

            OnLoggingEvent?.Invoke(this, new LoggingArgs(LogEventType.Fatal, message.ToString(), exception.Message));
        }

        public void FatalFormatted(string format, params object[] args)
        {
            _log.FatalFormat(CultureInfo.InvariantCulture, format, args);
        }

        public bool IsDebugEnabled => _log.IsDebugEnabled;

        public bool IsInfoEnabled => _log.IsInfoEnabled;

        public bool IsWarnEnabled => _log.IsWarnEnabled;

        public bool IsErrorEnabled => _log.IsErrorEnabled;

        public bool IsFatalEnabled => _log.IsFatalEnabled;
    }
}
