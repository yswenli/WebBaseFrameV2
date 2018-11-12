using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libraries;
using WeiXinAPIs;
using WeiXinAPIs.Models;

namespace Web.Controllers
{
    /// <summary>
    /// weixin VerifyURL这个是回调验证地址
    /// </summary>
    public class WeiXinController : BaseWeiXinController
    {
        //
        // GET: /WeiXin/

        /// <summary>
        /// 自行扩展微信交互回调模式逻辑
        /// </summary>
        /// <param name="messageAPI"></param>
        /// <param name="wxHelper"></param>
        /// <returns></returns>
        public override ContentResult ReciveMessage(MessageAPI messageAPI, WeiXinHelper wxHelper)
        {

            var result = new ContentResult();

            #region 接收到的消息类型处理

            BaseAPI.Log(messageAPI.Message.Element("MsgType").Value);

            switch (messageAPI.Message.Element("MsgType").Value)
            {
                case ReceiveMsgType.Text:

                    var userID = messageAPI.Message.Element("FromUserName").Value;

                    var user = new UserAPI().GetUser(userID);

                    result = wxHelper.ReplyImage("系统不会回复你的", "你以为它会回复你", user.Avatar, "http://www.yswenli.net");

                    break;
                case ReceiveMsgType.Image:

                    BaseAPI.Log("黄图哥，这张图片不错！");

                    result = wxHelper.ReplyTXT("黄图哥，这张图片不错！");

                    break;
                case ReceiveMsgType.Thumb:

                    break;
                case ReceiveMsgType.Link:

                    break;
                case ReceiveMsgType.Location:

                    break;
                case ReceiveMsgType.Voice:

                    break;
                case ReceiveMsgType.Music:

                    break;
                case ReceiveMsgType.Video:

                    break;
                case ReceiveMsgType.News:

                    break;
                case ReceiveMsgType.Event:

                    #region 事件类型处理

                    switch (messageAPI.Message.Element("Event").Value)
                    {
                        case ReceiveMsgType.EventType.SubscribeEvent:

                            result = wxHelper.ReplyTXT("欢迎您!" + messageAPI.Message.Element("FromUserName").Value);

                            break;

                        case ReceiveMsgType.EventType.EnterAgentEvent:

                            result = wxHelper.ReplyTXT("欢迎您!" + messageAPI.Message.Element("FromUserName").Value);

                            break;

                        case ReceiveMsgType.EventType.UnSubscribeEvent:

                            break;

                        case ReceiveMsgType.EventType.ScanEvent:

                            break;
                        case ReceiveMsgType.EventType.ClickEvent: //微信自定义EventKey
                            if (messageAPI.Message.Element("EventKey").Value == "Photo")
                            {
                                result = wxHelper.ReplyImage("test", "test", "http://wx.qlogo.cn/mmhead/Q3auHgzwzM7G71S7KIpSKIhvsnZoTsEw1qbMySdHZJH6rLYUmNKLFw/0", "http://www.yswenli.net");
                            }
                            break;
                        case ReceiveMsgType.EventType.ViewEvent:

                            break;

                        case ReceiveMsgType.EventType.LocationEvent:

                            break;
                        case ReceiveMsgType.EventType.PicSysPhotoEvent:
                            result = wxHelper.ReplyImage("test", "test", "http://wx.qlogo.cn/mmhead/Q3auHgzwzM7G71S7KIpSKIhvsnZoTsEw1qbMySdHZJH6rLYUmNKLFw/0", "http://www.yswenli.net");
                            break;
                        default:

                            break;
                    }
                    #endregion
                    break;
            }
            #endregion
            return result;
        }
        //



    }
}
