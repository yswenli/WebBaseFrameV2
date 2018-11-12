/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.Alipay
 *文件名：  AlipayHelper
 *版本号：  V1.0.0.0
 *唯一标识：e775c160-4201-4867-8363-1bebdd1ed49e
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：yswenli@outlook.com
 *创建时间：2015/7/27 11:36:16
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/27 11:36:16
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Common.Alipay
{
    /// <summary>
    /// 支付宝工具类
    /// </summary>
    public class AlipayHelper
    {
        /// <summary>
        /// 支付宝接口地址
        /// </summary>
        public const string INTERFACE_URL = "https://www.alipay.com/payto:";

        /// <summary>
        /// 卖家支付宝帐号
        /// </summary>
        public const string SELLER_EMAAIL = "wenguoli_520@hotmail.com";

        /// <summary>
        /// 卖家支付宝安全校验码
        /// </summary>
        private string _acCode;
        /// <summary>
        /// 卖家支付宝安全校验码
        /// </summary>
        public string AcCode
        {
            get { return _acCode; }
        }

        private string _url;
        /// <summary>
        /// 带参数的提交接口地址
        /// </summary>
        public string Url { get { return _url; } }

        private Models.Alipay _Alipay;
        /// <summary>
        /// 带参数的交易实体
        /// </summary>
        public Models.Alipay Alipay
        {
            get { return _Alipay; }
        }




        /// <summary>
        /// 支付宝工具类
        /// </summary>
        /// <param name="strCmd">命令字</param>
        /// <param name="strSubject">商品名</param>
        /// <param name="strBody">商品描述</param>
        /// <param name="strOrder_no">商户订单号</param>
        /// <param name="strPrice">商品单价 0.01～50000.00</param>
        /// <param name="rurl">商品展示网址</param>
        /// <param name="strType">type支付类型 1：商品购买2：服务购买3：网络拍卖4：捐赠</param>
        /// <param name="strNumber">购买数量</param>
        /// <param name="strTransport">发货方式 1：平邮2：快递3：虚拟物品</param>
        /// <param name="strOrdinary_fee">平邮运费</param>
        /// <param name="strExpress_fee">快递运费</param>
        /// <param name="strReadOnly">交易信息是否只读</param>
        /// <param name="strBuyer_msg">买家给卖家的留言</param>
        /// <param name="strBuyer">买家EMAIL</param>
        /// <param name="strBuyer_name">买家姓名</param>
        /// <param name="strBuyer_address">买家地址</param>
        /// <param name="strBuyer_zipcode">买家邮编</param>
        /// <param name="strBuyer_tel">买家电话号码</param>
        /// <param name="strBuyer_mobile">买家手机号码</param>
        /// <param name="strPartner">合作伙伴ID    保留字段</param>
        public AlipayHelper(
            string strCmd,
            string strSubject,
            string strBody,
            string strOrder_no,
            decimal strPrice,
            string rurl,
            TypeEnum strType,
            int strNumber,
            TransportEnum strTransport,
            string strOrdinary_fee,
            string strExpress_fee,
            string strReadOnly,
            string strBuyer_msg,
            string strBuyer,
            string strBuyer_name,
            string strBuyer_address,
            string strBuyer_zipcode,
            string strBuyer_tel,
            string strBuyer_mobile,
            string strPartner)
        {
            string acStr = "";
            acStr += "cmd" + strCmd + "subject" + strSubject;
            acStr += "body" + strBody;
            acStr += "order_no" + strOrder_no;
            acStr += "price" + strPrice;
            acStr += "url" + rurl;
            acStr += "type" + strType;
            acStr += "number" + strNumber;
            acStr += "transport" + strTransport;
            acStr += "ordinary_fee" + strOrdinary_fee;
            acStr += "express_fee" + strExpress_fee;
            acStr += "readonly" + strReadOnly;
            acStr += "buyer_msg" + strBuyer_msg;
            acStr += "seller" + SELLER_EMAAIL;
            acStr += "buyer" + strBuyer;
            acStr += "buyer_name" + strBuyer_name;
            acStr += "buyer_address" + strBuyer_address;
            acStr += "buyer_zipcode" + strBuyer_zipcode;
            acStr += "buyer_tel" + strBuyer_tel;
            acStr += "buyer_mobile" + strBuyer_mobile;
            acStr += "partner" + strPartner;

            this._acCode = GetMD5(acStr);

            string url = "";

            url += INTERFACE_URL + SELLER_EMAAIL + "?cmd=" + strCmd;
            url += "&subject=" + UrlHelper.Encode(strSubject);
            url += "&body=" + UrlHelper.Encode(strBody);
            url += "&order_no=" + strOrder_no;
            url += "&url=" + rurl;
            url += "&price=" + strPrice;
            url += "&type=" + strType;
            url += "&number=" + strNumber;
            url += "&transport=" + strTransport;
            url += "&ordinary_fee=" + strOrdinary_fee;
            url += "&express_fee=" + strExpress_fee;
            url += "&readonly=" + strReadOnly;
            url += "&buyer_msg=" + strBuyer_msg;
            url += "&buyer=" + strBuyer;
            url += "&buyer_name=" + UrlHelper.Encode(strBuyer_name);
            url += "&buyer_address=" + strBuyer_address;
            url += "&buyer_zipcode=" + strBuyer_zipcode;
            url += "&buyer_tel=" + strBuyer_tel;
            url += "&buyer_mobile=" + strBuyer_mobile;
            url += "&partner=" + strPartner;
            url += "&ac=" + this._acCode;

            this._url = url;

            _Alipay = new Models.Alipay()
            {
                cmd = strCmd,
                subject = strSubject,
                body = strBody,
                order_no = strOrder_no,
                ordinary_fee = strOrdinary_fee,
                url = rurl,
                price = strPrice,
                type = (int)strType,
                number = strNumber,
                transport = (int)strTransport,
                express_fee = strExpress_fee,
                Readonly = strReadOnly,
                buyer_msg = strBuyer_msg,
                buyer = strBuyer,
                buyer_name = strBuyer_name,
                buyer_address = strBuyer_address,
                buyer_zipcode = strBuyer_zipcode,
                buyer_tel = strBuyer_tel,
                buyer_mobile = strBuyer_mobile,
                partner = strPartner,
                ac = this._acCode
            };
        }


        /// <summary>
        /// 获取验证支付宝服务器发来的请求
        /// </summary>
        /// <param name="msg_id"></param>
        /// <param name="order_no"></param>
        /// <param name="gross"></param>
        /// <param name="buyer_email"></param>
        /// <param name="buyer_name"></param>
        /// <param name="buyer_address"></param>
        /// <param name="buyer_zipcode"></param>
        /// <param name="buyer_tel"></param>
        /// <param name="buyer_mobile"></param>
        /// <param name="action"></param>
        /// <param name="date"></param>
        /// <param name="ac"></param>
        /// <param name="notify_type"></param>
        /// <param name="constPaySecurityCode"></param>
        /// <returns></returns>
        public string NotifyQuery(string msg_id, string order_no, string gross, string buyer_email, string buyer_name, string buyer_address, string buyer_zipcode, string buyer_tel, string buyer_mobile, string action, string date, string ac, string notify_type, string constPaySecurityCode)
        {
            string returnTxt = "N";
            //支付宝查询接口URL
            string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?" + "msg_id=" + msg_id + "&email=" + SELLER_EMAAIL + "&order_no=" + order_no; ;
            string responseTxt = GetResponse(alipayNotifyURL, 120000);
            string acStr = "msg_id" + msg_id + "order_no" + order_no + "gross" + gross + "buyer_email" + buyer_email + "buyer_name" + buyer_name + "buyer_address" + buyer_address + "buyer_zipcode" + buyer_zipcode + "buyer_tel" + buyer_tel + "buyer_mobile" + buyer_mobile + "action" + action + "date" + date + constPaySecurityCode;
            string ac_code = GetMD5(acStr);
            if (action == "test") //支付宝接口测试是否有效
            {
                returnTxt = "Y";
            }
            if (action == "sendOff")  //发货通知
            {
                if (responseTxt.Substring(0, 4) == "true"
                    || responseTxt.Substring(0, 4) == "fals")//ATN，验证消息是否支付宝发过来 
                {
                    if (ac_code == ac)//验证消息是否被修改
                    {
                        //数据库操作
                    }
                }
            }
            if (action == "checkOut")  //交易完成通知 
            {
                returnTxt = "N";
                if (responseTxt.Substring(0, 4) == "true"
                    || responseTxt.Substring(0, 4) == "fals")//ATN，验证消息是否支付宝发过来 
                {
                    if (ac_code == ac)//验证消息是否被修改
                    {
                        //数据库操作    
                    }
                }
            }
            return returnTxt;
        }



        /// <summary>
        /// 用于生成卖家支付宝安全校验码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string GetMD5(string s)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding("gb2312").GetBytes(s));
            System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取HTTP请求
        /// </summary>
        /// <param name="requestURL"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private String GetResponse(String requestURL, int? timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(requestURL);
                myReq.Timeout = timeout ?? 120000;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                using (Stream myStream = HttpWResp.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(myStream, Encoding.Default);
                    StringBuilder strBuilder = new StringBuilder();
                    while (sr.Peek() > 0)
                    {
                        strBuilder.Append(sr.ReadLine());
                    }
                    strResult = strBuilder.ToString();
                }
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }
            return strResult;
        }

    }
}
