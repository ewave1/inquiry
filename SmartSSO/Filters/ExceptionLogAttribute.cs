using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InquiryDemo.Filters
{
    /// <summary>
    /// 异常持久化类
    /// </summary>
    public class ExceptionLogAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 触发异常时调用的方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {

            //string message = string.Format("消息类型：{0}<br>消息内容：{1}<br>引发异常的方法：{2}<br>引发异常的对象：{3}<br>异常目录：{4}<br>异常文件：{5}"
            //    , filterContext.Exception.GetType().Name
            //    , filterContext.Exception.Message
            //    , filterContext.Exception.TargetSite
            //    , filterContext.Exception.Source
            //    , filterContext.RouteData.GetRequiredString("controller")
            //    , filterContext.RouteData.GetRequiredString("action"));
            LogManager.GetLogger("global").Fatal(new LogException {
                Controller = filterContext.RouteData.GetRequiredString("controller"),
                Action = filterContext.RouteData.GetRequiredString("action"),
                Source = filterContext.Exception.Source,
                TargetSite = filterContext.Exception.TargetSite?.ToString(),
                Message= filterContext.Exception.Message,
                TypeName = filterContext.Exception.GetType().Name,
                Exception = filterContext.Exception
            });
            base.OnException(filterContext);
        }

        public class LogException
        {
            public string Controller { get; set; }
            public string Action { get; set; }

            public string Source { get; set; }

            public string TargetSite { get; set; }
            public string Message { get; set; }

            public string TypeName { get; set; }

            public Exception Exception { get; set; }
            public override string ToString()
            {
                return string.Format("Controller:{0}\r\n" +
                    "Action:{1}\r\n" +
                    "Source:{2}\r\n" +
                    "TargetSite:{3}\r\n" +
                    "Message:{4}\r\n" +
                    "TypeName:{5}\r\n" +
                    "Exception:{6}", Controller,Action,Source,TargetSite,Message,TypeName,Exception.ToString ());
            }
        }
    }
}