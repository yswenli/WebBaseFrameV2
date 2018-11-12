using System;
using System.Web;

namespace Common
{
    /// <summary>
    /// 日志辅助类
    /// </summary>
    public class LogHelper
    {
        #region 一天一文件
        /// <summary>
        /// 简单记录内容
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            LogHelper.Log(message, HttpContext.Current.Server.MapPath("/Logs/"));
        }
        /// <summary>
        /// 指定目录记录内容
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message, string path)
        {
            if (path.LastIndexOf("\\") < path.Length) path += "\\";
            DirHelper.CheckFolder(path);
            FileHelper.AppendFile(path + "logs" + DateTime.Now.ToString("yyyyMMdd") + ".htm", "<strong>客户机IP</strong>：" + HttpContext.Current.Request.UserHostAddress + "<br /><strong>错误地址</strong>：" + HttpContext.Current.Request.Url + message + Environment.NewLine);
        }

        /// <summary>
        /// 根据异常记录日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex)
        {
            LogHelper.Log(ex, HttpContext.Current.Server.MapPath("/Logs/"));
        }
        /// <summary>
        /// 指定目录根据异常记录日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="path"></param>
        public static void Log(Exception ex ,string path)
        {
            if (path.LastIndexOf("\\") < path.Length) path += "\\";
            DirHelper.CheckFolder(path);
            FileHelper.AppendFile(path + "log" + DateTime.Now.ToString("yyyyMMdd") + ".htm", "<strong>客户机IP</strong>：" + HttpContext.Current.Request.UserHostAddress + "<br /><strong>错误地址</strong>：" + HttpContext.Current.Request.Url + ex.Message + Environment.NewLine);
        }
        #endregion


        #region 一记录一文件
        public static void ErrorLog(string info)
        {
            var path = HttpContext.Current.Server.MapPath("/log/");
            DirHelper.CheckFolder(path);
            FileHelper.WriteFile(path + "log" + DateTime.Now.ToString("yyyyMMddHHmmss") + Character.Random(1000, 9999) + ".txt", "<br/><strong>客户机IP</strong>：" + HttpContext.Current.Request.UserHostAddress + "<br /><strong>错误地址</strong>：" + HttpContext.Current.Request.Url + info);
        }
        public static void ErrorLog(Exception ex)
        {
            var path = HttpContext.Current.Server.MapPath("/log/");
            DirHelper.CheckFolder(path);
            var info = "<br/><strong>客户机IP</strong>：" + HttpContext.Current.Request.UserHostAddress + "<br /><strong>错误地址</strong>：" + HttpContext.Current.Request.Url;
            FileHelper.WriteFile(path + "log" + DateTime.Now.ToString("yyyyMMddHHmmss") + Character.Random(1000, 9999) + ".txt", info + ";异常信息：" + ex.Message);
        }
        #endregion
    }
}
