/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：467c841a-37c3-4670-b024-67ee9af4f077
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs
 * 类名称：ThirdPartyAuthApi
 * 文件名：ThirdPartyAuthApi
 * 创建年份：2015
 * 创建时间：2015-11-17 12:59:52
 * 创建人：yswenli
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
    public class ThirdPartyAuthAPI
    {
        /// <summary>
        /// 获取应用套件令牌
        /// </summary>
        /// <param name="suiteId">应用套件id</param>
        /// <param name="suiteSecret">应用套件secret</param>
        /// <param name="suiteTicket">微信后台推送的ticket</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static SuiteToken GetSuiteToken(string suiteId, string suiteSecret, string suiteTicket, int timeOut = 10000)
        {
            var data = new
            {
                suite_id = suiteId,
                suite_secret = suiteSecret,
                suite_ticket = suiteTicket
            };
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.UploadString("https://qyapi.weixin.qq.com/cgi-bin/service/get_suite_token", Common.JsonHelper.ObjToJson(data));
                return Common.JsonHelper.JsonToObj<SuiteToken>(result);
            }
        }

        /// <summary>
        /// 获取预授权码
        /// </summary>
        /// <param name="suiteAccessToken"></param>
        /// <param name="suiteId">应用套件id</param>
        /// <param name="appId">应用id，本参数选填，表示用户能对本套件内的哪些应用授权，不填时默认用户有全部授权权限</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static PreAuthCode GetPreAuthCode(string suiteAccessToken, string suiteId, int[] appId)
        {
            var data = new
            {
                suite_id = suiteId,
                appid = appId
            };
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.UploadString(string.Format("https://qyapi.weixin.qq.com/cgi-bin/service/get_pre_auth_code?suite_access_token={0}", suiteAccessToken), Common.JsonHelper.ObjToJson(data));
                return Common.JsonHelper.JsonToObj<PreAuthCode>(result);
            }
        }

        /// <summary>
        /// 获取企业号的永久授权码
        /// </summary>
        /// <param name="suiteAccessToken"></param>
        /// <param name="suiteId">应用套件id</param>
        /// <param name="authCode">临时授权码会在授权成功时附加在redirect_uri中跳转回应用提供商网站。</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static PermanentCode GetPermanentCode(string suiteAccessToken, string suiteId, string authCode)
        {
            var data = new
            {
                suite_id = suiteId,
                auth_code = authCode
            };
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.UploadString(string.Format("https://qyapi.weixin.qq.com/cgi-bin/service/get_permanent_code?suite_access_token={0}", suiteAccessToken), Common.JsonHelper.ObjToJson(data));
                return Common.JsonHelper.JsonToObj<PermanentCode>(result);
            }
        }

        /// <summary>
        /// 获取企业号的授权信息
        /// </summary>
        /// <param name="suiteAccessToken"></param>
        /// <param name="suiteId">应用套件id</param>
        /// <param name="authCorpId">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，通过get_permanent_code获取</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static AuthInfo GetAuthInfo(string suiteAccessToken, string suiteId, string authCorpId, string permanentCode)
        {
            var data = new
            {
                suite_id = suiteId,
                auth_corpid = authCorpId,
                permanent_code = permanentCode
            };

            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.UploadString(string.Format("https://qyapi.weixin.qq.com/cgi-bin/service/get_auth_info?suite_access_token={0}", suiteAccessToken), Common.JsonHelper.ObjToJson(data));
                return Common.JsonHelper.JsonToObj<AuthInfo>(result);
            }
        }

        /// <summary>
        /// 获取企业号应用
        /// </summary>
        /// <param name="suiteAccessToken"></param>
        /// <param name="suiteId">应用套件id</param>
        /// <param name="authCorpId">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，从get_permanent_code接口中获取</param>
        /// <param name="agentId">授权方应用id</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static AgentResult GetAgent(string suiteAccessToken, string suiteId, string authCorpId, string permanentCode, string agentId)
        {
            var data = new
            {
                suite_id = suiteId,
                auth_corpid = authCorpId,
                permanent_code = permanentCode,
                agentid = agentId
            };

            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.UploadString(string.Format("https://qyapi.weixin.qq.com/cgi-bin/service/get_agent?suite_access_token={0}", suiteAccessToken), Common.JsonHelper.ObjToJson(data));
                return Common.JsonHelper.JsonToObj<AgentResult>(result);
            }
        }

        /// <summary>
        /// 设置企业号应用
        /// </summary>
        /// <param name="suiteAccessToken"></param>
        /// <param name="suiteId">应用套件id</param>
        /// <param name="authCorpId">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，从get_permanent_code接口中获取</param>
        /// <param name="agent">要设置的企业应用的信息</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static QyJsonResult SetAgent(string suiteAccessToken, string suiteId, string authCorpId, string permanentCode, ThirdParty_AgentData agent)
        {
            var data = new
            {
                suite_id = suiteId,
                auth_corpid = authCorpId,
                permanent_code = permanentCode,
                agent = agent
            };
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.UploadString(string.Format("https://qyapi.weixin.qq.com/cgi-bin/service/set_agent?suite_access_token={0}", suiteAccessToken), Common.JsonHelper.ObjToJson(data));
                return Common.JsonHelper.JsonToObj<QyJsonResult>(result);
            }
        }

        /// <summary>
        /// 获取企业号access_token
        /// </summary>
        /// <param name="suiteAccessToken"></param>
        /// <param name="suiteId">应用套件id</param>
        /// <param name="authCorpId">授权方corpid</param>
        /// <param name="permanentCode">永久授权码，通过get_permanent_code获取</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static CorpToken GetCorpToken(string suiteAccessToken, string suiteId, string authCorpId, string permanentCode)
        {
            var data = new
            {
                suite_id = suiteId,
                auth_corpid = authCorpId,
                permanent_code = permanentCode,
            };
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("UTF-8") })
            {
                var result = webClient.UploadString(string.Format("https://qyapi.weixin.qq.com/cgi-bin/service/get_corp_token?suite_access_token={0}", suiteAccessToken), Common.JsonHelper.ObjToJson(data));
                return Common.JsonHelper.JsonToObj<CorpToken>(result);
            }
        }
    }
}
