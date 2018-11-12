/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common
 *文件名：  IPHelper
 *版本号：  V1.0.0.0
 *唯一标识：d3276b1f-5b1d-48e8-b223-05cb2d963f49
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：ysyswenli@outlook.com
 *创建时间：2015/8/3 16:32:39
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/8/3 16:32:39
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/

using System;
using System.IO;
using System.Net;
using System.Text;

namespace Common
{
    public class IpHelper
    {
        public class IP
        {
            public String Ret { get; set; }

            public String Start { get; set; }

            public String End { get; set; }

            public String Country { get; set; }

            public String Province { get; set; }

            public String City { get; set; }

            public String District { get; set; }

            public String Isp { get; set; }

            public String Type { get; set; }

            public String Desc { get; set; }
        }


        /// <summary>
        /// 获取IP地址的详细信息，调用的借口为
        /// http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip={ip}
        /// </summary>
        /// <param name="ipAddress">请求分析得IP地址</param>
        /// <param name="sourceEncoding">服务器返回的编码类型</param>
        /// <returns>IpUtils.IpDetail</returns>
        public static IP Get(String ipAddress, Encoding sourceEncoding)
        {
            String ip = string.Empty;
            if (sourceEncoding == null)
                sourceEncoding = Encoding.UTF8;
            using (var receiveStream = WebRequest.Create("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip=" + ipAddress).GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(receiveStream, sourceEncoding))
                {
                    var readbuffer = new char[256];
                    int n = sr.Read(readbuffer, 0, readbuffer.Length);
                    int realLen = 0;
                    while (n > 0)
                    {
                        realLen = n;
                        n = sr.Read(readbuffer, 0, readbuffer.Length);
                    }
                    ip = sourceEncoding.GetString(sourceEncoding.GetBytes(readbuffer, 0, realLen));
                }
            }
            return !string.IsNullOrEmpty(ip) ? JsonHelper.JsonToObj<IP>(ip) : null;
        }
    }

}
