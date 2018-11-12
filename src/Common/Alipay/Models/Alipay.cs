/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.Alipay.Models
 *文件名：  Alipay
 *版本号：  V1.0.0.0
 *唯一标识：dfa3b416-9427-4e41-a08c-07d2068bbd14
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：ysyswenli@outlook.com
 *创建时间：2015/7/27 11:00:20
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/27 11:00:20
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Alipay.Models
{
    public class Alipay
    {
        private string _cmd;
        /// <summary>
        /// 命令字
        /// </summary>
        public string cmd
        {
            get { return _cmd; }
            set { _cmd = value; }
        }

        private string _subject;
        /// <summary>
        /// 商品名
        /// </summary>
        public string subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _body;
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body
        {
            get { return _body; }
            set { _body = value; }
        }

        private string _order_no;
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string order_no
        {
            get { return _order_no; }
            set { _order_no = value; }
        }

        private Decimal _price;
        /// <summary>
        /// 商品单价 0.01～50000.00
        /// </summary>
        public Decimal price
        {
            get { return _price; }
            set { _price = value; }
        }

        private string _url;
        /// <summary>
        /// 商品展示网址
        /// </summary>
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }

        private int _type;
        /// <summary>
        /// type支付类型 1：商品购买2：服务购买3：网络拍卖4：捐赠
        /// </summary>
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _number;
        /// <summary>
        /// 购买数量
        /// </summary>
        public int number
        {
            get { return _number; }
            set { _number = value; }
        }

        private int _transport;
        /// <summary>
        /// 发货方式 1：平邮2：快递3：虚拟物品
        /// </summary>
        public int transport
        {
            get { return _transport; }
            set { _transport = value; }
        }

        private string _ordinary_fee;
        /// <summary>
        /// 平邮运费
        /// </summary>
        public string ordinary_fee
        {
            get { return _ordinary_fee; }
            set { _ordinary_fee = value; }
        }

        private string _express_fee;
        /// <summary>
        /// 快递运费
        /// </summary>
        public string express_fee
        {
            get { return _express_fee; }
            set { _express_fee = value; }
        }

        private string _readonly;
        /// <summary>
        /// 交易信息是否只读
        /// </summary>
        public string Readonly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }

        private string _buyer_msg;
        /// <summary>
        /// 买家给卖家的留言
        /// </summary>
        public string buyer_msg
        {
            get { return _buyer_msg; }
            set { _buyer_msg = value; }
        }

        private string _seller;
        /// <summary>
        /// 卖家支付宝帐号
        /// </summary>
        public string seller
        {
            get { return _seller; }
            set { _seller = value; }
        }

        private string _buyer;
        /// <summary>
        /// 买家EMAIL
        /// </summary>
        public string buyer
        {
            get { return _buyer; }
            set { _buyer = value; }
        }

        private string _buyer_name;
        /// <summary>
        /// 买家姓名
        /// </summary>
        public string buyer_name
        {
            get { return _buyer_name; }
            set { _buyer_name = value; }
        }

        private string _buyer_address;
        /// <summary>
        /// 买家地址
        /// </summary>
        public string buyer_address
        {
            get { return _buyer_address; }
            set { _buyer_address = value; }
        }

        private string _buyer_zipcode;
        /// <summary>
        /// 买家邮编
        /// </summary>
        public string buyer_zipcode
        {
            get { return _buyer_zipcode; }
            set { _buyer_zipcode = value; }
        }

        private string _buyer_tel;
        /// <summary>
        /// 买家电话号码
        /// </summary>
        public string buyer_tel
        {
            get { return _buyer_tel; }
            set { _buyer_tel = value; }
        }

        private string _buyer_mobile;
        /// <summary>
        /// 买家手机号码
        /// </summary>
        public string buyer_mobile
        {
            get { return _buyer_mobile; }
            set { _buyer_mobile = value; }
        }

        private string _partner;
        /// <summary>
        /// 合作伙伴ID    保留字段
        /// </summary>
        public string partner
        {
            get { return _partner; }
            set { _partner = value; }
        }

        private string _ac;

        public string ac
        {
            get { return _ac; }
            set { _ac = value; }
        }
    }
}
