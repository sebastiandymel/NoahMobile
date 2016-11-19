using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    public static class Log
    {
        public enum Levels
        {
            Debug = 0,
            Info,
            Warning,
            Error
        }

        public delegate void LogCallback(Levels level, string message);


        private static LogCallback _callback = null;
        private static Levels _level;

        public static void Initialize(LogCallback callback, Levels level = Levels.Debug)
        {
            bool alreadyInitialized = _callback != null;
            if (alreadyInitialized)
                Write(Levels.Warning,
                    "Himsa.Noah.MobileAccessLayer.Log::Initialize: Function is called again. Destination of log information might have changed.");
            _callback = callback;
            _level = level;
            if (alreadyInitialized)
                Write(Levels.Warning, "Himsa.Noah.MobileAccessLayer.Log::Initialize: Function was already called.");
        }

        public static Levels Level
        {
            get { return _level; }
            set { _level = value; }
        }

        private static void Write(Levels level, string message)
        {
            if (_callback != null && level >= _level)
                _callback(level, message);
        }

        private static void Format(Levels level, string message, params object[] args)
        {
            string str = string.Format(message, args);
            Write(level, str);
        }

        #region Debug

        public static void Debug(string message)
        {
            Write(Levels.Debug, message);
        }

        public static void DebugFormat(string message, params object[] args)
        {
            Format(Levels.Debug, message, args);
        }

        #endregion //Debug

        #region Info

        public static void Info(string message)
        {
            Write(Levels.Info, message);
        }

        public static void InfoFormat(string message, params object[] args)
        {
            Format(Levels.Info, message, args);
        }

        #endregion //Info

        #region Warning

        public static void Warning(string message)
        {
            Write(Levels.Warning, message);
        }

        public static void WarningFormat(string message, params object[] args)
        {
            Format(Levels.Warning, message, args);
        }

        #endregion //Warning

        #region Error

        public static void Error(string message)
        {
            Write(Levels.Error, message);
        }

        public static void ErrorFormat(string message, params object[] args)
        {
            Format(Levels.Error, message, args);
        }

        #endregion //Error
    }
}
