using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinAPIs.Models
{
    /// <summary>
    /// 企业号JSON返回结果
    /// </summary>
    public class QyJsonResult
    {
        public ReturnCodeEnum errcode { get; set; }
        public string errmsg { get; set; }
        /// <summary>
        /// 为P2P返回结果做准备
        /// </summary>
        public virtual object P2PData { get; set; }
    }
}
