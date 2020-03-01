using NLog;
using System;

namespace Logger
{
    class Log
    {
        private static Log log = null;
        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public static void WriteLog(string message)
        {
            if (log == null)
                log = new Log();

            logger.Info(message);
        }

        private Log()
        {
            string format = @"Log\Log: " + DateTime.Now.ToShortDateString() + ".txt";
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = format,
                Layout = "${longdate} ${message}"
            };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
        }
    }
}
