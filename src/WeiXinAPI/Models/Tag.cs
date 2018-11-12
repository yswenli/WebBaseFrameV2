using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 微信企业号标签
    /// </summary>
    public class Tag
    {
        [JsonProperty("tagid")]
        public int TagID { get; set; }

        [JsonProperty("tagname")]
        public string TagName { get; set; }
    }

    public class TagsList : QyJsonResult
    {
        [JsonProperty("taglist")]
        public IList<Tag> Tags{ get; set; }
    }
}
