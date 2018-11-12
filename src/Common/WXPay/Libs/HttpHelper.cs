/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.WXPay.Libs
 *文件名：  HttpHelper
 *版本号：  V1.0.0.0
 *唯一标识：306adce6-ff5e-4c95-a535-33f8387f8268
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：yswenli@outlook.com
 *创建时间：2015/7/27 14:50:49
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/27 14:50:49
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common.WXPay.Libs
{
    /// <summary>
    /// WXPay定制
    /// </summary>
    public class HttpHelper
    {
        private const string sContentType = "application/x-www-form-urlencoded";

        private const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public static string Send(string url)
        {
            return Send(url, string.Empty);
        }

        public static string Send(string url, string data)
        {
            return Send(url, Encoding.GetEncoding("UTF-8").GetBytes(data));
        }

        public static string Send(string url, byte[] data)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                // request.UserAgent = sUserAgent;  
                request.ContentType = sContentType;
                request.Method = "POST";
                if (data != null && data.Length > 0)
                {
                    request.ContentLength = data.Length;
                }
                using (Stream requestStream = request.GetRequestStream())
                {
                    if (data != null && data.Length > 0)
                    {
                        requestStream.Write(data, 0, data.Length);
                    }
                    using (Stream responseStream = request.GetResponse().GetResponseStream())
                    {

                        using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                        {
                            result = reader.ReadToEnd();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }




        public HttpHelper(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        /// <summary>
        /// 引用的页面Context
        /// </summary>
        HttpContext _httpContext;

        /// <summary>
        /// 封装的请求的参数
        /// </summary>
        protected Hashtable parameters;

        /// <summary>
        /// 封装参数集
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void AddParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }
                parameters.Add(parameter, parameterValue);
            }
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="key"></param>
        /// <param name="appkey">paysignkey(非appkey 在微信商户平台设置</param>
        /// <returns></returns>
        public string GetSign(string key, string appkey)
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }
            sb.Append(key + "=" + appkey);
            return WXUtitiesHelper.MD5(sb.ToString(), _httpContext.Request.ContentEncoding.BodyName).ToUpper();
        }

        /// <summary>
        /// 将封装的参数输出XML
        /// </summary>
        /// <returns></returns>
        public string parseXML()
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            var akeys = new ArrayList(parameters.Keys);
            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                if (Regex.IsMatch(v, @"^[0-9.]$"))
                {
                    sb.Append("<" + k + ">" + v + "</" + k + ">");
                }
                else
                {
                    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }
       

    }
}
