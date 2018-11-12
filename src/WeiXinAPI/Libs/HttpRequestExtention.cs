/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：42cc25ad-791f-48e5-a42b-09f314423c54
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs.Libs
 * 类名称：HttpRequestExtention
 * 文件名：HttpRequestExtention
 * 创建年份：2015
 * 创建时间：2015-11-18 9:16:02
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WeiXinAPIs.Libs
{
    internal static class HttpRequestExtention
    {
        /// <summary>
        /// 微信Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Get<T>(this BaseAPI b, string url)
        {
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                return Common.JsonHelper.JsonToObj<T>(webClient.DownloadString(url));
            }
        }
        /// <summary>
        /// 微信Get请求,下载文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void Get(this BaseAPI b, string url,string fileName)
        {
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                webClient.DownloadFile(url, fileName);
            }
        }
        /// <summary>
        /// 微信Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Post<T>(this BaseAPI b, string url, object data)
        {
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                return Common.JsonHelper.JsonToObj<T>(webClient.UploadString(url, "POST", Common.JsonHelper.ObjToJson(data)));
            }
        }
        /// <summary>
        /// 微信Post请求,上传文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T Post<T>(this BaseAPI b, string url, byte[] file)
        {
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                return Common.JsonHelper.JsonToObj<T>(Encoding.UTF8.GetString(webClient.UploadData(url, "POST", file)));
            }
        }

        /// <summary>
        /// 微信Post请求,上传文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="b"></param>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static T Post<T>(this BaseAPI b, string url, string file)
        {
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                return Common.JsonHelper.JsonToObj<T>(Encoding.UTF8.GetString(webClient.UploadFile(url, "POST", file)));
            }
        }
    }
}
