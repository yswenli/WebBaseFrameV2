/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：414c5168-a74f-494b-a3d7-9078aa08b114
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs
 * 类名称：OAuthAPI
 * 文件名：OAuthAPI
 * 创建年份：2015
 * 创建时间：2015-11-17 10:43:54
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WeiXinAPIs.Models;

namespace WeiXinAPIs
{
    /// <summary>
    /// 应用授权作用域
    /// </summary>
    public enum OAuthScope
    {
        /// <summary>
        /// 不弹出授权页面，直接跳转，只能获取用户openid
        /// </summary>
        snsapi_base,
        /// <summary>
        /// 弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息
        /// </summary>
        snsapi_userinfo
    }

    public class OAuthAPI
    {
        /// <summary>
        /// 非企业号为appid,企业号为corpid
        /// </summary>
        public string WeiXinID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WeiXinID"].ToString();
            }
        }
        /// <summary>
        /// 开发者凭据
        /// </summary>
        public string Secret
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Secret"].ToString();
            }
        }

        /// <summary>
        /// 获取验证地址(这个地址只能在微信里打开,如果同意就会返回code)
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static string GetAuthorizeUrl(string appId, string redirectUrl, string state, OAuthScope scope, string responseType = "code")
        {
            var url =
                string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect",appId,System.Web.HttpUtility.UrlEncode(redirectUrl), responseType, scope, state);
            return url;
        }


        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public static QyJsonResult GetAccessToken(string appId, string secret, string code, string grantType = "authorization_code")
        {
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.DownloadString(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}",appId, secret, code, grantType));
                return Common.JsonHelper.JsonToObj<QyJsonResult>(result);
            }

        }

        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="code">通过员工授权获取到的code，每次员工授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期</param>
        /// 权限说明：管理员须拥有agent的使用权限；agentid必须和跳转链接时所在的企业应用ID相同。
        /// <returns></returns>
        public static OAuth2 GetOAuth2Result(string accessToken, string code, string agentId)
        {
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.DownloadString(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}&agentid={2}", accessToken, code, agentId));
                return Common.JsonHelper.JsonToObj<OAuth2>(result);
            }
        }

    }
}
