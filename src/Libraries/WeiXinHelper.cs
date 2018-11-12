using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WeiXinAPIs;
using WeiXinAPIs.Models;

namespace Libraries
{
    /// <summary>
    /// 微信消息辅助类
    /// </summary>
    public class WeiXinHelper
    {
        MessageAPI _messageAPI;

        public WeiXinHelper(MessageAPI messageAPI)
        {
            _messageAPI = messageAPI;
        }

        /// <summary>
        /// 回复TXT
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ContentResult ReplyTXT(string message)
        {
            ContentResult result = new ContentResult();
            var sendXml = MessageAPI.GetSendTxtContext(_messageAPI.Message,message);                    
            result.Content = MessageAPI.SendEncryptMsg(_messageAPI, sendXml);
            BaseAPI.Log(result.Content);
            return result;
        }
        /// <summary>
        /// 回复图文消息
        /// </summary>
        /// <param name="re"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="picUrl"></param>
        /// <param name="redrectUrl"></param>
        /// <returns></returns>
        public ContentResult ReplyImage(string title, string description, string picUrl, string redrectUrl)
        {
            ContentResult result = new ContentResult(); 
            var sendXml = MessageAPI.GetSendContext(_messageAPI.Message);
            var xe = MessageAPI.GetSendImageContext(sendXml, title, description, picUrl, redrectUrl);
            result.Content = MessageAPI.SendEncryptMsg(_messageAPI, xe);
            return result;
        }
    }


    /// <summary>
    /// 封装微信控制器
    /// </summary>
    public abstract class BaseWeiXinController : Controller
    {
        //
        // GET: /WeiXin/

        /// <summary>
        /// 回调URL
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyURL(string msg_signature, string timestamp, string nonce, string echostr)
        {
            if (Request.HttpMethod == "GET")
            {
                return Content(new MessageAPI(msg_signature, timestamp, nonce).ReplyVerify(echostr));
            }
            else
            {
                try
                {
                    MessageAPI messageAPI = new MessageAPI(new HttpReceive(Request));

                    WeiXinHelper wxHelper = new WeiXinHelper(messageAPI);

                    return ReciveMessage(messageAPI, wxHelper);
                }
                catch (Exception ex)
                {
                    BaseAPI.Log(ex.Source + ":" + ex.Message);
                    return Content("");
                }
            }
        }



        /// <summary>
        /// 获取微信返回消息后，需要自行实现逻辑
        /// </summary>
        public abstract ContentResult ReciveMessage(MessageAPI messageAPI, WeiXinHelper wxHelper);

    }
}