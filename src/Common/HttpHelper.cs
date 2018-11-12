using System;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class HttpHelper
    {
        #region Get

        public static object GET(string url)
        {
            return GET(url, string.Empty);
        }

        public static object GET(string url, string fileName)
        {
            string cookieStr = string.Empty;
            return GET(url, string.Empty, Encoding.UTF8, ref cookieStr, fileName);
        }

        public static object GET(string url, string referer, Encoding encoder, ref string cookieStr, string fileName)
        {
            string result = "";
            WebClient myClient = new WebClient();
            myClient.Encoding = Encoding.UTF8;
            if (cookieStr != "")
            {
                myClient.Headers.Add(cookieStr);
            }
            myClient.Encoding = encoder;
            if (string.IsNullOrEmpty(fileName))
            {
                result = myClient.DownloadString(url);
            }
            else
            {
                myClient.DownloadFile(url, fileName);
                result = string.Empty;
            }
            return result;
        }
        #endregion

        #region Post

        public static object Post(string url, string pamas, string fileName)
        {
            object result = null;
            WebClient myClient = new WebClient();
            myClient.Encoding = Encoding.UTF8;
            result = myClient.UploadFile(url + "?" + pamas, fileName);
            return result;
        }

        public static object Post(string url, string pamas, byte[] data)
        {
            object result = null;
            WebClient myClient = new WebClient();
            myClient.Encoding = Encoding.UTF8;
            result = myClient.UploadData(url + "?" + pamas, data);
            return result;
        }

        public static string POST(string url, string pamas)
        {
            string cookieStr = string.Empty;
            return POST(url, pamas, string.Empty, Encoding.UTF8, ref cookieStr);
        }

        public static string POST(string url, string pamas, string referer, Encoding encoder, ref string cookieStr)
        {
            string result = "";
            WebClient myClient = new WebClient();
            myClient.Encoding = Encoding.UTF8;
            if (cookieStr != "")
            {
                myClient.Headers.Add(cookieStr);
            }
            myClient.Encoding = encoder;
            result = myClient.UploadString(url, pamas);
            return result;
        }
        #endregion

        #region 私有方法
        private static string GetCookie(string CookieStr)
        {
            string result = "";
            string[] myArray = CookieStr.Split(',');
            if (myArray.Length > 0)
            {
                result = "Cookie: ";
                foreach (var str in myArray)
                {
                    string[] CookieArray = str.Split(';');
                    result += CookieArray[0].Trim();
                    result += "; ";
                }
                result = result.Substring(0, result.Length - 2);
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 获取HttpRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetRequest(string url, string param, string method)
        {
            string reStr = "";
            if (method.ToUpper() == "GET" && !string.IsNullOrEmpty(param))
            {
                url = url + "?" + param;
            }
            HttpWebRequest hwrq = (HttpWebRequest)WebRequest.Create(url);
            hwrq.Method = method.ToUpper();
            hwrq.KeepAlive = true;
            if (method.ToUpper() == "POST" && !string.IsNullOrEmpty(param))
            {
                ServicePointManager.Expect100Continue = false;
                hwrq.ContentType = "application/x-www-form-urlencoded";
                byte[] bs = Encoding.UTF8.GetBytes(param);
                hwrq.ContentLength = bs.Length;
                using (Stream reqStream = hwrq.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
            }
            hwrq.Timeout = 60000;
            using (HttpWebResponse hwrp = (HttpWebResponse)hwrq.GetResponse())
            {
                while (hwrp.StatusCode != HttpStatusCode.OK)
                {
                    System.Threading.Thread.Sleep(500);
                }
                Stream st = hwrp.GetResponseStream();
                reStr = new StreamReader(st, Encoding.UTF8).ReadToEnd();
                st.Close();
                st.Dispose();
            }
            return reStr;
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookies(string url)
        {
            string result = string.Empty;
            HttpWebRequest hwrq = (HttpWebRequest)WebRequest.Create(url);
            hwrq.Method = "GET";
            hwrq.KeepAlive = true;
            hwrq.CookieContainer = new CookieContainer();
            hwrq.AllowAutoRedirect = false;
            using (HttpWebResponse hwrp = (HttpWebResponse)hwrq.GetResponse())
            {
                if (hwrp.Headers.Count > 0)
                {
                    result = hwrp.Headers["Set-Cookie"];
                }
            }
            return result;
        }


    }
}
