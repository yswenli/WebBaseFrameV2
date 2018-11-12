/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：14daa290-0427-47fa-9dd5-959494126187
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs
 * 类名称：AttentionAPI
 * 文件名：AttentionAPI
 * 创建年份：2015
 * 创建时间：2015-11-17 16:07:34
 * 创建人：yswenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WeiXinAPIs.Models;
//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    /// <summary>
    /// 关注 二次验证
    /// </summary>
    public class AttentionAPI : BaseAPI
    {
        /// <summary>
        /// 二次验证
        /// </summary>
        /// <param name="userId">员工UserID</param>
        /// <returns></returns>
        public QyJsonResult TwoVerification(string accessToken, string userId)
        {
            return this.Get<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/authsucc?access_token={0}&userid={1}", AccessToken.access_token, userId));
        }
    }
}
