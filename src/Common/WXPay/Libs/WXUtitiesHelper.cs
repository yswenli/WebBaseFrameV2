/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.WXPay.Libs
 *文件名：  WXUtitiesHelper
 *版本号：  V1.0.0.0
 *唯一标识：86eb3386-f464-4ef6-ad29-17d3921c4d05
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：ysyswenli@outlook.com
 *创建时间：2015/7/28 14:28:53
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/28 14:28:53
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.WXPay.Libs
{
    public class WXUtitiesHelper
    {
        /// <summary>
        /// 微信签名用MD5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset">默认为GB2312</param>
        /// <returns></returns>
        public static string MD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }


        public static string getNoncestr()
        {
            Random random = new Random();
            return MD5(random.Next(1000).ToString(), "GBK");
        }


        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 取时间戳生成随即数,替换交易单号中的后10位流水号
        /// </summary>
        /// <returns></returns>
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }
        /// <summary>
        /// 取随机数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BuildRandomStr(int length)
        {
            Random rand = new Random();

            int num = rand.Next();

            string str = num.ToString();

            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str.Insert(0, "0");
                    n--;
                }
            }
            return str;
        }



        public static void Log(string filePath, string message)
        {
            File.AppendAllText(filePath, message + "\r\n----------------------------------------\r\n", Encoding.GetEncoding("utf-8"));
        }
        public static void Log(string message)
        {
            var filePath = System.Web.HttpContext.Current.Server.MapPath("") + "\\Log.txt";
            File.AppendAllText(filePath, message + "\r\n----------------------------------------\r\n", Encoding.GetEncoding("utf-8"));
        }
    }
}
