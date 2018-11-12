
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class UserInfo : QyJsonResult
    {
        [JsonProperty("userid")]
        public string UserID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 成员所属部门id列表 ,例："department": [1, 2],
        /// </summary>
        [JsonProperty("department")]
        public int[] Department { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 性别。gender=0表示男，=1表示女
        /// </summary>
        [JsonProperty("gender")]
        public int Gender { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>
        [JsonProperty("tel")]
        public string Tel { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("weixinid")]
        public string WeixinID { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 启用/禁用成员。1表示启用成员，0表示禁用成员
        /// </summary>
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("extattr")]
        public ExtAttrs ExtAttr { get; set; }

        /// <summary>
        /// 关注状态: 1=已关注，2=已冻结，4=未关注
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public enum Gender
    {
        /// <summary>
        /// 男性
        /// </summary>
        Male = 0,

        /// <summary>
        /// 女性
        /// </summary>
        FeMale = 1
    }

    public class ExtAttrs
    {
        [JsonProperty("attrs")]
        public IList<ExtAttr> Attrs { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Attrs != null && Attrs.Count > 0)
            {
                foreach (var extAttr in Attrs)
                {
                    sb.Append(string.Format("{{ Name: {0}, Value: {1} }} ", extAttr.Name, extAttr.Value));
                }
            }
            return sb.ToString();
        }
    }

    public class ExtAttr
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }


    public class UserSimpleList : QyJsonResult
    {
        [JsonProperty("userlist")]
        public IList<UserInfo> UserList { get; set; }
    }
}
