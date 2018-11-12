using Common;
using System;

namespace Web.Areas.Manager.Helper
{
    public class ProjectFileHelper
    {
        static string base_path = AppDomain.CurrentDomain.BaseDirectory;
        static string modelprojectFile_path = base_path + "..\\Models\\Models.csproj";
        static string webprojectFile_path = base_path + "Web.csproj";

        /// <summary>
        /// 将实体添加到项目Models中
        /// </summary>
        /// <param name="tableName"></param>
        public static void AddEntities(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string text = FileHelper.ReadFile(modelprojectFile_path);
                var index = text.LastIndexOf("Entity\\AutoGenerator\\");
                var appendText = "<Compile Include=\"Entity\\AutoGenerator\\" + tableName + ".cs\" />";
                if (index > -1 && text.IndexOf(appendText) < 0)
                {
                    var textPre = text.Substring(0, index + 21);
                    var textNext = text.Substring(index + 21);
                    var textAppendPre = textNext.Substring(0, textNext.IndexOf("/>") + 2);
                    appendText = System.Environment.NewLine + "    " + appendText;
                    var textAppendNext = textNext.Substring(textNext.IndexOf("/>") + 2);
                    text = textPre + textAppendPre + appendText + textAppendNext;
                    FileHelper.WriteFile(modelprojectFile_path, text);
                }
            }
        }
        /// <summary>
        /// 将逻辑添加到项目Models中
        /// </summary>
        /// <param name="tableName"></param>
        public static void AddLogic(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string text = FileHelper.ReadFile(modelprojectFile_path);
                var index = text.LastIndexOf("Include=\"Logic\\");
                var appendText = "<Compile Include=\"Logic\\" + tableName + "Logic.cs\" />";
                if (index > -1 && text.IndexOf(appendText) < 0)
                {
                    var textPre = text.Substring(0, index + 15);
                    var textNext = text.Substring(index + 15);
                    var textAppendPre = textNext.Substring(0, textNext.IndexOf("/>") + 2);
                    appendText = System.Environment.NewLine + "    " + appendText;
                    var textAppendNext = textNext.Substring(textNext.IndexOf("/>") + 2);
                    text = textPre + textAppendPre + appendText + textAppendNext;
                    FileHelper.WriteFile(modelprojectFile_path, text);
                }
            }
        }
        /// <summary>
        /// 将服务添加到项目Models中
        /// </summary>
        /// <param name="tableName"></param>
        public static void AddService(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string text = FileHelper.ReadFile(modelprojectFile_path);
                var index = text.LastIndexOf("Include=\"Service\\");
                var appendText = "<Compile Include=\"Service\\" + tableName + "Service.cs\" />";
                if (index > -1 && text.IndexOf(appendText) < 0)
                {
                    var textPre = text.Substring(0, index + 17);
                    var textNext = text.Substring(index + 17);
                    var textAppendPre = textNext.Substring(0, textNext.IndexOf("/>") + 2);
                    appendText = System.Environment.NewLine + "    " + appendText;
                    var textAppendNext = textNext.Substring(textNext.IndexOf("/>") + 2);
                    text = textPre + textAppendPre + appendText + textAppendNext;
                    FileHelper.WriteFile(modelprojectFile_path, text);
                }
            }

        }
        /// <summary>
        /// 将控制器添加到项目Web中
        /// </summary>
        /// <param name="tableName"></param>
        public static void AddControllers(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string text = FileHelper.ReadFile(webprojectFile_path);
                var index = text.LastIndexOf("Include=\"Areas\\Admin\\Controllers\\");
                var appendText = "<Compile Include=\"Areas\\Admin\\Controllers\\" + tableName + "Controller.cs\" />";
                if (index > -1 && text.IndexOf(appendText) < 0)
                {
                    var textPre = text.Substring(0, index + 33);
                    var textNext = text.Substring(index + 33);
                    var textAppendPre = textNext.Substring(0, textNext.IndexOf("/>") + 2);
                    appendText = System.Environment.NewLine + "    " + appendText;
                    var textAppendNext = textNext.Substring(textNext.IndexOf("/>") + 2);
                    text = textPre + textAppendPre + appendText + textAppendNext;
                    FileHelper.WriteFile(webprojectFile_path, text);
                }
            }
        }
        /// <summary>
        /// 将视图添加到项目Web中
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileName"></param>
        public static void AddViews(string tableName, string fileName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string text = FileHelper.ReadFile(webprojectFile_path);
                var index = text.LastIndexOf("Include=\"Areas\\Admin\\Views\\");
                var appendText = "<Content Include=\"Areas\\Admin\\Views\\" + tableName + "\\" + fileName + "\" />";
                if (index > -1 && text.IndexOf(appendText) < 0)
                {
                    var textPre = text.Substring(0, index + 27);
                    var textNext = text.Substring(index + 27);
                    var textAppendPre = textNext.Substring(0, textNext.IndexOf("/>") + 2);
                    appendText = System.Environment.NewLine + "    " + appendText;
                    var textAppendNext = textNext.Substring(textNext.IndexOf("/>") + 2);
                    text = textPre + textAppendPre + appendText + textAppendNext;
                    FileHelper.WriteFile(webprojectFile_path, text);
                }
            }
        }
    }
}