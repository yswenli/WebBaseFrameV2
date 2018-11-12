using Common;
using System;
using WEF;
//

namespace Web.Areas.Manager.Helper
{
    public class PathHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DBConnectionString
        {
            get
            {
                return new DBContext().Db.ConnectionString;     
            }
        }
        /// <summary>
        /// 当前应用所在地址
        /// </summary>
        static string basePath = AppDomain.CurrentDomain.BaseDirectory;

        public static string BasePath
        {
            get { return PathHelper.basePath; }
        }

        /// <summary>
        /// 模板地址
        /// </summary>
        static string templepath = AppDomain.CurrentDomain.BaseDirectory + "Areas\\Manager\\Template\\";

        public static string Templepath
        {
            get { return PathHelper.templepath; }
        }

        static string selectedTempPath;
        /// <summary>
        /// 选择的模板
        /// </summary>
        public static string SelectedTempPath
        {
            get { return PathHelper.selectedTempPath; }
            set { PathHelper.selectedTempPath = value; }
        }

        /// <summary>
        /// 生成实体集的地址
        /// </summary>
        static string entitypath = BasePath + "..\\Models\\Entity\\AutoGenerator\\";

        public static string Entitypath
        {
            get { return PathHelper.entitypath; }
        }

        /// <summary>
        /// 生成控制器地址
        /// </summary>
        static string controllerpath = BasePath + "Areas\\Admin\\Controllers\\";

        public static string Controllerpath
        {
            get { return PathHelper.controllerpath; }
        }

        /// <summary>
        /// 生成视图地址
        /// </summary>
        static string viewpath = BasePath + "Areas\\Admin\\Views\\";

        public static string Viewpath
        {
            get { return PathHelper.viewpath; }
            set { PathHelper.viewpath = value; }
        }
    }

    /// <summary>
    /// 模板类
    /// </summary>
    public class TempInfo
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; path = Web.Areas.Manager.Helper.PathHelper.Templepath + "temp1\\" + name + ".txt"; }
        }

        string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public TempInfo() { }
        public TempInfo(string _name, string _path)
        {
            name = _name;
            path = _path;
        }
        public TempInfo(string _name)
        {
            var defaultPath = Web.Areas.Manager.Helper.PathHelper.Templepath + "temp1\\";
            name = _name;
            path = defaultPath + _name + ".txt";
        }
        /// <summary>
        /// 获取模板内容
        /// </summary>
        /// <returns></returns>
        public string GetContent()
        {
            string result = "";
            if (!string.IsNullOrEmpty(path) && Common.FileHelper.IsFileExists(path))
            {
                result = Common.FileHelper.ReadFile(path);
            }
            return result;
        }
    }
}
