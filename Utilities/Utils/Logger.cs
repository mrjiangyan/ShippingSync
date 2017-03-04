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
    public class Logger
    {
        static Logger()
        {
            //log4net.Config.XmlConfigurator.Configure();

        }

        public static void Save()
        {
            var logger = LogManager.GetLogger(Assembly.GetEntryAssembly().DefinedTypes.First());

            logger.Info("消息");
            logger.Warn("警告");
            logger.Error("异常");
            logger.Fatal("错误");
        }
    }
}
