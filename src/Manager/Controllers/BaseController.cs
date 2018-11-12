using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Manager.Controllers
{
    public class BaseController : Controller
    {

        bool _IsMSSQL;

        public bool IsMSSQL { get { return _IsMSSQL; } }

        //
        // GET: /Manager/Base/
        /// <summary>
        /// site中开关控制
        /// </summary>
        public BaseController()
        {
            if (Libraries.SiteHelper.Default.IsDeleloper != true)
            {
                var response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.BufferOutput = true;
                response.StatusCode = 403;
                response.Write("<!Doctype html><html xmlns=http://www.w3.org/1999/xhtml><head><meta http-equiv=Content-Type content=\"text/html;charset=utf-8\"><title>403页面禁止非法访问 </title><body>");
                response.Write("<h2>:( 此功能未开启，请联系本站管理员</h2>");
                response.Write("</body></html>");
                response.Flush();
                response.End();
            }
            _IsMSSQL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsMSSQL"]);
        }

        /// <summary>
        /// ajaxPost返回的Icon图标
        /// </summary>
        public class ContentIcon
        {
            public static int Error = -1;
            public static int Warning = 0;
            public static int Succeed = 1;
            public static int Question = 2;
            public static int Face_Smile = 3;
            public static int Face_Sad = 4;
        }
    }
}
