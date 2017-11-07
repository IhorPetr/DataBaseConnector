using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace DataBaseMigrator.Infrastructure
{
    public class NLogCore
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void LogAplicationError(string cur)
        {
            logger.Log(LogLevel.Debug,cur);
        }
        public static void LogStatusAplication(string cur)
        {
            logger.Log(LogLevel.Info,cur);
        }
    }
}