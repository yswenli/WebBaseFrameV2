using Common;
using System;


namespace Libraries
{
    public class TemplateHelper
    {
        public static string site_title = CurrentSite.Title;
        public static string root_admin = UrlHelper.HostUrl + "/areas/admin";
        public static string root = UrlHelper.HostUrl;
        public static string root_logon = UrlHelper.HostUrl + "/account/logon";
        public static string root_logout = UrlHelper.HostUrl + "/account/logout";
        public static string root_template = UrlHelper.HostUrl + "/upload/template/{0}";
        /// <summary>
        /// 替换标签
        /// </summary>
        public static string ReplaceTag(string html)
        {
            string[] tags = { "{site_title}", "{root_admin}", "{root}", "{root_logon}", "{root_logout}", "{user_id}", "{user_realname}", "{date_now_int}", "{date_now}" };
            string[] values = { site_title, root_admin, root, root_logon, root_logout, CurrentMember.ID.ToString(), CurrentMember.Member.RealName, Character.DateTimeToInt(DateTime.Now).ToString(), DateTime.Now.ToString("yyyy-MM-dd") };
            for (int i = 0; i < tags.Length; i++)
            {
                html = html.Replace(tags[i], values[i]);
            }
            return html;
        }
    }

}
