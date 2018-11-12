/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：af30fbb3-a26b-4b66-9d65-23c1e3d0a349
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs.Models
 * 类名称：Material
 * 文件名：Material
 * 创建年份：2015
 * 创建时间：2015-11-17 16:23:04
 * 创建人：yswenli
 * 创建说明：
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    public class Material
    {

    }

    /// <summary>
    /// 上传临时媒体文件返回结果
    /// </summary>
    public class UploadTemporaryMaterial : QyJsonResult
    {
        public MediaFileTypeEnum type { get; set; }
        public string media_id { get; set; }
        public long created_at { get; set; }
    }

    /// <summary>
    /// 上传永久素材返回结果
    /// </summary>
    public class UploadForeverMaterial : QyJsonResult
    {
        public string media_id { get; set; }
    }
   
    /// <summary>
    /// 获取永久图文素材返回结果
    /// </summary>
    public class ForeverMpNewsWithType : QyJsonResult
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        public ForeverMpNews mpnews { get; set; }
    }

    public class ForeverMpNews
    {
        public List<MpNewsArticle> articles { get; set; }
    }

    /// <summary>
    /// 获取素材总数返回结果
    /// </summary>
    public class Count : QyJsonResult
    {
        /// <summary>
        /// 应用素材总数目
        /// </summary>
        public int total_count { get; set; }
        /// <summary>
        /// 图片素材总数目
        /// </summary>
        public int image_count { get; set; }
        /// <summary>
        /// 音频素材总数目
        /// </summary>
        public int voice_count { get; set; }
        /// <summary>
        /// 视频素材总数目
        /// </summary>
        public int video_count { get; set; }
        /// <summary>
        /// 文件素材总数目
        /// </summary>
        public int file_count { get; set; }
        /// <summary>
        /// 图文素材总数目
        /// </summary>
        public int mpnews_count { get; set; }
    }

    /// <summary>
    /// 获取素材列表返回结果
    /// </summary>
    public class BatchGetMaterialList : QyJsonResult
    {
        /// <summary>
        /// 素材类型，可以为图文(mpnews)、图片（image）、音频（voice）、视频（video）、文件（file）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 应用该类型素材总数目
        /// </summary>
        public int total_count { get; set; }
        /// <summary>
        /// 返回该类型素材数目
        /// </summary>
        public int item_count { get; set; }
        /// <summary>
        /// 素材列表
        /// </summary>
        public List<BatchGetMaterial> itemlist { get; set; }
    }

    public class BatchGetMaterial
    {
        /// <summary>
        /// 素材的媒体id
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public long update_time { get; set; }
        /// <summary>
        /// 图文消息，一个图文消息支持1到10个图文
        /// </summary>
        public List<MpNewsArticle> articles { get; set; }
    }

    public class MpNewsArticle
    {
        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 图文消息缩略图的media_id, 可以在上传多媒体文件接口中获得。此处thumb_media_id即上传接口返回的media_id
        /// </summary>
        public string thumb_media_id { get; set; }
        /// <summary>
        /// 图文消息的作者
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 图文消息点击“阅读原文”之后的页面链接
        /// </summary>
        public string content_source_url { get; set; }
        /// <summary>
        /// 图文消息的内容，支持html标签
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 图文消息的描述
        /// </summary>
        public string digest { get; set; }
        /// <summary>
        /// 是否显示封面，1为显示，0为不显示
        /// </summary>
        public string show_cover_pic { get; set; }
    }
}
