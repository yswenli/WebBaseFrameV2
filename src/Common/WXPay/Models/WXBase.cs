/************************************************************************************
 * Copyright (c)2015 All Rights Reserved.
 * CLR版本： 4.0.30319.34014
 *机器名称：ED6971
 *公司名称：
 *命名空间：Common.WXPay.Models
 *文件名：  WXBase
 *版本号：  V1.0.0.0
 *唯一标识：ac9abfb6-39dd-4745-9a59-7e433227b8f4
 *当前的用户域：SH
 *创建人：  Li.Wen
 *电子邮箱：yswenli@outlook.com
 *创建时间：2015/7/27 14:47:04
 *描述：
 *=====================================================================
 *修改标记
 *修改时间：2015/7/27 14:47:04
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
    /// <summary>
    /// 根据Code获取WXBase信息
    /// </summary>
    public class WXBase
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
}
