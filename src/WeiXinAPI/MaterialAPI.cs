/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：08c1b875-705f-4029-a483-551c73926ba7
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs
 * 类名称：MaterialAPI
 * 文件名：MaterialAPI
 * 创建年份：2015
 * 创建时间：2015-11-17 16:20:57
 * 创建人：yswenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using WeiXinAPIs.Models;
using System.IO;
//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    public class MaterialAPI : BaseAPI
    {
        /// <summary>
        /// 上传永久图文素材
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="timeOut"></param>
        /// <param name="mpNewsArticles"></param>
        /// <returns></returns>
        public UploadForeverMaterial AddMpNews(int agentId, params Message.MPNewsMessageClass[] mpNewsArticles)
        {
            var data = new
            {
                agentid = agentId,
                mpnews = new
                {
                    articles = mpNewsArticles
                }
            };
            return this.Post<UploadForeverMaterial>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/add_mpnews?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 上传其他类型永久素材
        /// </summary>
        /// <param name="type"></param>
        /// <param name="agentId"></param>
        /// <param name="media"></param>
        /// <returns></returns>
        public UploadForeverMaterial AddMaterial(MediaFileTypeEnum type, int agentId, string media,byte[] file)
        {
            return this.Post<UploadForeverMaterial>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/add_material?agentid={1}&type={2}&access_token={0}", AccessToken.access_token, agentId, type), file);
        }

        /// <summary>
        /// 获取永久图文素材
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public ForeverMpNewsWithType GetForeverMpNews(int agentId, string mediaId)
        {
            return this.Get<ForeverMpNewsWithType>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/get?access_token={0}&media_id={1}&agentid={2}", AccessToken.access_token, agentId, mediaId));
        }

        /// <summary>
        /// 获取临时媒体文件
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        public byte[] GetForeverMaterial(string accessToken, int agentId, string mediaId)
        {
            return this.Get<byte[]>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/get?access_token={0}&media_id={1}&agentid={2}", AccessToken.access_token, agentId, mediaId));
        }

        /// <summary>
        /// 删除永久素材
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public QyJsonResult DeleteForeverMaterial(int agentId, string mediaId)
        {
            return this.Get<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/del?access_token={0}&agentid={1}&media_id={2}", AccessToken.access_token, agentId, mediaId));
        }

        /// <summary>
        /// 修改永久图文素材
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="agentId"></param>
        /// <param name="timeOut"></param>
        /// <param name="mpNewsArticles"></param>
        /// <returns></returns>
        public UploadForeverMaterial UpdateMpNews(string mediaId, int agentId, params MpNewsArticle[] mpNewsArticles)
        {
            var data = new
            {
                agentid = agentId,
                media_id = mediaId,
                mpnews = new
                {
                    articles = mpNewsArticles
                }
            };
            return this.Post<UploadForeverMaterial>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/update_mpnews?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public Count GetCount(int agentId)
        {
            return this.Get<Count>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/get_count?access_token={0}&agentid={1}", AccessToken.access_token, agentId));
        }

        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="agentId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public BatchGetMaterialList BatchGetMaterial(MediaFileTypeEnum type, int agentId, int offset, int count)
        {
            var data = new
            {
                type = type.ToString(),
                agentid = agentId,
                offset = offset,
                count = count,
            };
            return this.Post<BatchGetMaterialList>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/material/batchget?access_token={0}", AccessToken.access_token), data);
        }
    }
}
