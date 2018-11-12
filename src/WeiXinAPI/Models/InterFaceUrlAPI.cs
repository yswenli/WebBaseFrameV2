using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs
{
    /// <summary>
    /// 微信接口URL
    /// </summary>
    public class InterFaceUrlAPI
    {
        /// <summary>
        /// 是否是微信企业号
        /// </summary>
        bool _isQY;
        /// <summary>
        /// 非企业号为appid,企业号为corpid
        /// </summary>
        string _weiXinID;
        /// <summary>
        /// 非企业号为appsecret，企业号为corpsecret
        /// </summary>
        string _secret;
        /// <summary>
        /// 微信接口URL
        /// </summary>
        /// <param name="isQY">是否是微信企业号</param>
        /// <param name="weiXinID">非企业号为appid,企业号为corpid</param>
        /// <param name="secret">非企业号为appsecret，企业号为corpsecret</param>
        public InterFaceUrlAPI(bool isQY, string weiXinID, string secret)
        {
            _isQY = isQY; _weiXinID = weiXinID; _secret = secret;
        }

        /// <summary>
        /// 获取连接码
        /// </summary>
        public string AccessTokenUrl
        {
            get
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _weiXinID, _secret);
                if (_isQY)
                {
                    url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", _weiXinID, _secret);
                }
                return url;
            }
        }

        /// <summary>
        /// 获取下载媒体地址
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaID"></param>
        /// <returns></returns>
        public string GetDownLoadMediaUrl(AccessToken accessToken, string mediaID)
        {
            string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accessToken.access_token, mediaID);
            if (_isQY)
            {
                url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accessToken.access_token, mediaID);

            }
            return url;
        }

        /// <summary>
        /// 获取上传媒体地址
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type">类型为：image、voice、video、file</param>
        /// <returns></returns>
        public string GetUploadMediaUrl(AccessToken accessToken, string type)
        {
            string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken.access_token, type);
            if (_isQY)
            {
                url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken.access_token, type);

            }
            return url;
        }
    }
}