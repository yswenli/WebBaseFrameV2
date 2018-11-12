using Libraries;
using System;
using System.Linq;
using System.Web.Mvc;
using WebBaseFrame.Models;

namespace Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        #region 登录
        [PermissionFilter(false)]
        public ActionResult LogOn()
        {
            if (CurrentMember.ID > 0)
                return Redirect("/admin");
            return View();
        }
        [PermissionFilter(false)]
        [HttpPost]
        public ActionResult LogOn(string username, string password, bool? auto, string returnUrl, FormCollection form)
        {
            //验证码必填项及验证码
            if (string.IsNullOrEmpty(username))
            {
                Common.VerifyCodeHelper.ChangeCode();
                return Content(ContentIcon.Error + "|请输入用户名");
            }
            if (string.IsNullOrEmpty(password))
            {
                Common.VerifyCodeHelper.ChangeCode();
                return Content(ContentIcon.Error + "|请输入密码");
            }
            string verifycode = form["verifycode"];
            if (string.IsNullOrEmpty(verifycode))
            {
                Common.VerifyCodeHelper.ChangeCode();
                return Content(ContentIcon.Error + "|请输入验证码");
            }
            else
            {
                if (Session["verifyCode"] == null)
                {
                    Common.VerifyCodeHelper.ChangeCode();
                    return Content(ContentIcon.Error + "|验证码已过期，请刷新验证码");
                }
                else
                {
                    if (verifycode.ToLower() != Session["verifyCode"].ToString().ToLower())
                    {
                        Common.VerifyCodeHelper.ChangeCode();
                        return Content(ContentIcon.Error + "|验证码不正确");
                    }
                }
            }
            //登录验证用户名、密码
            string url = string.Empty;
            string _message = string.Empty;
            if (auto == null)
                auto = false;
            bool _true = CurrentMember.LogOn(username, password, (bool)auto, out _message);
            if (_true)
            {
                url = "/admin";
                //防跳转钓鱼
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(url))
                {
                    url = returnUrl;
                }
                else
                {
                    url = "/admin";
                }
                Common.VerifyCodeHelper.ChangeCode();
                return Content(ContentIcon.Succeed + "|" + _message + "|" + url);
            }
            else
            {
                Common.VerifyCodeHelper.ChangeCode();
                return Content(ContentIcon.Error + "|" + _message);
            }
        }
        #endregion

        //注销
        public ActionResult LogOut()
        {
            CurrentMember.LogOut();
            return Redirect("/admin/account/logon");
        }

        //个人信息
        public ActionResult Personal()
        {
            return View(CurrentMember.Member);
        }
        [HttpPost]
        public ActionResult Personal(FormCollection form)
        {
            Member member = new MemberRepository().Search().Where(b => b.ID == CurrentMember.ID).First();
            UpdateModel(member);
            new MemberRepository().Update(member);
            Session.Remove(CurrentMember.Prefix + "Member");
            return Content(ContentIcon.Succeed + "|保存成功");
        }

        //重置密码
        public ActionResult ReSetPwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReSetPwd(FormCollection form)
        {
            Member member = new MemberRepository().Search().Where(b => b.ID == CurrentMember.ID).First();
            if (member.PwdNotMD5 != form["OldPwd"])
                return Content(ContentIcon.Error + "|旧密码填写错误");
            if (form["PwdNotMd5"] != form["ConfirmPwd"])
                return Content(ContentIcon.Error + "|两次密码填写不一致");
            if (form["OldPwd"] == form["ConfirmPwd"])
                return Content(ContentIcon.Error + "|新密码不能和旧密码一样");
            member.PwdNotMD5 = form["PwdNotMd5"];
            member.Password = Common.Character.EncrytPassword(member.PwdNotMD5);
            new MemberRepository().Update(member);
            Session.Remove(CurrentMember.Prefix + "Member");
            return Content(ContentIcon.Succeed + "|保存成功");
        }
        //忘记密码
        [PermissionFilter(false)]
        public ActionResult ForgetPwd()
        {
            return View();
        }
        [PermissionFilter(false)]
        [HttpPost]
        public ActionResult ForgetPwd(string email, FormCollection form)
        {
            if (!Common.Validate.IsEmail(email))
                return Content(ContentIcon.Error + "|邮箱格式错误");
            else
            {
                string error = string.Empty;
                Member member = new MemberRepository().Search().Where(b => b.Email == email).First();
                if (member != null)
                {
                    string body = "<p>尊敬的 <b>" + member.RealName + @"</b> 先生/女士，您好：</p>" +
                        "<p style='text-indent:21pt'>您于" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "进行了<b>忘记密码</b>操作，您的账户信息为：</p>" +
                        "<p style='text-indent:21pt'>用户名：" + member.UserName + "</p>" +
                        "<p style='text-indent:21pt'>密码：" + member.PwdNotMD5 + "</p>" +
                        "<p style='text-align:right'>本邮件系统自动发送，请勿回复。</p>";
                    MailSettingRepository ml = new MailSettingRepository();
                    var mt = ml.GetList(1, 20).First();
                    var mail = new Common.MailHelper(mt.MailServer, mt.MailFrom, mt.MailPassword, mt.MailPort ?? 465);
                    mail.Send(email, "【找回密码】-" + CurrentSite.Title, body);
                    return Content(ContentIcon.Succeed + "|密码已经发送至您的邮箱，请注意查收");
                }
                else
                    return Content(ContentIcon.Error + "|无效的邮箱地址");
            }
        }
        public ActionResult Avatar()
        {
            return View();
        }
    }
}
