/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：177acbf2-e870-4736-9408-29d92557a759
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs.Models
 * 类名称：ThirdPartyAuth
 * 文件名：ThirdPartyAuth
 * 创建年份：2015
 * 创建时间：2015-11-17 12:43:15
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 获取应用套件令牌返回结果
    /// </summary>
    public class SuiteToken
    {
        /// <summary>
        /// 应用套件access_token
        /// </summary>
        public string suite_access_token { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 获取预授权码返回结果
    /// </summary>
    public class PreAuthCode
    {
        /// <summary>
        /// 预授权码
        /// </summary>
        public string pre_auth_code { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int expires_in { get; set; }
    }

    public class PermanentCode
    {
        /// <summary>
        /// 授权方（企业）access_token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 授权方（企业）access_token超时时间
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 企业号永久授权码
        /// </summary>
        public string permanent_code { get; set; }

        /// <summary>
        /// 授权方企业信息
        /// </summary>
        public ThirdParty_AuthCorpInfo auth_corp_info { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public ThirdParty_AuthInfo auth_info { get; set; }
    }

    public class ThirdParty_AuthCorpInfo
    {
        /// <summary>
        /// 授权方企业号id
        /// </summary>
        public string corpid { get; set; }

        /// <summary>
        /// 授权方企业号名称
        /// </summary>
        public string corp_name { get; set; }

        /// <summary>
        /// 授权方企业号类型，认证号：verified, 注册号：unverified，体验号：test
        /// </summary>
        public string corp_type { get; set; }

        /// <summary>
        /// 授权方企业号圆形头像
        /// </summary>
        public string corp_round_logo_url { get; set; }

        /// <summary>
        /// 授权方企业号方形头像
        /// </summary>
        public string corp_square_logo_url { get; set; }

        /// <summary>
        /// 授权方企业号用户规模
        /// </summary>
        public string corp_user_max { get; set; }

        /// <summary>
        /// 授权方企业号应用规模
        /// </summary>
        public string corp_agent_max { get; set; }
    }

    public class ThirdParty_AuthInfo
    {
        /// <summary>
        /// 授权的应用信息
        /// </summary>
        public List<ThirdParty_Agent> agent { get; set; }

        /// <summary>
        /// 授权的通讯录部门
        /// </summary>
        public List<ThirdParty_Department> department { get; set; }
    }

    public class ThirdParty_Agent
    {
        /// <summary>
        /// 授权方应用id
        /// </summary>
        public string agentid { get; set; }

        /// <summary>
        /// 授权方应用名字
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 授权方应用方形头像
        /// </summary>
        public string square_logo_url { get; set; }

        /// <summary>
        /// 授权方应用圆形头像
        /// </summary>
        public string round_logo_url { get; set; }

        /// <summary>
        /// 服务商套件中的对应应用id
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 授权方应用敏感权限组，目前仅有get_location，表示是否有权限设置应用获取地理位置的开关
        /// </summary>
        public string[] api_group { get; set; }
    }

    public class ThirdParty_Department
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 父部门id
        /// </summary>
        public string parentid { get; set; }

        /// <summary>
        /// 是否具有该部门的写权限
        /// </summary>
        public string writable { get; set; }
    }

    /// <summary>
    /// 获取企业号的授权信息返回结果
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// 授权方企业信息
        /// </summary>
        public ThirdParty_AuthCorpInfo auth_corp_info { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public ThirdParty_AuthInfo auth_info { get; set; }
    }

    public class AgentResult
    {
        /// <summary>
        /// 授权方企业应用id
        /// </summary>
        public string agentid { get; set; }

        /// <summary>
        /// 授权方企业应用名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 授权方企业应用方形头像
        /// </summary>
        public string square_logo_url { get; set; }

        /// <summary>
        /// 授权方企业应用圆形头像
        /// </summary>
        public string round_logo_url { get; set; }

        /// <summary>
        /// 授权方企业应用详情
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 授权方企业应用可见范围（人员），其中包括userid和关注状态state
        /// </summary>
        public ThirdParty_AllowUserinfos allow_userinfos { get; set; }

        /// <summary>
        /// 授权方企业应用可见范围（部门）
        /// </summary>
        public ThirdParty_AllowPartys allow_partys { get; set; }

        /// <summary>
        /// 授权方企业应用可见范围（标签）
        /// </summary>
        public ThirdParty_AllowTags allow_tags { get; set; }

        /// <summary>
        /// 授权方企业应用是否被禁用
        /// </summary>
        public int close { get; set; }

        /// <summary>
        /// 授权方企业应用可信域名
        /// </summary>
        public string redirect_domain { get; set; }

        /// <summary>
        /// 授权方企业应用是否打开地理位置上报 0：不上报；1：进入会话上报；2：持续上报
        /// </summary>
        public int report_location_flag { get; set; }

        /// <summary>
        /// 是否接收用户变更通知。0：不接收；1：接收
        /// </summary>
        public int isreportuser { get; set; }
    }

    public class ThirdParty_AllowUserinfos
    {
        public List<ThirdParty_User> user { get; set; }
    }

    public class ThirdParty_User
    {
        public string userid { get; set; }
        public string status { get; set; }
    }

    public class ThirdParty_AllowPartys
    {
        public int[] partyid { get; set; }
    }

    public class ThirdParty_AllowTags
    {
        public int[] tagid { get; set; }
    }

    /// <summary>
    /// 获取企业号access_token返回结果
    /// </summary>
    public class CorpToken
    {
        /// <summary>
        /// 授权方（企业）access_token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 授权方（企业）access_token超时时间
        /// </summary>
        public int expires_in { get; set; }
    }

    public class ThirdParty_AgentData
    {
        /// <summary>
        /// 企业应用的id
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
        public ReportUserAgentEnum isreportuser { get; set; }
    }

    public enum ReportUserAgentEnum
    {
        不接受 = 0,
        接收 = 1
    }
}
