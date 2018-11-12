using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
namespace Common
{
    public class UrlHelper
    {

        private static System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        /// <summary>
        /// 当前域名
        /// </summary>
        public static string HostUrl
        {
            get
            {
                if (CurrentUrl.IndexOf("https:") != -1)
                    return "https://" + HttpContext.Current.Request.Url.Authority.ToLower();
                else
                    return "http://" + HttpContext.Current.Request.Url.Authority.ToLower();
            }
        }
        /// <summary>
        /// 当前url
        /// </summary>
        public static string CurrentUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.ToString().ToLower();
            }
        }
        /// <summary>
        /// 上个url
        /// </summary>
        public static string BackUrl
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
                }
                catch
                {
                    return CurrentUrl.ToLower();
                }
            }
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        public static string IP
        {
            get
            {
                string userIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (userIP == null || userIP == "")
                {
                    userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return userIP;
            }
        }

        #region URL编码解码
        public static string Encode(string parmas)
        {
            return HttpUtility.UrlEncode(parmas);
        }
        public static string Decode(string parmas)
        {
            return HttpUtility.UrlDecode(parmas);
        }
        #endregion



        #region URLBase64加密
        public static string Base64Encrypt(string sourthUrl)
        {
            string eurl = HttpUtility.UrlEncode(sourthUrl);
            eurl = Convert.ToBase64String(encoding.GetBytes(eurl));
            return eurl;
        }
        #endregion

        #region URLBase64解密
        public static string Base64Decrypt(string eStr)
        {
            if (!IsBase64(eStr))
            {
                return eStr;
            }
            byte[] buffer = Convert.FromBase64String(eStr);
            string sourthUrl = encoding.GetString(buffer);
            sourthUrl = HttpUtility.UrlDecode(sourthUrl);
            return sourthUrl;
        }
        /// <summary>  
        /// 判断是否是Base64编码
        /// </summary>  
        /// <param name="eStr"></param>  
        /// <returns></returns>  
        public static bool IsBase64(string eStr)
        {
            if ((eStr.Length % 4) != 0)
            {
                return false;
            }
            if (!Regex.IsMatch(eStr, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }
        #endregion


        #region URL 参数处理

        /// <summary>  
        /// URL中添加参数
        /// </summary>  
        public static string AddParam(string url, string paramName, string value)
        {
            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query))
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "?" + paramName + "=" + eval);
            }
            else
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "&" + paramName + "=" + eval);
            }
        }
        /// <summary>  
        /// 更新URL中的参数
        /// </summary>  
        public static string UpdateParam(string url, string paramName, string value)
        {
            string keyWord = paramName + "=";
            int index = url.IndexOf(keyWord) + keyWord.Length;
            int index1 = url.IndexOf("&", index);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, value);
                return url;
            }
            url = url.Remove(index, index1 - index);
            url = url.Insert(index, value);
            return url;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetPamas<T>(T t)
        {
            var result = string.Empty;
            try
            {
                var properties = t.GetType().GetProperties();
                foreach (PropertyInfo item in properties)
                {
                    var value = item.GetValue(t, null) == null ? "" : item.GetValue(t, null).ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = Common.Character.UrlEscape(value);
                        result += item.Name + "=" + value + "&";
                    }
                }
                if (result.Substring(result.Length - 1) == "&")
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 将string分成字典
        /// </summary>
        /// <param name="pamas"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(string pamas)
        {
            Dictionary<string, string> plts = new Dictionary<string, string>();
            var parray = pamas.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parray.Length; i++)
            {
                var itemArray = parray[i].Split(new string[] { "=" }, StringSplitOptions.None);
                plts.Add(itemArray[0], itemArray[1]);
            }
            return plts;
        }

        #endregion
    }
}
