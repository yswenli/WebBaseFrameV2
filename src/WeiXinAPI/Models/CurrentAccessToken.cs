using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 保存当前AccessToken的时间和Token
    /// </summary>
    internal sealed class CurrentAccessToken
    {
        public static DateTime ExpiresTime { get; set; }

        public static AccessToken AccessToken { get; set; }
    }
}
