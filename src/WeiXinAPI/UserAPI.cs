
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WeiXinAPIs.Models;

//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    public class UserAPI : BaseAPI
    {
        public QyJsonResult CreateUser(UserInfo user)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token={0}", AccessToken.access_token), user);
        }

        public QyJsonResult UpdateUser(UserInfo user)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token={0}", AccessToken.access_token), user);
        }

        public QyJsonResult DeleteUser(string userid)
        {
            return this.Get<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/delete?access_token={0}&userid={1}", AccessToken.access_token, userid));
        }

        public UserInfo GetUser(string userid)
        {
            return this.Get<UserInfo>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}", AccessToken.access_token, userid));
        }

        public UserSimpleList GetUserSimpleList(int department_id, int fetch_child, int status)
        {
            return this.Get<UserSimpleList>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/simplelist?access_token={0}&department_id={1}&fetch_child={2}&status={3}", AccessToken.access_token, department_id, fetch_child, status));
        }

        public UserSimpleList GetUserListByTag(int tagID)
        {
            return this.Get<UserSimpleList>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/tag/get?access_token={0}&tagid={1}", AccessToken.access_token, tagID));

        }
    }
}
