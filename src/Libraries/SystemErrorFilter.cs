using System;
using System.Web.Mvc;

namespace Libraries
{
    /// <summary>
    /// 自定义服务器错误处理页
    /// </summary>
    public class SystemErrorFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            try
            {
                base.OnException(context);
                context.HttpContext.Application["error"] = "";
                if (!context.ExceptionHandled)
                    return;
                context.HttpContext.Application["error"] ="详细："+ context.Exception.Message;
            }
            catch(Exception ex)
            {
                string fileName = DateTime.Now.ToString().Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "")+".txt";
                Common.FileHelper.AppendFile(context.HttpContext.Server.MapPath("/ErrorLogs/") + fileName, ex.Message);
            }
        }
    }
}