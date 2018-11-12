using Newtonsoft.Json;
using System.Net;
using System.Text;
using WeiXinAPIs.Models;
//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    public class TagAPI : BaseAPI
    {
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public QyJsonResult CreateTag(Tag tag)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/create?access_token={0}", AccessToken.access_token), tag);
        }
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public QyJsonResult UpdateTag(Tag tag)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/update?access_token={0}", AccessToken.access_token), tag);
        }
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        public QyJsonResult DeleteTag(int tagID)
        {
            return this.Get<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/delete?access_token={0}&tagid={1}", AccessToken.access_token, tagID));
        }
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        public TagsList GetTagList()
        {
            return this.Get<TagsList>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/list?access_token={0}", AccessToken.access_token));
        }
    }
}
