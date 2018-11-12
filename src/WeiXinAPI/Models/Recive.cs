using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 收到回复消息类型
    /// </summary>
    public static class ReceiveMsgType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        public const string Text = "text";

        /// <summary>
        /// 图片消息
        /// </summary>
        public const string Image = "image";

        /// <summary>
        /// 语音消息
        /// </summary>
        public const string Voice = "voice";

        /// <summary>
        /// 视频消息
        /// </summary>
        public const string Video = "video";

        /// <summary>
        /// 地理位置消息
        /// </summary>
        public const string Location = "location";

        /// <summary>
        /// 链接消息
        /// </summary>
        public const string Link = "link";

        /// <summary>
        /// 音乐消息
        /// </summary>
        public const string Music = "music";

        /// <summary>
        /// 图文消息
        /// </summary>
        public const string News = "news";

        /// <summary>
        /// 缩略图(主要用于文件上传)
        /// </summary>
        public const string Thumb = "thumb";

        /// <summary>
        /// 事件推送
        /// </summary>
        public const string Event = "event";

        /// <summary>
        /// 事件类型
        /// </summary>
        public class EventType
        {
            /// <summary>
            /// 关注
            /// </summary>
            public const string SubscribeEvent = "subscribe";
            /// <summary>
            /// 进入应用事件
            /// </summary>
            public const string EnterAgentEvent = "enter_agent";
            /// <summary>
            /// 取消关注
            /// </summary>
            public const string UnSubscribeEvent = "unsubscribe";
            /// <summary>
            /// 二维码扫描
            /// </summary>
            public const string ScanEvent = "scan";
            /// <summary>
            /// 上报地理位置事件
            /// </summary>
            public const string LocationEvent = "location";
            /// <summary>
            /// 自定义菜单点击事件
            /// </summary>
            public const string ClickEvent = "click";
            /// <summary>
            /// 点击菜单跳转链接时的事件推送
            /// </summary>
            public const string ViewEvent = "view";
            /// <summary>
            /// 打开照相机
            /// </summary>
            public const string PicSysPhotoEvent = "pic_sysphoto";
        }
    }
}
