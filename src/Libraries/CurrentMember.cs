using Common;
using System;
using WebBaseFrame.Models;

namespace Libraries
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class CurrentMember
    {
        public const string Prefix = "CurrentMember-";

        #region 属性
        /// <summary>
        /// 登录用户IDer
        /// </summary>
        public static int ID
        {
            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Request.Cookies[Prefix + "ID"] != null
                        && !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[Prefix + "ID"].Value))
                    {
                        return int.Parse(System.Web.HttpContext.Current.Request.Cookies[Prefix + "ID"].Value);
                    }
                    else
                    {
                        if (System.Web.HttpContext.Current.Session[Prefix + "ID"] != null)
                            return Convert.ToInt32(System.Web.HttpContext.Current.Session[Prefix + "ID"]);
                        else
                            return 0;
                    }
                }
                catch { }
                return 0;

            }
        }
        /// <summary>
        /// 登录用户UserName
        /// </summary>
        public static string UserName
        {
            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Request.Cookies[Prefix + "UserName"] != null
                        && !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[Prefix + "UserName"].Value))
                    {
                        return Common.Character.DecryptBase64(System.Web.HttpContext.Current.Request.Cookies[Prefix + "UserName"].Value);
                    }
                    else
                    {
                        if (System.Web.HttpContext.Current.Session[Prefix + "UserName"] != null)
                            return System.Web.HttpContext.Current.Session[Prefix + "UserName"].ToString();
                        else
                            return "";
                    }
                }
                catch
                {

                }
                return "";
            }
        }
        /// <summary>
        /// 登录用户GroupID
        /// </summary>
        public static int RoleID
        {
            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Request.Cookies[Prefix + "RoleID"] != null
                        && !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[Prefix + "RoleID"].Value))
                    {
                        return int.Parse(System.Web.HttpContext.Current.Request.Cookies[Prefix + "RoleID"].Value);
                    }
                    else
                    {
                        if (System.Web.HttpContext.Current.Session[Prefix + "RoleID"] != null)
                            return int.Parse(System.Web.HttpContext.Current.Session[Prefix + "RoleID"].ToString());
                        return 0;
                    }
                }
                catch { }
                return 0;
            }
        }
        /// <summary>
        /// 登录用户Member对象
        /// </summary>
        public static Member Member
        {
            get
            {
                try
                {
                    MemberRepository userlogic = new MemberRepository();
                    if (ID > 0)
                    {
                        if (System.Web.HttpContext.Current.Session[Prefix + "Member"] != null)
                            return System.Web.HttpContext.Current.Session[Prefix + "Member"] as Member;
                        else
                        {
                            Member _member = userlogic.Search().Where(b => b.ID == ID).First();
                            System.Web.HttpContext.Current.Session[Prefix + "Member"] = _member;
                            if (_member == null || _member.ID < 0)
                            {
                                System.Web.HttpContext.Current.Response.Redirect("/admin/Account/LogOut");
                            }
                            return _member;
                        }
                    }
                    else
                        return new Member();
                }
                catch { }
                return new Member();
            }
        }

        public static Role Role
        {
            get
            {
                try
                {
                    Role _role = new RoleRepository().Search().Where(b=>b.ID==CurrentMember.RoleID).First();
                    if (_role == null || _role.ID < 0)
                    {
                        System.Web.HttpContext.Current.Response.Redirect("/admin/Account/LogOut");
                    }
                    return _role ?? new Role();
                }
                catch { }
                return new Role();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 登录
        /// </summary>
        public static bool LogOn(string username, string password, bool auto, out string message)
        {
            MemberRepository userlogic = new MemberRepository();
            Member user = new Member();
            bool flag = true;
            message = "";
            user = userlogic.Search().Where(b=>b.UserName==username && b.Password== Character.EncrytPassword(password)).FirstDefault();
            if (user == null)
            {
                flag = false;
                message = "登录失败，账户信息错误";
            }
            else
            {
                if (auto)
                {
                    System.Web.HttpCookie cookie = new System.Web.HttpCookie(Prefix + "ID");
                    cookie.Value = user.ID.ToString();
                    cookie.Expires = DateTime.Now.AddDays(7);
                    System.Web.HttpContext.Current.Response.AppendCookie(cookie);

                    System.Web.HttpCookie cookie2 = new System.Web.HttpCookie(Prefix + "RoleID");
                    cookie2.Value = user.RoleID.ToString();
                    cookie2.Expires = DateTime.Now.AddDays(7);
                    System.Web.HttpContext.Current.Response.AppendCookie(cookie2);

                    System.Web.HttpCookie cookie3 = new System.Web.HttpCookie(Prefix + "UserName");
                    cookie3.Value = Common.Character.EncryptBase64(user.UserName.ToString());
                    cookie3.Expires = DateTime.Now.AddDays(7);
                    System.Web.HttpContext.Current.Response.AppendCookie(cookie3);
                }
                else
                {
                    System.Web.HttpContext.Current.Session[Prefix + "ID"] = user.ID;
                    System.Web.HttpContext.Current.Session[Prefix + "UserName"] = user.UserName;
                    System.Web.HttpContext.Current.Session[Prefix + "RoleID"] = user.RoleID;
                    System.Web.HttpContext.Current.Session[Prefix + "Member"] = user;
                }

                message = "登录成功，欢迎回来 " + user.RealName + "（" + user.UserName + "）";
            }
            return flag;
        }
        /// <summary>
        /// 注销
        /// </summary>
        public static void LogOut()
        {
            //清除Cookie
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(Prefix + "ID");
            cookie.Expires = DateTime.Now.AddSeconds(-1);
            System.Web.HttpContext.Current.Response.AppendCookie(cookie);
            System.Web.HttpCookie cookie2 = new System.Web.HttpCookie(Prefix + "RoleID");
            cookie2.Expires = DateTime.Now.AddSeconds(-1);
            System.Web.HttpContext.Current.Response.AppendCookie(cookie2);
            System.Web.HttpCookie cookie3 = new System.Web.HttpCookie(Prefix + "UserName");
            cookie3.Expires = DateTime.Now.AddSeconds(-1);
            System.Web.HttpContext.Current.Response.AppendCookie(cookie3);
            //删除Session
            System.Web.HttpContext.Current.Session.Clear();
        }
        /// <summary>
        /// 权限当前用户访问权限
        /// </summary>
        public static void CheckAccess()
        {
            if (ID == 0)//没有登陆
            {
                System.Web.HttpContext.Current.Response.Redirect("/account/logon?url=" + Microsoft.JScript.GlobalObject.escape(Common.UrlHelper.CurrentUrl));
                return;
            }
        }
        #endregion
    }
}