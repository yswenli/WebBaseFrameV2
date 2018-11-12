using Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WeiXinAPIs.Models;
//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    /// <summary>
    /// 微信消息处理类
    /// </summary>
    public class MessageAPI : BaseAPI
    {
        #region 回调模式

        #region 初始化
        string _signature;
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature
        {
            get { return _signature; }
        }

        string _timestamp;
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp
        {
            get { return _timestamp; }
        }

        string _nonce;
        /// <summary>
        /// 随机数
        /// </summary>
        public string Nonce
        {
            get { return _nonce; }
        }


        XElement _message;
        /// <summary>
        /// 获取从微信发来的内容(已解密)
        /// </summary>
        public XElement Message
        {
            get
            {
                return _message;
            }

        }

        /// <summary>
        /// 微信消息处理类 （回调模式）
        /// </summary>
        /// <param name="receive"></param>
        public MessageAPI(HttpReceive receive)
            : this(receive.Signature, receive.TimeStamp, receive.Nonce, receive.Message)
        {

        }
        /// <summary>
        /// 微信消息处理类
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        public MessageAPI(string signature, string timestamp, string nonce)
            : this(signature, timestamp, nonce, null)
        {
        }

        /// <summary>
        /// 微信消息处理类（回调模式）
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="sPostData"></param>
        private MessageAPI(string signature, string timestamp, string nonce, XElement sPostData)
        {
            _signature = signature;
            _timestamp = timestamp;
            _nonce = nonce;
            if (IsQY && sPostData != null && !string.IsNullOrEmpty(sPostData.ToString()))
            {
                string _msg = string.Empty;
                this.DecryptMsg(sPostData.ToString(), ref _msg);
                _message = XElement.Parse(_msg);
            }
            else
            {
                _message = sPostData;
            }
        }
        #endregion


        #region 连接验证
        /// <summary>
        /// 返回验证内容
        /// </summary>
        /// <param name="msg_signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        public string ReplyVerify(string echostr)
        {
            string rEchoStr = "";
            int ret = 0;
            try
            {
                if (EncodingAESKey.Length != 43)
                {
                    ret = (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
                }
                ret = VerifySignature(Token, _timestamp, _nonce, _signature, echostr);
                if (ret == 0)
                {
                    rEchoStr = "";
                    string cpid = "";
                    try
                    {
                        rEchoStr = Cryptography.AES_decrypt(echostr, EncodingAESKey, ref cpid); //CorpID);
                    }
                    catch (Exception)
                    {
                        rEchoStr = "";
                        ret = (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecryptAES_Error;
                    }
                    if (cpid != WeiXinID)
                    {
                        rEchoStr = "";
                        ret = (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateCorpid_Error;
                    }
                }
            }
            catch
            {
            }
            if (ret != 0)
            {
                rEchoStr = "ERR: VerifyURL fail, ret: " + ret + "  echostr:" + echostr;
            }
            return rEchoStr;
        }
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="sToken"></param>
        /// <param name="sTimeStamp"></param>
        /// <param name="sNonce"></param>
        /// <param name="sMsgEncrypt"></param>
        /// <param name="sMsgSignature"></param>
        /// <returns></returns>
        public static int GenarateSinature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, ref string sMsgSignature)
        {
            ArrayList al = new ArrayList();
            al.Add(sToken);
            al.Add(sTimeStamp);
            al.Add(sNonce);
            al.Add(sMsgEncrypt);
            al.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < al.Count; ++i)
            {
                raw += al[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ComputeSignature_Error;
            }
            sMsgSignature = hash;
            return 0;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="sToken"></param>
        /// <param name="sTimeStamp"></param>
        /// <param name="sNonce"></param>
        /// <param name="sMsgEncrypt"></param>
        /// <param name="sSigture"></param>
        /// <returns></returns>
        private static int VerifySignature(string sToken, string sTimeStamp, string sNonce, string sSigture, string sMsgEncrypt)
        {
            string hash = "";
            int ret = 0;
            ret = GenarateSinature(sToken, sTimeStamp, sNonce, sMsgEncrypt, ref hash);
            if (ret != 0)
                return ret;
            if (hash == sSigture)
                return 0;
            else
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateSignature_Error;
            }
        }

        #endregion


        #region 解密密文

        /// <summary>
        /// 检验消息的真实性，并且获取解密后的明文
        /// </summary>
        /// <param name="sMsgSignature">签名串，对应URL参数的msg_signature</param>
        /// <param name="sTimeStamp">时间戳，对应URL参数的timestamp</param>
        /// <param name="sNonce">随机串，对应URL参数的nonce</param>
        /// <param name="sPostData">密文，对应POST请求的数据</param>
        /// <param name="sMsg">解密后的原文，当return返回0时有效</param>
        /// <returns>成功0，失败返回对应的错误码</returns>
        public int DecryptMsg(string sPostData, ref string sMsg)
        {
            if (EncodingAESKey.Length != 43)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
            }
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            string sEncryptMsg;
            try
            {
                doc.LoadXml(sPostData);
                root = doc.FirstChild;
                sEncryptMsg = root["Encrypt"].InnerText;
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ParseXml_Error;
            }
            //verify signature
            int ret = 0;
            ret = VerifySignature(Token, _timestamp, _nonce, _signature, sEncryptMsg);
            if (ret != 0)
                return ret;
            //decrypt
            string cpid = "";
            try
            {
                sMsg = Cryptography.AES_decrypt(sEncryptMsg, EncodingAESKey, ref cpid);
            }
            catch (FormatException)
            {
                sMsg = "";
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecodeBase64_Error;
            }
            catch (Exception)
            {
                sMsg = "";
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_DecryptAES_Error;
            }
            if (cpid != WeiXinID)
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_ValidateCorpid_Error;
            return 0;
        }

        // 将企业号回复用户的消息加密打包
        // @param sReplyMsg: 企业号待回复用户的消息，xml格式的字符串
        // @param sTimeStamp: 时间戳，可以自己生成，也可以用URL参数的timestamp
        // @param sNonce: 随机串，可以自己生成，也可以用URL参数的nonce
        // @param sEncryptMsg: 加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,
        // 当return返回0时有效
        // return：成功0，失败返回对应的错误码
        public int EncryptMsg(string sReplyMsg, ref string sEncryptMsg)
        {
            if (EncodingAESKey.Length != 43)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_IllegalAesKey;
            }
            string raw = "";
            try
            {
                raw = Cryptography.AES_encrypt(sReplyMsg, EncodingAESKey, WeiXinID);
            }
            catch (Exception)
            {
                return (int)WXBizMsgCryptErrorCode.WXBizMsgCrypt_EncryptAES_Error;
            }
            string MsgSigature = "";
            int ret = 0;
            ret = GenarateSinature(Token, _timestamp, _nonce, raw, ref MsgSigature);
            if (0 != ret)
                return ret;
            sEncryptMsg = "";

            string EncryptLabelHead = "<Encrypt><![CDATA[";
            string EncryptLabelTail = "]]></Encrypt>";
            string MsgSigLabelHead = "<MsgSignature><![CDATA[";
            string MsgSigLabelTail = "]]></MsgSignature>";
            string TimeStampLabelHead = "<TimeStamp><![CDATA[";
            string TimeStampLabelTail = "]]></TimeStamp>";
            string NonceLabelHead = "<Nonce><![CDATA[";
            string NonceLabelTail = "]]></Nonce>";
            sEncryptMsg = sEncryptMsg + "<xml>" + EncryptLabelHead + raw + EncryptLabelTail;
            sEncryptMsg = sEncryptMsg + MsgSigLabelHead + MsgSigature + MsgSigLabelTail;
            sEncryptMsg = sEncryptMsg + TimeStampLabelHead + _timestamp + TimeStampLabelTail;
            sEncryptMsg = sEncryptMsg + NonceLabelHead + _nonce + NonceLabelTail;
            sEncryptMsg += "</xml>";
            return 0;
        }
        #endregion


        #region 消息处理方法

        /// <summary>
        /// 调整消息为发出的消息作准备
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns></returns>
        public static XElement GetSendContext(XElement xElement)
        {
            XElement ex = XElement.Parse(xElement.ToString());

            ex.Element("ToUserName").SetValue(xElement.Element("FromUserName").Value);

            ex.Element("FromUserName").SetValue(xElement.Element("ToUserName").Value);

            ex.Element("CreateTime").SetValue(DateTime.Now.Ticks);

            return ex;
        }

        /// <summary>
        /// 将文本封装到XML中
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static XElement GetSendTxtContext(XElement xElement, string message)
        {
            XElement ex = new XElement("xml", new XElement("ToUserName"), new XElement("FromUserName"), new XElement("CreateTime"), new XElement("MsgType"), new XElement("Content"), new XElement("AgentID"), new XElement("FuncFlag"));

            ex.Element("ToUserName").SetValue(xElement.Element("FromUserName").Value);

            ex.Element("FromUserName").SetValue(xElement.Element("ToUserName").Value);

            ex.Element("MsgType").SetValue("text");

            ex.Element("Content").SetValue(message);

            ex.Element("CreateTime").SetValue(DateTime.Now.Ticks);

            ex.Element("AgentID").SetValue(xElement.Element("AgentID").Value);

            ex.Element("FuncFlag").SetValue("1");

            return ex;
        }
        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <param name="toUserName"></param>
        /// <param name="agentID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="picUrl"></param>
        /// <param name="redrectUrl"></param>
        /// <returns></returns>
        public static XElement GetSendImageContext(XElement xElement, string title, string description, string picUrl, string redrectUrl)
        {

            XElement ex = new XElement("xml", new XElement("ToUserName"), new XElement("FromUserName"), new XElement("CreateTime"), new XElement("MsgType"), new XElement("Content"), new XElement("ArticleCount"), new XElement("Articles"), new XElement("FuncFlag"));

            ex.Element("ToUserName").SetValue(xElement.Element("FromUserName").Value);

            ex.Element("FromUserName").SetValue(xElement.Element("ToUserName").Value);

            ex.Element("MsgType").SetValue("news");

            ex.Element("CreateTime").SetValue(DateTime.Now.Ticks);

            ex.Element("ArticleCount").SetValue(1);

            ex.Element("FuncFlag").SetValue("1");

            ex.Element("Articles").Add(new XElement("item",
                                        new XElement("Title", title),
                                        new XElement("Description", description),
                                        new XElement("PicUrl", picUrl),
                                        new XElement("Url", redrectUrl.Replace("_OpenID_", xElement.Element("FromUserName").Value).Replace("_AgentID_", xElement.Element("AgentID").Value))));

            return ex;
        }
        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <param name="toUserName"></param>
        /// <param name="agentID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="picUrl"></param>
        /// <param name="redrectUrl"></param>
        /// <returns></returns>
        public static XElement GetSendImageContext(string fromUserName, string toUserName, string agentID, string title, string description, string picUrl, string redrectUrl)
        {

            XElement ex = new XElement("xml", new XElement("ToUserName"), new XElement("FromUserName"), new XElement("CreateTime"), new XElement("MsgType"), new XElement("Content"), new XElement("ArticleCount"), new XElement("Articles"), new XElement("FuncFlag"));

            ex.Element("ToUserName").SetValue(fromUserName);

            ex.Element("FromUserName").SetValue(toUserName);

            ex.Element("MsgType").SetValue("news");

            ex.Element("CreateTime").SetValue(DateTime.Now.Ticks);

            ex.Element("ArticleCount").SetValue(1);

            ex.Element("FuncFlag").SetValue("1");

            ex.Element("Articles").Add(new XElement("item",
                                        new XElement("Title", title),
                                        new XElement("Description", description),
                                        new XElement("PicUrl", picUrl),
                                        new XElement("Url", redrectUrl.Replace("_OpenID_", fromUserName).Replace("_AgentID_", agentID))));

            return ex;
        }
        /// <summary>
        /// 将图片数据封装到XML中
        /// </summary>
        /// <param name="ToUserName"></param>
        /// <param name="FromUserName"></param>
        /// <param name="Articles"></param>
        /// <returns></returns>
        public static XElement GetSendImageContext(string ToUserName, string FromUserName, IList<Message.Article> Articles)
        {
            XElement ex = new XElement("xml", new XElement("ToUserName"), new XElement("FromUserName"), new XElement("CreateTime"), new XElement("MsgType"), new XElement("Content"), new XElement("ArticleCount"), new XElement("Articles"), new XElement("FuncFlag"));

            ex.Element("ToUserName").SetValue(ToUserName);

            ex.Element("FromUserName").SetValue(FromUserName);

            ex.Element("MsgType").SetValue("news");

            ex.Element("CreateTime").SetValue(DateTime.Now.Ticks);

            ex.Element("ArticleCount").SetValue(Articles.Count);

            ex.Element("FuncFlag").SetValue("1");

            foreach (var item in Articles)
            {
                ex.Element("Articles").Add(new XElement("item", new XElement("Title", item.Title), new XElement("Description", item.Description), new XElement("PicUrl", item.Picurl), new XElement("Url", item.Url.Replace("_OpenID_", ToUserName))));
            }

            return ex;
        }

        /// <summary>
        /// 发送加密信息
        /// </summary>
        /// <param name="messageAPI"></param>
        /// <param name="sendxel"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static string SendEncryptMsg(MessageAPI messageAPI, XElement sendxel)
        {
            var sEncryptMsg = "";
            var ret = messageAPI.EncryptMsg(sendxel.ToString(), ref sEncryptMsg);
            if (ret == 0)
                return sEncryptMsg;
            else
            {
                return "加密失败！";
            }
        }
        #endregion

        #endregion


        #region 主动模式
        /// <summary>
        /// 微信消息处理类 （主动模式）
        /// </summary>
        public MessageAPI()
        {

        }

        /// <summary>
        /// 主动推送文本消息
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        public QyJsonResult SendTxtMessage(Message.TextMessageClass tc)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken.access_token), tc);

        }
        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="ic"></param>
        /// <returns></returns>
        public QyJsonResult SendImageMessage(Message.ImageMessageClass ic)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken.access_token), ic);
        }

        public QyJsonResult SendVoiceMessage(Message.VoiceMessageClass vc)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken.access_token), vc);
        }
        public QyJsonResult SendVideoMessage(Message.VideoMessageClass vc)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken.access_token), vc);
        }
        public QyJsonResult SendFileMessage(Message.FileMessageClass fc)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken.access_token), fc);
        }
        public QyJsonResult SendNewsMessage(Message.NewsMessageClass nc)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken.access_token), nc);
        }
        public QyJsonResult SendMPNewsMessage(Message.MPNewsMessageClass nc)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", AccessToken.access_token), nc);
        }
        #endregion
    }
}
