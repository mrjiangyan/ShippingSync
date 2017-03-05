using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helper;

namespace Utilities.Utils
{
    public class LogUtility
    {
        private static ILog _logdebug = LogManager.GetLogger("logdebug");
        private static ILog _loginfo = LogManager.GetLogger("loginfo");
        private static ILog _logwarn = LogManager.GetLogger("logwarn");
        private static ILog _logerror = LogManager.GetLogger("logerror");
        private static ILog _logfatal = LogManager.GetLogger("logfatal");


        public static void Debug(string msg)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logdebug.Debug(msg);
        }


        public static void Debug(string msg, Exception e)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logdebug.Debug(msg, e);
        }


       

        public static void Info(object obj)
        {
            string msg = "";
            if (obj is string)
                msg = obj.ToString();
            else
            {
                msg = JsonHelper.JsonObjectSerialize(obj);
            }
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _loginfo.Info(msg);
        }
        public static void Info(string msg, Exception e)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _loginfo.Info(msg, e);
        }




        public static void Warn(string msg)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logwarn.Warn(msg);
        }
        public static void Warn(string msg, Exception e)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logwarn.Warn(msg, e);
        }


        public static void Error(string msg)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logerror.Error(msg);
        }


        public static void Error(string msg, Exception e)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logerror.Error(msg, e);
        }


        public static void Fatal(string msg)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logfatal.Fatal(msg);
        }


        public static void Fatal(string msg, Exception e)
        {
            StackFrame sf = new StackFrame(1);
            msg = string.Format("{0}.{1}:{2}", sf.GetMethod().ReflectedType.FullName, sf.GetMethod().Name, msg);
            _logfatal.Fatal(msg, e);
        }


    }
}
