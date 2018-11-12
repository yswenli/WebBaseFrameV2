
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WeiXinAPIs.Models
{
    public class Department : QyJsonResult
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 部门名称。长度限制为1~64个字符
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 父亲部门id。根部门id为1
        /// </summary>
        [JsonProperty("parentid")]
        public int ParentID { get; set; }

        /// <summary>
        /// 在父部门中的次序。从1开始，数字越大排序越靠后
        /// </summary>
        [JsonProperty("order")]
        public int Order { get; set; }
    }

    public class DepartmentList : QyJsonResult
    {
        [JsonProperty("department")]
        public IList<Department> Departments;
    }
}
