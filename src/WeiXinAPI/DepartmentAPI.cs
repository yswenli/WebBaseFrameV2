
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WeiXinAPIs.Models;
//
using WeiXinAPIs.Libs;

namespace WeiXinAPIs
{
    public class DepartmentAPI : BaseAPI
    {
        public Department CreateDepartment(Department department)
        {
            return this.Post<Department>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/create?access_token={0}", AccessToken.access_token), department);
        }

        public QyJsonResult UpdateDepartment(Department department)
        {
            return this.Post<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/update?access_token={0}", AccessToken.access_token), department);
        }

        public QyJsonResult DeleteDepartment(int id)
        {
            return this.Get<QyJsonResult>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/delete?access_token={0}&id={1}", AccessToken.access_token, id));
        }

        public DepartmentList GetDepartmentList()
        {
            return this.Get<DepartmentList>(string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/list?access_token={0}", AccessToken.access_token));
        }
    }
}
