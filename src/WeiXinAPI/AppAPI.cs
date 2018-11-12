/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：b342ad14-a83b-48dc-9a1e-576632d237b2
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs
 * 类名称：AppManagerAPI
 * 文件名：AppManagerAPI
 * 创建年份：2015
 * 创建时间：2015-11-17 15:40:56
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
    /// 企业号、APP管理
    /// </summary>
    public class AppAPI : BaseAPI
    {
        /// <summary>
        /// 获取企业号应用信息
        /// </summary>
        /// <param name="agentId">企业应用的id，可在应用的设置页面查看</param>
        /// <returns></returns>
        public AppInfo GetAppInfo(int agentId)
        {
            return this.Get<AppInfo>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/agent/get?access_token={0}&agentid={1}", AccessToken.access_token, agentId));
            
        }

        /// <summary>
        /// 设置企业号应用
        /// 此App只能修改现有的并且有权限管理的应用，无法创建新应用（因为新应用没有权限）
        /// </summary>
        /// <param name="data">设置应用需要Post的数据</param>
        /// <returns></returns>
        public QyJsonResult SetApp(AppPostData data)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/agent/set?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 获取应用概况列表
        /// </summary>
        /// <returns></returns>
        public QYInfoList GetAppList()
        {
            return this.Get<QYInfoList>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/agent/list?access_token={0}", AccessToken.access_token));
        }
    }
}
