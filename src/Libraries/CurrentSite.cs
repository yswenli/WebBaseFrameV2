using WebBaseFrame.Models;

namespace Libraries
{
    public class CurrentSite
    {
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public static Site Site
        {
            get
            {
                return new SiteRepository().Search().First();
            }
        }
        /// <summary>
        /// 站点名称
        /// </summary>
        public static string Title
        {
            get { return Site.Name; }
        }
        /// <summary>
        /// SEO关键字
        /// </summary>
        public static string KeyWords
        {
            get { return Site.KeyWords; }
        }
        /// <summary>
        /// SEO描述
        /// </summary>
        public static string Description
        {
            get { return Site.Description; }
        }
        /// <summary>
        /// 统计代码
        /// </summary>
        public static string BaiduJS
        {
            get { return Site.GoogleJS; }
        }
    }
}