using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs
{
    /// <summary>
    /// 微信返回成功内容
    /// </summary>
    public class AccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}
