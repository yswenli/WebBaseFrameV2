/*****************************************************************************************************
 * 本代码版权归yswenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：yswenli-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：5e9a70ee-a40d-4dcd-99a3-13dd56c9ed02
 * 机器名称：yswenli-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WeiXinAPIs
 * 类名称：ChatAPI
 * 文件名：ChatAPI
 * 创建年份：2015
 * 创建时间：2015-11-17 15:06:37
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
    public class ChatAPI : BaseAPI
    {
        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="chatId">会话id。字符串类型，最长32个字符。只允许字符0-9及字母a-zA-Z, 如果值内容为64bit无符号整型：要求值范围在[1, 2^63)之间，[2^63, 2^64)为系统分配会话id区间</param>
        /// <param name="name">会话标题</param>
        /// <param name="owner">管理员userid，必须是该会话userlist的成员之一</param>
        /// <param name="userlist">会话成员列表，成员用userid来标识。会话成员必须在3人或以上，1000人以下</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public QyJsonResult CreateChat(string chatId, string name, string owner, string[] userlist)
        {
            var data = new
            {
                chatid = chatId,
                name = name,
                owner = owner,
                userlist = userlist
            };
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/chat/create?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public Chat GetChat(string chatId)
        {
            return this.Get<Chat>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/chat/get?access_token={0}&chatid={1}", AccessToken.access_token, chatId));
        }

        /// <summary>
        /// 修改会话信息
        /// </summary>
        /// <param name="chatId">会话id</param>
        /// <param name="opUser">操作人userid</param>
        /// <param name="name">会话标题</param>
        /// <param name="owner">管理员userid，必须是该会话userlist的成员之一</param>
        /// <param name="addUserList">会话新增成员列表，成员用userid来标识</param>
        /// <param name="delUserList">会话退出成员列表，成员用userid来标识</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public QyJsonResult UpdateChat(string chatId, string opUser, string name = null, string owner = null, string[] addUserList = null, string[] delUserList = null)
        {
            var data = new
            {
                chatid = chatId,
                op_user = opUser,
                name = name,
                owner = owner,
                add_user_list = addUserList,
                del_user_list = delUserList
            };
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/chat/update?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 退出会话
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="opUser"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public QyJsonResult QuitChat(string chatId, string opUser)
        {
            var data = new
            {
                chatid = chatId,
                op_user = opUser,
            };
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/chat/quit?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 清除消息未读状态
        /// </summary>
        /// <param name="opUser">会话所有者的userid</param>
        /// <param name="type">会话类型：single|group，分别表示：群聊|单聊</param>
        /// <param name="chatIdOrUserId">会话值，为userid|chatid，分别表示：成员id|会话id，单聊是userid，群聊是chatid</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public QyJsonResult ClearNotify(string opUser, ChatTypeEnum type, string chatIdOrUserId)
        {
            var data = new
            {
                op_user = opUser,
                chat = new
                {
                    type = type,
                    id = chatIdOrUserId
                }
            };
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/chat/clearnotify?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="sender">发送人的userId</param>
        /// <param name="type">接收人类型：single|group，分别表示：群聊|单聊</param>
        /// <param name="msgType">消息类型,text|image|file</param>
        /// <param name="chatIdOrUserId">会话值，为userid|chatid，分别表示：成员id|会话id，单聊是userid，群聊是chatid</param>
        /// <param name="contentOrMediaId">文本消息是content，图片或文件是mediaId</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public QyJsonResult SendChatMessage(string sender, ChatTypeEnum type, ChatMsgTypeEnum msgType, string chatIdOrUserId, string contentOrMediaId)
        {
            BaseSendChatMessageData data;

            switch (msgType)
            {
                case ChatMsgTypeEnum.text:
                    data = new SendTextMessageData()
                    {
                        receiver = new Receiver()
                        {
                            type = type.ToString(),
                            id = chatIdOrUserId
                        },
                        sender = sender,
                        msgtype = msgType.ToString(),
                        text = new Chat_Content()
                        {
                            content = contentOrMediaId
                        }
                    };
                    break;
                case ChatMsgTypeEnum.image:
                    data = new SendImageMessageData()
                    {
                        receiver = new Receiver()
                        {
                            type = type.ToString(),
                            id = chatIdOrUserId
                        },
                        sender = sender,
                        msgtype = msgType.ToString(),
                        image = new ChatImage()
                        {
                            media_id = contentOrMediaId
                        }
                    };
                    break;
                case ChatMsgTypeEnum.file:
                    data = new SendFileMessageData()
                    {
                        receiver = new Receiver()
                        {
                            type = type.ToString(),
                            id = chatIdOrUserId
                        },
                        sender = sender,
                        msgtype = msgType.ToString(),
                        file = new ChatFile()
                        {
                            media_id = contentOrMediaId
                        }
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("msgType");
            }
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/chat/send?access_token={0}", AccessToken.access_token), data);
        }

        /// <summary>
        /// 设置成员新消息免打扰
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userMuteList">成员新消息免打扰参数，数组，最大支持10000个成员</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public Mute SetMute(List<UserMute> userMuteList)
        {
            var data = new
            {
                user_mute_list = userMuteList
            };
            return this.Post<Mute>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/chat/setmute?access_token={0}", AccessToken.access_token), data);
        }
    }
}
