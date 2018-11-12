/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.WXPay.Models
 *文件名：  Order
 *版本号：  V1.0.0.0
 *唯一标识：f495660e-7ff8-4f5a-bc68-42296b6440cf
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：ysyswenli@outlook.com
 *创建时间：2015/7/27 17:49:04
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/27 17:49:04
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WXPay.Models
{
    public class Order
    {
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string openid { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 商品信息 127字符
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 支付完成后回调页面
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 商品金额,以分为单位(money * 100).ToString()
        /// </summary>
        public decimal total_fee { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public string trade_type { get { return "JSAPI"; } }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        
    }
}
