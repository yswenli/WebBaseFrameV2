using System.Web.Mvc;

namespace Libraries
{
    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        bool _mustValid = false;

        public PermissionFilterAttribute(bool mustValid)
        {
            _mustValid = mustValid;
        }

        /// <summary>
        /// 在执行操作方法之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (_mustValid)
            {
                //过滤器只过滤未登陆
                if (Libraries.CurrentMember.ID < 1)
                {
                    ValideFail(filterContext);
                }
            }
        }


        /// <summary>
        /// 在执行操作方法之后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// OnResultExecuting 在执行操作结果之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        /// <summary>
        /// OnResultExecuted 在执行操作结果后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// 权限验证失败后处理
        /// </summary>
        /// <param name="filterContext"></param>
        private void ValideFail(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.Write("<script>alert('当前操作需要登录~');top.location.href='/admin/account/logon';</script>");
            filterContext.HttpContext.Response.End();
        }
    }    
}