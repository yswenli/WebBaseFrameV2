/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：a003c3c4-ded7-47fe-842d-810134cba768
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs.Models
 * 类名称：AppManager
 * 文件名：AppManager
 * 创建年份：2015
 * 创建时间：2015-11-17 15:49:29
 * 创建人：yswenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 获取企业号应用返回结果
    /// </summary>
    public class AppInfo : QyJsonResult
    {
        /// <summary>
        /// 企业应用id
        /// </summary>
        public string agentid { get; set; }
        /// <summary>
        /// 企业应用名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 企业应用方形头像
        /// </summary>
        public string square_logo_url { get; set; }
        /// <summary>
        /// 企业应用圆形头像
        /// </summary>
        public string round_logo_url { get; set; }
        /// <summary>
        /// 企业应用详情
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 企业应用可见范围（人员），其中包括userid和关注状态state
        /// </summary>
        public AllowUserInfos allow_userinfos { get; set; }
        /// <summary>
        /// 企业应用可见范围（部门）
        /// </summary>
        public AllowPartys allow_partys { get; set; }
        /// <summary>
        /// 企业应用可见范围（标签）
        /// </summary>
        public AllowTags allow_tags { get; set; }
        /// <summary>
        /// 企业应用是否被禁用
        /// </summary>
        public int close { get; set; }
        /// <summary>
        /// 企业应用可信域名
        /// </summary>
        public string redirect_domain { get; set; }
        /// <summary>
        /// 企业应用是否打开地理位置上报 0：不上报；1：进入会话上报；2：持续上报
        /// </summary>
        public int report_location_flag { get; set; }
        /// <summary>
        /// 是否接收用户变更通知。0：不接收；1：接收
        /// </summary>
        public int isreportuser { get; set; }
        /// <summary>
        /// 是否上报用户进入应用事件。0：不接收；1：接收
        /// </summary>
        public int isreportenter { get; set; }
    }

    public class AllowUserInfos
    {
        public List<AllowUser> user { get; set; }
    }

    public class AllowUser
    {
        public string userid { get; set; }
        public string status { get; set; }
    }

    public class AllowPartys
    {
        public int[] partyid { get; set; }
    }

    public class AllowTags
    {
        public int[] tagid { get; set; }
    }
    /// <summary>
    /// 设置企业号应用需要Post的数据
    /// </summary>
    public class QYInfoList : QyJsonResult
    {
        public List<QYInfo> agentlist { get; set; }
    }

    public class QYInfo
    {
        /// <summary>
        /// 企业应用id
        /// </summary>
        public string agentid { get; set; }
        /// <summary>
        /// 企业应用名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 企业应用方形头像
        /// </summary>
        public string square_logo_url { get; set; }
        /// <summary>
        /// 企业应用圆形头像
        /// </summary>
        public string round_logo_url { get; set; }
    }
    /// <summary>
    /// 设置企业号应用需要Post的数据
    /// </summary>
    public class AppPostData
    {
        /// <summary>
        /// 企业应用id
        /// </summary>
        public string agentid { get; set; }
        /// <summary>
        /// 企业应用是否打开地理位置上报 0：不上报；1：进入会话上报；2：持续上报
        /// </summary>
        public string report_location_flag { get; set; }
        /// <summary>
        /// 企业应用头像的mediaid，通过多媒体接口上传图片获得mediaid，上传后会自动裁剪成方形和圆形两个头像
        /// </summary>
        public string logo_mediaid { get; set; }
        /// <summary>
        /// 企业应用名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 企业应用详情
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 企业应用可信域名
        /// </summary>
        public string redirect_domain { get; set; }
        /// <summary>
        /// 是否接收用户变更通知。0：不接收；1：接收
        /// </summary>
        public int isreportuser { get; set; }
        /// <summary>
        /// 是否上报用户进入应用事件。0：不接收；1：接收
        /// </summary>
        public int isreportenter { get; set; }
    }
}
