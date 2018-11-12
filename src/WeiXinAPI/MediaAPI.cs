using System.Net;
using System.Text;
using WeiXinAPIs.Models;
//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    /// <summary>
    /// 微信媒体操作API
    /// </summary>
    public class MediaAPI : BaseAPI
    {
        /// <summary>
        /// 微信媒体操作API
        /// </summary>
        public MediaAPI()
        {

        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video），普通文件(file)</param>
        /// <returns></returns>
        public QyJsonResult Upload(string fileName, MediaFileTypeEnum type)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", AccessToken.access_token, type.ToString()), fileName);
        }

        /// <summary>
        /// 下载微信媒体文件
        /// </summary>
        /// <param name="mediaID"></param>
        /// <param name="fileName"></param>
        public void DownLoad(string mediaID, string fileName)
        {
            this.Get(string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", AccessToken.access_token, mediaID), fileName);
        }

    }
}
