
using System;
namespace Common
{
    /// <summary>
    /// 判断客户端是PC还是移动
    /// </summary>
    public class ClientTypeHelper
    {
        /// <summary>
        /// 判断客户端是否是移动端的
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsMobileDevice(System.Web.HttpContext context)
        {
            var request = context.Request;
            string strUserAgent = request.UserAgent.ToString().ToLower();
            if (request.Browser.IsMobileDevice || strUserAgent.Contains("iphone") ||
                strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
                strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") ||
                strUserAgent.Contains("palm"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测浏览器类型
        /// </summary>
        /// <returns></returns>
        public static bool IsIE()
        {
            string USER_AGENT = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            if (USER_AGENT.IndexOf("MSIE") < 0) return false;

            string version = USER_AGENT.Substring(USER_AGENT.IndexOf("MSIE") + 5, 1);
            var outInt = 0;
            if (int.TryParse(version, out outInt))
            {
                return false;
            }
            return true;
        }
    }
}
