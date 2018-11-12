/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：39bb8bed-e188-4b78-ab77-da9b44e4f6f7
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs.Models
 * 类名称：Chat
 * 文件名：Chat
 * 创建年份：2015
 * 创建时间：2015-11-17 15:16:33
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
    /// 获取会话返回结果
    /// </summary>
    public class Chat : QyJsonResult
    {
        /// <summary>
        /// 会话信息
        /// </summary>
        public ChatInfo chat_info { get; set; }
    }

    public class ChatInfo
    {
        /// <summary>
        /// 会话id
        /// </summary>
        public string chatid { get; set; }
        /// <summary>
        /// 会话标题
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 管理员userid
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 会话成员列表，成员用userid来标识
        /// </summary>
        public List<string> userlist { get; set; }
    }

    /// <summary>
    /// 设置成员新消息免打扰返回结果
    /// </summary>
    public class Mute : QyJsonResult
    {
        /// <summary>
        /// 列表中不存在的成员会返回在invaliduser里，剩余合法成员会继续执行
        /// </summary>
        public List<string> invaliduser { get; set; }
    }


    /// <summary>
    /// 发送消息基础数据
    /// </summary>
    public class BaseSendChatMessageData
    {
        /// <summary>
        /// 接收人
        /// </summary>
        public Receiver receiver { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public string sender { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }
    }

    public class Receiver
    {
        /// <summary>
        /// 接收人类型：single|group，分别表示：群聊|单聊
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 接收人的值，为userid|chatid，分别表示：成员id|会话id
        /// </summary>
        public string id { get; set; }
    }

    /// <summary>
    /// 发送text消息数据
    /// </summary>
    public class SendTextMessageData : BaseSendChatMessageData
    {
        public Chat_Content text { get; set; }
    }

    public class Chat_Content
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
    }

    /// <summary>
    /// 发送image消息数据
    /// </summary>
    public class SendImageMessageData : BaseSendChatMessageData
    {
        public ChatImage image { get; set; }
    }

    public class ChatImage
    {
        /// <summary>
        /// 图片媒体文件id，可以调用上传素材文件接口获取
        /// </summary>
        public string media_id { get; set; }
    }

    /// <summary>
    /// 发送file消息数据
    /// </summary>
    public class SendFileMessageData : BaseSendChatMessageData
    {
        public ChatFile file { get; set; }
    }

    public class ChatFile
    {
        /// <summary>
        /// 图片媒体文件id，可以调用上传素材文件接口获取
        /// </summary>
        public string media_id { get; set; }
    }

    /// <summary>
    /// 成员新消息免打扰参数
    /// </summary>
    public class UserMute
    {
        /// <summary>
        /// 成员UserID
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 免打扰状态
        /// </summary>
        public MuteStatusEnum status { get; set; }
    }
}
