using Libraries;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using WEF;

namespace Web.Areas.Admin.Controllers
{
    [SystemErrorFilter]
    [PermissionFilter(true)]
    public class BaseController : Controller
    {

        #region 当前帐号信息

        /// <summary>
        /// 保存当前帐号编号
        /// </summary>
        public int ID
        {
            get
            {
                return CurrentMember.ID;
            }

        }
        /// <summary>
        /// 保存当前帐号名称
        /// </summary>
        public string RealName
        {
            get { return CurrentMember.Member.RealName; }
        }
        /// <summary>
        /// 保存当前帐号所属组
        /// </summary>
        public string UserName
        {
            get { return CurrentMember.UserName; }
        }
        #endregion

        #region 分页信息

        /// <summary>
        /// 默认当前页面是第一页
        /// </summary>
        public const int PageIndex = 1;
        /// <summary>
        /// 默认每页显示10条信息
        /// </summary>
        public int PageSize
        {
            get
            {
                string pagesize = System.Web.HttpContext.Current.Request.QueryString["pagesize"];
                if (!string.IsNullOrEmpty(pagesize))
                    return int.Parse(pagesize);
                else
                    return 10;
            }
        }
        /// <summary>
        /// int最大值
        /// </summary>
        public const int SortMax = int.MaxValue;
        /// <summary>
        /// int最大值
        /// </summary>
        public const int PageSizeMax = int.MaxValue;
        #endregion

        /// <summary>
        /// 数据操作上下文对象
        /// </summary>
        public DBContext dbHelper
        {
            get { return new DBContext(); }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Order
        {
            get
            {
                string order = System.Web.HttpContext.Current.Request.QueryString["order"];
                if (string.IsNullOrEmpty(order))
                    order = "ID";
                return order;
            }
        }
        /// <summary>
        /// 排序访视
        /// </summary>
        public bool By
        {
            get
            {
                var byStr = System.Web.HttpContext.Current.Request.QueryString["by"];
                if (string.IsNullOrEmpty(byStr))
                    byStr = "Desc";
                return byStr == "Desc" ? false : true;
            }
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

        public string ErrorWirter(RouteData routeData, string content)
        {
            try
            {
                //string controller = RouteData.Values["controller"].ToString().ToLower();
                //string action = RouteData.Values["action"].ToString().ToLower();
                //string errorNo = "错误号：" + controller + "-" + action + "-" + DateTime.Now.Ticks + " ";

                ////BSystemLog obj = new BSystemLog { ID = GID.NewGID(), CreateDate = DateTime.Now, ModifyDate = DateTime.Now, CreateUser = CurrentRealName, ModifyUser = CurrentRealName, IsDeleted = false, LogType = LogType.Error.ToString(), ControllerName = controllerName, ActionName = actionName, Content = errorNo + content };

                ////SystemLogLogic sll = new SystemLogLogic();

                ////sll.Add(obj);

                //return errorNo + "-" + content;
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// sql过滤
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SqlFilter(string source)
        {
            //单引号替换成两个单引号
            source = source.Replace("'", "''");

            //半角封号替换为全角封号，防止多语句执行
            source = source.Replace(";", "；");

            //半角括号替换为全角括号
            source = source.Replace("(", "（");
            source = source.Replace(")", "）");

            ///////////////要用正则表达式替换，防止字母大小写得情况////////////////////

            //去除执行存储过程的命令关键字
            source = source.Replace("Exec", "");
            source = source.Replace("Execute", "");

            //去除系统存储过程或扩展存储过程关键字
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");

            //防止16进制注入
            source = source.Replace("0x", "0 x");

            return source;
        }
    }
}
