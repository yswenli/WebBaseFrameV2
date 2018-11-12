/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.WXPay
 *文件名：  WXPayHelper
 *版本号：  V1.0.0.0
 *唯一标识：595b05ce-9731-4385-ac04-029383ba44a0
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：yswenli@outlook.com
 *创建时间：2015/7/27 14:47:39
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/27 14:47:39
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.WXPay.Models;
using Newtonsoft.Json;
using System.Xml;

namespace Common.WXPay
{
    public class WXPayHelper
    {

        public WXPayHelper(string code, string appId, string appkey, string appsecret, string order_no, string mchid, string timeStamp, string nonceStr, string partnerid, string notify_url)
        {
            WXAPP wxApp = new WXAPP()
            {
                AppId = appId,
                AppSecret = appsecret
            };
            //
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", wxApp.AppId, wxApp.AppSecret, code);
            string returnStr = Libs.HttpHelper.Send(url);
            var wxBase = JsonConvert.DeserializeObject<WXBase>(returnStr);

            //
            url = string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}",
                    appId, wxBase.refresh_token);
            returnStr = Libs.HttpHelper.Send(url, string.Empty);
            wxBase = JsonConvert.DeserializeObject<WXBase>(returnStr);
            //
            url = string.Format(
                    "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
                    wxBase.access_token, wxBase.openid);
            returnStr = Libs.HttpHelper.Send(url);
            //
            //当前时间 yyyyMMdd
            string date = DateTime.Now.ToString("yyyyMMdd");

            if (String.IsNullOrEmpty(order_no))
            {
                //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                order_no = DateTime.Now.ToString("HHmmss") + Libs.WXUtitiesHelper.BuildRandomStr(4);
            }
            order_no = partnerid + order_no;
            //

            //创建支付应答对象
            var httpHelper = new Libs.HttpHelper(System.Web.HttpContext.Current);
            httpHelper.AddParameter("body", "test"); //商品信息 127字符
            httpHelper.AddParameter("appid", appId);
            httpHelper.AddParameter("mch_id", mchid);
            httpHelper.AddParameter("nonce_str", nonceStr.ToLower());
            httpHelper.AddParameter("notify_url", notify_url);
            httpHelper.AddParameter("openid", wxBase.openid);
            httpHelper.AddParameter("out_trade_no", order_no); //商家订单号
            httpHelper.AddParameter("spbill_create_ip", System.Web.HttpContext.Current.Request.UserHostAddress);
            httpHelper.AddParameter("total_fee", "1"); //商品金额,以分为单位(money * 100).ToString()
            httpHelper.AddParameter("trade_type", "JSAPI");
            //获取package包
            var sign = httpHelper.GetSign("key", appkey);
            httpHelper.AddParameter("sign", sign);
            string data = httpHelper.parseXML();
            string prepayXml = Libs.HttpHelper.Send("https://api.mch.weixin.qq.com/pay/unifiedorder", data);
            //获取预支付ID
            string prepayId = string.Empty;
            string package = string.Empty;
            var xdoc = new XmlDocument();
            xdoc.LoadXml(prepayXml);
            XmlNode xn = xdoc.SelectSingleNode("xml");
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl.Count > 7)
            {
                prepayId = xnl[7].InnerText;
                package = string.Format("prepay_id={0}", prepayId);
                Libs.WXUtitiesHelper.Log(package);
            }
            //设置支付参数
            httpHelper = new Libs.HttpHelper(System.Web.HttpContext.Current);
            httpHelper.AddParameter("appId", appId);
            httpHelper.AddParameter("timeStamp", timeStamp);
            httpHelper.AddParameter("nonceStr", nonceStr);
            httpHelper.AddParameter("package", package);
            httpHelper.AddParameter("signType", "MD5");
            var paySign = httpHelper.GetSign("key", appkey);
            Libs.WXUtitiesHelper.Log("paySign:" + paySign);
        }

        public string GetTimeStamp()
        {
            return Libs.WXUtitiesHelper.getTimestamp();
        }

        public string GetNoncestr()
        {
            return Libs.WXUtitiesHelper.getTimestamp();
        }

    }
}
