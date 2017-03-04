using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Utils
{
    public class LogUtility
    {
        static readonly ILog logger;
        static LogUtility()
        {
            logger = LogManager.GetLogger(Assembly.GetEntryAssembly().DefinedTypes.First());


        }

        public static void Info(object message)
        {
            logger.Info(message);
           
        }

        public static void Warn(object message)
        {
            logger.Warn(message);
            
        }

        public static void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
           
        }

        public static void Fatal(object message,Exception exception)
        {
            logger.Fatal(message,exception);
        }


    }
}
