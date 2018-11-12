using Microsoft.JScript;
using System;
using System.Web.Mail;

namespace Common
{
    public class MailHelper
    {
        #region 静态方法发送
        /// <summary>
        /// 简易发信
        /// </summary>
        /// <param name="to"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        public static void SimpleSend(string to, string Subject, string Body)
        {
            string from = "serviceweb@163.com";
            string password = "Password01!";
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = from;
            mailMsg.To = to.Replace("；", ";").Replace(",", ";").Replace("，", ";");
            mailMsg.Subject = Subject;
            mailMsg.BodyFormat = MailFormat.Html;
            mailMsg.Body = Body;
            mailMsg.Priority = MailPriority.High;
            SmtpMail.SmtpServer = "smtp.163.com";
            mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", from);
            mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", password);
            mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);
            mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", true);
            SmtpMail.Send(mailMsg);
        }

        /// <summary>
        /// 公司邮件服务发信
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Cc"></param>
        /// <param name="Bcc"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="senddate"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool Send(string To, string Cc, string Bcc, string Subject, string Body, DateTime? senddate, out string error)
        {
            try
            {
                if (string.IsNullOrEmpty(To))
                {
                    error = "收件人邮箱为空";
                    return false;
                }
                senddate = senddate == null ? DateTime.Now : senddate;
                error = HttpHelper.POST("http://mailapi.medwin.cn/PublishAPI/SendMail",
                    "deviceNum=E720C93D-C750-4B78-9A75-E36FF5DDE4EC"
                    + "&to=" + ReplaceChar(To)
                    + "&cc=" + ReplaceChar(Cc)
                    + "&bcc=" + ReplaceChar(Bcc)
                    + "&subject=" + ReplaceChar(Subject)
                    + "&content=" + ReplaceChar(Body)
                    + "&sendDate=" + senddate
                    ).ToString();
                return true;
            }
            catch (Exception ex)
            {
                error = "发送失败：" + ex.Message;
                return false;
            }
        }
        internal static string ReplaceChar(string str)
        {
            return str.Replace("&", "{SpecialChar}");
        }
        #endregion

        #region 实例化发送

        string _smtp;
        string _from;
        string _password;
        int _port;
        bool _auth;
        bool _ssl;


        /// <summary>
        /// 默认使用163初始化
        /// </summary>
        public MailHelper()
            : this("smtp.163.com", "serviceweb@163.com", "Password01!", 465)
        {

        }
        /// <summary>
        /// 读取配置初始化
        /// </summary>
        /// <param name="smtp"></param>
        /// <param name="from"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        public MailHelper(string smtp, string from, string password, int port)
            : this(smtp, from, password, port, true, true)
        {
        }
        /// <summary>
        /// 读取配置初始化
        /// </summary>
        /// <param name="smtp"></param>
        /// <param name="from"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        /// <param name="auth"></param>
        /// <param name="ssl"></param>
        public MailHelper(string smtp, string from, string password, int port, bool auth, bool ssl)
        {
            _smtp = smtp; _from = from; _password = password; _port = port; _auth = auth; _ssl = ssl;
        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="attachs"></param>
        public void Send(string to, string cc, string Subject, string Body, string attachs)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = _from;
            mailMsg.To = to.Replace("；", ";").Replace(",", ";").Replace("，", ";");
            if (!string.IsNullOrEmpty(cc))
            {
                mailMsg.Cc = cc;
            }
            mailMsg.Subject = Subject;
            mailMsg.BodyFormat = MailFormat.Html;
            mailMsg.Body = Body;
            mailMsg.Priority = MailPriority.High;
            SmtpMail.SmtpServer = _smtp;
            if (_auth)
            {
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", _from);
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", _password);
            }
            if (_ssl)
            {
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", true);
            }
            else
            {
                mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", false);
            }
            mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", _port);

            if (!string.IsNullOrEmpty(attachs))
            {
                var attachsArr = attachs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (attachsArr != null && attachsArr.Length > 0)
                {
                    foreach (var attachitem in attachsArr)
                    {
                        mailMsg.Attachments.Add(new MailAttachment(attachitem));
                    }
                }
                else
                {
                    mailMsg.Attachments.Add(new MailAttachment(attachs));
                }
            }

            SmtpMail.Send(mailMsg);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        public void Send(string to, string Subject, string Body)
        {
            this.Send(to, "", Subject, Body, "");
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        public void Send(string to, string cc, string Subject, string Body)
        {
            this.Send(to, cc, Subject, Body, string.Empty);
        }


        #endregion
    }
}
