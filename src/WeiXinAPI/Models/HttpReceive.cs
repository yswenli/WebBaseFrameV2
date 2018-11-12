using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 从微信返回数据的封装
    /// </summary>
    public class HttpReceive
    {
        string _Signature;
        public string Signature
        {
            get
            {
                return _Signature;
            }

        }

        string _TimeStamp;
        public string TimeStamp
        {
            get
            {
                return _TimeStamp;

            }
        }

        string _Nonce;
        public string Nonce
        {
            get
            {
                return _Nonce;
            }
        }

        XElement _Message;
        /// <summary>
        /// 密文xml
        /// </summary>
        public XElement Message
        {
            get
            {
                return _Message;
            }
        }
        /// <summary>
        /// 从微信返回数据的封装
        /// </summary>
        /// <param name="request"></param>
        public HttpReceive(HttpRequestBase request)
        {
            _Signature = request.QueryString["msg_signature"];
            _TimeStamp = request.QueryString["timestamp"];
            _Nonce = request.QueryString["nonce"];            
            _Message = XElement.Load(request.InputStream, LoadOptions.None);
        }


    }
}
