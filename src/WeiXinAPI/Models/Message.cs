using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    ///上传的媒体文件限制
    ///图片（image）:1MB，支持JPG格式
    ///语音（voice）：2MB，播放长度不超过60s，支持AMR格式
    ///视频（video）：10MB，支持MP4格式
    ///普通文件（file）：10MB
    /// </summary>
    public class Message
    {
        #region 文本消息
        public class TextMessageClass
        {
            [JsonProperty("touser")]
            public string Touser { get; set; }

            [JsonProperty("toparty")]
            public string Toparty { get; set; }

            [JsonProperty("totag")]
            public string Totag { get; set; }

            [JsonProperty("msgtype")]
            public string Msgtype { get; set; }

            [JsonProperty("agentid")]
            public string Agentid { get; set; }

            [JsonProperty("text")]
            public Text Text { get; set; }

            [JsonProperty("safe")]
            public string Safe { get; set; }
        }
        public class Text
        {

            [JsonProperty("content")]
            public string Content { get; set; }
        }
        #endregion

        #region 图文消息
        public class ImageMessageClass
        {

            [JsonProperty("touser")]
            public string Touser { get; set; }

            [JsonProperty("toparty")]
            public string Toparty { get; set; }

            [JsonProperty("totag")]
            public string Totag { get; set; }

            [JsonProperty("msgtype")]
            public string Msgtype { get; set; }

            [JsonProperty("agentid")]
            public string Agentid { get; set; }

            [JsonProperty("image")]
            public Image Image { get; set; }

            [JsonProperty("safe")]
            public string Safe { get; set; }
        }
        public class Image
        {

            [JsonProperty("media_id")]
            public string MediaId { get; set; }
        }
        #endregion

        #region Vocie
        public class VoiceMessageClass
        {

            [JsonProperty("touser")]
            public string Touser { get; set; }

            [JsonProperty("toparty")]
            public string Toparty { get; set; }

            [JsonProperty("totag")]
            public string Totag { get; set; }

            [JsonProperty("msgtype")]
            public string Msgtype { get; set; }

            [JsonProperty("agentid")]
            public string Agentid { get; set; }

            [JsonProperty("voice")]
            public Voice Voice { get; set; }

            [JsonProperty("safe")]
            public string Safe { get; set; }
        }
        public class Voice
        {

            [JsonProperty("media_id")]
            public string MediaId { get; set; }
        }
        #endregion


        #region Video
        public class VideoMessageClass
        {

            [JsonProperty("touser")]
            public string Touser { get; set; }

            [JsonProperty("toparty")]
            public string Toparty { get; set; }

            [JsonProperty("totag")]
            public string Totag { get; set; }

            [JsonProperty("msgtype")]
            public string Msgtype { get; set; }

            [JsonProperty("agentid")]
            public string Agentid { get; set; }

            [JsonProperty("video")]
            public Video Video { get; set; }

            [JsonProperty("safe")]
            public string Safe { get; set; }
        }
        public class Video
        {

            [JsonProperty("media_id")]
            public string MediaId { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }
        }
        #endregion

        #region 文件消息
        public class FileMessageClass
        {

            [JsonProperty("touser")]
            public string Touser { get; set; }

            [JsonProperty("toparty")]
            public string Toparty { get; set; }

            [JsonProperty("totag")]
            public string Totag { get; set; }

            [JsonProperty("msgtype")]
            public string Msgtype { get; set; }

            [JsonProperty("agentid")]
            public string Agentid { get; set; }

            [JsonProperty("file")]
            public File File { get; set; }

            [JsonProperty("safe")]
            public string Safe { get; set; }
        }
        public class File
        {

            [JsonProperty("media_id")]
            public string MediaId { get; set; }
        }
        #endregion


        #region News
        public class NewsMessageClass
        {

            [JsonProperty("touser")]
            public string Touser { get; set; }

            [JsonProperty("toparty")]
            public string Toparty { get; set; }

            [JsonProperty("totag")]
            public string Totag { get; set; }

            [JsonProperty("msgtype")]
            public string Msgtype { get; set; }

            [JsonProperty("agentid")]
            public string Agentid { get; set; }

            [JsonProperty("news")]
            public News News { get; set; }
        }
        public class News
        {

            [JsonProperty("articles")]
            public IList<Article> Articles { get; set; }
        }
        public class Article
        {

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("picurl")]
            public string Picurl { get; set; }
        }

        #endregion


        #region MPNews
        public class MPNewsMessageClass
        {

            [JsonProperty("touser")]
            public string Touser { get; set; }

            [JsonProperty("toparty")]
            public string Toparty { get; set; }

            [JsonProperty("totag")]
            public string Totag { get; set; }

            [JsonProperty("msgtype")]
            public string Msgtype { get; set; }

            [JsonProperty("agentid")]
            public string Agentid { get; set; }

            [JsonProperty("mpnews")]
            public Mpnews Mpnews { get; set; }

            [JsonProperty("safe")]
            public string Safe { get; set; }
        }
        public class Mpnews
        {

            [JsonProperty("articles")]
            public IList<Article> Articles { get; set; }
        }
        #endregion
    }
}
