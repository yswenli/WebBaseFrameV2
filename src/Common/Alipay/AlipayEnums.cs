/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.Alipay
 *文件名：  AlipayEnums
 *版本号：  V1.0.0.0
 *唯一标识：62e088be-8a6d-4a87-95c3-446fd219c6ba
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：yswenli@outlook.com
 *创建时间：2015/7/27 11:29:05
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/27 11:29:05
 *修改人： Li.Wen
 *版本号： V1.0.0.0
 *描述：
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Alipay
{
    /// <summary>
    /// type支付类型
    /// </summary>
    public enum TypeEnum
    {
        /// <summary>
        /// 商品
        /// </summary>
        Commodity = 1,
        /// <summary>
        /// 服务
        /// </summary>
        Service = 2,
        /// <summary>
        /// 拍卖
        /// </summary>
        Auction = 3,
        /// <summary>
        /// 捐赠
        /// </summary>
        Donation = 4
    }
    /// <summary>
    /// 发货方式
    /// </summary>
    public enum TransportEnum
    {
        /// <summary>
        /// 平邮
        /// </summary>
        Mail = 1,
        /// <summary>
        /// 快递
        /// </summary>
        Courier = 2,
        /// <summary>
        /// 虚拟物品
        /// </summary>
        VirtualGoods = 3
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ActionEnum
    {
        /// <summary>
        /// 测试
        /// </summary>
        test,
        /// <summary>
        /// 发货通知
        /// </summary>
        sendOff,
        /// <summary>
        /// 交易完成通知
        /// </summary>
        checkOut
    }
}
